using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using PokeGuide.Model;

namespace PokeGuide.Service
{
    public interface IPokemonService : IDataService
    {
        Task<IEnumerable<SpeciesName>> LoadAllSpeciesAsync(int displayLanguage, CancellationToken token);
        Task<Species> LoadSpeciesAsync(int id, GameVersion version, int displayLanguage, CancellationToken token);
        Task<GrowthRate> LoadGrowthRateAsync(int id, int displayLanguage, CancellationToken token);
        Task<PokedexEntry> LoadPokedexEntryAsync(int dexId, int speciesId, int displayLanguage, CancellationToken token);
        Task<IEnumerable<PokemonForm>> LoadFormsAsync(int speciesId, GameVersion version, int displayLanguage, CancellationToken token);
        Task<ElementType> LoadTypeAsync(int id, GameVersion version, int displayLanguage, CancellationToken token);
        Task<Ability> LoadAbilityAsync(int id, GameVersion version, int displayLanguage, CancellationToken token);
    }
}
