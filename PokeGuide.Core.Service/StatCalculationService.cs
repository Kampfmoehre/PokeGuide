using System;

namespace PokeGuide.Core.Calculations
{
    /// <summary>
    /// Service for stat value calculation
    /// </summary>
    public class StatCalculationService : IStatCalculationService
    {
        public ushort CalculateStat(byte baseStat, byte level, ushort ev, byte iv, byte generation, double natureMod, bool isHp)
        {
            switch (generation)
            {
                case 1:
                case 2:
                    return CalculateStatFirstAndSecondGen(baseStat, level, ev, iv, isHp);
                case 3:
                case 4:
                case 5:
                case 6:
                    return CalculateStat(baseStat, level, (byte)ev, iv, natureMod, isHp);
            }
            throw new ArgumentOutOfRangeException(nameof(generation), generation, "Generation must be between 1 and 6");
        }

        /// <summary>
        /// Calculates a stat value for first and secind generation
        /// </summary>
        /// <param name="baseStat">The base stat value of the Pokémon</param>
        /// <param name="level">The current level of the Pokémon</param>
        /// <param name="ev">The amount of EV of the stat</param>
        /// <param name="dv">The DV of the stat</param>
        /// <param name="isHp"><c>True</c> when calculating Hit Points</param>
        /// <returns>The calculated stat value</returns>
        ushort CalculateStatFirstAndSecondGen(byte baseStat, byte level, ushort ev, byte dv, bool isHp)
        {
            if (dv > 15)
                throw new ArgumentOutOfRangeException(nameof(dv), dv, "DV for 1st and 2nd generation can not be higher than 15");

            // When ev is 0 or 1, game uses 0, otherwise it uses ev - 1
            double a = 0;
            if (ev > 1)
                a = Math.Sqrt(ev - 1.0);

            // Base calculation, make sure to round down on the calculations
            int b = Convert.ToInt32(Math.Floor((a + 1.0) / 4.0));
            double stat = ((((baseStat + dv) * 2.0 + b) * level) / 100.0);

            // Different for Hit Points
            if (isHp)
                return Convert.ToUInt16(Math.Floor(stat + level + 10));

            return Convert.ToUInt16(Math.Floor(stat + 5.0));
        }
        /// <summary>
        /// Calculates a stat value
        /// </summary>
        /// <param name="baseStat">The base stat value of the Pokémon</param>
        /// <param name="level">The current level of the Pokémon</param>
        /// <param name="ev">The amount of EV of the stat</param>
        /// <param name="iv">The IV of the stat</param>
        /// <param name="natureMod">The modifier of the nature</param>
        /// <param name="isHp"><c>True</c> when calculating Hit Points</param>
        /// <returns>The calculated stat</returns>
        ushort CalculateStat(byte baseStat, byte level, byte ev, byte iv, double natureMod, bool isHp)
        {
            if (iv > 31)
                throw new ArgumentOutOfRangeException(nameof(iv), iv, "IV can not be higher than 31");
            
            // This is equal for all stats
            // We have to round down for certain calculations
            double value = Math.Floor((baseStat * 2.0 + iv + Math.Floor(ev / 4.0)) * level / 100.0);

            // Different for Hit Points
            if (isHp)
                return Convert.ToUInt16(value + 10 + level);

            return Convert.ToUInt16(Math.Floor((value + 5) * natureMod));
        }
    }
}
