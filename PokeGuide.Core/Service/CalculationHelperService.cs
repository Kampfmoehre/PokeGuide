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
                throw new Exception("There can be no fight without participants");

            byte teamCount = 0;
            byte expShareCount = 0;

            if (generation == 1)
            {
                if (fightInfo.ExpAllActive)
                    teamCount = Convert.ToByte(fightInfo.Team.Count);
            }
            else
                expShareCount = Convert.ToByte(fightInfo.Team.Count(c => c.HoldsExpShare));
            
            // Loop through all team members
            foreach (Fighter fighter in fightInfo.Team)
            {
                if (fighter.HasParticipated)
                {
                    // Calculate experience that is earned directly through actively defeating the enemy
                    if (generation == 1)
                        fighter.EarnedExperience = _calculator.CalculateExperienceForFirstGen(fightInfo.EnemyBaseExperience, fightInfo.EnemyLevel, participated, fightInfo.EnemyIsWild, fighter.IsTraded, fightInfo.ExpAllActive, 0);
                    else
                        fighter.EarnedExperience = _calculator.CalculateExperience(generation, fightInfo.EnemyBaseExperience, fightInfo.EnemyLevel, participated, fightInfo.EnemyIsWild, fighter.IsTraded, fighter.HoldsLuckyEgg, expShareCount);
                }
                
                // if the active fighter also holds Exp.Share he will be given both direct exp from fight and Exp.Share output, so we sum them
                if (fighter.HoldsExpShare && generation > 1)
                    fighter.EarnedExperience += _calculator.CalculateExperience(generation, fightInfo.EnemyBaseExperience, fightInfo.EnemyLevel, 1, fightInfo.EnemyIsWild, fighter.IsTraded, fighter.HoldsLuckyEgg, expShareCount, true);

                // In first gen with activated Exp.All you get half the Exp + the Exp.All output so we calculate both and sum them
                if (teamCount > 0 && generation == 1)
                    fighter.EarnedExperience += _calculator.CalculateExperienceForFirstGen(fightInfo.EnemyBaseExperience, fightInfo.EnemyLevel, participated, fightInfo.EnemyIsWild, fighter.IsTraded, fightInfo.ExpAllActive, teamCount);
            }

            return fightInfo.Team;
        }
    }
}
