using System;
using System.Collections.Generic;
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

        public Task<GrowthRate> GetGrowthRateAsync(int id)
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
                EggGroup2 = new EggGroup { Id = 12, Name = "Humanotyp" },
                GenderRate   = new GenderRate { Female = 50, Male = 50},
                GrowthRate = GetGrowthRateAsync(1).Result,
                HatchCounter = 20,
                Id = 6,
                Name = "Glurak",
                PossibleEvolutions = LoadPossibleEvolutionsAsync(id, version, displayLanguage, token).Result
            });
            return tcs.Task;
        }

        public Task<ObservableCollection<PokemonForm>> LoadFormsAsync(SpeciesName species, GameVersion version, int displayLanguage, CancellationToken token)
        {
            var tcs = new TaskCompletionSource<ObservableCollection<PokemonForm>>();
            tcs.SetResult(new ObservableCollection<PokemonForm> {
                new PokemonForm
                {
                    BaseExperience = 255,
                    Height = 12,
                    Id = 12,
                    Name = "Mega Glurak X",
                    Species = LoadSpeciesAsync(species.Id, version, displayLanguage, token).Result,
                    Weight = 100
                }
            });
            return tcs.Task;
        }

        public Task<ElementType> GetTypeAsync(int id, GameVersion version)
        {
            var tcs = new TaskCompletionSource<ElementType>();
            tcs.SetResult(new ElementType { Id = 12, Name = "Feuer" });
            return tcs.Task;
        }

        public Task<Ability> LoadAbilityAsync(int id, int versionGroup, int displayLanguage, CancellationToken token)
        {
            var tcs = new TaskCompletionSource<Ability>();
            tcs.SetResult(new Ability { Description = "Awesome Description", Effect = "Strange effect", FlavorText = "Fancy ingame text", Id = 12, Name = "Temposchub" });
            return tcs.Task;
        }

        public Task<EggGroup> LoadEggGroupAsync(int id, int displayLanguage, CancellationToken token)
        {
            var tcs = new TaskCompletionSource<EggGroup>();
            tcs.SetResult(new EggGroup { Id = 1, Name = "Wasser" });
            return tcs.Task;
        }

        public Task<ObservableCollection<Stat>> LoadPokemonStatsAsync(int formId, GameVersion version, int displayLanguage, CancellationToken token)
        {
            var tcs = new TaskCompletionSource<ObservableCollection<Stat>>();
            tcs.SetResult(new ObservableCollection<Stat>
            {
                new Stat { EffortValue = 1, Id = 1, Name = "KP", StatValue = 110 },
                new Stat { EffortValue = 0, Id = 2, Name = "Angriff", StatValue = 60 },
                new Stat { EffortValue = 1, Id = 3, Name = "Verteidigung", StatValue = 75 },
                new Stat { EffortValue = 0, Id = 4, Name = "Spezialangriff", StatValue = 55 },
                new Stat { EffortValue = 1, Id = 5, Name = "Spezialverteidigung", StatValue = 70 },
                new Stat { EffortValue = 0, Id = 6, Name = "Initative", StatValue = 70 }
            });
            return tcs.Task;
        }

        public Task<PokemonForm> LoadFormAsync(int formId, GameVersion version, int displayLanguage, CancellationToken token)
        {
            var tcs = new TaskCompletionSource<PokemonForm>();
            tcs.SetResult(new PokemonForm
            {
                Ability1 = new Ability { Description = "blub", Effect = "blob", FlavorText = "blubber", Id = 12, Name = "Adlerauge" },
                Ability2 = LoadAbilityAsync(1, version.VersionGroup, displayLanguage, token).Result,
                BaseExperience = 255,
                Height = 12,
                HeldItem = new Item { Id = 15, Name = "Beere" },
                HeldItemRarity = 5,
                Id = 12,
                HiddenAbility = new Ability { Description = "Secret Ability", Effect = "Awesome effect", FlavorText = "You know nothing", Id = 24, Name = "versteckt"},
                Name = "Mega Glurak X",
                Stats = LoadPokemonStatsAsync(1, version, displayLanguage, token).Result,
                Species = LoadSpeciesAsync(12, version, displayLanguage, token).Result,
                Type1 = new ElementType { DamageClassId = 1, Id = 9, Name = "Unlicht" },
                Type2 = GetTypeAsync(1, version).Result,
                Weight = 100
            });
            return tcs.Task;
        }

        public void InitializeResources(int displayLanguage, CancellationToken token)
        {
            
        }
              
        public Task<DamageClass> GetDamageClassAsync(int id)
        {
            var tcs = new TaskCompletionSource<DamageClass>();
            tcs.SetResult(new DamageClass { Id = id, Name = "Physisch" });
            return tcs.Task;
        }

        public Task<Move> LoadMoveAsync(int id, GameVersion version, int displayLanguage, CancellationToken token)
        {
            var tcs = new TaskCompletionSource<Move>();
            tcs.SetResult(new Move
            {
                Accuracy = 90,
                DamageClass = GetDamageClassAsync(1).Result,
                Id = 2,
                Name = "Flügelschlag",
                Power = 60,
                PowerPoints = 15,
                Priority = 0,
                Type = new ElementType { Id = 1, Name = "Flug" }
            });
            return tcs.Task;
        }

        public Task<MoveLearnMethod> LoadMoveLearnMethodAsync(int id, int displayLanguage, CancellationToken token)
        {
            var tcs = new TaskCompletionSource<MoveLearnMethod>();
            tcs.SetResult(new MoveLearnMethod { Id = 1, Name = "Level" });
            return tcs.Task;
        }

        public Task<ObservableCollection<PokemonMove>> LoadMoveSetAsync(int pokemonId, GameVersion version, int displayLanguage, CancellationToken token)
        {
            var tcs = new TaskCompletionSource<ObservableCollection<PokemonMove>>();
            tcs.SetResult(new ObservableCollection<PokemonMove>
            {
                new PokemonMove
                {
                    LearnMethod = new MoveLearnMethod { Id = 1, Name = "Level" },
                    Level = 12,
                    Move = LoadMoveAsync(1, version, displayLanguage, token).Result
                },
                new PokemonMove
                {
                    LearnMethod = new MoveLearnMethod { Id = 1, Name = "Level" },
                    Level = 24,
                    Move = new Move
                    {
                        Accuracy = 100,
                        DamageClass = new DamageClass { Id = 2, Name = "Speziell" },
                        Id = 156,
                        Name = "Flammenwurf",
                        Power = 120,
                        PowerPoints = 5,
                        Priority = -2,
                        Type = new ElementType { DamageClassId = 1, Generation = 1, Id = 7, Name = "Coleottero" }
                    }
                },
                new PokemonMove
                {
                    LearnMethod = new MoveLearnMethod { Id = 1, Name = "Level" },
                    Level = 24,
                    Move = new Move
                    {
                        Accuracy = null,
                        DamageClass = new DamageClass { Id = 2, Name = "Status" },
                        Id = 156,
                        Name = "Giftpuder",
                        Power = null,
                        PowerPoints = 25,
                        Priority = 2,
                        Type = new ElementType { DamageClassId = 1, Generation = 1, Id = 7, Name = "Gift" }
                    }
                }
            });
            return tcs.Task;
        }

        public Task<ObservableCollection<PokemonEvolution>> LoadPossibleEvolutionsAsync(int speciesId, GameVersion version, int displayLanguage, CancellationToken token)
        {
            var tcs = new TaskCompletionSource<ObservableCollection<PokemonEvolution>>();
            tcs.SetResult(new ObservableCollection<PokemonEvolution>
            {
                new PokemonEvolution
                {
                    EvolutionTrigger = "Gegenstand benutzen",
                    EvolutionItem = new Item { Id = 12, Name = "Feuerstein" },
                    EvolvesTo = new SpeciesName { Id = 122, Name = "Pumpdjinn" },
                    MinLevel = 16
                }
            });
            return tcs.Task;
        }

        public Task<Item> LoadItemAsync(int id, int displayLanguage, CancellationToken token)
        {
            throw new NotImplementedException();
        }

        public Task<ObservableCollection<PokemonEvolution>> LoadEvolutionGroupAsync(int speciesId, GameVersion version, int displayLanguage, CancellationToken token)
        {
            var tcs = new TaskCompletionSource<ObservableCollection<PokemonEvolution>>();
            tcs.SetResult(new ObservableCollection<PokemonEvolution>
            {
                new PokemonEvolution
                {
                    EvolutionItem = LoadItemAsync(1, displayLanguage, token).Result,
                    EvolutionTrigger = "Platz im Team und ein Pokéball",
                    EvolvesTo = new SpeciesName { Id = 45, Name = "Arkani" }                    
                },
                new PokemonEvolution
                {
                    EvolvesTo = new SpeciesName { Id = 482, Name = "Dragoran" },
                    MinLevel = 58
                },
                new PokemonEvolution
                {
                    EvolutionLocation = LoadLocationFromIdAsync(1, version, displayLanguage, token).Result,
                    EvolvesTo = new SpeciesName { Id = 245, Name = "Magneton" }
                }
            });
            return tcs.Task;
        }

        public Task<Location> LoadLocationFromIdAsync(int id, GameVersion version, int displayLanguage, CancellationToken token)
        {
            var tcs = new TaskCompletionSource<Location>();
            tcs.SetResult(new Location
            {
                Id = 12,
                Name = "Kraterberg"
            });
            return tcs.Task;
        }

        public Task<Location> LoadLocationFromAreaAsync(int areaId, GameVersion version, int displayLanguage, CancellationToken token)
        {
            var tcs = new TaskCompletionSource<Location>();
            tcs.SetResult(new Location
            {
                Id = 12,
                Name = "Kraterberg",
                AreaId = 13,
                AreaName = "1F"
            });
            return tcs.Task;
        }

        public Task<ObservableCollection<PokemonLocation>> LoadPokemonEncountersAsync(int pokemonId, GameVersion version, int displayLanguage, CancellationToken token)
        {
            var tcs = new TaskCompletionSource<ObservableCollection<PokemonLocation>>();
            tcs.SetResult(new ObservableCollection<PokemonLocation>
            {
                new PokemonLocation
                {
                    Conditions = new List<EncounterCondition> { new EncounterCondition { Id = 1, Name = "Durch Benutzung des Pokéradars" } },
                    EncounterMethod = new EncounterMethod
                    {
                        Id = 1,
                        Name = "Im hohen Gras oder in einer Höhle laufen"
                    },
                    Location = LoadLocationFromAreaAsync(1, version, displayLanguage, token).Result,
                    MaxLevel = 5,
                    MinLevel = 5,
                    Rarity = 12
                },
                new PokemonLocation
                {
                    EncounterMethod = new EncounterMethod
                    {
                        Id = 1,
                        Name = "Mit einer normalen Angel angeln"
                    },
                    Location = new Location { Id = 1, Name = "Route 11" },
                    MaxLevel = 30,
                    MinLevel = 20,
                    Rarity = 100
                },
                new PokemonLocation
                {
                    Conditions = new List<EncounterCondition>
                    {
                        new EncounterCondition { Id = 4, Name = "Am Tag" },
                        new EncounterCondition { Id = 13, Name = "Radio aus" }
                    },
                    EncounterMethod = new EncounterMethod
                    {
                        Id = 1,
                        Name = "Im raschelndem Gras laufen"
                    },
                    Location = new Location
                    {
                        AreaId = 112,
                        AreaName = "Unten rechts",
                        Id = 133,
                        Name = "Safari Zone"
                    },
                    MaxLevel = 8,
                    MinLevel = 6,
                    Rarity = 40
                }
            });
            return tcs.Task;
        }

        public Task<EncounterMethod> GetEncounterMethodAsync(int id)
        {
            var tcs = new TaskCompletionSource<EncounterMethod>();
            tcs.SetResult(new EncounterMethod
            {
                Id = 12,
                Name = "Surfer"
            });
            return tcs.Task;
        }

        public Task<EncounterCondition> GetEncounterConditionAsync(int id)
        {
            var tcs = new TaskCompletionSource<EncounterCondition>();
            tcs.SetResult(new EncounterCondition
            {
                Id = 12,
                Name = "Durch Benutzung des Pokéradars"
            });
            return tcs.Task;
        }

        public Task<EggGroup> GetEggGroupAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Ability>> LoadAbilitiesAsync(int generation, int displayLanguage, CancellationToken token)
        {
            var tcs = new TaskCompletionSource<IEnumerable<Ability>>();
            tcs.SetResult(new List<Ability>
            {
                new Ability {Id = 12, Name = "Dösigkeit" },
                new Ability {Id = 24, Name = "Rauhaut" },
            });
            return tcs.Task;
        }
    }
}