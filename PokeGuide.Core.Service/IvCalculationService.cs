using System;
using System.Collections.Generic;

namespace PokeGuide.Core.Calculations
{
    public class IvCalculationService : IIvCalculationService
    {
        IStatCalculationService _statService;

        public IvCalculationService(IStatCalculationService service)
        {
            _statService = service;
        }

        public List<byte> CalculateHitPointsIv(byte baseHp, ushort hp, byte level, ushort ev, List<byte> knownIvs, byte generation)
        {
            switch (generation)
            {
                case 1:
                case 2:
                    return CalculateHitPointsIvForFirstAndSecondGen(baseHp, hp, level, ev, knownIvs, generation);
            }
            throw new ArgumentOutOfRangeException(nameof(generation), generation, "Generation must be between 1 and 6");
        }

        List<byte> CalculateHitPointsIvForFirstAndSecondGen(byte baseHp, ushort hp, byte level, ushort ev, List<byte> knownIvs, byte generation)
        {
            var possibleIvs = new List<byte>();
            foreach (byte iv in knownIvs)
            {
                int stat = _statService.CalculateHitPoints(baseHp, level, ev, iv, generation);
                if (stat == hp)
                    possibleIvs.Add(iv);
            }

            return possibleIvs;
        }
    }
}
