using System.Collections.Generic;

namespace PokeGuide.Core.Calculations
{
    /// <summary>
    /// Interface for a service that calculates Pokémon individual values
    /// </summary>
    public interface IIvCalculationService
    {
        /// <summary>
        /// Calculates the Hit Points individual value of a Pokémon
        /// </summary>
        /// <param name="baseHp">The base Hit Points of the Pokémon</param>
        /// <param name="hp">The current max Hit Points of the Pokémon</param>
        /// <param name="level">The level of the Pokémon</param>
        /// <param name="ev">The Hit Points effort values of the Pokémon</param>
        /// <param name="knownIvs">The possible Hit Point IVs of the Pokémon</param>
        /// <param name="generation">The generation for which the IV is calculated</param>
        /// <returns>A list of possible IVs</returns>
        List<byte> CalculateHitPointsIv(byte baseHp, ushort hp, byte level, ushort ev, List<byte> knownIvs, byte generation);
        /// <summary>
        /// Calculates the individual value of a Pokémon (except Hit Points)
        /// </summary>
        /// <param name="baseStat">The base value of the stat</param>
        /// <param name="stat">The actual value of the stat</param>
        /// <param name="level">The level of the Pokémon</param>
        /// <param name="statEv">The effort value for this stat</param>
        /// <param name="knownIvs">The possible IVs</param>
        /// <param name="generation">The generation for which the IV is calculated</param>
        /// <param name="nature">The nature modifier for the stat (0.9 lower, 1.1 higher, else 1.0)</param>
        /// <returns>A list of possible IVs</returns>
        List<byte> CalculateIv(byte baseStat, ushort stat, byte level, ushort statEv, List<byte> knownIvs, byte generation, double nature = 1.0);
    }
}
