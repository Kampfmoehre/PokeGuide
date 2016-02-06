using System;
using System.Collections.Generic;
using System.Linq;
using PokeGuide.Core.Enum;
using PokeGuide.Core.Model;
using PokeGuide.Core.Service.Interface;

namespace PokeGuide.Core.Service
{
    public class CalculationHelperService : ICalculationHelperService
    {
        readonly IExperienceCalculationService _experienceCalculator;

        public CalculationHelperService(IExperienceCalculationService experienceCalculator)
        {
            _experienceCalculator = experienceCalculator;
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
                        fighter.EarnedExperience = _experienceCalculator.CalculateExperienceForFirstGen(fightInfo.EnemyBaseExperience, fightInfo.EnemyLevel, participated, fightInfo.EnemyIsWild, fighter.TradeState == TradeState.TradedNational, fightInfo.ExpAllActive, 0);
                    else
                        fighter.EarnedExperience = CalculateExperienceYield(generation, fightInfo.EnemyBaseExperience, fightInfo.EnemyLevel, fighter.Level, participated, fightInfo.EnemyIsWild, fighter.TradeState, fighter.HoldsLuckyEgg, expShareCount, false, fightInfo.ExpPowerState);
                }
                
                // if the active fighter also holds Exp.Share he will be given both direct exp from fight and Exp.Share output, so we sum them
                if (fighter.HoldsExpShare && generation > 1)
                    fighter.EarnedExperience += CalculateExperienceYield(generation, fightInfo.EnemyBaseExperience, fightInfo.EnemyLevel, fighter.Level, 1, fightInfo.EnemyIsWild, fighter.TradeState, fighter.HoldsLuckyEgg, expShareCount, true, fightInfo.ExpPowerState);

                // In first gen with activated Exp.All you get half the Exp + the Exp.All output so we calculate both and sum them
                if (teamCount > 0 && generation == 1)
                    fighter.EarnedExperience += _experienceCalculator.CalculateExperienceForFirstGen(fightInfo.EnemyBaseExperience, fightInfo.EnemyLevel, participated, fightInfo.EnemyIsWild, fighter.TradeState == TradeState.TradedNational, fightInfo.ExpAllActive, teamCount);
            }

            return fightInfo.Team;
        }

        int CalculateExperienceYield(int generation, ushort baseExperience, byte enemyLevel, byte ownLevel, byte participated, bool isWild, TradeState tradeState, bool holdsLuckyEgg, byte expShareCount, bool holdsExpShare, ExpPower expPower)
        {
            bool isTraded = tradeState == TradeState.TradedNational;
            switch (generation)
            {
                case 2:
                    return _experienceCalculator.CalculateExperienceForSecondGen(baseExperience, enemyLevel, participated, isWild, isTraded, holdsLuckyEgg, expShareCount, holdsExpShare);
                case 3:
                    return _experienceCalculator.CalculateExperienceForThirdGen(baseExperience, enemyLevel, participated, isWild, isTraded, holdsLuckyEgg, expShareCount, holdsExpShare);
                case 4:
                    return _experienceCalculator.CalculateExperienceForFourthGen(baseExperience, enemyLevel, participated, isWild, tradeState, holdsLuckyEgg, expShareCount, holdsExpShare);
                case 5:
                    return _experienceCalculator.CalculateExperienceForFifthGen(baseExperience, enemyLevel, ownLevel, participated, isWild, tradeState, holdsLuckyEgg, expShareCount, holdsExpShare, expPower);
            }

            return 0;
        }
    }
}
