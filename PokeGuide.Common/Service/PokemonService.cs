using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using PokeGuide.Model;
using PokeGuide.Model.Database;
using SQLite.Net.Interop;

namespace PokeGuide.Service
{
    public class PokemonService : DataService, IPokemonService
    {
        public PokemonService(IStorageService storageService, ISQLitePlatform sqlitePlatform) 
            : base(storageService, sqlitePlatform)
        { }

        public async Task<GrowthRate> LoadGrowthRateAsync(int id, int displayLanguage, CancellationToken token)
        {
            try
            {
                string query = "SELECT gr.id, grd.name FROM pokemon_v2_growthrate gr\n" +
                    "LEFT JOIN\n(SELECT e.growth_rate_id AS id, COALESCE(o.description, e.description) AS name FROM pokemon_v2_growthratedescription e\n" +
                    "LEFT OUTER JOIN pokemon_v2_growthratedescription o ON e.growth_rate_id = o.growth_rate_id and o.language_id = ?\n" +
                    "WHERE e.language_id = 9\nGROUP BY e.growth_rate_id)\nAS grd ON gr.id = grd.id\n" + 
                    "WHERE gr.id = ?";
                IEnumerable<DbGrowthRate> growthRates = await _connection.QueryAsync<DbGrowthRate>(token, query, new object[] { displayLanguage, id });
                DbGrowthRate growthRate = growthRates.First(); // await _connection.ExecuteScalarAsync<DbGrowthRate>(token, query, new object[] { displayLanguage, id });
                return new GrowthRate { Id = growthRate.Id, Name = growthRate.Name };
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<PokedexEntry> LoadPokedexEntryAsync(int dexId, int speciesId, int displayLanguage, CancellationToken token)
        {
            try
            {
                var tableQuery = _connection.Table<DbPokemonDexNumber>().Where(w => w.PokedexId == dexId && w.PokemonSpeciesId == speciesId);
                List<DbPokemonDexNumber> entries = await tableQuery.ToListAsync(token);
                var result = new PokedexEntry { Id = dexId };
                if (entries.Any())
                {
                    result.DexNumber = entries.First().PokedexNumber;
                }
                return result;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<ObservableCollection<SpeciesName>> LoadAllSpeciesAsync(GameVersion version, int displayLanguage, CancellationToken token)
        {
            string query = "SELECT ps.id, psn.name, ps.generation_id FROM pokemon_v2_pokemonspecies AS ps\n" +
                "LEFT JOIN\n(SELECT def.pokemon_species_id AS id, IFNULL(curr.name, def.name) AS name FROM pokemon_v2_pokemonspeciesname def\n" +
                "LEFT JOIN pokemon_v2_pokemonspeciesname curr ON def.pokemon_species_id = curr.pokemon_species_id AND def.language_id = 9 AND curr.language_id = ?\n" +
                "GROUP BY def.pokemon_species_id)\nAS psn ON ps.id = psn.id\nWHERE ps.generation_id <= ?";
            IEnumerable<DbPokemonSpecies> list = await _connection.QueryAsync<DbPokemonSpecies>(token, query, new object[] { displayLanguage, version.Generation });
            return new ObservableCollection<SpeciesName>(list.Select(s => new SpeciesName { Generation = s.GenerationId, Id = s.Id, Name = s.Name }));
        }

        public async Task<Species> LoadSpeciesAsync(int id, GameVersion version, int displayLanguage, CancellationToken token)
        {
            try
            {
                string query = "SELECT ps.id, psn.name, psn.genus, ps.gender_rate, ps.capture_rate, ps.base_happiness, ps.hatch_counter, ps.growth_rate_id FROM pokemon_v2_pokemonspecies AS ps\n" +
                    "LEFT JOIN\n(SELECT def.pokemon_species_id AS id, IFNULL(curr.name, def.name) AS name, IFNULL(curr.genus, def.genus) AS genus FROM pokemon_v2_pokemonspeciesname def\n" +
                    "LEFT JOIN pokemon_v2_pokemonspeciesname curr ON def.pokemon_species_id = curr.pokemon_species_id AND def.language_id = 9 AND curr.language_id = ?\n" +
                    "GROUP BY def.pokemon_species_id)\nAS psn ON ps.id = psn.id\n" +                    
                    "WHERE ps.id <= ?";
                IEnumerable<DbPokemonSpecies> list = await _connection.QueryAsync<DbPokemonSpecies>(token, query, new object[] { displayLanguage, version.Generation });                
                query = "SELECT p.id, pn.name FROM pokemon_v2_pokedex p\n" +
                    "LEFT JOIN\n(SELECT e.pokedex_id AS id, COALESCE(o.name, e.name) AS name FROM pokemon_v2_pokedexdescription e\n" +
                    "LEFT OUTER JOIN pokemon_v2_pokedexdescription o ON e.pokedex_id = o.pokedex_id and o.language_id = ?\n" +
                    "WHERE e.language_id = 9\nGROUP BY e.pokedex_id)\nAS pn ON p.id = pn.id\n" +
                    "LEFT JOIN pokemon_v2_pokedexversiongroup pvg ON p.id = pvg.pokedex_id\n" +
                    "WHERE pvg.version_group_id = ?";
                IEnumerable<DbPokedex> temp = await _connection.QueryAsync<DbPokedex>(token, query, new object[] { displayLanguage, version.VersionGroup });
                DbPokemonSpecies dbSpecies = list.First();
                DbPokedex dex = temp.First();

                var species = new Species
                {
                    BaseHappiness = dbSpecies.BaseHappiness,
                    CatchRate = dbSpecies.CaptureRate,
                    HatchCounter = dbSpecies.HatchCounter,
                    Id = dbSpecies.Id,
                    Name = dbSpecies.Name
                };
                species.GrowthRate = await LoadGrowthRateAsync(dbSpecies.GrowthRateId, displayLanguage, token);
                species.DexEntry = await LoadPokedexEntryAsync(dex.Id,  dbSpecies.Id, displayLanguage, token);
                species.DexEntry.Name = dex.Name;

                return species;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<ObservableCollection<PokemonForm>> LoadFormsAsync(int speciesId, GameVersion version, int displayLanguage, CancellationToken token)
        {
            string query = "SELECT pf.id, pfn.name, p.height, p.weight, p.base_experience, pt1.type_id AS type1, pt2.type_id AS type2, " +
                "pa1.ability_id AS ability1, pa2.ability_id AS ability2, pa3.ability_id AS hidden_ability FROM pokemon_v2_pokemonform pf\n" +
                "LEFT JOIN\n(SELECT e.pokemon_form_id AS id, COALESCE(o.name, e.name) AS name FROM pokemon_v2_pokemonformname e\n" +
                "LEFT OUTER JOIN pokemon_v2_pokemonformname o ON e.pokemon_form_id = o.pokemon_form_id and o.language_id = ?\n" +
                "WHERE e.language_id = 9\nGROUP BY e.pokemon_form_id)\nAS pfn ON pf.id = pfn.id\n" +
                "LEFT JOIN pokemon_v2_pokemon p ON pf.pokemon_id = p.id\n" +
                "LEFT JOIN pokemon_v2_pokemontype AS pt1 ON p.id = pt1.pokemon_id AND pt1.slot = 1\n" +
                "LEFT JOIN pokemon_v2_pokemontype AS pt2 ON p.id = pt2.pokemon_id AND pt2.slot = 2\n" +
                "LEFT JOIN pokemon_v2_pokemonability AS pa1 ON p.id = pa1.pokemon_id AND pa1.slot = 1\n" +
                "LEFT JOIN pokemon_v2_pokemonability AS pa2 ON p.id = pa2.pokemon_id AND pa2.slot = 2\n" +
                "LEFT JOIN pokemon_v2_pokemonability AS pa3 ON p.id = pa3.pokemon_id AND pa3.slot = 3\n" +
                "WHERE p.pokemon_species_id = ?";// AND pf.version_group_id = ?

            IEnumerable<DbPokemonForm> forms = await _connection.QueryAsync<DbPokemonForm>(token, query, new object[] { displayLanguage, speciesId });
            Species species = await LoadSpeciesAsync(speciesId, version, displayLanguage, token);
            var result = new ObservableCollection<PokemonForm>();
            foreach (DbPokemonForm f in forms)
            {
                var form = new PokemonForm
                {
                    BaseExperience = f.BaseExperience,
                    Height = f.Height,
                    Id = f.Id,
                    Name = f.Name,
                    Species = species,
                    Weight = f.Weight
                };
                // Handle Fairy before Gen 6
                if (version.Generation < 6 && f.Type1 == 18)
                    f.Type1 = 1;
                form.Type1 = await LoadTypeAsync(f.Type1, version, displayLanguage, token);
                if (f.Type2 != null)
                    form.Type2 = await LoadTypeAsync((int)f.Type2, version, displayLanguage, token);

                if (version.Generation >= 3)
                {
                    form.Ability1 = await LoadAbilityAsync(f.Ability1, version, displayLanguage, token);
                    if (f.Ability2 != null)
                        form.Ability2 = await LoadAbilityAsync((int)f.Ability2, version, displayLanguage, token);
                    if (version.Generation >= 5 && f.HiddenAbility != null)
                        form.HiddenAbility = await LoadAbilityAsync((int)f.HiddenAbility, version, displayLanguage, token);
                }

                result.Add(form);
            }

            return result;
        }

        public async Task<ElementType> LoadTypeAsync(int id, GameVersion version, int displayLanguage, CancellationToken token)
        {
            try
            {
                string query = "SELECT t.id, tn.name, t.move_damage_class_id FROM pokemon_v2_type t\n" +
                    "LEFT JOIN\n(SELECT e.type_id AS id, COALESCE(o.name, e.name) AS name FROM pokemon_v2_typename e\n" +
                    "LEFT OUTER JOIN pokemon_v2_typename o ON e.type_id = o.type_id and o.language_id = ?\n" +
                    "WHERE e.language_id = 9\nGROUP BY e.type_id)\nAS tn ON t.id = tn.id\n" +
                    "WHERE t.id = ? AND t.generation_id <= ?";
                IEnumerable<DbType> types = await _connection.QueryAsync<DbType>(token, query, new object[] { displayLanguage, id, version.Generation });
                DbType type = types.First(); // await _connection.ExecuteScalarAsync<DbType>(token, query, new object[] { displayLanguage, id, version.Generation });
                if (type == null)
                    return null;
                return new ElementType { Id = type.Id, Name = type.Name };
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<Ability> LoadAbilityAsync(int id, GameVersion version, int displayLanguage, CancellationToken token)
        {
            try
            {
                string query = "SELECT a.id, an.name, ad.short_effect, ad.effect, aft.flavor_text FROM pokemon_v2_ability AS a\n" +
                    "LEFT JOIN\n(SELECT def.ability_id AS id, IFNULL(curr.name, def.name) AS name FROM pokemon_v2_abilityname def\n" +
                    "LEFT JOIN pokemon_v2_abilityname curr ON def.ability_id = curr.ability_id AND def.language_id = 9 AND curr.language_id = ?\n" +
                    "GROUP BY def.ability_id)\nAS an ON a.id = an.id\n" +
                    "LEFT JOIN\n(SELECT def.ability_id AS id, IFNULL(curr.short_effect, def.short_effect) AS short_effect, IFNULL(curr.effect, def.effect) AS effect FROM pokemon_v2_abilitydescription def\n" +
                    "LEFT JOIN pokemon_v2_abilitydescription curr ON def.ability_id = curr.ability_id AND def.language_id = 9 AND curr.language_id = ?\n" +
                    "GROUP BY def.ability_id)\nAS ad ON a.id = ad.id\n" +
                    "LEFT JOIN\n(SELECT e.ability_id AS id, COALESCE(o.flavor_text, e.flavor_text) AS flavor_text, e.version_group_id FROM pokemon_v2_abilityflavortext e\n" +
                    "LEFT OUTER JOIN pokemon_v2_abilityflavortext o ON e.ability_id = o.ability_id and o.language_id = ?\n" +
                    "WHERE e.language_id = 9 AND e.version_group_id = ?\n" +
                    "GROUP BY e.ability_id)\nAS aft ON a.id = aft.id\n" +
                    "WHERE aft.version_group_id = ? AND a.id = ?";
                IEnumerable<DbAbility> abilities = await _connection.QueryAsync<DbAbility>(token, query, new object[]
                    {
                        displayLanguage,
                        displayLanguage,
                        displayLanguage,
                        version.VersionGroup,
                        version.VersionGroup,
                        id
                    });
                DbAbility dbAbility = abilities.First();
                //DbAbility dbAbility = await _connection.ExecuteScalarAsync<DbAbility>(token, query, new object[] 
                //    {
                //        displayLanguage,
                //        displayLanguage,
                //        displayLanguage,
                //        version.VersionGroup,
                //        version.VersionGroup,
                //        id
                //    });
                return new Ability
                {
                    Description = dbAbility.Effect,
                    Effect = dbAbility.ShortEffect,
                    FlavorText = dbAbility.FlavorText,
                    Id = dbAbility.Id,
                    Name = dbAbility.Name
                };
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
