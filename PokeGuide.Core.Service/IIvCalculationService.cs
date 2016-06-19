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
        /// <param name="knownIvs">The know Hit Points IVs of the Pokémon</param>
        /// <param name="generation">The generation for which the IV is calculated</param>
        /// <returns>A list of possible IVs</returns>
        List<byte> CalculateHitPointsIv(byte baseHp, ushort hp, byte level, ushort ev, List<byte> knownIvs, byte generation);
    }
}
