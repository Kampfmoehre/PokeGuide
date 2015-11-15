using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;
using PokeGuide.Model;

namespace PokeGuide.Service.Interface
{
    public interface IStaticDataService : IDataService
    {
        Task<ObservableCollection<Language>> LoadLanguagesAsync(int displayLanguage, CancellationToken token);
        Task<ObservableCollection<GameVersion>> LoadVersionsAsync(int displayLanguage, CancellationToken token);
        Task<GameVersion> LoadVersionAsync(int id, int displayLanguage, CancellationToken token);
        Task<ObservableCollection<Ability>> LoadAbilitiesAsync(int displayLanguage, CancellationToken token);
        //Task<IEnumerable<ElementType>> LoadTypesAsync(int displayLanguage, CancellationToken token);
        //Task<IEnumerable<Ability>> LoadAbilitiesAsync(int displayLanguage, CancellationToken token);
    }
}
