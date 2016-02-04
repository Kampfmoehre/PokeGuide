using System;
using System.Collections.Generic;
using System.Linq;

using PokeGuide.Core.Model;
using PokeGuide.Core.Service.Interface;

namespace PokeGuide.Core.Service
{
    public class CalculationHelperService : ICalculationHelperService
    {
        readonly ICalculationService _calculator;

        public CalculationHelperService(ICalculationService calculator)
        {
            _calculator = calculator;
        }

        public IList<Fighter> CalculateBattleResult(byte generation, FightInformation fightInfo)
        {
            byte participated = Convert.ToByte(fightInfo.Team.Count(c => c.HasParticipated));
            if (participated == 0)
                return fightInfo.Team;

            byte teamCount = 0;            
            if (fightInfo.ExpAllActive && generation == 1)
                teamCount = Convert.ToByte(fightInfo.Team.Count);
            
            foreach (Fighter fighter in fightInfo.Team)
            {
                bool expShare = generation == 1 ? fightInfo.ExpAllActive : fightInfo.Team.Any(a => a.HoldsExpShare);

                if (fighter.HasParticipated)
                    fighter.EarnedExperience = _calculator.CalculateExperience(generation, fightInfo.EnemyBaseExperience, fightInfo.EnemyLevel, participated, fightInfo.EnemyIsWild, fighter.IsTraded, fighter.HoldsLuckyEgg, expShare, 0);

                // if the active fighter also holds Exp.Share he will be given both direct exp from fight and Exp.Share output
                if (fighter.HoldsExpShare && generation > 1)
                    fighter.EarnedExperience += _calculator.CalculateExperience(generation, fightInfo.EnemyBaseExperience, fightInfo.EnemyLevel, 1, fightInfo.EnemyIsWild, fighter.IsTraded, fighter.HoldsLuckyEgg, expShare, 0);

                // In first gen with activated Exp.All you get half the Exp + the Exp.All output so we calculate both and sum them
                if (teamCount > 0 && generation == 1)
                    fighter.EarnedExperience += _calculator.CalculateExperience(generation, fightInfo.EnemyBaseExperience, fightInfo.EnemyLevel, participated, fightInfo.EnemyIsWild, fighter.IsTraded, fighter.HoldsLuckyEgg, fightInfo.ExpAllActive, teamCount);
            }

            return fightInfo.Team;
        }
    }
}
