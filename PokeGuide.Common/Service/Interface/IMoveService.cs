using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;
using PokeGuide.Model;

namespace PokeGuide.Service.Interface
{
    public interface IMoveService
    {
        Task<Move> LoadMoveAsync(int id, GameVersion version, int displayLanguage, CancellationToken token);
        Task<DamageClass> LoadDamageClassAsync(int id, int displayLanguage, CancellationToken token);
        Task<ObservableCollection<PokemonMove>> LoadMoveSetAsync(int pokemonId, GameVersion version, int displayLanguage, CancellationToken token);
        Task<MoveLearnMethod> LoadMoveLearnMethodAsync(int id, int displayLanguage, CancellationToken token);
    }
}