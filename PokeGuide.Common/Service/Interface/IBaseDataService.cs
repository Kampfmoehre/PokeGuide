using System.Threading;
using System.Threading.Tasks;
using PokeGuide.Model;

namespace PokeGuide.Service.Interface
{
    public interface IBaseDataService
    {
        Task<ElementType> GetTypeAsync(int id, GameVersion version);
        Task<GrowthRate> GetGrowthRateAsync(int id);
        Task<DamageClass> GetDamageClassAsync(int id);
        Task<EncounterMethod> GetEncounterMethodAsync(int id);
        Task<EncounterCondition> GetEncounterConditionAsync(int id);
        Task<EggGroup> GetEggGroupAsync(int id);
        void InitializeResources(int displayLanguage, CancellationToken token);
    }
}
