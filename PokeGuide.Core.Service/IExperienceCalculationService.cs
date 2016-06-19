using PokeGuide.Core.Enum;

namespace PokeGuide.Core.Calculations
{
    /// <summary>
    /// Interface for an experience calculation service
    /// </summary>
    public interface IExperienceCalculationService
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
        int CalculateExperienceFirstGen(ushort baseExperience, byte enemyLevel, byte participated, bool isWild, bool isTraded, bool useExpAll, byte teamCount);
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
        int CalculateExperienceSecondGen(ushort baseExperience, byte enemyLevel, byte participated, bool isWild, bool isTraded, bool holdsLuckyEgg, byte expShareCount, bool holdsExpShare);
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
        int CalculateExperienceThirdGen(ushort baseExperience, byte enemyLevel, byte participated, bool isWild, bool isTraded, bool holdsLuckyEgg, byte expShareCount, bool holdsExpShare);
        /// <summary>
        /// Calulcates the amount of experience points that a battle in fourth generation Pokémon games will yield
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
        int CalculateExperienceFourthGen(ushort baseExperience, byte enemyLevel, byte participated, bool isWild, TradeState tradeState, bool holdsLuckyEgg, byte expShareCount, bool holdsExpShare);
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
        /// <param name="expPower">The ExpPower that was active during the battle (Pass Power from C-Gear)</param>
        /// <returns>The amount of Experience points for the defeated Pokémon</returns>
        int CalculateExperienceFifthGen(ushort baseExperience, byte enemyLevel, byte ownLevel, byte participated, bool isWild, TradeState tradeState, bool holdsLuckyEgg, byte expShareCount, bool holdsExpShare, ExpPower expPower);
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
        int CalculateExperienceSixthGen(ushort baseExperience, byte enemyLevel, bool isWild, TradeState tradeState, bool holdsLuckyEgg, ExpPower expPower, bool hasAffection, bool couldEvolve, bool expShareActive, bool isActive);
    }
}
