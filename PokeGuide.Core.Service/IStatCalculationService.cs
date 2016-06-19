namespace PokeGuide.Core.Calculations
{
    /// <summary>
    /// Interface for a service that calculates stats and IVs
    /// </summary>
    public interface IStatCalculationService
    {
        /// <summary>
        /// Calculates Hit Points for a Pokémon
        /// </summary>
        /// <param name="baseHitPoints">The base Hit Points of the Pokémon species</param>
        /// <param name="level">The level of the Pokémon</param>
        /// <param name="ev">The Hit Points effort value of the Pokémon</param>
        /// <param name="hpIv">The Hit Points individual value of the Pokémon</param>
        /// <param name="generation">The game generation for which the calculation is done</param>
        /// <returns>The calculated Hit Points</returns>
        ushort CalculateHitPoints(byte baseHitPoints, byte level, ushort ev, byte hpIv, byte generation);
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
        ushort CalculateStat(byte baseStat, byte level, ushort ev, byte iv, byte generation, double natureMod);
    }
}
