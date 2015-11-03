using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using PokeGuide.Model;

namespace PokeGuide.Service.Interface
{
    public interface ITestService
    {
        Task<IEnumerable<Model.Db.GameVersion>> LoadVersionsNewAsync(int language, CancellationToken token);
        Task<IEnumerable<GameVersion>> LoadVersionsOldAsync(int language, CancellationToken token);
        Task<IEnumerable<Model.Db.Ability>> LoadAbilitiesNewAsync(int generation, int language, CancellationToken token);
        Task<IEnumerable<Ability>> LoadAbilitiesOldAsync(int generation, int language, CancellationToken token);
    }
}
