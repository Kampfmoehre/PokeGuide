using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

using PokeGuide.Model;
using PokeGuide.Model.Database;
using PokeGuide.Service.Interface;

using SQLite.Net.Interop;

using SQLiteNetExtensionsAsync.Extensions;

namespace PokeGuide.Service
{
    public class TestService : BaseDataService, ITestService
    {
        public TestService(IStorageService storageService, ISQLitePlatform sqlitePlatform)
            : base(storageService, sqlitePlatform)
        { }

        public async Task<IEnumerable<Model.Db.GameVersion>> LoadVersionsNewAsync(int language, CancellationToken token)
        {
            try
            {
                return await _connection.LocalizeItemsAsnyc(() => _connection.GetAllWithChildrenAsync<Model.Db.GameVersion>(), token, "version_names", "version_id", 6);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<IEnumerable<GameVersion>> LoadVersionsOldAsync(int language, CancellationToken token)
        {
            try
            {
                string query = "SELECT v.id, vn.name, v.version_group_id, vg.generation_id FROM versions AS v\n" +
                 "LEFT JOIN version_groups AS vg ON v.version_group_id = vg.id\n" +
                 "LEFT JOIN\n(SELECT e.version_id AS id, COALESCE(o.name, e.name) AS name FROM version_names e\n" +
                 "LEFT OUTER JOIN version_names o ON e.version_id = o.version_id and o.local_language_id = ?\n" +
                 "WHERE e.local_language_id = 9\nGROUP BY e.version_id)\nAS vn ON v.id = vn.id";
                IEnumerable<DbVersion> versions = await _connection.QueryAsync<DbVersion>(token, query, new object[] { 6 });
                return versions.Select(s => new GameVersion { Generation = s.Generation, Id = s.Id, Name = s.Name, VersionGroup = s.VersionGroupId });
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<IEnumerable<Model.Db.Ability>> LoadAbilitiesNewAsync(int generation, int language, CancellationToken token)
        {
            try
            {
                return await _connection.LocalizeItemsAsnyc(() => _connection.GetAllWithChildrenAsync<Model.Db.Ability>(w => w.GenerationId == generation), token, "ability_names", "ability_id", 6);
            }
            catch (Exception)
            {
                return null;
            }
        }
        public async Task<IEnumerable<Ability>> LoadAbilitiesOldAsync(int generation, int language, CancellationToken token)
        {
            try
            {
                string query = "SELECT a.id, an.name, a.identifier, a.generation_id, a.is_main_series FROM abilities AS a\n" +
                    "LEFT JOIN\n(SELECT e.ability_id AS id, COALESCE(o.name, e.name) AS name FROM ability_names e\n" +
                    "LEFT OUTER JOIN ability_names o ON e.ability_id = o.ability_id and o.local_language_id = ?\n" +
                    "WHERE e.local_language_id = 9\nGROUP BY e.ability_id)\nAS an ON a.id = an.id\n" +
                    "WHERE a.generation_id = ?";
                IEnumerable<DbAbility> abilities = await _connection.QueryAsync<DbAbility>(token, query, new object[]
                    {
                        language,
                        generation
                    });
                return abilities.Select(s => new Ability { Description = s.Identifier, Effect = s.GenerationId.ToString(), Id = s.Id, Name = s.Name, FlavorText = s.IsMainSeries.ToString() });

            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<List<EggGroup>> LoadEggGroupsAsync(int speciesId, int language, CancellationToken token)
        {
            try
            {
                string query = String.Format("SELECT egg_group_id AS id FROM pokemon_egg_groups WHERE species_id = {0}", speciesId);
                IEnumerable<DbEggGroup> groups = await _connection.QueryAsync<DbEggGroup>(token, query, new object[0]).ConfigureAwait(false);
                var result = new List<EggGroup>();
                foreach (DbEggGroup group in groups)
                {
                    result.Add(await GetEggGroupAsync(group.Id));
                }
                return result;
            }
            catch (Exception)
            {
                return new List<EggGroup>();
            }
        }

        public async Task<Species> LoadSpeciesByIdAsync(int id, int versionGroupId, int language, CancellationToken token)
        {
            try
            {
                Task<List<EggGroup>> loadEggGroupsTask = Task.Factory.StartNew(() => 
                {
                    return LoadEggGroupsAsync(id, language, token).Result;
                }, token);

                string query = String.Format(@"
                    SELECT ps.id, psn.name, psn.genus, ps.gender_rate, ps.capture_rate, ps.base_happiness, ps.hatch_counter, ps.growth_rate_id, grn.name AS growth_rate, dexEntry.id AS dex_id, dexEntry.pokedex_number, dexEntry.name AS dex_name
                    FROM pokemon_species AS ps
                    LEFT JOIN (SELECT e.pokemon_species_id AS id, COALESCE(o.name, e.name) AS name, COALESCE(o.genus, e.genus) AS genus
                               FROM pokemon_species_names e
                               LEFT OUTER JOIN pokemon_species_names o ON e.pokemon_species_id = o.pokemon_species_id AND o.local_language_id = {0}
                               WHERE e.local_language_id = 9
                               GROUP BY e.pokemon_species_id)
                    AS psn ON ps.id = psn.id
                    LEFT JOIN (SELECT dex.id, pdn.species_id, pdn.pokedex_number, pdn.name, pvg.version_group_id
                               FROM pokemon_dex_numbers pdn
                               LEFT JOIN pokedexes dex ON pdn.pokedex_id = dex.id
                               LEFT JOIN pokedex_version_groups pvg ON dex.id = pvg.pokedex_id
                               LEFT JOIN (SELECT e.pokedex_id AS id, COALESCE(o.name, e.name) AS name
                                          FROM pokedex_prose e
                                          LEFT OUTER JOIN pokedex_prose o ON e.pokedex_id = o.pokedex_id AND o.local_language_id = {0}
                                          WHERE e.local_language_id = 9
                                          GROUP BY e.pokedex_id)
                               AS pdn ON dex.id = pdn.id
                               WHERE pvg.version_group_id = {2})
                    AS dexEntry ON ps.id = dexEntry.species_id
                    LEFT JOIN (SELECT e.growth_rate_id AS id, COALESCE(o.name, e.name) AS name
                               FROM growth_rate_prose e
                               LEFT OUTER JOIN growth_rate_prose o ON e.growth_rate_id = o.growth_rate_id AND o.local_language_id = {0}
                               WHERE e.local_language_id = 9
                               GROUP BY e.growth_rate_id)
                    AS grn ON ps.growth_rate_id = grn.id
                    WHERE ps.id = {1}
                    ", language, id, versionGroupId);
                IEnumerable<DbPokemonSpecies> speciesList = await _connection.QueryAsync<DbPokemonSpecies>(token, query, new object[0]).ConfigureAwait(false);
                DbPokemonSpecies temp = speciesList.Single();
                var species = new Species
                {
                    BaseHappiness = temp.BaseHappiness,
                    CatchRate = temp.CaptureRate,
                    GenderRate = CalculateGenderRate(temp.GenderRate),
                    GrowthRate = new GrowthRate { Id = temp.GrowthRateId, Name = temp.GrowthRate },
                    HatchCounter = temp.HatchCounter,
                    Id = temp.Id,
                    Name = temp.Name
                };
                if (temp.PokedexNumber != null)
                    species.DexEntry = new PokedexEntry { DexNumber = (int)temp.PokedexNumber, Id = (int)temp.PokedexId, Name = temp.PokedexName };
                await Task.WhenAll(new Task[] { loadEggGroupsTask }).ConfigureAwait(false);
                List<EggGroup> eggGroups = loadEggGroupsTask.Result;
                species.EggGroup1 = new EggGroup { Id = eggGroups[0].Id, Name = eggGroups[0].Name };
                if (eggGroups.Count == 2)
                    species.EggGroup2 = new EggGroup { Id = eggGroups[1].Id, Name = eggGroups[1].Name };

                return species;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<Species> LoadSpeciesByIdOldAsync(int id, int versionGroupid, int displayLanguage, CancellationToken token)
        {
            try
            {
                string query = "SELECT ps.id, psn.name, psn.genus, ps.gender_rate, ps.capture_rate, ps.base_happiness, ps.hatch_counter, ps.growth_rate_id FROM pokemon_species AS ps\n" +
                    "LEFT JOIN\n(SELECT def.pokemon_species_id AS id, IFNULL(curr.name, def.name) AS name, IFNULL(curr.genus, def.genus) AS genus FROM pokemon_species_names def\n" +
                    "LEFT JOIN pokemon_species_names curr ON def.pokemon_species_id = curr.pokemon_species_id AND def.local_language_id = 9 AND curr.local_language_id = ?\n" +
                    "GROUP BY def.pokemon_species_id)\nAS psn ON ps.id = psn.id\n" +
                    "WHERE ps.id = ?";
                IEnumerable<DbPokemonSpecies> list = await _connection.QueryAsync<DbPokemonSpecies>(token, query, new object[] { displayLanguage, id });
                query = "SELECT p.id, pn.name FROM pokedexes p\n" +
                    "LEFT JOIN\n(SELECT e.pokedex_id AS id, COALESCE(o.name, e.name) AS name FROM pokedex_prose e\n" +
                    "LEFT OUTER JOIN pokedex_prose o ON e.pokedex_id = o.pokedex_id and o.local_language_id = ?\n" +
                    "WHERE e.local_language_id = 9\nGROUP BY e.pokedex_id)\nAS pn ON p.id = pn.id\n" +
                    "LEFT JOIN pokedex_version_groups pvg ON p.id = pvg.pokedex_id\n" +
                    "WHERE pvg.version_group_id = ?";
                IEnumerable<DbPokedex> temp = await _connection.QueryAsync<DbPokedex>(token, query, new object[] { displayLanguage, versionGroupid });
                query = "SELECT egg_group_id FROM pokemon_egg_groups WHERE species_id = ?";
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
                species.GrowthRate = await GetGrowthRateAsync(dbSpecies.GrowthRateId);
                species.DexEntry = await LoadPokedexEntryAsync(dex.Id, dbSpecies.Id, displayLanguage, token);
                species.DexEntry.Name = dex.Name;
                species.EggGroup1 = await LoadEggGroupAsync(eggGroups[0].EggGroupId, displayLanguage, token);
                if (eggGroups.Count > 1)
                    species.EggGroup2 = await LoadEggGroupAsync(eggGroups[1].EggGroupId, displayLanguage, token);
                //species.PossibleEvolutions = await LoadPossibleEvolutionsAsync(id, version, displayLanguage, token);
                species.GenderRate = CalculateGenderRate(dbSpecies.GenderRate);

                return species;
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

        public async Task<EggGroup> LoadEggGroupAsync(int id, int displayLanguage, CancellationToken token)
        {
            try
            {
                string query = "SELECT eg.id, egn.name FROM egg_groups AS eg\n" +
                    "LEFT JOIN\n(SELECT e.egg_group_id AS id, COALESCE(o.name, e.name) AS name FROM egg_group_prose e\n" +
                    "LEFT OUTER JOIN egg_group_prose o ON e.egg_group_id = o.egg_group_id and o.local_language_id = ?\n" +
                    "WHERE e.local_language_id = 9\nGROUP BY e.egg_group_id)\nAS egn ON eg.id = egn.id\n" +
                    "WHERE egn.id = ?";
                IEnumerable<DbEggGroup> eggGroups = await _connection.QueryAsync<DbEggGroup>(token, query, new object[] { displayLanguage, id });
                DbEggGroup eggGroup = eggGroups.First();
                return new EggGroup { Id = eggGroup.Id, Name = eggGroup.Name };
            }
            catch (Exception)
            {
                return null;
            }
        }

        GenderRate CalculateGenderRate(int femaleEights)
        {
            var rate = new GenderRate
            {
                Id = femaleEights
            };

            if (femaleEights == -1)
            {
                rate.Female = null;
                rate.Male = null;
            }
            else
            {
                rate.Female = ((double)femaleEights / 8) * 100;
                rate.Male = ((double)(8 - femaleEights) / 8) * 100;
            }
            return rate;
        }

        public async Task<PokemonForm> LoadFormByIdAsync(int id, GameVersion version, int language, CancellationToken token)
        {
            try
            {
                string query = String.Format(@"
                    SELECT pf.id, pf.pokemon_id, p.species_id, pfn.name, p.height, p.weight, p.base_experience, pt1.type_id AS type1, pt2.type_id AS type2, pa1.ability_id AS ability1, pa2.ability_id AS ability2, pa3.ability_id AS hidden_ability, pi.rarity, pi.item_id
                    FROM pokemon_forms pf
                    LEFT JOIN (SELECT e.pokemon_form_id AS id, COALESCE(o.form_name, e.form_name) AS name
                               FROM pokemon_form_names e
                               LEFT OUTER JOIN pokemon_form_names o ON e.pokemon_form_id = o.pokemon_form_id and o.local_language_id = {0}
                               WHERE e.local_language_id = 9
                               GROUP BY e.pokemon_form_id)
                    AS pfn ON pf.id = pfn.id
                    LEFT JOIN pokemon p ON pf.pokemon_id = p.id
                    LEFT JOIN pokemon_types AS pt1 ON p.id = pt1.pokemon_id AND pt1.slot = 1
                    LEFT JOIN pokemon_types AS pt2 ON p.id = pt2.pokemon_id AND pt2.slot = 2
                    LEFT JOIN pokemon_abilities AS pa1 ON p.id = pa1.pokemon_id AND pa1.slot = 1
                    LEFT JOIN pokemon_abilities AS pa2 ON p.id = pa2.pokemon_id AND pa2.slot = 2
                    LEFT JOIN pokemon_abilities AS pa3 ON p.id = pa3.pokemon_id AND pa3.slot = 3
                    LEFT JOIN pokemon_items AS pi ON p.id = pi.pokemon_id AND pi.version_id = {1}
                    WHERE pf.id = {2}
                ", language, version.Id, id);
                IEnumerable<DbPokemonForm> forms = await _connection.QueryAsync<DbPokemonForm>(token, query, new object[0]).ConfigureAwait(false);
                DbPokemonForm f = forms.Single();
                // Handle Fairy Type before gen 6
                if (version.Generation < 6 && f.Type1 == 18)
                    f.Type1 = 1;
                         
                Task<ElementType> type1Task = GetTypeAsync(f.Type1, version);
                Task<ElementType> type2Task = null;
                Task<Item> itemTask = null;
                Task<Ability> ability1Task = null;
                Task<Ability> ability2Task = null;
                Task<Ability> hiddenAbilityTask = null;
                var tasks = new List<Task> { type1Task, type2Task, itemTask, ability1Task, ability2Task, hiddenAbilityTask };

                if (f.Type2 != null)
                    type2Task = GetTypeAsync((int)f.Type2, version);

                if (version.Generation >= 2 && f.ItemId != null)
                    itemTask = LoadItemByIdAsync((int)f.ItemId, language, token);

                if (version.Generation >= 3)
                {
                    ability1Task = LoadAbilityByIdAsync(f.Ability1, version, language, token);
                    if (f.Ability2 != null)
                        ability2Task = LoadAbilityByIdAsync((int)f.Ability2, version, language, token);
                }
                if (version.Generation >= 5 && f.HiddenAbility != null)
                    hiddenAbilityTask = LoadAbilityByIdAsync((int)f.HiddenAbility, version, language, token);

                var form = new PokemonForm
                {
                    BaseExperience = f.BaseExperience,
                    Height = Math.Round((double)f.Height / 10, 2),
                    HeldItemRarity = f.ItemRarity,
                    Id = f.Id,
                    Weight = Math.Round((double)f.Weight / 10, 2)
                };
                //form.Species = await LoadSpeciesAsync(f.PokemonSpeciesId, version, displayLanguage, token);
                //if (String.IsNullOrWhiteSpace(f.Name))
                //    form.Name = form.Species.Name;
                //else
                //    form.Name = f.Name;
                await Task.WhenAll(tasks.Where(w => w != null));
                form.Type1 = type1Task.Result;
                if (type2Task != null)
                    form.Type2 = type2Task.Result;
                if (itemTask != null)
                    form.HeldItem = itemTask.Result;
                if (ability1Task != null)
                    form.Ability1 = ability1Task.Result;
                if (ability2Task != null)
                    form.Ability2 = ability2Task.Result;
                if (hiddenAbilityTask != null)
                    form.HiddenAbility = hiddenAbilityTask.Result;
                //form.Type1 = await GetTypeAsync(f.Type1, version);
                //if (f.Type2 != null)
                //    form.Type2 = await GetTypeAsync((int)f.Type2, version);
                //if (f.ItemId != null)
                //    form.HeldItem = await LoadItemByIdAsync((int)f.ItemId, language, token);
                //if (version.Generation >= 3)
                //{
                //    form.Ability1 = await LoadAbilityByIdAsync(f.Ability1, version, language, token);
                //    if (f.Ability2 != null)
                //        form.Ability2 = await LoadAbilityByIdAsync((int)f.Ability2, version, language, token);
                //}
                //if (version.Generation >= 5 && f.HiddenAbility != null)
                //    form.HiddenAbility = await LoadAbilityByIdAsync((int)f.HiddenAbility, version, language, token);

                return form;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<Ability> LoadAbilityByIdAsync(int id, GameVersion version, int language, CancellationToken token)
        {
            try
            {
                string query = String.Format(@"
                    SELECT a.id, an.name, aft.flavor_text, ap.short_effect, IFNULL(acp.effect, ap.effect) AS effect
                    FROM abilities AS a
                    LEFT JOIN (SELECT e.ability_id AS id, COALESCE(o.name, e.name) AS name FROM ability_names e
                               LEFT OUTER JOIN ability_names o ON e.ability_id = o.ability_id and o.local_language_id = {0}
                               WHERE e.local_language_id = 9
                               GROUP BY e.ability_id)
                    AS an ON a.id = an.id
                    LEFT JOIN (SELECT e.ability_id AS id, COALESCE(o.flavor_text, e.flavor_text) AS flavor_text
                               FROM ability_flavor_text e
                               LEFT OUTER JOIN ability_flavor_text o ON e.ability_id = o.ability_id and o.language_id = {0}
                               WHERE e.language_id = 9
                               GROUP BY e.ability_id)
                    AS aft ON a.id = aft.id
                    LEFT JOIN (SELECT e.ability_id AS id, COALESCE(o.short_effect, e.short_effect) AS short_effect, COALESCE(o.effect, e.effect) AS effect
                               FROM ability_prose e
                               LEFT OUTER JOIN  ability_prose o ON e.ability_id = o.ability_id and o.local_language_id = {0}
                               WHERE e.local_language_id = 9
                               GROUP BY e.ability_id)
                    AS ap ON a.id = ap.id
                    LEFT JOIN ability_changelog ac ON a.id = ac.ability_id AND ac.changed_in_version_group_id <= {1}
                    LEFT JOIN (SELECT e.ability_changelog_id AS id, COALESCE(o.effect, e.effect) AS effect
                               FROM ability_changelog_prose e
                               LEFT OUTER JOIN ability_changelog_prose o ON e.ability_changelog_id = o.ability_changelog_id and o.local_language_id = {0}
                               WHERE e.local_language_id = 9
                               GROUP BY e.ability_changelog_id)
                    AS acp ON ac.id = acp.id
                    WHERE a.id = {2} AND a.is_main_series = 1 AND a.generation_id <= {3}
                ", language, version.VersionGroup, id, version.Generation);
                IEnumerable<DbAbility> abilities = await _connection.QueryAsync<DbAbility>(token, query, new object[0]).ConfigureAwait(false);
                DbAbility ability = abilities.FirstOrDefault();
                if (ability == null)
                    return null;
                return new Ability
                {
                    Description = ability.ShortEffect,
                    Effect = ability.Effect,
                    FlavorText = ability.FlavorText,
                    Id = ability.Id,
                    Name = ability.Name
                };
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<PokemonForm> LoadFormByIdOldAsync(int formId, GameVersion version, int displayLanguage, CancellationToken token)
        {
            try
            {
                string query = "SELECT pf.id, pf.pokemon_id, p.species_id, pfn.name, p.height, p.weight, p.base_experience, pt1.type_id AS type1, pt2.type_id AS type2, " +
                "pa1.ability_id AS ability1, pa2.ability_id AS ability2, pa3.ability_id AS hidden_ability, pi.rarity, pi.item_id FROM pokemon_forms pf\n" +
                "LEFT JOIN\n(SELECT e.pokemon_form_id AS id, COALESCE(o.form_name, e.form_name) AS name FROM pokemon_form_names e\n" +
                "LEFT OUTER JOIN pokemon_form_names o ON e.pokemon_form_id = o.pokemon_form_id and o.local_language_id = ?\n" +
                "WHERE e.local_language_id = 9\nGROUP BY e.pokemon_form_id)\nAS pfn ON pf.id = pfn.id\n" +
                "LEFT JOIN pokemon p ON pf.pokemon_id = p.id\n" +
                "LEFT JOIN pokemon_types AS pt1 ON p.id = pt1.pokemon_id AND pt1.slot = 1\n" +
                "LEFT JOIN pokemon_types AS pt2 ON p.id = pt2.pokemon_id AND pt2.slot = 2\n" +
                "LEFT JOIN pokemon_abilities AS pa1 ON p.id = pa1.pokemon_id AND pa1.slot = 1\n" +
                "LEFT JOIN pokemon_abilities AS pa2 ON p.id = pa2.pokemon_id AND pa2.slot = 2\n" +
                "LEFT JOIN pokemon_abilities AS pa3 ON p.id = pa3.pokemon_id AND pa3.slot = 3\n" +
                "LEFT JOIN pokemon_items AS pi ON p.id = pi.pokemon_id AND pi.version_id = ?\n" +
                "WHERE pf.id = ?";
                IEnumerable<DbPokemonForm> forms = await _connection.QueryAsync<DbPokemonForm>(token, query, new object[] { displayLanguage, version.Id, formId });
                DbPokemonForm f = forms.First();

                var form = new PokemonForm
                {
                    BaseExperience = f.BaseExperience,
                    Height = Math.Round((double)f.Height / 10, 2),
                    HeldItemRarity = f.ItemRarity,
                    Id = f.Id,
                    Weight = Math.Round((double)f.Weight / 10, 2)
                };
                //form.Species = await LoadSpeciesAsync(f.PokemonSpeciesId, version, displayLanguage, token);
                //if (String.IsNullOrWhiteSpace(f.Name))
                //    form.Name = form.Species.Name;
                //else
                //    form.Name = f.Name;

                // Handle Fairy before Gen 6
                if (version.Generation < 6 && f.Type1 == 18)
                    f.Type1 = 1;
                form.Type1 = await GetTypeAsync(f.Type1, version);
                if (f.Type2 != null)
                    form.Type2 = await GetTypeAsync((int)f.Type2, version);

                if (version.Generation >= 3)
                {
                    form.Ability1 = await LoadAbilityAsync(f.Ability1, version, displayLanguage, token);
                    if (f.Ability2 != null)
                        form.Ability2 = await LoadAbilityAsync((int)f.Ability2, version, displayLanguage, token);
                    if (version.Generation >= 5 && f.HiddenAbility != null)
                        form.HiddenAbility = await LoadAbilityAsync((int)f.HiddenAbility, version, displayLanguage, token);
                }
                if (f.ItemId != null)
                    form.HeldItem = await LoadItemAsync((int)f.ItemId, displayLanguage, token);

                //form.Stats = await LoadPokemonStatsAsync(f.PokemonId, version, displayLanguage, token);

                return form;
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
                string query = "SELECT a.id, an.name, ad.short_effect, ad.effect, aft.flavor_text FROM abilities AS a\n" +
                    "LEFT JOIN\n(SELECT def.ability_id AS id, IFNULL(curr.name, def.name) AS name FROM ability_names def\n" +
                    "LEFT JOIN ability_names curr ON def.ability_id = curr.ability_id AND def.local_language_id = 9 AND curr.local_language_id = ?\n" +
                    "GROUP BY def.ability_id)\nAS an ON a.id = an.id\n" +
                    "LEFT JOIN\n(SELECT def.ability_id AS id, IFNULL(curr.short_effect, def.short_effect) AS short_effect, IFNULL(curr.effect, def.effect) AS effect FROM ability_prose def\n" +
                    "LEFT JOIN ability_prose curr ON def.ability_id = curr.ability_id AND def.local_language_id = 9 AND curr.local_language_id = ?\n" +
                    "GROUP BY def.ability_id)\nAS ad ON a.id = ad.id\n" +
                    "LEFT JOIN\n(SELECT e.ability_id AS id, COALESCE(o.flavor_text, e.flavor_text) AS flavor_text, e.version_group_id FROM ability_flavor_text e\n" +
                    "LEFT OUTER JOIN ability_flavor_text o ON e.ability_id = o.ability_id and o.language_id = ?\n" +
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
                DbAbility dbAbility = abilities.FirstOrDefault();
                if (dbAbility == null)
                    return null;
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

        public async Task<Item> LoadItemAsync(int id, int displayLanguage, CancellationToken token)
        {
            try
            {
                string query = "SELECT it.id, itn.name FROM items it\n" +
                    "LEFT JOIN\n(SELECT e.item_id AS id, COALESCE(o.name, e.name) AS name FROM item_names e\n" +
                    "LEFT OUTER JOIN item_names o ON e.item_id = o.item_id and o.local_language_id = ?\n" +
                    "WHERE e.local_language_id = 9\nGROUP BY e.item_id)\nAS itn ON it.id = itn.id\n" +
                    "WHERE it.id = ?";
                IEnumerable<DbItem> items = await _connection.QueryAsync<DbItem>(token, query, new object[] { displayLanguage, id });
                DbItem item = items.First();
                return new Item { Id = item.Id, Name = item.Name };
            }
            catch (Exception)
            {
                return null;
                throw;
            }
        }

        public async Task<Item> LoadItemByIdAsync(int id, int language, CancellationToken token)
        {
            try
            {
                string query = String.Format(@"
                    SELECT it.id, itn.name 
                    FROM items it
                    LEFT JOIN(SELECT e.item_id AS id, COALESCE(o.name, e.name) AS name
                              FROM item_names e
                              LEFT OUTER JOIN item_names o ON e.item_id = o.item_id and o.local_language_id = {0}
                              WHERE e.local_language_id = 9
                              GROUP BY e.item_id)
                    AS itn ON it.id = itn.id
                    WHERE it.id = {1}
                    ", language, id);
                IEnumerable<DbItem> items = await _connection.QueryAsync<DbItem>(token, query, new object[0]).ConfigureAwait(false);
                DbItem item = items.Single();
                return new Item { Id = item.Id, Name = item.Name };
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<List<Ability>> LoadAbilitiesAsync(int displayLanguage, CancellationToken token)
        {
            try
            {
                string query = String.Format(@"
                    SELECT ab.id, abn.name, abft.flavor_text, abp.short_effect, abp.effect
                    FROM abilities AS ab
                    LEFT JOIN (SELECT e.ability_id AS id, COALESCE(o.name, e.name) AS name
                        FROM ability_names e
                        LEFT OUTER JOIN ability_names o ON e.ability_id = o.ability_id AND o.local_language_id = {0}
                        WHERE e.local_language_id = 9
                        GROUP BY e.ability_id)
                    AS abn ON ab.id = abn.id
                    LEFT JOIN (SELECT e.ability_id AS id, COALESCE(o.flavor_text, e.flavor_text) AS flavor_text, e.version_group_id
                        FROM ability_flavor_text e
                        LEFT OUTER JOIN ability_flavor_text o ON e.ability_id = o.ability_id AND o.language_id = {0}
                        WHERE e.language_id = 9 AND e.version_group_id = 15
                        GROUP BY e.ability_id)
                    AS abft ON ab.id = abft.id
                    LEFT JOIN (SELECT e.ability_id AS id, COALESCE(o.short_effect, e.short_effect) AS short_effect, COALESCE(o.effect, e.effect) AS effect
                        FROM ability_prose e
                        LEFT OUTER JOIN ability_prose o ON e.ability_id = o.ability_id AND o.local_language_id = {0}
                        WHERE e.local_language_id = 9
                        GROUP BY e.ability_id)
                    AS abp ON ab.id = abp.id
                    WHERE ab.is_main_series
                ", displayLanguage);
                IEnumerable<DbAbility> abilities = await _connection.QueryAsync<DbAbility>(token, query, new object[0]).ConfigureAwait(false);
                var result = new List<Ability>();
                foreach (DbAbility ability in abilities)
                {
                    var ab = new Ability
                    {
                        FlavorText = ability.FlavorText,
                        Id = ability.Id,
                        Name = ability.Name
                    };
                    ab.Description = await ProzessAbilityText(ability.ShortEffect, displayLanguage, token);
                    ab.Effect = await ProzessAbilityText(ability.Effect, displayLanguage, token);
                    result.Add(ab);
                }
                return new List<Ability>(result);
            }
            catch (Exception)
            {
                return null;
            }
        }

        async Task<string> ProzessAbilityText(string input, int language, CancellationToken token)
        {
            if (String.IsNullOrWhiteSpace(input))
                return String.Empty;
            string result = input;
            string nameRegexString = @"\[[\w\s]*\]";
            string identifierRegexString = @"{\w*:\w*}";
            var regex = new Regex(nameRegexString + identifierRegexString);
            foreach (Match match in regex.Matches(input))
            {
                string name = String.Empty;
                Match nameMatch = Regex.Match(match.Value, nameRegexString);
                name = nameMatch.Value.Trim(new char[] { '[', ']' });
                if (String.IsNullOrWhiteSpace(name))
                {
                    Match identifierMatch = Regex.Match(match.Value, identifierRegexString);
                    string identifier = identifierMatch.Value.Trim(new char[] { '{', '}' });
                    name = await GetDefaultNameForIdentifier(identifier, language, token);
                }
                result = result.Replace(match.Value, name);
            }
            return result;
        }

        async Task<string> GetDefaultNameForIdentifier(string identifier, int language, CancellationToken token)
        {
            int splitIndex = identifier.IndexOf(':');
            string type = identifier.Remove(splitIndex);
            string element = identifier.Substring(splitIndex + 1);

            switch (type)
            {
                case "mechanic":
                    switch (element)
                    {
                        case "rain":
                            return "Regen";
                        case "speed":
                            return "Initiative";
                        case "hp":
                            return "KP";
                        case "weather":
                            return "Wetter";
                        case "paralysis":
                            return "Paralyse";
                        case "evasion":
                            return "Fluchtwert";
                        case "infatuation":
                            return "Anziehung";
                        case "accuracy":
                            return "Genauigkeit";
                        case "sleep":
                            return "Schlaf";
                        case "poison":
                            return "Vergiftung";
                        case "confusion":
                            return "Verwirrung";
                        case "attack":
                            return "Angriff";
                        case "burn":
                            return "Verbrennung";
                        case "sandstorm":
                            return "Sandsturm";
                        case "pp":
                            return "AP";
                        case "fog":
                            return "Nebel";
                        case "defense":
                            return "Verteidigung";
                        case "hail":
                            return "Hagel";
                        default:
                            break;
                    }
                    break;
                case "move":
                    return await LoadMoveNameByIdentifierAsync(element, language, token);
                case "ability":
                    return await LoadAbilityNameByIdentifierAsync(element, language, token);
                case "type":
                    return await LoadTypeNameByIdentifierAsync(element, language, token);
                case "pokemon":
                    return await LoadPokemonNameByIdentifierAsync(element, language, token);
                case "item":
                    return await LoadItemNameByIdentifierAsync(element, language, token);
                default:
                    break;
            }
            if (type == "mechanic")
            {
                
            }
            return String.Empty;
        }

        async Task<string> LoadMoveNameByIdentifierAsync(string identifier, int language, CancellationToken token)
        {
            try
            {
                string query = String.Format(@"
                    SELECT mo.id, mon.name
                    FROM moves mo
                    LEFT JOIN (SELECT e.move_id AS id, COALESCE(o.name, e.name) AS name
                        FROM move_names e
                        LEFT OUTER JOIN move_names o ON e.move_id = o.move_id AND o.local_language_id = {1}
                        WHERE e.local_language_id = 9
                        GROUP BY e.move_id)
                    AS mon ON mo.id = mon.id
                    WHERE mo.identifier = '{0}'
                ", identifier, language);
                IEnumerable<DbMove> moves = await _connection.QueryAsync<DbMove>(token, query, new object[0]).ConfigureAwait(false);
                DbMove move = moves.Single();
                return move.Name;
            }
            catch (Exception)
            {
                return null;
            }
        }
        async Task<string> LoadAbilityNameByIdentifierAsync(string identifier, int language, CancellationToken token)
        {
            try
            {
                string query = String.Format(@"
                    SELECT ab.id, abn.name
                    FROM abilities ab
                    LEFT JOIN (SELECT e.ability_id AS id, COALESCE(o.name, e.name) AS name
                        FROM ability_names e
                        LEFT OUTER JOIN ability_names o ON e.ability_id = o.ability_id AND o.local_language_id = {1}
                        WHERE e.local_language_id = 9
                        GROUP BY e.ability_id)
                    AS abn ON ab.id = abn.id
                    WHERE ab.identifier = '{0}'
                ", identifier, language);
                IEnumerable<DbAbility> abilities = await _connection.QueryAsync<DbAbility>(token, query, new object[0]).ConfigureAwait(false);
                DbAbility ability = abilities.Single();
                return ability.Name;
            }
            catch (Exception)
            {
                return null;
            }
        }
        async Task<string> LoadTypeNameByIdentifierAsync(string identifier, int language, CancellationToken token)
        {
            try
            {
                string query = String.Format(@"
                    SELECT ty.id, tyn.name
                    FROM types ty
                    LEFT JOIN (SELECT e.type_id AS id, COALESCE(o.name, e.name) AS name
                        FROM type_names e
                        LEFT OUTER JOIN type_names o ON e.type_id = o.type_id AND o.local_language_id = {1}
                        WHERE e.local_language_id = 9
                        GROUP BY e.type_id)
                    AS tyn ON ty.id = tyn.id
                    WHERE ty.identifier = '{0}'
                ", identifier, language);
                IEnumerable<DbAbility> types = await _connection.QueryAsync<DbAbility>(token, query, new object[0]).ConfigureAwait(false);
                DbAbility type = types.Single();
                return type.Name;
            }
            catch (Exception)
            {
                return null;
            }
        }
        async Task<string> LoadPokemonNameByIdentifierAsync(string identifier, int language, CancellationToken token)
        {
            try
            {
                string query = String.Format(@"
                    SELECT ps.id, psn.name
                    FROM pokemon_species ps
                    LEFT JOIN (SELECT e.pokemon_species_id AS id, COALESCE(o.name, e.name) AS name
                        FROM pokemon_species_names e
                        LEFT OUTER JOIN pokemon_species_names o ON e.pokemon_species_id = o.pokemon_species_id AND o.local_language_id = {1}
                        WHERE e.local_language_id = 9
                        GROUP BY e.pokemon_species_id)
                    AS psn ON ps.id = psn.id
                    WHERE ps.identifier = '{0}'
                ", identifier, language);
                IEnumerable<DbPokemonSpecies> pokemon = await _connection.QueryAsync<DbPokemonSpecies>(token, query, new object[0]).ConfigureAwait(false);
                DbPokemonSpecies species = pokemon.Single();
                return species.Name;
            }
            catch (Exception)
            {
                return null;
            }
        }
        async Task<string> LoadItemNameByIdentifierAsync(string identifier, int language, CancellationToken token)
        {
            try
            {
                string query = String.Format(@"
                    SELECT it.id, itn.name
                    FROM items it
                    LEFT JOIN (SELECT e.item_id AS id, COALESCE(o.name, e.name) AS name
                        FROM item_names e
                        LEFT OUTER JOIN item_names o ON e.item_id = o.item_id AND o.local_language_id = {1}
                        WHERE e.local_language_id = 9
                        GROUP BY e.item_id)
                    AS itn ON it.id = itn.id
                    WHERE it.identifier = '{0}'
                ", identifier, language);
                IEnumerable<DbItem> items = await _connection.QueryAsync<DbItem>(token, query, new object[0]).ConfigureAwait(false);
                DbItem item = items.Single();
                return item.Name;
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
