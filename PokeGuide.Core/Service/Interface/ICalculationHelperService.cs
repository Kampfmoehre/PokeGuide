using System.Collections.Generic;
using PokeGuide.Core.Model;

namespace PokeGuide.Core.Service.Interface
{
    public interface ICalculationHelperService
    {
        IList<Fighter> CalculateBattleResult(byte generation, FightInformation fightInfo);
    }
}
