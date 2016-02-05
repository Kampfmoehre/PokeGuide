namespace PokeGuide.Core.Service.Interface
{
    public interface ICalculationService
    {
        ushort CalculateHitPoints(byte baseHitPoints, byte level, ushort ev, byte hpIv, byte generation);
        ushort CalculateStat(byte baseStat, byte level, ushort ev, byte iv, byte generation, double natureMod);
        int CalculateExperienceForFirstGen(ushort baseExperience, byte enemyLevel, byte participatedPokemon, bool isWild, bool isTraded, bool useExpAll, byte teamCount);
        int CalculateExperience(byte generation, ushort baseExperience, byte enemyLevel, byte participatedPokemon, bool isWild = false, bool isTraded = false, bool holdsLuckyEgg = false, byte expShareCount = 0, bool holdsExpShare = false);
    }
}
