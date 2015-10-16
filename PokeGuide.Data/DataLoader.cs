using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SQLite;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using PokeGuide.Data.Model;

namespace PokeGuide.Data
{
    /// <summary>
    /// Loads data from the database
    /// </summary>
    public class DataLoader : IDisposable
    {
        /// <summary>
        /// The connection
        /// </summary>
        protected SQLiteConnection Connection { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="DataLoader"/> class
        /// </summary>
        /// <param name="databaseName">The name of the database to load</param>                     
        public DataLoader(string databaseName)
        {
            string dllPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            string dataSource = Path.Combine(dllPath, databaseName);
            if (!File.Exists(dataSource))
                throw new Exception(String.Format("Could not find database at {0}", dataSource));

            string conString = String.Format("Data Source={0}", dataSource);
            Connection = new SQLiteConnection(conString);
            Connection.Open();            
        }

        /// <summary>
        /// Executes an sql query against the database and returns the reader with the result
        /// </summary>
        /// <param name="sql">The full query</param>
        /// <param name="token">A token to cancel the query</param>
        /// <returns>A <see cref="DbDataReader"/> with the result</returns>
        internal async Task<DbDataReader> ExecuteReaderAsync(string sql, CancellationToken token)
        {
            var command = new SQLiteCommand(Connection) { CommandText = sql };

            return await command.ExecuteReaderAsync(token);
        }

        /// <summary>
        /// Loads all games that exists in the database
        /// </summary>
        /// <param name="language">The ID of the language</param>
        /// <param name="token">A token to cancel the query</param>
        /// <returns>A list of <see cref="GameVersion"/>s.</returns>
        public async Task<List<GameVersion>> LoadGamesAsync(int language, CancellationToken token)
        {
            string sql = String.Format("SELECT v.id, vn.name FROM pokemon_v2_version as v INNER JOIN pokemon_v2_versionname as vn on v.id = vn.version_id where vn.language_id = {0}", language);

            var mapping = new List<Mapping>
            {
                new Mapping { PropertyName = "Id", Column = 0, TypeToCast = typeof(Int32) },
                new Mapping { PropertyName = "Name", Column = 1, TypeToCast = typeof(String) }
            };
            var mapper = new DatabaseMapper<GameVersion>();
            DbDataReader reader = await ExecuteReaderAsync(sql, token);

            return mapper.MapFromQuery(reader, mapping);
        }

        /// <summary>
        /// Loads all abilities for a given version and fills description text in the given language if present
        /// </summary>
        /// <param name="version">The ID of the version</param>
        /// <param name="language">The ID of the language</param>
        /// <param name="token">A token to cancel the query</param>
        /// <returns>A list of <see cref="Ability"/>s.</returns>
        public async Task<List<Ability>> LoadAbilitiesAsync(int version, int language, CancellationToken token)
        {
            string sql = String.Format("SELECT a.id, an.name, ad.short_effect, ad.effect, af.flavor_text, af.version FROM pokemon_v2_ability AS a\n");
            sql = String.Format("{0}LEFT JOIN\n(SELECT def.ability_id AS id, IFNULL(curr.name, def.name) AS name FROM pokemon_v2_abilityname def\n", sql);
            sql = String.Format("{0} LEFT JOIN pokemon_v2_abilityname curr ON def.ability_id = curr.ability_id AND def.language_id = 9 AND curr.language_id = {1}\n", sql, language);
            sql = String.Format("{0} GROUP BY def.ability_id)\nAS an ON a.id = an.id\n", sql);
            sql = String.Format("{0} LEFT JOIN\n(SELECT def.ability_id AS id, IFNULL(curr.short_effect, def.short_effect) AS short_effect, IFNULL(curr.effect, def.effect) AS effect FROM pokemon_v2_abilitydescription def\n", sql);
            sql = String.Format("{0} LEFT JOIN pokemon_v2_abilitydescription curr ON def.ability_id = curr.ability_id AND def.language_id = 9 AND curr.language_id = {1}\n", sql, language);
            sql = String.Format("{0} GROUP BY def.ability_id)\nAS ad ON a.id = ad.id\n", sql);
            sql = String.Format("{0} LEFT JOIN\n(SELECT def.ability_id AS id, vg.id as version, IFNULL(curr.flavor_text, def.flavor_text) AS flavor_text FROM pokemon_v2_abilityflavortext def\n", sql);
            sql = String.Format("{0} LEFT JOIN pokemon_v2_versiongroup vg ON def.version_group_id = vg.id AND vg.id =\n", sql);
            sql = String.Format("{0} (SELECT version_group_id FROM pokemon_v2_version WHERE id = {1})\n", sql, version);
            sql = String.Format("{0} LEFT JOIN pokemon_v2_abilityflavortext curr ON def.ability_id = curr.ability_id AND def.language_id = 9 AND curr.language_id = {1}\n", sql, language);
            sql = String.Format("{0} GROUP BY def.id)\nAS af ON a.id = af.id\nWHERE af.version IS NOT NULL", sql);
            DbDataReader reader = await ExecuteReaderAsync(sql, token);

            var mapping = new List<Mapping>
            {
                new Mapping { Column = 0, PropertyName = "Id", TypeToCast = typeof(Int32) },
                new Mapping { Column = 1, PropertyName = "Name", TypeToCast = typeof(String) },
                new Mapping { Column = 2, PropertyName = "Effect", TypeToCast = typeof(String) },
                new Mapping { Column = 3, PropertyName = "Description", TypeToCast = typeof(String) },
                new Mapping { Column = 4, PropertyName = "FlavorText", TypeToCast = typeof(String) }
            };

            var mapper = new DatabaseMapper<Ability>();
            return mapper.MapFromQuery(reader, mapping);
        }

        public async Task<Ability> LoadAbilityAsync(int id, int version, int language, CancellationToken token)
        {
            string sql = String.Format("SELECT a.id, an.name, ad.short_effect, ad.effect, af.flavor_text, af.version FROM pokemon_v2_ability AS a\n");
            sql = String.Format("{0}LEFT JOIN\n(SELECT def.ability_id AS id, IFNULL(curr.name, def.name) AS name FROM pokemon_v2_abilityname def\n", sql);
            sql = String.Format("{0} LEFT JOIN pokemon_v2_abilityname curr ON def.ability_id = curr.ability_id AND def.language_id = 9 AND curr.language_id = {1}\n", sql, language);
            sql = String.Format("{0} GROUP BY def.ability_id)\nAS an ON a.id = an.id\n", sql);
            sql = String.Format("{0} LEFT JOIN\n(SELECT def.ability_id AS id, IFNULL(curr.short_effect, def.short_effect) AS short_effect, IFNULL(curr.effect, def.effect) AS effect FROM pokemon_v2_abilitydescription def\n", sql);
            sql = String.Format("{0} LEFT JOIN pokemon_v2_abilitydescription curr ON def.ability_id = curr.ability_id AND def.language_id = 9 AND curr.language_id = {1}\n", sql, language);
            sql = String.Format("{0} GROUP BY def.ability_id)\nAS ad ON a.id = ad.id\n", sql);
            sql = String.Format("{0} LEFT JOIN\n(SELECT def.ability_id AS id, vg.id as version, IFNULL(curr.flavor_text, def.flavor_text) AS flavor_text FROM pokemon_v2_abilityflavortext def\n", sql);
            sql = String.Format("{0} LEFT JOIN pokemon_v2_versiongroup vg ON def.version_group_id = vg.id AND vg.id =\n", sql);
            sql = String.Format("{0} (SELECT version_group_id FROM pokemon_v2_version WHERE id = {1})\n", sql, version);
            sql = String.Format("{0} LEFT JOIN pokemon_v2_abilityflavortext curr ON def.ability_id = curr.ability_id AND def.language_id = 9 AND curr.language_id = {1}\n", sql, language);
            sql = String.Format("{0} GROUP BY def.id)\nAS af ON a.id = af.id\nWHERE a.id = {1} AND af.version IS NOT NULL", sql, id);
            DbDataReader reader = await ExecuteReaderAsync(sql, token);
            var mapping = new List<Mapping>
            {
                new Mapping { Column = 0, PropertyName = "Id", TypeToCast = typeof(Int32) },
                new Mapping { Column = 1, PropertyName = "Name", TypeToCast = typeof(String) },
                new Mapping { Column = 2, PropertyName = "Effect", TypeToCast = typeof(String) },
                new Mapping { Column = 3, PropertyName = "Description", TypeToCast = typeof(String) },
                new Mapping { Column = 4, PropertyName = "FlavorText", TypeToCast = typeof(String) }
            };
            var mapper = new DatabaseMapper<Ability>();
            return await mapper.MapSingleObject(reader, mapping);
        }

        /// <summary>
        /// Loads all egg groups with names in a given language
        /// </summary>
        /// <param name="language">The ID of the language</param>
        /// <param name="token">A token to cancel the query</param>
        /// <returns>A list of <see cref="EggGroup"/>s.</returns>
        public async Task<List<EggGroup>> LoadEggGroupsAsync(int language, CancellationToken token)
        {
            string sql = String.Format("SELECT e.id, en.name FROM pokemon_v2_egggroup as e\n");
            sql = String.Format("{0} LEFT JOIN\n(SELECT def.egg_group_id AS id, IFNULL(curr.name, def.name) AS name FROM pokemon_v2_egggroupname def\n", sql);
            sql = String.Format("{0} LEFT JOIN pokemon_v2_egggroupname curr ON def.egg_group_id = curr.egg_group_id AND def.language_id = 9 AND curr.language_id = {1}\n", sql, language);
            sql = String.Format("{0} GROUP BY def.egg_group_id)\nAS en ON e.id = en.id", sql);
            DbDataReader reader = await ExecuteReaderAsync(sql, token);

            var mapping = new List<Mapping>
            {
                new Mapping { Column = 0, PropertyName = "Id", TypeToCast = typeof(Int32) },
                new Mapping { Column = 1, PropertyName = "Name", TypeToCast = typeof(String) }
            };

            var mapper = new DatabaseMapper<EggGroup>();
            return mapper.MapFromQuery(reader, mapping);
        }

        public async Task<EggGroup> LoadEggGroupAsync(int id, int language, CancellationToken token)
        {
            string sql = String.Format("SELECT e.id, en.name FROM pokemon_v2_egggroup as e\n");
            sql = String.Format("{0} LEFT JOIN\n(SELECT def.egg_group_id AS id, IFNULL(curr.name, def.name) AS name FROM pokemon_v2_egggroupname def\n", sql);
            sql = String.Format("{0} LEFT JOIN pokemon_v2_egggroupname curr ON def.egg_group_id = curr.egg_group_id AND def.language_id = 9 AND curr.language_id = {1}\n", sql, language);
            sql = String.Format("{0} GROUP BY def.egg_group_id)\nAS en ON e.id = en.id\nWHERE e.id = {1}", sql, id);
            DbDataReader reader = await ExecuteReaderAsync(sql, token);

            var mapping = new List<Mapping>
            {
                new Mapping { Column = 0, PropertyName = "Id", TypeToCast = typeof(Int32) },
                new Mapping { Column = 1, PropertyName = "Name", TypeToCast = typeof(String) }
            };

            var mapper = new DatabaseMapper<EggGroup>();
            return await mapper.MapSingleObject(reader, mapping);
        }

        /// <summary>
        /// Loads all types with names in a given language
        /// </summary>
        /// <param name="language">The ID of the language</param>
        /// <param name="token">A token to cancel the query</param>
        /// <returns>A list of <see cref="ElementType"/>s.</returns>
        public async Task<List<ElementType>> LoadTypesAsync(int language, CancellationToken token)
        {
            string sql = String.Format("SELECT t.id, tn.name FROM pokemon_v2_type as t\n");
            sql = String.Format("{0} LEFT JOIN\n(SELECT def.type_id AS id, IFNULL(curr.name, def.name) AS name FROM pokemon_v2_typename def\n", sql);
            sql = String.Format("{0} LEFT JOIN pokemon_v2_typename curr ON def.type_id = curr.type_id AND def.language_id = 9 AND curr.language_id = {1}\n", sql, language);
            sql = String.Format("{0} GROUP BY def.type_id)\nAS tn ON t.id = tn.id", sql);
            DbDataReader reader = await ExecuteReaderAsync(sql, token);

            var mapping = new List<Mapping>
            {
                new Mapping { Column = 0, PropertyName = "Id", TypeToCast = typeof(Int32) },
                new Mapping { Column = 1, PropertyName = "Name", TypeToCast = typeof(String) }
            };

            var mapper = new DatabaseMapper<ElementType>();
            return mapper.MapFromQuery(reader, mapping);
        }

        public async Task<List<Pokemon>> LoadAllPokemonAsync(int version, int language, CancellationToken token)
        {
            string sql = String.Format("SELECT ps.id, psn.name FROM pokemon_v2_pokemonspecies as ps\n");
            sql = String.Format("{0} LEFT JOIN\n(SELECT def.pokemon_species_id AS id, IFNULL(curr.name, def.name) AS name, IFNULL(curr.genus, def.genus) AS genus FROM pokemon_v2_pokemonspeciesname def\n", sql);
            sql = String.Format("{0} LEFT JOIN pokemon_v2_pokemonspeciesname curr ON def.pokemon_species_id = curr.pokemon_species_id AND def.language_id = 9 AND curr.language_id = {1}\n", sql, language);
            sql = String.Format("{0} GROUP BY def.pokemon_species_id)\nAS psn ON ps.id = psn.id\n", sql);
            sql = String.Format("{0} LEFT JOIN pokemon_v2_pokemonform pf ON ps.id = pf.pokemon_id\n", sql);
            sql = String.Format("{0} WHERE pf.version_group_id = (SELECT version_group_id FROM pokemon_v2_version WHERE id = {1})\n", sql, version);
            sql = String.Format("{0} order by ps.id", sql);
            DbDataReader reader = await ExecuteReaderAsync(sql, token);

            var mapping = new List<Mapping>
            {
                new Mapping { Column = 0, PropertyName = "Id", TypeToCast = typeof(Int32) },
                new Mapping { Column = 1, PropertyName = "Name", TypeToCast = typeof(String) }
            };

            var mapper = new DatabaseMapper<Pokemon>();
            return mapper.MapFromQuery(reader, mapping);
        }

        public async Task<Pokemon> LoadPokemonAsync(int id, int version, int language, CancellationToken token)
        {
            string sql = String.Format("SELECT * FROM pokemon_v2_pokemon WHERE id = {0}", id);
            DbDataReader reader = await ExecuteReaderAsync(sql, token);
            var pokemon = new Pokemon();
            if (await reader.ReadAsync(token))
            {
                pokemon.Height = Convert.ToDouble(reader["height"]) / 10;
                pokemon.Id = Convert.ToInt32(reader["id"]);
                pokemon.Weight = Convert.ToDouble(reader["weight"]) / 10;
            }

            sql = String.Format("SELECT * FROM pokemon_v2_pokemonability WHERE pokemon_id = {0}", id);
            DbDataReader reader2 = await ExecuteReaderAsync(sql, token);
            while (await reader2.ReadAsync(token))
            {
                int abilityId = reader2.GetInt32(reader2.GetOrdinal("ability_id"));
                int slot = reader2.GetInt32(reader2.GetOrdinal("slot"));
                Ability ability = await LoadAbilityAsync(abilityId, version, language, token);
                switch (slot)
                {
                    case 1:
                        pokemon.Ability1 = ability;
                        break;
                    case 2:
                        pokemon.Ability2 = ability;
                        break;
                    case 3:
                        pokemon.HiddenAbility = ability;
                        break;
                }
            }

            sql = String.Format("SELECT ps.id, ps.capture_rate, ps.base_happiness, ps.hatch_counter, psn.name, psn.genus FROM pokemon_v2_pokemonspecies as ps\n");
            sql = String.Format("{0} LEFT JOIN\n(SELECT def.pokemon_species_id AS id, IFNULL(curr.name, def.name) AS name, IFNULL(curr.genus, def.genus) AS genus FROM pokemon_v2_pokemonspeciesname def\n", sql);
            sql = String.Format("{0} LEFT JOIN pokemon_v2_pokemonspeciesname curr ON def.pokemon_species_id = curr.pokemon_species_id AND def.language_id = 9 AND curr.language_id = {1}\n", sql, language);
            sql = String.Format("{0} GROUP BY def.pokemon_species_id)\nAS psn ON ps.id = psn.id", sql);
            sql = String.Format("{0} WHERE ps.id = {1} order by ps.id", sql, id);

            DbDataReader reader3 = await ExecuteReaderAsync(sql, token);
            if (await reader3.ReadAsync(token))
            {
                pokemon.Name = reader3.GetString(reader3.GetOrdinal("name"));
                pokemon.Genus = reader3.GetString(reader3.GetOrdinal("genus"));
                pokemon.CaptureRate = reader3.GetInt32(reader3.GetOrdinal("capture_rate"));
                pokemon.BaseHappiness = reader3.GetInt32(reader3.GetOrdinal("base_happiness"));
                pokemon.HatchCounter = reader3.GetInt32(reader3.GetOrdinal("hatch_counter"));
            }

            sql = String.Format("SELECT * FROM pokemon_v2_pokemonegggroup WHERE pokemon_species_id = {0}", id);
            DbDataReader reader4 = await ExecuteReaderAsync(sql, token);
            int counter = 1;
            while (await reader4.ReadAsync(token))
            {
                int eggGroupId = reader4.GetInt32(reader4.GetOrdinal("egg_group_id"));
                EggGroup eggGroup = await LoadEggGroupAsync(eggGroupId, language, token);
                if (counter == 1)
                    pokemon.EggGroup1 = eggGroup;
                else
                    pokemon.EggGroup2 = eggGroup;
                counter++;
            }
            return pokemon;
        }

        /// <summary>
        /// Disposes the object and closes the connection.
        /// </summary>
        public void Dispose()
        {
            Connection.Close();
        }
    }
}
