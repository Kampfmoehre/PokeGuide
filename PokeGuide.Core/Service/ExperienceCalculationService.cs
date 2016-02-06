using System;

using PokeGuide.Core.Enum;
using PokeGuide.Core.Service.Interface;

namespace PokeGuide.Core.Service
{
    /// <summary>
    /// Calculator for all experience calculations
    /// </summary>
    public class ExperienceCalculationService : IExperienceCalculationService
    {
        /// <summary>
        /// Calculates experience points gained from a battle in first generation (Green, Red, Blue, Yellow)
        /// </summary>
        /// <param name="baseExperience">The base experience of the defeated Pokémon</param>
        /// <param name="enemyLevel">The level of the defeated Pokémon</param>
        /// <param name="participatedPokemon">The number of Pokémon that participated in the battle and did not fainted</param>
        /// <param name="isWild"><c>True</c> if the enemy is a wild Pokémon</param>
        /// <param name="isTraded"><c>True</c> if the Trainer ID of the Pokémon for which the experience is calculated does not match the Trainer ID of the player</param>
        /// <param name="useExpAll"><c>True</c> if the player has Exp.All in the bag</param>
        /// <param name="teamCount">The number of Pokémon that are in the team (relevant if Exp.All is active)</param>
        /// <returns>The calculated experience points for the Pokémon</returns>
        public int CalculateExperienceForFirstGen(ushort baseExperience, byte enemyLevel, byte participatedPokemon, bool isWild, bool isTraded, bool useExpAll, byte teamCount)
        {
            double experience = participatedPokemon;
            if (useExpAll) // Only half experience when Exp.All is activated
                experience = experience * 2.0;

            // Every step must be rounded down
            experience = Math.Floor(baseExperience / experience);
            if (teamCount > 0) // When Exp.All is used, take the number of Pokémon in the team
                experience = Math.Floor(experience / teamCount);

            experience = Math.Floor(experience * enemyLevel);
            experience = Math.Floor(experience / 7.0);

            if (!isWild) // Modifier for Trainer Battle
                experience = Math.Floor(experience * 1.5);
            if (isTraded) // Modifier for traded Pokémon
                experience = Math.Floor(experience * 1.5);

            return Convert.ToInt32(experience);
        }
        /// <summary>
        /// Calculates experience points gained from a battle in second generation (Gold, Silver, Crystal)
        /// </summary>
        /// <param name="baseExperience">The base experience of the defeated Pokémon</param>
        /// <param name="enemyLevel">The level of the defeated Pokémon</param>
        /// <param name="participatedPokemon">The number of Pokémon that participated in the battle and did not fainted</param>
        /// <param name="isWild"><c>True</c> if the enemy is a wild Pokémon</param>
        /// <param name="isTraded"><c>True</c> if the Trainer ID of the Pokémon for which the experience is calculated does not match the Trainer ID of the player</param>
        /// <param name="holdsLuckyEgg"><c>True</c> if the Pokémon holds a Lucky Egg</param>
        /// <param name="expShareCount">The amount of Pokémon in the team that are holding an Exp.Share</param>
        /// <param name="holdsExpShare"><c>True</c> if the Pokémon holds an Exp.Share</param>
        /// <returns>The calculated experience points for the Pokémon</returns>
        public int CalculateExperienceForSecondGen(ushort baseExperience, byte enemyLevel, byte participatedPokemon, bool isWild, bool isTraded, bool holdsLuckyEgg, byte expShareCount, bool holdsExpShare)
        {
            double experience = CalculateEnemyCount(participatedPokemon, expShareCount, holdsExpShare);

            // Every step must be rounded down
            experience = Math.Floor(baseExperience / experience);
            experience = Math.Floor(experience * enemyLevel);
            experience = Math.Floor(experience / 7.0);
            TradeState tradeState = isTraded ? TradeState.TradedNational : TradeState.Original;
            experience = ApplyExperienceBonus(experience, isWild, holdsLuckyEgg, tradeState);

            return Convert.ToInt32(experience);
        }
        /// <summary>
        /// Calculates experience points gained from a battle in third generation (RSE, FrLg, ...)
        /// </summary>
        /// <param name="baseExperience">The base experience of the defeated Pokémon</param>
        /// <param name="enemyLevel">The level of the defeated Pokémon</param>
        /// <param name="participatedPokemon">The number of Pokémon that participated in the battle and did not fainted</param>
        /// <param name="isWild"><c>True</c> if the enemy is a wild Pokémon</param>
        /// <param name="isTraded"><c>True</c> if the Trainer ID of the Pokémon for which the experience is calculated does not match the Trainer ID of the player</param>
        /// <param name="holdsLuckyEgg"><c>True</c> if the Pokémon holds a Lucky Egg</param>
        /// <param name="expShareCount">The amount of Pokémon in the team that are holding an Exp.Share</param>
        /// <param name="holdsExpShare"><c>True</c> if the Pokémon holds an Exp.Share</param>
        /// <returns>The calculated experience points for the Pokémon</returns>
        public int CalculateExperienceForThirdGen(ushort baseExperience, byte enemyLevel, byte participatedPokemon, bool isWild, bool isTraded, bool holdsLuckyEgg, byte expShareCount, bool holdsExpShare)
        {
            double enemyCount = CalculateEnemyCount(participatedPokemon, expShareCount, holdsExpShare);

            // Unlike the first two generations these steps don't have to be rounded down
            double experience = (baseExperience * enemyLevel) / (enemyCount * 7.0);
            TradeState tradeState = isTraded ? TradeState.TradedNational : TradeState.Original;
            experience = ApplyExperienceBonus(experience, isWild, holdsLuckyEgg, tradeState);

            return Convert.ToInt32(Math.Floor(experience));
        }
        /// <summary>
        /// Calculates experience points gained from a battle in fourth generation (DPP, HgSs, ...)
        /// </summary>
        /// <param name="baseExperience">The base experience of the defeated Pokémon</param>
        /// <param name="enemyLevel">The level of the defeated Pokémon</param>
        /// <param name="participatedPokemon">The number of Pokémon that participated in the battle and did not fainted</param>
        /// <param name="isWild"><c>True</c> if the enemy is a wild Pokémon</param>
        /// <param name="tradingState">The state of the Pokémon (Owned by Player, traded, traded from another country)</param>
        /// <param name="holdsLuckyEgg"><c>True</c> if the Pokémon holds a Lucky Egg</param>
        /// <param name="expShareCount">The amount of Pokémon in the team that are holding an Exp.Share</param>
        /// <param name="holdsExpShare"><c>True</c> if the Pokémon holds an Exp.Share</param>
        /// <returns>The calculated experience points for the Pokémon</returns>
        public int CalculateExperienceForFourthGen(ushort baseExperience, byte enemyLevel, byte participatedPokemon, bool isWild, TradeState tradingState, bool holdsLuckyEgg, byte expShareCount, bool holdsExpShare)
        {
            double enemyCount = CalculateEnemyCount(participatedPokemon, expShareCount, holdsExpShare);

            // Unlike the first two generations these steps don't have to be rounded down
            double experience = (baseExperience * enemyLevel) / (enemyCount * 7.0);
            experience = ApplyExperienceBonus(experience, isWild, holdsLuckyEgg, tradingState);

            return Convert.ToInt32(Math.Floor(experience));
        }
        /// <summary>
        /// Calculates experience points gained from a battle in fifth generation (BW, B2W2, ...)
        /// </summary>
        /// <param name="baseExperience">The base experience of the defeated Pokémon</param>
        /// <param name="enemyLevel">The level of the defeated Pokémon</param>
        /// <param name="ownLevel">The level of the Pokémon for which the Exp are calculated</param>
        /// <param name="participatedPokemon">The number of Pokémon that participated in the battle and did not fainted</param>
        /// <param name="isWild"><c>True</c> if the enemy is a wild Pokémon</param>
        /// <param name="tradingState">The state of the Pokémon (Owned by Player, traded, traded from another country)</param>
        /// <param name="holdsLuckyEgg"><c>True</c> if the Pokémon holds a Lucky Egg</param>
        /// <param name="expShareCount">The amount of Pokémon in the team that are holding an Exp.Share</param>
        /// <param name="holdsExpShare"><c>True</c> if the Pokémon holds an Exp.Share</param>
        /// <param name="ExpPower">The state of Pass Power (5th Gen C-Gear feature)</param>
        /// <returns>The calculated experience points for the Pokémon</returns>
        public int CalculateExperienceForFifthGen(ushort baseExperience, byte enemyLevel, byte ownLevel, byte participatedPokemon, bool isWild, TradeState tradingState, bool holdsLuckyEgg, byte expShareCount, bool holdsExpShare, ExpPower expPower)
        {
            double enemyCount = CalculateEnemyCount(participatedPokemon, expShareCount, holdsExpShare);
            double a = baseExperience * enemyLevel;
            double b = 5.0 * enemyCount;
            double c = a / b;

            double d = Math.Pow((2.0 * enemyLevel) + 10.0, 2.5);
            double e = Math.Pow(enemyLevel + ownLevel + 10.0, 2.5);
            double f = d / e;

            double g = (c * f) + 1.0;

            double experience = ApplyExperienceBonus(g, isWild, holdsLuckyEgg, tradingState, expPower);

            return Convert.ToInt32(experience);
        }

