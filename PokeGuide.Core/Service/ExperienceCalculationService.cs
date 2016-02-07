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
            double a = (enemyLevel * 2.0) + 10;
            double c = enemyLevel + ownLevel + 10.0;
            double enemyCount = CalculateEnemyCount(participatedPokemon, expShareCount, holdsExpShare);
            
            double b = (baseExperience * enemyLevel) / (5.0 * enemyCount);
            // Trainer Bonus must be applied here
            if (!isWild)
                b = b * 1.5;

            // Adapted formula from http://www.serebii.net/games/exp.shtml
            double experience = Math.Floor(Math.Floor(Math.Sqrt(a) * (a * a)) * Math.Floor(b) / Math.Floor(Math.Sqrt(c) * (c * c))) + 1.0;
            
            // Apply all other experience bonuses
            experience = ApplyExperienceBonus(experience, holdsLuckyEgg, tradingState, expPower);

            return Convert.ToInt32(Math.Floor(experience));
        }
        /// <summary>
        /// Calculates experience points gained from a battle in sixth generation (XY, OrAs, ...)
        /// </summary>
        /// <param name="baseExperience">The base experience of the defeated Pokémon</param>
        /// <param name="enemyLevel">The level of the defeated Pokémon</param>
        /// <param name="isWild"><c>True</c> if the enemy is a wild Pokémon</param>
        /// <param name="tradingState">The state of the Pokémon (Owned by Player, traded, traded from another country)</param>
        /// <param name="holdsLuckyEgg"><c>True</c> if the Pokémon holds a Lucky Egg</param>
        /// <param name="expPower">The state of Pass Power (5th Gen C-Gear feature)</param>
        /// <param name="couldEvolve"><c>True</c> if the Pokémon is at a level where it could have evolved but didn't</param>
        /// <param name="hasAffection"><c>True</c> when the Pokémon has 2 or more hearts in PokémonAmie</param>
        /// <param name="expShareActive"><c>True</c> when Exp.Share is active</param>
        /// <param name="isActive"><c>False</c> when the Pokémon was not in the battle and Exp.Share is active, else <c>true</c></param>
        /// <returns>The calculated experience points for the Pokémon</returns>
        public int CalculateExperienceForSixthGen(ushort baseExperience, byte enemyLevel, bool isWild, TradeState tradingState, bool holdsLuckyEgg, ExpPower expPower, bool hasAffection, bool couldEvolve, bool expShareActive, bool isActive)
        {
            double a = isWild ? 1.0 : 1.5;
            double b = holdsLuckyEgg ? 1.5 : 1.0;
            double c = hasAffection ? 1.2 : 1.0;
            double d = ConvertExpPowerToDouble(expPower);
            double e = couldEvolve ? 1.2 : 1.0;
            double f = 1.0;
            if (tradingState == TradeState.TradedNational)
                f = 1.5;
            else if (tradingState == TradeState.TradedInternational)
                f = 1.7;
            double g = isActive ? 1.0 : 2.0;

            //double h = (baseExperience * enemyLevel * a * b * c * d * e * f) / (7.0 * g);
            //return Convert.ToInt32(Math.Round(h, MidpointRounding.AwayFromZero));

            //double decimalPart = experience - Math.Truncate(experience);
            //if (decimalPart == 0.5)
            //    return Convert.ToInt32(Math.Floor(experience));

            //return Convert.ToInt32(Math.Floor(h));
            //return Convert.ToInt32(Math.Round(h, MidpointRounding.AwayFromZero));

            double enemyCount = isActive ? 1.0 : 2.0;

            double experience = Math.Floor((baseExperience * enemyLevel) / (enemyCount * 7.0));
            if (tradingState != TradeState.Original)
                experience = Math.Floor(experience * f);
            if (holdsLuckyEgg)
                experience = Math.Floor(experience * b);
            if (!isWild)
                experience = Math.Floor(experience * a);
            if (d != 1.0)
                experience = Math.Floor(experience * d);
            if (hasAffection)
                experience = Math.Floor(experience * c);
            if (couldEvolve)
                experience = Math.Floor(experience * e);
            //experience = ApplyExperienceBonus(experience, isWild, holdsLuckyEgg, tradingState, expPower, hasAffection, couldEvolve);
            //if (!isWild)
            //    experience = Math.Floor(experience + (experience * 0.5));
            //double multi = f * b * c * e * d;
            //experience = experience * multi;
            //if (tradingState == TradeState.TradedNational)
            //    experience = Math.Floor(experience * 1.5);
            //else if (tradingState == TradeState.TradedInternational)
            //    experience = experience * 1.7;
            //experience = experience * b;
            //experience = experience * c;
            //experience = experience * e;
            //experience = experience * d;

            return Convert.ToInt32(Math.Floor(experience));
            //double decimalPart = experience - Math.Truncate(experience);
            //if (decimalPart == 0.5)
            //    return Convert.ToInt32(Math.Floor(experience));
            //return Convert.ToInt32(Math.Round(experience, MidpointRounding.AwayFromZero));
        }

        double CheckRoundDown(double a)
        {
            double decimalPart = a - Math.Truncate(a);
            if (Math.Round(decimalPart, 1, MidpointRounding.AwayFromZero) == 0.5)
                return Convert.ToInt32(Math.Floor(a));
            return a;
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
                count = count * 2.0 * expShareCount;
            else if (expShareCount > 1.0)
                count = count * expShareCount;

            return count;
        }
        /// <summary>
        /// Takes calculated experience and applies bonus multiplier for some conditions
        /// </summary>
        /// <param name="experience">The experience</param>
        /// <param name="isWild"><c>True</c> if the enemy is a wild Pokémon</param>
        /// <param name="holdsLuckyEgg"><c>True</c> if the Pokémon holds a Lucky Egg</param>
        /// <param name="tradingState">The trading state of the Pokémon</param>
        /// <returns>The experience with applied bonus</returns>
        double ApplyExperienceBonus(double experience, bool isWild, bool holdsLuckyEgg, TradeState tradingState)
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
            return experience;
        }
        /// <summary>
        /// Takes calculated experience and applies bonus multiplier for some conditions
        /// </summary>
        /// <param name="experience">The experience</param>
        /// <param name="holdsLuckyEgg"><c>True</c> if the Pokémon holds a Lucky Egg</param>
        /// <param name="tradingState">The trading state of the Pokémon</param>
        /// <param name="expPower">The state of Pass Power or O-Power</param>
        /// <returns>The experience with applied bonus</returns>
        double ApplyExperienceBonus(double experience, bool holdsLuckyEgg, TradeState tradingState, ExpPower expPower)
        {
            if (holdsLuckyEgg) // Modifier for lucky egg
                experience = experience * 1.5;

            // Modifier for traded Pokémon
            if (tradingState == TradeState.TradedNational)
                experience = Math.Floor(experience * 1.5);
            else if (tradingState == TradeState.TradedInternational)
                experience = Math.Floor(experience * 1.7);

            if (expPower != ExpPower.None)
            {
                // Modifier for Exp Point Power
                double expMod = ConvertExpPowerToDouble(expPower);
                experience = Math.Floor(experience * expMod);
            }

            return experience;
        }
        /// <summary>
        /// Takes calculated experience and applies bonus multiplier for some conditions
        /// </summary>
        /// <param name="experience">The experience</param>
        /// <param name="isWild"><c>True</c> if the enemy is a wild Pokémon</param>
        /// <param name="holdsLuckyEgg"><c>True</c> if the Pokémon holds a Lucky Egg</param>
        /// <param name="tradingState">The trading state of the Pokémon</param>
        /// <param name="expPower">The state of Pass Power or O-Power</param>
        /// <param name="hasAffection"><c>True</c> if the Pokémon has 2 or more hearts in Pokémon-Amie</param>
        /// <param name="couldEvolve"><c>True</c> if the Pokémon is at a level where it could have evolved but didn't</param>
        /// <returns>The experience with applied bonus</returns>
        double ApplyExperienceBonus(double experience, bool isWild, bool holdsLuckyEgg, TradeState tradingState, ExpPower expPower, bool hasAffection, bool couldEvolve)
        {
            experience = ApplyExperienceBonus(experience, holdsLuckyEgg, tradingState, expPower);
            //experience = Math.Round(experience, MidpointRounding.AwayFromZero);

            if (!isWild) // Modifier for Trainer
                experience = experience * 1.5;

            if (hasAffection) // Modifier for Pokémon-Amie
                experience = experience * 1.2;

            if (couldEvolve) // Modifier for Pokémon that could have evolved
                experience = experience * 1.2;

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
