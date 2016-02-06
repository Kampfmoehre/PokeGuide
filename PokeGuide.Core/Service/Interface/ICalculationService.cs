namespace PokeGuide.Core.Service.Interface
{
    public interface ICalculationService
    {
        ushort CalculateHitPoints(byte baseHitPoints, byte level, ushort ev, byte hpIv, byte generation);
        ushort CalculateStat(byte baseStat, byte level, ushort ev, byte iv, byte generation, double natureMod);
    }
}
