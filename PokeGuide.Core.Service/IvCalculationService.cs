using System.Collections.Generic;

namespace PokeGuide.Core.Calculations
{
    /// <summary>
    /// Calculator for individual and effort values
    /// </summary>
    public class IvCalculationService : IIvCalculationService
    {
        IStatCalculationService _statService;

        /// <summary>
        /// Initializes a new instance if the <see cref="IvCalculationService"/> class
        /// </summary>
        /// <param name="service">An instance of a StatCalculationService</param>
        public IvCalculationService(IStatCalculationService service)
        {
            _statService = service;
        }

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
        /// <param name="isHp"><c>True</c> when calculating Hit Points IV</param>
        /// <returns>A list of possible IVs</returns>
        public List<byte> CalculateIv(byte baseStat, ushort stat, byte level, ushort statEv, List<byte> knownIvs, byte generation, double nature = 1.0, bool isHp = false)
        {
            var possibleIvs = new List<byte>();
            foreach (byte iv in knownIvs)
            {
                int statValue = _statService.CalculateStat(baseStat, level, statEv, iv, generation, nature, isHp);
                if (statValue == stat)
                    possibleIvs.Add(iv);
            }

            return possibleIvs;
        }
    }
}
