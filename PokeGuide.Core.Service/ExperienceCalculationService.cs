using System;

using PokeGuide.Core.Enum;

namespace PokeGuide.Core.Calculations
{
    /// <summary>
    /// Calculator for all experience related calculations
    /// </summary>
    public class ExperienceCalculationService : IExperienceCalculationService
    {
        /// <summary>
        /// Calulcates the amount of experience points that a battle in first generation Pokémon games will yield
        /// </summary>
        /// <param name="baseExperience">The amount of base experience that the defeated enemy provides</param>
        /// <param name="enemyLevel">The level of the defeated enemy</param>
        /// <param name="participated">The count of non defeated Pokémon that participated in the fight</param>
        /// <param name="isWild"><c>True</c> when the enemy was a wild Pokémon</param>
        /// <param name="isTraded"><c>True</c> when the Pokémon for which the experience is calculated is a traded Pokémon</param>
        /// <param name="useExpAll"><c>True</c> when Exp.All. is active</param>
        /// <param name="teamCount">The number of Pokémon that the player has in the team</param>
        /// <returns>The amount of Experience points for the defeated Pokémon</returns>
        public int CalculateExperienceFirstGen(ushort baseExperience, byte enemyLevel, byte participated, bool isWild, bool isTraded, bool useExpAll, byte teamCount)
        {
            double experience = participated;
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
        /// Calulcates the amount of experience points that a battle in second generation Pokémon games will yield
        /// </summary>
        /// <param name="baseExperience">The amount of base experience that the defeated enemy provides</param>
        /// <param name="enemyLevel">The level of the defeated enemy</param>
        /// <param name="participated">The count of non defeated Pokémon that participated in the fight</param>
        /// <param name="isWild"><c>True</c> when the enemy was a wild Pokémon</param>
        /// <param name="isTraded"><c>True</c> when the Pokémon for which the experience is calculated is a traded Pokémon</param>
        /// <param name="holdsLuckyEgg"><c>True</c> when the Pokémon for which the experience is calculated holds a Lucky Egg</param>
        /// <param name="expShareCount">The number of Pokémon in the players team that holds an ExpShare</param>
        /// <param name="holdsExpShare"><c>True</c> when the Pokémon for which the experience is calculated holds an ExpShare</param>
        /// <returns>The amount of Experience points for the defeated Pokémon</returns>
        public int CalculateExperienceSecondGen(ushort baseExperience, byte enemyLevel, byte participated, bool isWild, bool isTraded, bool holdsLuckyEgg, byte expShareCount, bool holdsExpShare)
        {
            double experience = CalculateEnemyCount(participated, expShareCount, holdsExpShare);

            // Every step must be rounded down
            experience = Math.Floor(baseExperience / experience);
            experience = Math.Floor(experience * enemyLevel);
            experience = Math.Floor(experience / 7.0);
            TradeState tradeState = isTraded ? TradeState.TradedNational : TradeState.Original;
            experience = ApplyExperienceBonus(experience, isWild, holdsLuckyEgg, tradeState);

            return Convert.ToInt32(experience);
        }

        /// <summary>
        /// Calulcates the amount of experience points that a battle in third generation Pokémon games will yield
        /// </summary>
        /// <param name="baseExperience">The amount of base experience that the defeated enemy provides</param>
        /// <param name="enemyLevel">The level of the defeated enemy</param>
        /// <param name="participated">The count of non defeated Pokémon that participated in the fight</param>
        /// <param name="isWild"><c>True</c> when the enemy was a wild Pokémon</param>
        /// <param name="isTraded"><c>True</c> when the Pokémon for which the experience is calculated is a traded Pokémon</param>
        /// <param name="holdsLuckyEgg"><c>True</c> when the Pokémon for which the experience is calculated holds a Lucky Egg</param>
        /// <param name="expShareCount">The number of Pokémon in the players team that holds an ExpShare</param>
        /// <param name="holdsExpShare"><c>True</c> when the Pokémon for which the experience is calculated holds an ExpShare</param>
        /// <returns>The amount of Experience points for the defeated Pokémon</returns>
        public int CalculateExperienceThirdGen(ushort baseExperience, byte enemyLevel, byte participated, bool isWild, bool isTraded, bool holdsLuckyEgg, byte expShareCount, bool holdsExpShare)
        {
            double enemyCount = CalculateEnemyCount(participated, expShareCount, holdsExpShare);

            // Unlike the first two generations these steps don't have to be rounded down
            double experience = (baseExperience * enemyLevel) / (enemyCount * 7.0);
            TradeState tradeState = isTraded ? TradeState.TradedNational : TradeState.Original;
            experience = ApplyExperienceBonus(experience, isWild, holdsLuckyEgg, tradeState);

            return Convert.ToInt32(Math.Floor(experience));
        }

        /// <summary>
        /// alulcates the amount of experience points that a battle in fourth generation Pokémon games will yield
        /// </summary>
        /// <param name="baseExperience">The amount of base experience that the defeated enemy provides</param>
        /// <param name="enemyLevel">The level of the defeated enemy</param>
        /// <param name="participated">The count of non defeated Pokémon that participated in the fight</param>
        /// <param name="isWild"><c>True</c> when the enemy was a wild Pokémon</param>
        /// <param name="tradeState">The trade state of the Pokémon for which the experience is calculated</param>
        /// <param name="holdsLuckyEgg"><c>True</c> when the Pokémon for which the experience is calculated holds a Lucky Egg</param>
        /// <param name="expShareCount">The number of Pokémon in the players team that holds an ExpShare</param>
        /// <param name="holdsExpShare"><c>True</c> when the Pokémon for which the experience is calculated holds an ExpShare</param>
        /// <returns>The amount of Experience points for the defeated Pokémon</returns>
        public int CalculateExperienceFourthGen(ushort baseExperience, byte enemyLevel, byte participated, bool isWild, TradeState tradeState, bool holdsLuckyEgg, byte expShareCount, bool holdsExpShare)
        {
            double enemyCount = CalculateEnemyCount(participated, expShareCount, holdsExpShare);

            // Unlike the first two generations these steps don't have to be rounded down
            double experience = (baseExperience * enemyLevel) / (enemyCount * 7.0);
            experience = ApplyExperienceBonus(experience, isWild, holdsLuckyEgg, tradeState);

            return Convert.ToInt32(Math.Floor(experience));
        }

        /// <summary>
        /// Calulcates the amount of experience points that a battle in fifth generation Pokémon games will yield
        /// </summary>
        /// <param name="baseExperience">The amount of base experience that the defeated enemy provides</param>
        /// <param name="enemyLevel">The level of the defeated enemy</param>
        /// <param name="ownLevel">The level of the Pokémon for which the experience is calculated</param>
        /// <param name="participated">The count of non defeated Pokémon that participated in the fight</param>
        /// <param name="isWild"><c>True</c> when the enemy was a wild Pokémon</param>
        /// <param name="tradeState">The trade state of the Pokémon for which the experience is calculated</param>
        /// <param name="holdsLuckyEgg"><c>True</c> when the Pokémon for which the experience is calculated holds a Lucky Egg</param>
        /// <param name="expShareCount">The number of Pokémon in the players team that holds an ExpShare</param>
        /// <param name="holdsExpShare"><c>True</c> when the Pokémon for which the experience is calculated holds an ExpShare</param>
        /// <param name="expPower">The ExpPower that was active during the battle</param>
        /// <returns>The amount of Experience points for the defeated Pokémon</returns>
        public int CalculateExperienceFifthGen(ushort baseExperience, byte enemyLevel, byte ownLevel, byte participated, bool isWild, TradeState tradeState, bool holdsLuckyEgg, byte expShareCount, bool holdsExpShare, ExpPower expPower)
        {
            double a = (enemyLevel * 2.0) + 10;
            double c = enemyLevel + ownLevel + 10.0;
            double enemyCount = CalculateEnemyCount(participated, expShareCount, holdsExpShare);

            double b = (baseExperience * enemyLevel) / (5.0 * enemyCount);
            // Trainer Bonus must be applied here
            if (!isWild)
                b = b * 1.5;

            // Adapted formula from http://www.serebii.net/games/exp.shtml
            double experience = Math.Floor(Math.Floor(Math.Sqrt(a) * (a * a)) * Math.Floor(b) / Math.Floor(Math.Sqrt(c) * (c * c))) + 1.0;

            // Apply all other experience bonuses
            experience = ApplyExperienceBonus(experience, holdsLuckyEgg, tradeState, expPower);

            return Convert.ToInt32(Math.Floor(experience));
        }

        /// <summary>
        /// Calulcates the amount of experience points that a battle in sixth generation Pokémon games will yield
        /// </summary>
        /// <param name="baseExperience">The amount of base experience that the defeated enemy provides</param>
        /// <param name="enemyLevel">The level of the defeated enemy</param>
        /// <param name="isWild"><c>True</c> when the enemy was a wild Pokémon</param>
        /// <param name="tradeState">The trade state of the Pokémon for which the experience is calculated</param>
        /// <param name="holdsLuckyEgg"><c>True</c> when the Pokémon for which the experience is calculated holds a Lucky Egg</param>
        /// <param name="expPower">The ExpPower that was active during the battle (O-Power)</param>
        /// <param name="hasAffection"><c>True</c> when the Pokémon for which the experience is calculated has 2 or more hearts in PokémonAmie</param>
        /// <param name="couldEvolve"><c>True</c> if the Pokémon for which the experience is calculated is at a level where it could have evolved but didn't</param>
        /// <param name="expShareActive"><c>True</c> when Exp.Share is active</param>
        /// <param name="isActive"><c>False</c> when the Pokémon for which the experience is calculated was not in the battle and Exp.Share is active, else <c>true</c></param>
        /// <returns>The amount of Experience points for the defeated Pokémon</returns>
        public int CalculateExperienceSixthGen(ushort baseExperience, byte enemyLevel, bool isWild, TradeState tradeState, bool holdsLuckyEgg, ExpPower expPower, bool hasAffection, bool couldEvolve, bool expShareActive, bool isActive)
        {
            double a = isWild ? 1.0 : 1.5;
            double b = holdsLuckyEgg ? 1.5 : 1.0;
            double c = hasAffection ? 1.2 : 1.0;
            double d = ConvertExpPowerToDouble(expPower);
            double e = couldEvolve ? 1.2 : 1.0;
            double f = 1.0;
            if (tradeState == TradeState.TradedNational)
                f = 1.5;
            else if (tradeState == TradeState.TradedInternational)
                f = 1.7;
            double g = isActive ? 1.0 : 2.0;

            double enemyCount = isActive ? 1.0 : 2.0;

            double experience = Math.Floor((baseExperience * enemyLevel) / (enemyCount * 7.0));
            if (tradeState != TradeState.Original)
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
                experience = Math.Floor(experience * e);;

            return Convert.ToInt32(Math.Floor(experience));
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
