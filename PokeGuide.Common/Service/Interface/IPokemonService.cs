using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;
using PokeGuide.Model;

namespace PokeGuide.Service.Interface
{
    public interface IPokemonService : IDataService
    {
        Task<ObservableCollection<SpeciesName>> LoadAllSpeciesAsync(GameVersion version, int displayLanguage, CancellationToken token);
        Task<Species> LoadSpeciesAsync(int id, GameVersion version, int displayLanguage, CancellationToken token);
        Task<GrowthRate> LoadGrowthRateAsync(int id, int displayLanguage, CancellationToken token);
        Task<PokedexEntry> LoadPokedexEntryAsync(int dexId, int speciesId, int displayLanguage, CancellationToken token);
        Task<ObservableCollection<PokemonForm>> LoadFormsAsync(int speciesId, GameVersion version, int displayLanguage, CancellationToken token);
        //Task<PokemonForm> LoadFormAsync(int formId, GameVersion version, int displayLanguage, CancellationToken token);        
        Task<Ability> LoadAbilityAsync(int id, GameVersion version, int displayLanguage, CancellationToken token);
        Task<EggGroup> LoadEggGroupAsync(int id, int displayLanguage, CancellationToken token);
        Task<ObservableCollection<Stat>> LoadPokemonStats(int formId, GameVersion version, int displayLanguage, CancellationToken token);
    }
}
