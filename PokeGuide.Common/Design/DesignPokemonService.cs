using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;
using PokeGuide.Model;
using PokeGuide.Service.Interface;

namespace PokeGuide.Design
{
    public class DesignPokemonService : IPokemonService
    {
        public void Cleanup()
        { }

        public Task<GrowthRate> LoadGrowthRateAsync(int id, int displayLanguage, CancellationToken token)
        {
            var tcs = new TaskCompletionSource<GrowthRate>();
            tcs.SetResult(new GrowthRate
            {
                    Id = 1,
                    Name = "Langsam"
            });
            return tcs.Task;
        }

        public Task<PokedexEntry> LoadPokedexEntryAsync(int dexId, int speciesId, int displayLanguage, CancellationToken token)
        {
            var tcs = new TaskCompletionSource<PokedexEntry>();
            tcs.SetResult(new PokedexEntry
            {
                DexNumber = 6,
                Id = dexId,
                Name = "Neue Hoenn"
            });
            return tcs.Task;
        }

        public Task<ObservableCollection<SpeciesName>> LoadAllSpeciesAsync(GameVersion version, int displayLanguage, CancellationToken token)
        {
            var tcs = new TaskCompletionSource<ObservableCollection<SpeciesName>>();
            tcs.SetResult(new ObservableCollection<SpeciesName>
            {
                new SpeciesName { Id = 6, Name = "Glurak", Generation = 1 },
                new SpeciesName { Id = 7, Name = "Schiggy", Generation = 1 }
            });
            return tcs.Task;
        }

        public Task<Species> LoadSpeciesAsync(int id, GameVersion version, int displayLanguage, CancellationToken token)
        {
            var tcs = new TaskCompletionSource<Species>();
            tcs.SetResult(new Species
            {
                BaseHappiness = 70,
                CatchRate = 45,
                DexEntry = LoadPokedexEntryAsync(39, 6, 6, token).Result,
                EggGroup1 = LoadEggGroupAsync(1, 6, token).Result,
                GrowthRate = LoadGrowthRateAsync(1, displayLanguage, token).Result,
                HatchCounter = 20,
                Id = 6,
                Name = "Glurak"
            });
            return tcs.Task;
        }

        public Task<ObservableCollection<PokemonForm>> LoadFormsAsync(int speciesId, GameVersion version, int displayLanguage, CancellationToken token)
        {
            var tcs = new TaskCompletionSource<ObservableCollection<PokemonForm>>();
            tcs.SetResult(new ObservableCollection<PokemonForm> {
                new PokemonForm
                {
                    BaseExperience = 255,
                    Height = 12,
                    Id = 12,
                    Name = "Mega Glurak X",
                    Species = LoadSpeciesAsync(speciesId, version, displayLanguage, token).Result,
                    Weight = 100
                }
            });
            return tcs.Task;
        }

        public Task<ElementType> LoadTypeAsync(int id, GameVersion version, int displayLanguage, CancellationToken token)
        {
            var tcs = new TaskCompletionSource<ElementType>();
            tcs.SetResult(new ElementType { Id = 12, Name = "Feuer" });
            return tcs.Task;
        }

        public Task<Ability> LoadAbilityAsync(int id, GameVersion version, int displayLanguage, CancellationToken token)
        {
            var tcs = new TaskCompletionSource<Ability>();
            tcs.SetResult(new Ability { Description = "Awesome Description", Effect = "Strange effect", FlavorText = "Fancy ingame text", Id = 12, Name = "Feuer" });
            return tcs.Task;
        }

        public Task<EggGroup> LoadEggGroupAsync(int id, int displayLanguage, CancellationToken token)
        {
            var tcs = new TaskCompletionSource<EggGroup>();
            tcs.SetResult(new EggGroup { Id = 1, Name = "Wasser" });
            return tcs.Task;
        }

        public Task<ObservableCollection<Stat>> LoadPokemonStats(int formId, GameVersion version, int displayLanguage, CancellationToken token)
        {
            var tcs = new TaskCompletionSource<ObservableCollection<Stat>>();
            tcs.SetResult(new ObservableCollection<Stat>
            {
                new Stat { EffortValue = 1, Id = 1, Name = "KP" }
            });
            return tcs.Task;
        }
    }
}