        /// <summary>
        /// Calculates the divisor for experience calculations
        /// </summary>
        /// <param name="participatedPokemon">The number of Pokémon that participated in the battle and did not fainted</param>
        /// <param name="expShareCount">The number of Pokémon that are in the team and hold an Exp.Share</param>
        /// <param name="holdsExpShare"><c>True</c> if the Pokémon holds an Exp.Share</param>
        /// <returns>The divisor for experiene calculation</returns>
        double CalculateEnemyCount(byte participatedPokemon, byte expShareCount, bool holdsExpShare)
        {
            double count = participatedPokemon;
            if (expShareCount == 1.0 || holdsExpShare)
                count = count * 2.0;
            if (expShareCount > 1.0)
                count = count * 2.0;

            return count;
        }
        /// <summary>
        /// Takes calculated experience and applies bonus multiplier for some conditions
        /// </summary>
        /// <param name="experience">The experience</param>
        /// <param name="isWild"><c>True</c> if the Pokémon is a wild Pokémon</param>
        /// <param name="isTraded"><c>True</c> if the Pokémon has a different Trainer ID then the player</param>
        /// <param name="holdsLuckyEgg"><c>True</c> if the Pokémon holds a Lucky Egg</param>
        /// <returns>The experience with applied bonus</returns>
        double ApplyExperienceBonus(double experience, bool isWild, bool holdsLuckyEgg, TradeState tradingState, ExpPower expPower = ExpPower.None)
        {            
            // These must be rounded down for all generations
            if (!isWild) // Modifier for Trainer Battle
                experience = Math.Floor(experience * 1.5);
            if (holdsLuckyEgg) // Modifier for lucky egg
                experience = Math.Floor(experience * 1.5);

            // Modifier for traded Pokémon
            if (tradingState == TradeState.TradedNational)
                experience = Math.Floor(experience * 1.5);
            else if (tradingState == TradeState.TradedInternational)
                experience = Math.Floor(experience * 1.7);

            if (expPower != ExpPower.None)
            {
                // Modifier for Exp Point Power
                double expMod = ConvertExpPowerToDouble(expPower);
                experience = experience * expMod;
            }
            return experience;
        }
        /// <summary>
        /// Converts <see cref="PokeGuide.Core.Enum.ExpPower"/> to a double value
        /// </summary>
        /// <param name="expPower">The Exp Power</param>
        /// <returns>A value representing the power that can be used to modify experience yield</returns>
        double ConvertExpPowerToDouble(ExpPower expPower)
        {
            switch (expPower)
            {
                case ExpPower.NegativeStageThree:
                    return 0.5;
                case ExpPower.NegativeStageTwo:
                    return 0.66;
                case ExpPower.NegativeStageOne:
                    return 0.8;
                case ExpPower.PositiveStageOne:
                    return 1.2;
                case ExpPower.PositiveStageTwo:
                    return 1.5;
                case ExpPower.PositiveStageThree:
                    return 2.0;
            }
            return 1.0;
        }
    }
}
