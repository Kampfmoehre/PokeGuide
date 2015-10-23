using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using PokeGuide.Wpf.Model;

namespace PokeGuide.Wpf.Design
{
    class DesignPokemonService : IPokemonService
    {
        public async Task<List<Species>> LoadAllSpeciesAsync(int generation, int language, IProgress<double> progress, CancellationToken token)
        {
            progress.Report(50);
            return await Task.Factory.StartNew(() =>
            {
                return new List<Species>
                {
                    ExampleSpecies()
                };
            });
        }

        public async Task<List<PokemonForm>> LoadPokemonFormsAsync(Species species, int versionGroup, int language, IProgress<double> progress, CancellationToken token)
        {
            progress.Report(65);
            return await Task.Factory.StartNew(() =>
            {
                return new List<PokemonForm>
                {
                    new PokemonForm
                    {
                        Ability1 = new Ability
                        {
                            Description = "Summons [strong sunlight]{mechanic:strong-sunlight} that lasts indefinitely upon entering battle.",
                            Effect = "The [weather]{mechanic:weather} changes to [strong sunlight]{mechanic:strong-sunlight} when this Pokémon enters battle and does not end unless cancelled by another weather condition.\n\n" +
                                "If multiple Pokémon with this ability, []{ability:drizzle}, []{ability:sand-stream}, or []{ability:snow-warning} are sent out at the same time, the abilities will activate in order of [Speed]{mechanic:speed}, respecting []{move:trick-room}.  Each ability's weather will cancel the previous weather, and only the weather summoned by the slowest of the Pokémon will stay.",
                            FlavorText = "The Pokémon makes it\nsunny if it is in battle.",
                            Id = 70,
                            Name = "Dürre"
                        },
                        BaseExperience = 285,
                        Id = 10034,
                        Name = "Mega Glurak X",
                        Height = 170,
                        Species = ExampleSpecies(),
                        Type1 = new ElementType { Id = 10, Name = "Feuer" },
                        Type2 = new ElementType { Id = 16, Name = "Drache" }
                    }
                };
            });
        }

        static Species ExampleSpecies()
        {
            return new Species
            {
                BaseHappiness = 70,
                CaptureRate = 45,
                Genus = "Flamme",
                HatchCounter = 20,
                Id = 6,
                Name = "Glurak"
            };
        }
    }
}
