using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using PokeGuide.Model;

namespace PokeGuide.Service.Interface
{
    public interface ITestService : IBaseDataService
    {
        Task<IEnumerable<Model.Db.GameVersion>> LoadVersionsNewAsync(int language, CancellationToken token);
        Task<IEnumerable<GameVersion>> LoadVersionsOldAsync(int language, CancellationToken token);
        Task<IEnumerable<Model.Db.Ability>> LoadAbilitiesNewAsync(int generation, int language, CancellationToken token);
        Task<IEnumerable<Ability>> LoadAbilitiesOldAsync(int generation, int language, CancellationToken token);
        Task<Species> LoadSpeciesByIdAsync(int id, int versionGroupId, int language, CancellationToken token);
        Task<Species> LoadSpeciesByIdOldAsync(int id, int versionGroupid, int displayLanguage, CancellationToken token);
        Task<PokemonForm> LoadFormByIdAsync(int id, GameVersion version, int language, CancellationToken token);
        Task<PokemonForm> LoadFormByIdOldAsync(int formId, GameVersion version, int displayLanguage, CancellationToken token);
        Task<List<Ability>> LoadAbilitiesAsync(int displayLanguage, CancellationToken token);
    }
}
