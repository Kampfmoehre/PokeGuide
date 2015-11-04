using System;
using System.Collections.Generic;
using System.Linq;
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

        public async Task<IEnumerable<Species>> LoadSpeciesByIdAsync(int id, int versionGroupId, int language, CancellationToken token)
        {
            try
            {
                string query = String.Format(@"
                    SELECT ps.id, psn.name, psn.genus, ps.gender_rate, ps.capture_rate, ps.base_happiness, ps.hatch_counter, grn.name AS growth_rate, dexEntry.pokedex_number, dexEntry.name AS dex_name
                    FROM pokemon_species AS ps
                    LEFT JOIN (SELECT e.pokemon_species_id AS id, COALESCE(o.name, e.name) AS name, COALESCE(o.genus, e.genus) AS genus
                               FROM pokemon_species_names e
                               LEFT OUTER JOIN pokemon_species_names o ON e.pokemon_species_id = o.pokemon_species_id AND o.local_language_id = {0}
                               WHERE e.local_language_id = 9
                               GROUP BY e.pokemon_species_id)
                    AS psn ON ps.id = psn.id
                    LEFT JOIN (SELECT pdn.species_id, pdn.pokedex_number, pdn.name, pvg.version_group_id
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

            }
        }

        public async Task<PokemonForm> LoadFormByIdAsync(int id, int version, int language, CancellationToken token)
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
                ", language, version, id)
            }
        }
    }
}
