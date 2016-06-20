using System;

namespace PokeGuide.Core.Calculations
{
    /// <summary>
    /// Service for stat value calculation
    /// </summary>
    public class StatCalculationService : IStatCalculationService
    {
        /// <summary>
        /// Calculates Hit Points for a Pokémon
        /// </summary>
        /// <param name="baseHitPoints">The base Hit Points of the Pokémon</param>
        /// <param name="level">The level of the Pokémon</param>
        /// <param name="ev">The Hit Points effort value of the Pokémon</param>
        /// <param name="hpIv">The Hit Points individual value of the Pokémon</param>
        /// <param name="generation">The game generation for which the calculation is done</param>
        /// <returns>The calculated Hit Points</returns>
        /// <exception cref="ArgumentOutOfRangeException">When generation is less than 1 or greater than 6</exception>
        public ushort CalculateHitPoints(byte baseHitPoints, byte level, ushort ev, byte hpIv, byte generation)
        {
            switch (generation)
            {
                case 1:
                case 2:
                    return CalculateHitPointsFirstAndSecondGen(baseHitPoints, level, ev, hpIv);
                case 3:
                case 4:
                case 5:
                case 6:
                    return CalculateHitPoints(baseHitPoints, level, ev, hpIv);
            }
            throw new ArgumentOutOfRangeException(nameof(generation), generation, "Generation must be between 1 and 6");
        }

        /// <summary>
        /// Calculates a stat of a Pokémon
        /// </summary>
        /// <param name="baseStat">The base stat of the Pokémon species</param>
        /// <param name="level">The level of the Pokémon</param>
        /// <param name="ev">The effort value for the stat of the Pokémon</param>
        /// <param name="iv">The individual value for the stat of the Pokémon</param>
        /// <param name="generation">The game generation for which the calculation is done</param>
        /// <param name="natureMod">The modified of the nature of the Pokémon</param>
        /// <returns>The calculated stat value</returns>
        /// <exception cref="ArgumentOutOfRangeException">When generation is less than 1 or greater than 6</exception>
        public ushort CalculateStat(byte baseStat, byte level, ushort ev, byte iv, byte generation, double natureMod)
        {
            switch (generation)
            {
                case 1:
                case 2:
                    return CalculateStatFirstAndSecondGen(baseStat, level, ev, iv);
                case 3:
                case 4:
                case 5:
                case 6:
                    return CalculateStat(baseStat, level, (byte)ev, iv, natureMod);
            }
            throw new ArgumentOutOfRangeException(nameof(generation), generation, "Generation must be between 1 and 6");
        }

