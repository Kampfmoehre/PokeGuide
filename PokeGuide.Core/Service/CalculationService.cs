using System;

using PokeGuide.Core.Service.Interface;

namespace PokeGuide.Core.Service
{
    public class CalculationService : ICalculationService
    {
        /// <summary>
        /// Calculates the Hit Points (HP) for a Pokémon
        /// </summary>
        /// <param name="baseHitPoints">The base HP of the Pokémon form</param>
        /// <param name="level">The level of the Pokémon</param>
        /// <param name="ev">The amount of EV (or stat exp for first and second generation)</param>
        /// <param name="hpIv">The Hit Point IV</param>
        /// <param name="generation">The game generation for which the calculation should be done</param>
        /// <returns>The calculated Hit Points</returns>
        /// <exception cref="ArgumentOutOfRangeException">When you specify a generation that is not supported</exception>
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
        /// Calculates a stat value of a Pokémon
        /// </summary>
        /// <param name="baseStat">The base stat of the Pokémon form</param>
        /// <param name="level">The level of the Pokémon</param>
        /// <param name="ev">The EV (or stat exp for first and second generation)</param>
        /// <param name="iv">The IV of the stat</param>
        /// <param name="generation">The generation, for which the calculation should be done</param>
        /// <param name="natureMod">The modifier for the nature, 0.9 if the nature is lowering the stat, 1.1 if the nature is raising the stat or 1.0 if the stat is unaffected by the nature</param>
        /// <returns>The calculated stat</returns>
        /// <exception cref="ArgumentOutOfRangeException">When you specify a generation that is not supported</exception>
        public ushort CalculateStat(byte baseStat, byte level, ushort ev, byte iv, byte generation, double natureMod = 1.0)
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
        /// <param name="iv">The Hit Points IV</param>
        /// <returns>The calculated Hit Points</returns>
        ushort CalculateHitPointsFirstAndSecondGen(byte baseHitPoints, byte level, ushort ev, byte iv)
        {
            int a = Convert.ToInt32(Math.Floor((Math.Sqrt(ev - 1.0) + 1.0) / 4.0));
            double b = ((((baseHitPoints + iv) * 2.0 + a) * level) / 100.0) + level + 10.0;
            return Convert.ToUInt16(Math.Floor(b));

            //double a = (baseHitPoints + hpIv) * 2.0;
            //double b = Math.Sqrt(ev) / 4.0;
            //double c = ((a + b) * level) / 100.0;
            //double d = c + level + 10.0;
            //return Convert.ToInt16(Math.Floor(d));

            //double effort = Math.Sqrt(ev) / 8.0;
            //double stat = (hpIv + baseHitPoints) + effort + 50.0;
            //stat = Math.Floor(stat * level);
            //stat = Math.Floor(stat / 50);
            //stat = Math.Floor(stat + 10);
            //return Convert.ToInt16(Math.Floor(stat));
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
            double a = (baseHitPoints * 2.0) + iv + (ev / 4.0);
            double b = ((a * level) / 100.0) + level + 10.0;
            
            return Convert.ToUInt16(Math.Floor(b));

            //double effort = Math.Floor(ev / 4.0);
            //double baseValue = Math.Floor(baseHitPoints * 2.0);
            //double individual = Math.Floor(baseValue + iv + effort + 100);
            //double stat = Math.Floor(individual * level);
            //stat = Math.Floor(stat / 100.0);
            //stat = Math.Floor(stat + 10.0);
            //return Convert.ToInt16(stat);
        }
        /// <summary>
        /// Calculates a stat value for first and secind generation
        /// </summary>
        /// <param name="baseStat">The base stat value of the Pokémon</param>
        /// <param name="level">The current level of the Pokémon</param>
        /// <param name="ev">The amount of EV of the stat</param>
        /// <param name="iv">The IV of the stat</param>
        /// <returns>The calculated stat value</returns>
        ushort CalculateStatFirstAndSecondGen(byte baseStat, byte level, ushort ev, byte iv)
        {
            int a = Convert.ToInt32(Math.Floor((Math.Sqrt(ev - 1.0) + 1.0) / 4.0));
            double b = ((((baseStat + iv) * 2.0 + a) * level) / 100.0) + 5.0;
            return Convert.ToUInt16(Math.Floor(b));

            //double effort = Math.Floor(Math.Sqrt(ev) / 8.0);
            //double stat = Math.Floor(iv + baseStat + effort);
            //stat = Math.Floor(stat * level);
            //stat = Math.Floor(stat / 50);
            //stat = Math.Floor(stat + 5);
            //return Convert.ToInt16(stat);
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
            double a = (2 * baseStat) + iv + (ev / 4.0);
            double b = (((a * level) / 100) + 5) * natureMod;

            return Convert.ToUInt16(Math.Floor(b));

            //double effort = Math.Floor(ev / 4.0);
            //double baseValue = Math.Floor(baseStat * 2.0);
            //double stat = Math.Floor(effort + iv + baseValue);
            //stat = Math.Floor(stat * level);
            //stat = Math.Floor(stat / 100);
            //stat = Math.Floor(stat + 5.0);
            //stat = Math.Floor(stat * natureMod);
            //return Convert.ToInt16(stat);
        }
    }
}
