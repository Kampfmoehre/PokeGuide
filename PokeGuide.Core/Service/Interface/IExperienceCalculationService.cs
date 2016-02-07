using PokeGuide.Core.Enum;

namespace PokeGuide.Core.Service.Interface
{
    /// <summary>
    /// Interface for experience calculators
    /// </summary>
    public interface IExperienceCalculationService
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
        int CalculateExperienceForFirstGen(ushort baseExperience, byte enemyLevel, byte participatedPokemon, bool isWild, bool isTraded, bool useExpAll, byte teamCount);
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
        int CalculateExperienceForSecondGen(ushort baseExperience, byte enemyLevel, byte participatedPokemon, bool isWild, bool isTraded, bool holdsLuckyEgg, byte expShareCount, bool holdsExpShare);
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
        int CalculateExperienceForThirdGen(ushort baseExperience, byte enemyLevel, byte participatedPokemon, bool isWild, bool isTraded, bool holdsLuckyEgg, byte expShareCount, bool holdsExpShare);
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
        int CalculateExperienceForFourthGen(ushort baseExperience, byte enemyLevel, byte participatedPokemon, bool isWild, TradeState tradingState, bool holdsLuckyEgg, byte expShareCount, bool holdsExpShare);
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
        int CalculateExperienceForFifthGen(ushort baseExperience, byte enemyLevel, byte ownLevel, byte participatedPokemon, bool isWild, TradeState tradingState, bool holdsLuckyEgg, byte expShareCount, bool holdsExpShare, ExpPower expPower);
        /// <summary>
        /// Calculates experience points gained from a battle in sixth generation (XY, OrAs, ...)
        /// </summary>
        /// <param name="baseExperience">The base experience of the defeated Pokémon</param>
        /// <param name="enemyLevel">The level of the defeated Pokémon</param>
        /// <param name="isWild"><c>True</c> if the enemy is a wild Pokémon</param>
        /// <param name="tradingState">The state of the Pokémon (Owned by Player, traded, traded from another country)</param>
        /// <param name="holdsLuckyEgg"><c>True</c> if the Pokémon holds a Lucky Egg</param>
        /// <param name="expPower">The state of Pass Power (5th Gen C-Gear feature)</param>
        /// <param name="hasAffection"><c>True</c> when the Pokémon has 2 or more hearts in PokémonAmie</param>
        /// <param name="couldEvolve"><c>True</c> if the Pokémon is at a level where it could have evolved but didn't</param>
        /// <param name="expShareActive"><c>True</c> when Exp.Share is active</param>
        /// <param name="isActive"><c>False</c> when the Pokémon was not in the battle and Exp.Share is active, else <c>true</c></param>
        /// <returns>The calculated experience points for the Pokémon</returns>
        int CalculateExperienceForSixthGen(ushort baseExperience, byte enemyLevel, bool isWild, TradeState tradingState, bool holdsLuckyEgg, ExpPower expPower, bool hasAffection, bool couldEvolve, bool expShareActive, bool isActive);
    }
}
