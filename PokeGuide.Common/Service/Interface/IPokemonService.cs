using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;
using PokeGuide.Model;

namespace PokeGuide.Service.Interface
{
    public interface IPokemonService : IDataService, IBaseDataService
    {
        Task<ObservableCollection<SpeciesName>> LoadAllSpeciesAsync(GameVersion version, int displayLanguage, CancellationToken token);
        Task<Species> LoadSpeciesAsync(int id, GameVersion version, int displayLanguage, CancellationToken token);
        //Task<GrowthRate> LoadGrowthRateAsync(int id, int displayLanguage, CancellationToken token);
        Task<PokedexEntry> LoadPokedexEntryAsync(int dexId, int speciesId, int displayLanguage, CancellationToken token);
        Task<ObservableCollection<PokemonForm>> LoadFormsAsync(SpeciesName speciesId, GameVersion version, int displayLanguage, CancellationToken token);
        Task<PokemonForm> LoadFormAsync(int formId, GameVersion version, int displayLanguage, CancellationToken token);
        Task<Ability> LoadAbilityAsync(int id, GameVersion version, int displayLanguage, CancellationToken token);
        Task<EggGroup> LoadEggGroupAsync(int id, int displayLanguage, CancellationToken token);
        Task<ObservableCollection<Stat>> LoadPokemonStatsAsync(int formId, GameVersion version, int displayLanguage, CancellationToken token);
        Task<ObservableCollection<PokemonEvolution>> LoadPossibleEvolutionsAsync(int speciesId, GameVersion version, int displayLanguage, CancellationToken token);
        Task<Item> LoadItemAsync(int id, int displayLanguage, CancellationToken token);

        Task<Move> LoadMoveAsync(int id, GameVersion version, int displayLanguage, CancellationToken token);
        //Task<DamageClass> LoadDamageClassAsync(int id, int displayLanguage, CancellationToken token);
        Task<ObservableCollection<PokemonMove>> LoadMoveSetAsync(int pokemonId, GameVersion version, int displayLanguage, CancellationToken token);
        Task<MoveLearnMethod> LoadMoveLearnMethodAsync(int id, int displayLanguage, CancellationToken token);
    }
}
