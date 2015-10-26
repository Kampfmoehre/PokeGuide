using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using PokeGuide.Model;
using PokeGuide.Model.Database;
using PokeGuide.Service.Interface;
using SQLite.Net.Interop;

namespace PokeGuide.Service
{
    public class PokemonService : BaseDataService, IPokemonService
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
            try
            {
                string query = "SELECT ps.id, psn.name, ps.generation_id FROM pokemon_v2_pokemonspecies AS ps\n" +
                    "LEFT JOIN\n(SELECT def.pokemon_species_id AS id, IFNULL(curr.name, def.name) AS name FROM pokemon_v2_pokemonspeciesname def\n" +
                    "LEFT JOIN pokemon_v2_pokemonspeciesname curr ON def.pokemon_species_id = curr.pokemon_species_id AND def.language_id = 9 AND curr.language_id = ?\n" +
                    "GROUP BY def.pokemon_species_id)\nAS psn ON ps.id = psn.id\nWHERE ps.generation_id <= ?";
                IEnumerable<DbPokemonSpecies> list = await _connection.QueryAsync<DbPokemonSpecies>(token, query, new object[] { displayLanguage, version.Generation });
                return new ObservableCollection<SpeciesName>(list.Select(s => new SpeciesName { Generation = s.GenerationId, Id = s.Id, Name = s.Name }));
            }
            catch (Exception)
            {
                return new ObservableCollection<SpeciesName>();
            }
        }

        public async Task<Species> LoadSpeciesAsync(int id, GameVersion version, int displayLanguage, CancellationToken token)
        {
            try
            {
                string query = "SELECT ps.id, psn.name, psn.genus, ps.gender_rate, ps.capture_rate, ps.base_happiness, ps.hatch_counter, ps.growth_rate_id FROM pokemon_v2_pokemonspecies AS ps\n" +
                    "LEFT JOIN\n(SELECT def.pokemon_species_id AS id, IFNULL(curr.name, def.name) AS name, IFNULL(curr.genus, def.genus) AS genus FROM pokemon_v2_pokemonspeciesname def\n" +
                    "LEFT JOIN pokemon_v2_pokemonspeciesname curr ON def.pokemon_species_id = curr.pokemon_species_id AND def.language_id = 9 AND curr.language_id = ?\n" +
                    "GROUP BY def.pokemon_species_id)\nAS psn ON ps.id = psn.id\n" +                    
                    "WHERE ps.id = ?";
                IEnumerable<DbPokemonSpecies> list = await _connection.QueryAsync<DbPokemonSpecies>(token, query, new object[] { displayLanguage, id });
                query = "SELECT p.id, pn.name FROM pokemon_v2_pokedex p\n" +
                    "LEFT JOIN\n(SELECT e.pokedex_id AS id, COALESCE(o.name, e.name) AS name FROM pokemon_v2_pokedexdescription e\n" +
                    "LEFT OUTER JOIN pokemon_v2_pokedexdescription o ON e.pokedex_id = o.pokedex_id and o.language_id = ?\n" +
                    "WHERE e.language_id = 9\nGROUP BY e.pokedex_id)\nAS pn ON p.id = pn.id\n" +
                    "LEFT JOIN pokemon_v2_pokedexversiongroup pvg ON p.id = pvg.pokedex_id\n" +
                    "WHERE pvg.version_group_id = ?";
                IEnumerable<DbPokedex> temp = await _connection.QueryAsync<DbPokedex>(token, query, new object[] { displayLanguage, version.VersionGroup });
                query = "SELECT egg_group_id FROM pokemon_v2_pokemonegggroup WHERE pokemon_species_id = ?";
                var eggGroups = new List<DbPokemonEggGroup>(await _connection.QueryAsync<DbPokemonEggGroup>(token, query, new object[] { id }));

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
                species.EggGroup1 = await LoadEggGroupAsync(eggGroups[0].EggGroupId, displayLanguage, token);
                if (eggGroups.Count > 1)
                    species.EggGroup2 = await LoadEggGroupAsync(eggGroups[1].EggGroupId, displayLanguage, token);

                return species;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<ObservableCollection<PokemonForm>> LoadFormsAsync(int speciesId, GameVersion version, int displayLanguage, CancellationToken token)
        {
            try
            {
                string query = "SELECT pf.id, pf.pokemon_id, pfn.name, p.height, p.weight, p.base_experience, pt1.type_id AS type1, pt2.type_id AS type2, " +
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

                var bag = new ConcurrentBag<PokemonForm>();

                var tasks = forms.Select(async (f) =>
                {
                    var form = new PokemonForm
                    {
                        BaseExperience = f.BaseExperience,
                        Height = Math.Round((double)f.Height / 10, 2),
                        Id = f.Id,
                        //Name = f.Name,
                        Species = species,
                        Weight = Math.Round((double)f.Weight / 10, 2)
                    };
                    if (String.IsNullOrWhiteSpace(f.Name))
                        form.Name = species.Name;
                    else
                        form.Name = f.Name;

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

                    form.Stats = await LoadPokemonStats(f.PokemonId, version, displayLanguage, token);

                    bag.Add(form);
                });
                await Task.WhenAll(tasks);
                //foreach (DbPokemonForm f in forms)
                //{
                    
                //}

                return new ObservableCollection<PokemonForm>(bag.OrderBy(o => o.Id));
            }
            catch (Exception)
            {
                return new ObservableCollection<PokemonForm>();
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

        public async Task<EggGroup> LoadEggGroupAsync(int id, int displayLanguage, CancellationToken token)
        {
            try
            {
                string query = "SELECT eg.id, egn.name FROM pokemon_v2_egggroup AS eg\n" +
                    "LEFT JOIN\n(SELECT e.egg_group_id AS id, COALESCE(o.name, e.name) AS name FROM pokemon_v2_egggroupname e\n" +
                    "LEFT OUTER JOIN pokemon_v2_egggroupname o ON e.egg_group_id = o.egg_group_id and o.language_id = ?\n" +
                    "WHERE e.language_id = 9\nGROUP BY e.egg_group_id)\nAS egn ON eg.id = egn.id\n" +
                    "WHERE egn.id = ?";
                IEnumerable <DbEggGroup> eggGroups = await _connection.QueryAsync<DbEggGroup>(token, query, new object[] { displayLanguage, id });
                DbEggGroup eggGroup = eggGroups.First();
                return new EggGroup { Id = eggGroup.Id, Name = eggGroup.Name };
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<ObservableCollection<Stat>> LoadPokemonStats(int formId, GameVersion version, int displayLanguage, CancellationToken token)
        {
            try
            {
                string query = "SELECT ps.pokemon_id, ps.stat_id, sn.name, ps.base_stat, ps.effort FROM pokemon_v2_pokemonstat ps\n" +
                    "LEFT JOIN pokemon_v2_stat s ON ps.stat_id = s.id\n" +
                    "LEFT JOIN\n(SELECT e.stat_id AS id, COALESCE(o.name, e.name) AS name FROM pokemon_v2_statname e\n" +
                    "LEFT OUTER JOIN pokemon_v2_statname o ON e.stat_id = o.stat_id and o.language_id = ?\n" +
                    "WHERE e.language_id = 9\nGROUP BY e.stat_id)\nAS sn ON s.id = sn.id\n" +
                    "WHERE ps.pokemon_id = ?";
                IEnumerable<DbPokemonStat> stats = await _connection.QueryAsync<DbPokemonStat>(token, query, new object[] { displayLanguage, formId });
                query = "SELECT psc.pokemon_id, psc.stat_id, sn.name, psc.base_stat FROM pokemon_v2_pokemonstatchange psc\n" +
                    "LEFT JOIN pokemon_v2_stat s ON psc.stat_id = s.id\n" +
                    "LEFT JOIN\n(SELECT e.stat_id AS id, COALESCE(o.name, e.name) AS name FROM pokemon_v2_statname e\n" +
                    "LEFT OUTER JOIN pokemon_v2_statname o ON e.stat_id = o.stat_id and o.language_id = ?\n" +
                    "WHERE e.language_id = 9\nGROUP BY e.stat_id)\nAS sn ON s.id = sn.id\n" +
                    "WHERE psc.pokemon_id = ? AND psc.min_generation_id >= ? AND psc.max_generation_id <= ?";
                IEnumerable<DbPokemonStat> statChanges = await _connection.QueryAsync<DbPokemonStat>(token, query, new object[] { displayLanguage, formId, version.Generation, version.Generation });                
                List<Stat> statTemp = stats.Select(s => new Stat
                {
                    EffortValue = s.Effort,
                    Id = s.StatId,
                    Name = s.Name,
                    StatValue = s.BaseStat
                }).ToList();
                foreach (DbPokemonStat stat in statChanges)
                {
                    Stat newStat = statTemp.FirstOrDefault(f => f.Id == stat.StatId);
                    if (newStat == null)
                        statTemp.Add(new Stat { Id = stat.StatId, Name = stat.Name, StatValue = stat.BaseStat });
                    else
                        newStat.StatValue = stat.BaseStat;
                }
                return new ObservableCollection<Stat>(statTemp.Where(w => w.StatValue > 0));
            }
            catch (Exception)
            {
                return new ObservableCollection<Stat>();
            }
        }
    }
}
