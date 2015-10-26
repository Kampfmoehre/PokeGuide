using System.Threading;
using System.Threading.Tasks;
using PokeGuide.Model;

namespace PokeGuide.Service.Interface
{
    public interface IBaseDataService
    {
        Task<ElementType> LoadTypeAsync(int id, GameVersion version, int displayLanguage, CancellationToken token);
    }
}