        /// <summary>
        /// Calculates Hit Points of a Pokémon for first and second generation
        /// </summary>
        /// <param name="baseHitPoints">The base Hit Points of the Pokémon</param>
        /// <param name="level">The current level of the Pokémon</param>
        /// <param name="ev">The amount of Hit Points EVs</param>
        /// <param name="dv">The Hit Points DV</param>
        /// <returns>The calculated Hit Points</returns>
        ushort CalculateHitPointsFirstAndSecondGen(byte baseHitPoints, byte level, ushort ev, byte dv)
        {
            double stat = CalculateFirstAndSecondGenStat(baseHitPoints, level, ev, dv);
            stat = stat + level + 10.0;
            return Convert.ToUInt16(Math.Floor(stat));
        }
        /// <summary>
        /// Calculates Hit Points of a Pokémon for any generation from third onwards
        /// </summary>
        /// <param name="baseHitPoints">The base Hit Points of the Pokémon</param>
        /// <param name="level">The current level of the Pokémon</param>
        /// <param name="ev">The amount of Hit Points EVs</param>
        /// <param name="iv">The Hit Points IV</param>
        /// <returns>The calculated Hit Points</returns>
        ushort CalculateHitPoints(byte baseHitPoints, byte level, ushort ev, byte iv)
        {
            if (iv > 31)
                throw new ArgumentOutOfRangeException(nameof(iv), iv, "IV can not be higher than 31");
            return (ushort)((iv + 2 * baseHitPoints + (int)ev / 4 + 100) * (int)level / 100 + 10);
        }
        /// <summary>
        /// Calculates a stat value for first and secind generation
        /// </summary>
        /// <param name="baseStat">The base stat value of the Pokémon</param>
        /// <param name="level">The current level of the Pokémon</param>
        /// <param name="ev">The amount of EV of the stat</param>
        /// <param name="dv">The DV of the stat</param>
        /// <returns>The calculated stat value</returns>
        ushort CalculateStatFirstAndSecondGen(byte baseStat, byte level, ushort ev, byte dv)
        {
            double stat = CalculateFirstAndSecondGenStat(baseStat, level, ev, dv);
            stat = stat + 5.0;
            return Convert.ToUInt16(Math.Floor(stat));
        }
        /// <summary>
        /// Calculates a stat value for any generation from three onwards
        /// </summary>
        /// <param name="baseStat">The base stat value of the Pokémon</param>
        /// <param name="level">The current level of the Pokémon</param>
        /// <param name="ev">The amount of EV of the stat</param>
        /// <param name="iv">The IV of the stat</param>
        /// <param name="natureMod">The modifier for nature</param>
        /// <returns>The calculated stat value</returns>
        ushort CalculateStat(byte baseStat, byte level, byte ev, byte iv, double natureMod)
        {
            if (iv > 31)
                throw new ArgumentOutOfRangeException(nameof(iv), iv, "IV can not be higher than 31");

            ushort result = (ushort)(((iv + 2 * baseStat + ev / 4) * level / 100 + 5) * natureMod);

            //if (natureMod == 0.9)
            //{
            //    result *= 9;
            //    result /= 10;
            //}
            //if (natureMod == 1.1)
            //{
            //    result *= 11;
            //    result /= 10;
            //}

            return result;
            //double stat = CalculateLaterGenStat(baseStat, level, ev, iv);
            //stat = (stat + 5.0) * natureMod;

            //return Convert.ToUInt16(Math.Floor(stat));
        }
        /// <summary>
        /// Performs base calculation for generation &lt; 3
        /// </summary>
        /// <param name="baseStat">The base stat value of the Pokémon</param>
        /// <param name="level">The current level of the Pokémon</param>
        /// <param name="ev">The amount of EV of the stat</param>
        /// <param name="dv">The DV of the stat</param>
        /// <returns>The calculated stat value</returns>
        /// <exception cref="ArgumentOutOfRangeException">When DV is greater than 15</exception>
        double CalculateFirstAndSecondGenStat(byte baseStat, byte level, ushort ev, byte dv)
        {
            if (dv > 15)
                throw new ArgumentOutOfRangeException(nameof(dv), dv, "DV for 1st and 2nd generation can not be higher than 15");

            double a = 0;
            if (ev > 1)
                a = Math.Sqrt(ev - 1.0);
            int b = Convert.ToInt32(Math.Floor((a + 1.0) / 4.0));
            return ((((baseStat + dv) * 2.0 + b) * level) / 100.0);
        }
        /// <summary>
        /// Performs base calculation for generation > 2
        /// </summary>
        /// <param name="baseStat">The base stat value of the Pokémon</param>
        /// <param name="level">The current level of the Pokémon</param>
        /// <param name="ev">The amount of EV of the stat</param>
        /// <param name="iv">The IV of the stat</param>
        /// <returns>The calculated stat value</returns>
        /// <exception cref="ArgumentOutOfRangeException">When IV is greater than 31</exception>
        double CalculateLaterGenStat(byte baseStat, byte level, ushort ev, byte iv)
        {
            if (iv > 31)
                throw new ArgumentOutOfRangeException(nameof(iv), iv, "IV can not be higher than 31");

            double a = (baseStat * 2.0) + iv + Math.Floor((ev / 4.0));
            return ((a * level) / 100.0);
        }
    }
}
