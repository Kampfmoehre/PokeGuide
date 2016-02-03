namespace PokeGuide.Core.Service.Interface
{
    public interface ICalculationService
    {
        ushort CalculateHitPoints(byte baseHitPoints, byte level, ushort ev, byte hpIv, byte generation);
        ushort CalculateStat(byte baseStat, byte level, ushort ev, byte iv, byte generation, double natureMod);
        int CalculateExperience(byte generation, ushort baseExperience, byte enemyLevel, byte participatedPokemon, bool isWild = false, bool isTraded = false, bool holdsLuckyEgg = false, bool hasExpAll = false, byte teamCount = 0);
    }
}
