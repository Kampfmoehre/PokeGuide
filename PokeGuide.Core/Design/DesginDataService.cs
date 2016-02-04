using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using PokeGuide.Core.Model;
using PokeGuide.Core.Service.Interface;

namespace PokeGuide.Core.Design
{
    public class DesginDataService : IDataService
    {
        public void InitializeResources(int displayLanguage, CancellationToken token)
        {}

        public Task<Ability> LoadAbilityByIdAsync(int id, int versionGroup, int language, CancellationToken token)
        {
            var tcs = new TaskCompletionSource<Ability>();
            tcs.SetResult(new Ability
            {
                Description = String.Format(@"Ein Pokémon mit dieser Fähigkeit ist immun gegen [Feuer]{type:fire} Attacken. Sobald es von einer [Feuer]{type:fire} Attacke getroffen wurde, erhöht sich der Schaden der eigenen [Feuer]{type:fire} Attacken um 50% bis es den Kampf verlässt.

Die Fähigkeit hat keinen Effekt, wenn das Pokémon[eingefroren]{ mechanic:freezing } ist.Der Bonus bleibt erhalten wenn das Pokémon[eingefroren]{ mechanic:freezing } ist und wieder auftaut oder wenn es die Fähigkeit wechselt bzw.diese deaktiviert wird. [Feuer]{ type:fire } Attacken ignorieren den [Delegator]{ move:substitute }. Der Effekt wirkt auch bei [Feuer]{ type:fire } Attacken die keinen Schaden verursachen, wie z.B. [Irrlicht]{ move:will-o-wisp }."),
                Id = 18,
                IngameText = "Powers up the Pokémon’s\nFire - type moves if it’s hit by one.",
                Name = "Feuerfänger",
                ShortDescription = "Schützt vor [Feuer]{type:fire} Attacken. Sobald eine [Feuer]{type:fire} Attacke blockiert wurde, erhöht sich der Schaden der eigenen [Feuer]{type:fire} Attacken um 50%.",
                VersionChangelog = "[Irrlicht]{move:will-o-wisp} triggert die Fähigkeit nicht bei Pokémon die immun gegen [Verbrennung]{mechanic:burn} sind."
            });
            return tcs.Task;
        }

        public Task<IEnumerable<ModelNameBase>> LoadAbilityNamesAsync(int language, int generation, CancellationToken token)
        {
            var tcs = new TaskCompletionSource<IEnumerable<ModelNameBase>>();
            tcs.SetResult(new List<ModelNameBase>()
            {
                new ModelNameBase { Id = 1, Name = "Duftnote" },
                new ModelNameBase { Id = 2, Name = "Niesel" },
                new ModelNameBase { Id = 3, Name = "Temposchub" }
            });
            return tcs.Task;
        }

        public Task<DisplayLanguage> LoadLanguageByIdAsync(int id, int displayLanguage, CancellationToken token)
        {
            var tcs = new TaskCompletionSource<DisplayLanguage>();
            tcs.SetResult(new DisplayLanguage { Id = 6, Iso639 = "de", Name = "deutsch" });
            return tcs.Task;
        }

        public Task<Move> LoadMoveByIdAsync(int id, GameVersion version, int language, CancellationToken token)
        {
            var tcs = new TaskCompletionSource<Move>();
            tcs.SetResult(new Move
            {
                Accuracy = 100,
                DamageCategory = new DamageClass
                {
                    Description = "Physischer Schaden, beeinflusst von Angriff und Verteidigung",
                    Id = 2,
                    Identifier = "physical",
                    Name = "physisch"
                },
                Description = "blub",
                Id = 1,
                IngameText = "Ein Hieb mit den Vorderbeinen oder dem Schweif.",
                Name = "Karateschlag",
                Power = 80,
                PowerPoints = 25,
                Priority = 0,
                ShortDescription = "bla",
                Type = new ElementType
                {
                    DamageClassId = 2,
                    Generation = 1,
                    Id = 2,
                    Identifier = "fighting",
                    Name = "Kampf"
                },
                VersionChangelog = "blubber blöb"
            });
            return tcs.Task;
        }

        public Task<IEnumerable<ModelNameBase>> LoadMoveNamesAsync(int language, int generation, CancellationToken token)
        {
            var tcs = new TaskCompletionSource<IEnumerable<ModelNameBase>>();
            tcs.SetResult(new List<ModelNameBase>
            {
                new ModelNameBase { Id = 1, Name = "Pfund" },
                new ModelNameBase { Id = 2, Name = "Karateschlag" },
                new ModelNameBase { Id = 3, Name = "Duplexhieb" }
            });
            return tcs.Task;
        }

        public Task<IEnumerable<ModelNameBase>> LoadPokemonAsync(int generation, int language, CancellationToken token)
        {
            var tcs = new TaskCompletionSource<IEnumerable<ModelNameBase>>();
            tcs.SetResult(new List<ModelNameBase>
            {
                new ModelNameBase { Id = 1, Name = "Bisasam" },
                new ModelNameBase { Id = 2, Name = "Bisaflor" },
                new ModelNameBase { Id = 3, Name = "Bisaknosp" }
            });
            return tcs.Task;
        }

        public Task<IEnumerable<PokemonAbility>> LoadPokemonByAbilityAsync(int abilityId, int versionGroupId, int language, CancellationToken token)
        {
            var tcs = new TaskCompletionSource<IEnumerable<PokemonAbility>>();
            tcs.SetResult(new List<PokemonAbility>
            {
                new PokemonAbility
                {
                    Id = 1,
                    IsHidden = false,
                    Pokemon = new ModelNameBase { Id = 1, Name = "Glumanda" },
                    Slot = 1
                },
                new PokemonAbility
                {
                    Id = 2,
                    IsHidden = false,
                    Pokemon = new ModelNameBase { Id = 10, Name = "Raupy" },
                    Slot = 2
                },
                new PokemonAbility
                {
                    Id = 3,
                    IsHidden = true,
                    Pokemon = new ModelNameBase { Id = 25, Name = "Pikachu" },
                    Slot = 3
                }
            });
            return tcs.Task;
        }

        public Task<PokemonForm> LoadPokemonFormByIdAsync(int id, GameVersion version, int language, CancellationToken token)
        {
            var tcs = new TaskCompletionSource<PokemonForm>();
            tcs.SetResult(new PokemonForm
            {
                Ability1 = new ModelNameBase { Id = 12, Name = "Facettenauge" },
                Ability2 = new ModelNameBase { Id = 24, Name = "Rauhaut" },
                BaseExperience = 155,
                BaseHappiness = 70,
                CaptureRate = 255,
                Color = new PokemonColor { Id = 1, Name = "Grün" },
                DexEntry = new PokedexEntry
                {
                    DexDescription = "Dieses Pokémon trägt von Geburt an einen Samen\nauf dem Rücken,\nder mit ihm keimt und wächst.",
                    DexNumber = 15,
                    Id = 2,
                    Name = "Sinnoh"
                },
                Genus = "Blume",
                GrowthRate = new ModelNameBase { Id = 1, Name = "Mittel schnell" },
                Habitat = new ModelNameBase { Id = 1, Name = "Gras" },
                HatchCounter = 5120,
                Height = 102,
                HeldItem = new ModelNameBase { Id = 1, Name = "Sinelbeere" },
                HiddenAbility = new ModelNameBase { Id = 36, Name = "Wassertempo" },
                Id = 1,
                IsBaby = false,
                ItemRarity = 15,
                Name = "Garados",
                Shape = new ModelUriBase { Id = 1, Name = "Aufrecht" },
                Species = new ModelNameBase { Id = 1, Name = "Glurak" },
                Type1 = new ElementType
                {
                    DamageClassId = 2,
                    Generation = 1,
                    Id = 2,
                    Identifier = "fighting",
                    Name = "Kampf"
                },
                Weight = 50
            });
            return tcs.Task;
        }

        public Task<IEnumerable<ModelNameBase>> LoadPokemonFormsAsync(int versionGroupId, int language, CancellationToken token)
        {
            var tcs = new TaskCompletionSource<IEnumerable<ModelNameBase>>();
            tcs.SetResult(new List<ModelNameBase>
            {
                new ModelNameBase { Id = 1, Name = "Plinfa" },
                new ModelNameBase { Id = 20, Name = "Serpifeu" },
                new ModelNameBase { Id = 301, Name = "Rotom" }
            });
            return tcs.Task;
        }

        public Task<GameVersion> LoadVersionByIdAsync(int id, int language, CancellationToken token)
        {
            var tcs = new TaskCompletionSource<GameVersion>();
            tcs.SetResult(new GameVersion
            {
                Generation = 6,
                Id = 25,
                Name = "Omega Rubin",
                VersionGroup = 15
            });
            return tcs.Task;
        }

        public Task<IEnumerable<GameVersion>> LoadVersionsAsync(int language, CancellationToken token)
        {
            var tcs = new TaskCompletionSource<IEnumerable<GameVersion>>();
            tcs.SetResult(new List<GameVersion>
            {
                new GameVersion { Generation = 1, Id = 1, Name = "Rot", VersionGroup = 1 },
                new GameVersion { Generation = 2, Id = 4, Name = "Gold", VersionGroup = 3 },
                new GameVersion { Generation = 3, Id = 8, Name = "Rubin", VersionGroup = 5 }
            });
            return tcs.Task;
        }
    }
}
