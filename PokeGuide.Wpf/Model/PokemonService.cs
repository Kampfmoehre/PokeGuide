using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace PokeGuide.Wpf.Model
{
    public class PokemonService : IPokemonService
    {
        readonly string _database;
        public PokemonService(string database)
        {
            _database = database;
        }

        public async Task<List<Species>> LoadAllSpeciesAsync(int generation, int language, IProgress<double> progress, CancellationToken token)
        {
            try
            {
                var loader = new Data.DataLoader(_database);
                List<Data.Model.Species> species = await loader.LoadAllSpeciesAsync(generation, language, progress, token);
                return species.Select(s => new Species
                {
                    BaseHappiness = s.BaseHappiness,
                    CaptureRate = s.CaptureRate,
                    Genus = s.Genus,
                    HatchCounter = s.HatchCounter,
                    Id = s.Id,
                    Name = s.Name
                }).ToList();
            }
            catch (TaskCanceledException)
            {
                return null;
            }            
        }

        public async Task<List<PokemonForm>> LoadPokemonFormsAsync(Species species, int versionGroup, int language, IProgress<double> progress, CancellationToken token)
        {
            try
            {
                var loader = new Data.DataLoader(_database);
                List<Data.Model.PokemonForm> forms = await loader.LoadFormsAsync(species.Id, versionGroup, language, progress, token);
                return forms.Select(s => new PokemonForm
                {
                    Ability1 = RemapAbility(s.Ability1),
                    Ability2 = RemapAbility(s.Ability2),
                    BaseExperience = s.BaseExperience,
                    Height = s.Height,
                    HiddenAbility = RemapAbility(s.HiddenAbility),
                    Id = s.Id,
                    Name = s.Name,
                    Species = species,
                    Type1 = RemapType(s.Type1),
                    Type2 = RemapType(s.Type2)
                }).ToList();
            }
            catch (TaskCanceledException)
            {
                return null;
            }            
        }

        static Ability RemapAbility(Data.Model.Ability ability)
        {
            if (ability == null)
                return null;
            return new Ability
            {
                Description = ability.Description,
                Effect = ability.Effect,
                FlavorText = ability.FlavorText,
                Id = ability.Id,
                Name = ability.Name
            };
        }
        static ElementType RemapType(Data.Model.ElementType type)
        {
            if (type == null)
                return null;
            return new ElementType
            {
                Id = type.Id,
                Name = type.Name
            };
        }
    }
}
