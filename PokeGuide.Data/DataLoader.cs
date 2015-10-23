using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SQLite;
using System.IO;
using System.Linq;
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
        /// Executes a sql query and returns the first result
        /// </summary>
        /// <param name="sql">The sql command</param>
        /// <param name="token">The cancellation token</param>
        /// <returns>The first result of the query</returns>
        internal async Task<object> ExecuteScalarAsync(string sql, CancellationToken token)
        {
            var command = new SQLiteCommand(Connection) { CommandText = sql };

            return await command.ExecuteScalarAsync(token);
        }

        /// <summary>
        /// Executes a sql query against the database and returns the reader with the result
        /// </summary>
        /// <param name="sql">The full query</param>
        /// <param name="token">A token to cancel the query</param>
        /// <returns>A <see cref="DbDataReader"/> with the result</returns>
        internal async Task<DbDataReader> ExecuteReaderAsync(string sql, CancellationToken token)
        {
            var command = new SQLiteCommand(Connection) { CommandText = sql };

            return await command.ExecuteReaderAsync(token);
        }

        public async Task<int> GetGenerationFromVersionGroup(int versionGroup, CancellationToken token)
        {
            string sql = String.Format("SELECT generation_id FROM pokemon_v2_versiongroup WHERE id = {0}", versionGroup);
            object generation = await ExecuteScalarAsync(sql, token);
            return Convert.ToInt32(generation);
        }

        /// <summary>
        /// Loads a list of all languages with localized names
        /// </summary>
        /// <param name="language">The ID of the language</param>
        /// <param name="token">A token to cancel the query</param>
        /// <returns>A list of languages</returns>
        public async Task<List<Language>> LoadLanguagesAsync(int language, CancellationToken token)
        {
            var mapper = new DatabaseMapper<Language>(Connection);
            return await mapper.MapFromQueryAsync(new object[] { language }, null, token);
        }

        /// <summary>
        /// Loads all games that exists in the database.
        /// </summary>
        /// <param name="language">The ID of the language</param>
        /// <param name="progress">An interface to track progress of the query</param>
        /// <param name="token">A token to cancel the query</param>
        /// <returns>A list of <see cref="GameVersion"/>s.</returns>
        public async Task<List<GameVersion>> LoadGamesAsync(int language, IProgress<double> progress, CancellationToken token)
        {
            var mapper = new DatabaseMapper<GameVersion>(Connection);
            return await mapper.MapFromQueryAsync(new object[] { language }, progress, token);
        }

        /// <summary>
        /// Loads all abilities for a given version and fills description text in the given language if present.
        /// </summary>
        /// <param name="versionGroup">The ID of the version group</param>
        /// <param name="language">The ID of the language</param>
        /// <param name="progress">An interface to track the query progress</param>
        /// <param name="token">A token to cancel the query</param>
        /// <returns>A list of <see cref="Ability"/>s.</returns>
        public async Task<List<Ability>> LoadAbilitiesAsync(int versionGroup, int language, IProgress<double> progress, CancellationToken token)
        {
            var mapper = new DatabaseMapper<Ability>(Connection);
            return await mapper.MapFromQueryAsync(new object[] { language, versionGroup }, progress, token);
        }

        /// <summary>
        /// Loads an ability for a given version with name and description in a given language if present.
        /// If the ability does not exist in the given version, <c>null</c> is returned.
        /// </summary>
        /// <param name="id">The ID of the ability</param>
        /// <param name="versionGroup">The ID of the game version</param>
        /// <param name="language">The ID of the language</param>
        /// <param name="token">A token to cancel the query</param>
        /// <returns>The ability</returns>
        public async Task<Ability> LoadAbilityAsync(int id, int versionGroup, int language, CancellationToken token)
        {
            var mapper = new DatabaseMapper<Ability>(Connection);
            return await mapper.MapSingleObjectAsync(new object[] { language, versionGroup, id }, token);
        }

        /// <summary>
        /// Loads all egg groups with names in a given language
        /// </summary>
        /// <param name="language">The ID of the language</param>
        /// <param name="token">A token to cancel the query</param>
        /// <returns>A list of <see cref="EggGroup"/>s.</returns>
        public async Task<List<EggGroup>> LoadEggGroupsAsync(int language, CancellationToken token)
        {
            var mapper = new DatabaseMapper<EggGroup>(Connection);
            return await mapper.MapFromQueryAsync(new object[] { language }, null, token);
        }

        /// <summary>
        /// Loads an egg group with name in the given language
        /// </summary>
        /// <param name="id">The ID of the egg group</param>
        /// <param name="language">The ID of the language</param>
        /// <param name="token">A token to cancel the query</param>
        /// <returns>An egg group</returns>
        public async Task<EggGroup> LoadEggGroupAsync(int id, int language, CancellationToken token)
        {
            var mapper = new DatabaseMapper<EggGroup>(Connection);
            return await mapper.MapSingleObjectAsync(new object[] { language, id }, token);
        }

        /// <summary>
        /// Loads all types with names in a given language
        /// </summary>
        /// <param name="language">The ID of the language</param>
        /// <param name="generation">The ID of the generation</param>
        /// <param name="token">A token to cancel the query</param>
        /// <returns>A list of <see cref="ElementType"/>s.</returns>
        public async Task<List<ElementType>> LoadTypesAsync(int generation, int language, CancellationToken token)
        {
            var mapper = new DatabaseMapper<ElementType>(Connection);
            return await mapper.MapFromQueryAsync(new object[] { language, generation }, null, token);
        }

        /// <summary>
        /// Loads a type with localized name.
        /// If the type does not exist in the given generation, <c>null</c> is returned.
        /// </summary>
        /// <param name="id">The ID of the type</param>
        /// <param name="generation">The ID of the game generation</param>
        /// <param name="language">The ID of the language</param>
        /// <param name="token">A token to cancel the query</param>
        /// <returns>A type</returns>
        public async Task<ElementType> LoadTypeAsync(int id, int generation, int language, CancellationToken token)
        {
            var mapper = new DatabaseMapper<ElementType>(Connection);
            // Convert Fairy to normal before 6th gen
            if (generation < 6 && id == 18)
                id = 1;
            return await mapper.MapSingleObjectAsync(new object[] { language, generation, id }, token);
        }

        /// <summary>
        /// Loads all species with localized names
        /// </summary>
        /// <param name="generation">The ID of the generation</param>
        /// <param name="language">The ID of the language</param>
        /// <param name="progress">An interface to track the progress of the query</param>
        /// <param name="token">A token to cancel the query</param>
        /// <returns>A list of species</returns>
        public async Task<List<Species>> LoadAllSpeciesAsync(int generation, int language, IProgress<double> progress, CancellationToken token)
        {
            var mapper = new DatabaseMapper<Species>(Connection);
            return await mapper.MapFromQueryAsync(new object[] { language, generation }, progress, token);
        }

        /// <summary>
        /// Loads all forms for a species with localized names
        /// </summary>
        /// <param name="species">The ID of the species</param>
        /// <param name="versionGroup">The ID of the version group</param>
        /// <param name="language">The ID of the language</param>
        /// <param name="token">A token to cancel the query</param>
        /// <returns>A list of forms</returns>
        public async Task<List<PokemonForm>> LoadFormsAsync(int species, int versionGroup, int language, IProgress<double> progress, CancellationToken token)
        {
            var subProgress = new Progress<double>();
            subProgress.ProgressChanged += (s, e) => { progress.Report(Math.Round(e / 2, 1)); };
            var mapper = new DatabaseMapper<PokemonForm>(Connection);
            List<PokemonForm> result = await mapper.MapFromQueryAsync(new object[] { language, species, versionGroup }, subProgress, token);
            int gen = await GetGenerationFromVersionGroup(versionGroup, token);
            //foreach (PokemonForm form in result)
            //{
            //    form.Type1 = await LoadTypeAsync(form.Type1.Id, gen, language, token);
            //    if (form.Type2 != null)
            //        form.Type2 = await LoadTypeAsync(form.Type2.Id, gen, language, token);
            //    form.Ability1 = await LoadAbilityAsync(form.Ability1.Id, versionGroup, language, token);
            //    if (form.Ability2 != null)
            //        form.Ability2 = await LoadAbilityAsync(form.Ability2.Id, versionGroup, language, token);
            //    if (form.HiddenAbility != null)
            //        form.HiddenAbility = await LoadAbilityAsync(form.HiddenAbility.Id, versionGroup, language, token);
            //}
            //Parallel.ForEach<PokemonForm>(result, async x =>
            //{
            //    x.Type1 = await LoadTypeAsync(x.Type1.Id, gen, language, token);
            //    if (x.Type2 != null)
            //        x.Type2 = await LoadTypeAsync(x.Type2.Id, gen, language, token);
            //    x.Ability1 = await LoadAbilityAsync(x.Ability1.Id, versionGroup, language, token);
            //    if (x.Ability2 != null)
            //        x.Ability2 = await LoadAbilityAsync(x.Ability2.Id, versionGroup, language, token);
            //    if (x.HiddenAbility != null)
            //        x.HiddenAbility = await LoadAbilityAsync(x.HiddenAbility.Id, versionGroup, language, token);
            //});
            int processed = result.Count;
            result.ForEach(async x =>
            {
                x.Type1 = await LoadTypeAsync(x.Type1.Id, gen, language, token);
                if (x.Type2 != null)
                    x.Type2 = await LoadTypeAsync(x.Type2.Id, gen, language, token);
                x.Ability1 = await LoadAbilityAsync(x.Ability1.Id, versionGroup, language, token);
                if (x.Ability2 != null)
                    x.Ability2 = await LoadAbilityAsync(x.Ability2.Id, versionGroup, language, token);
                if (gen >= 5 && x.HiddenAbility != null)
                    x.HiddenAbility = await LoadAbilityAsync(x.HiddenAbility.Id, versionGroup, language, token);
                processed++;
                progress.Report(Math.Round(processed / (double)(result.Count * 2) * 100, 1));
            });
            return result;
        }        

        //public async Task<List<Pokemon>> LoadAllPokemonAsync(int version, int language, CancellationToken token)
        //{
        //    string sql = String.Format("SELECT ps.id, rtrim(psn.name || ' ' || pf.form_name) AS name FROM pokemon_v2_pokemonspecies as ps\n");
        //    sql = String.Format("{0} LEFT JOIN\n(SELECT def.pokemon_species_id AS id, IFNULL(curr.name, def.name) AS name, IFNULL(curr.genus, def.genus) AS genus FROM pokemon_v2_pokemonspeciesname def\n", sql);
        //    sql = String.Format("{0} LEFT JOIN pokemon_v2_pokemonspeciesname curr ON def.pokemon_species_id = curr.pokemon_species_id AND def.language_id = 9 AND curr.language_id = {1}\n", sql, language);
        //    sql = String.Format("{0} GROUP BY def.pokemon_species_id)\nAS psn ON ps.id = psn.id\n", sql);
        //    sql = String.Format("{0} LEFT JOIN pokemon_v2_pokemonform pf ON ps.id = pf.pokemon_id\n", sql);
        //    sql = String.Format("{0} WHERE pf.version_group_id <= (SELECT version_group_id FROM pokemon_v2_version WHERE id = {1})\n", sql, version);
        //    sql = String.Format("{0} order by ps.id", sql);
        //    DbDataReader reader = await ExecuteReaderAsync(sql, token);

        //    var mapper = new DatabaseMapper<Pokemon>();
        //    return mapper.MapFromQueryAsync(reader);
        //}

        //public async Task<Pokemon> LoadPokemonAsync(int id, int version, int language, CancellationToken token)
        //{
        //    string sql = String.Format("SELECT * FROM pokemon_v2_pokemon WHERE id = {0}", id);
        //    DbDataReader reader = await ExecuteReaderAsync(sql, token);
        //    var pokemon = new Pokemon();
        //    if (await reader.ReadAsync(token))
        //    {
        //        pokemon.Height = Convert.ToDouble(reader["height"]) / 10;
        //        pokemon.Id = Convert.ToInt32(reader["id"]);
        //        pokemon.Weight = Convert.ToDouble(reader["weight"]) / 10;
        //    }

        //    sql = String.Format("SELECT * FROM pokemon_v2_pokemonability WHERE pokemon_id = {0}", id);
        //    DbDataReader reader2 = await ExecuteReaderAsync(sql, token);
        //    while (await reader2.ReadAsync(token))
        //    {
        //        int abilityId = reader2.GetInt32(reader2.GetOrdinal("ability_id"));
        //        int slot = reader2.GetInt32(reader2.GetOrdinal("slot"));
        //        Ability ability = await LoadAbilityAsync(abilityId, version, language, token);
        //        switch (slot)
        //        {
        //            case 1:
        //                pokemon.Ability1 = ability;
        //                break;
        //            case 2:
        //                pokemon.Ability2 = ability;
        //                break;
        //            case 3:
        //                pokemon.HiddenAbility = ability;
        //                break;
        //        }
        //    }

        //    sql = String.Format("SELECT ps.id, ps.capture_rate, ps.base_happiness, ps.hatch_counter, psn.name, psn.genus FROM pokemon_v2_pokemonspecies as ps\n");
        //    sql = String.Format("{0} LEFT JOIN\n(SELECT def.pokemon_species_id AS id, IFNULL(curr.name, def.name) AS name, IFNULL(curr.genus, def.genus) AS genus FROM pokemon_v2_pokemonspeciesname def\n", sql);
        //    sql = String.Format("{0} LEFT JOIN pokemon_v2_pokemonspeciesname curr ON def.pokemon_species_id = curr.pokemon_species_id AND def.language_id = 9 AND curr.language_id = {1}\n", sql, language);
        //    sql = String.Format("{0} GROUP BY def.pokemon_species_id)\nAS psn ON ps.id = psn.id", sql);
        //    sql = String.Format("{0} WHERE ps.id = {1} order by ps.id", sql, id);

        //    DbDataReader reader3 = await ExecuteReaderAsync(sql, token);
        //    if (await reader3.ReadAsync(token))
        //    {
        //        pokemon.Name = reader3.GetString(reader3.GetOrdinal("name"));
        //        pokemon.Genus = reader3.GetString(reader3.GetOrdinal("genus"));
        //        pokemon.CaptureRate = reader3.GetInt32(reader3.GetOrdinal("capture_rate"));
        //        pokemon.BaseHappiness = reader3.GetInt32(reader3.GetOrdinal("base_happiness"));
        //        pokemon.HatchCounter = reader3.GetInt32(reader3.GetOrdinal("hatch_counter"));
        //    }

        //    sql = String.Format("SELECT * FROM pokemon_v2_pokemonegggroup WHERE pokemon_species_id = {0}", id);
        //    DbDataReader reader4 = await ExecuteReaderAsync(sql, token);
        //    int counter = 1;
        //    while (await reader4.ReadAsync(token))
        //    {
        //        int eggGroupId = reader4.GetInt32(reader4.GetOrdinal("egg_group_id"));
        //        EggGroup eggGroup = await LoadEggGroupAsync(eggGroupId, language, token);
        //        if (counter == 1)
        //            pokemon.EggGroup1 = eggGroup;
        //        else
        //            pokemon.EggGroup2 = eggGroup;
        //        counter++;
        //    }

        //    sql = String.Format("{0}\n{1}\n{2}\n{3}\n{4}\n{5}",
        //                        "SELECT ps.pokemon_id, ps.stat_id, sn.name, ps.base_stat, ps.effort FROM pokemon_v2_pokemonstat ps",
        //                        "LEFT JOIN pokemon_v2_stat s ON ps.stat_id = s.id",
        //                        "LEFT JOIN\n(SELECT def.stat_id AS id, IFNULL(curr.name, def.name) AS name FROM pokemon_v2_statname def",
        //                        String.Format("LEFT JOIN pokemon_v2_statname curr ON def.stat_id = curr.stat_id AND def.language_id = 9 AND curr.language_id = {0}", language),
        //                        "GROUP BY def.stat_id)\nAS sn ON s.id = sn.id\n",
        //                        String.Format("WHERE pokemon_id = {0}", id));
        //    DbDataReader reader5 = await ExecuteReaderAsync(sql, token);
        //    //while (await reader5.ReadAsync(token))
        //    //{
        //    //    int stat = reader5.GetInt32(reader5.GetOrdinal("stat_id"));
        //    //    int statOrdinal = reader5.GetOrdinal("base_stat");
        //    //    int effortOrdinal = reader5.GetOrdinal("effort");
        //    //    switch (stat)
        //    //    {
        //    //        case 1:
        //    //            pokemon.HitPoints = reader5.GetInt32(statOrdinal);
        //    //            pokemon.HitPointEv = reader5.GetInt32(effortOrdinal);
        //    //            break;
        //    //        case 2:
        //    //            pokemon.Attack = reader5.GetInt32(statOrdinal);
        //    //            pokemon.AttackEv = reader5.GetInt32(effortOrdinal);
        //    //            break;
        //    //        case 3:
        //    //            pokemon.Defense = reader5.GetInt32(statOrdinal);
        //    //            pokemon.DefenseEv = reader5.GetInt32(effortOrdinal);
        //    //            break;
        //    //        case 4:
        //    //            pokemon.SpecialAttack = reader5.GetInt32(statOrdinal);
        //    //            pokemon.SpecialAttackEv = reader5.GetInt32(effortOrdinal);
        //    //            break;
        //    //        case 5:
        //    //            pokemon.SpecialDefense = reader5.GetInt32(statOrdinal);
        //    //            pokemon.SpecialDefenseEv = reader5.GetInt32(effortOrdinal);
        //    //            break;
        //    //        case 6:
        //    //            pokemon.Speed = reader5.GetInt32(statOrdinal);
        //    //            pokemon.SpeedEv = reader5.GetInt32(effortOrdinal);
        //    //            break;
        //    //    }
        //    //}
        //    var mapper = new DatabaseMapper<Stat>();
        //    pokemon.Stats = mapper.MapFromQueryAsync(reader5);

        //    sql = String.Format("SELECT slot, type_id FROM pokemon_v2_pokemontype\nWHERE pokemon_id = {0}", id);
        //    DbDataReader reader6 = await ExecuteReaderAsync(sql, token);
        //    while (await reader6.ReadAsync(token))
        //    {
        //        int typeId = reader6.GetInt32(reader6.GetOrdinal("type_id"));
        //        int slot = reader6.GetInt32(reader6.GetOrdinal("slot"));
        //        if (slot == 1)
        //            pokemon.Type1 = await LoadTypeAsync(typeId, language, version, token);
        //        else
        //            pokemon.Type2 = await LoadTypeAsync(typeId, language, version, token);
        //    }
        //    return pokemon;
        //}

        //public async Task<DamageClass> LoadDamageClassAsync(int id, int language, CancellationToken token)
        //{
        //    string sql = String.Format("SELECT mdc.id, mdcd.name FROM pokemon_v2_movedamageclass as mdc");
        //    sql = String.Format("{0} LEFT JOIN\n(SELECT def.move_damage_class_id AS id, IFNULL(curr.description, def.description) AS name FROM pokemon_v2_movedamageclassdescription def\n", sql);
        //    sql = String.Format("{0} LEFT JOIN pokemon_v2_movedamageclassdescription curr ON def.move_damage_class_id = curr.move_damage_class_id AND def.language_id = 9 AND curr.language_id = {1}\n", sql, language);
        //    sql = String.Format("{0} GROUP BY def.move_damage_class_id)\nAS mdcd ON mdc.id = mdcd.id\n", sql);
        //    sql = String.Format("{0} WHERE mdc.id = {1}", sql, id);
        //    DbDataReader reader = await ExecuteReaderAsync(sql, token);
        //    var mapper = new DatabaseMapper<DamageClass>();
        //    return await mapper.MapSingleObjectAsync(reader, token);
        //}

        //public async Task<Move> LoadMoveAsync(int id, int version, int language, CancellationToken token)
        //{
        //    string sql = String.Format("SELECT m.id, m.power, m.pp, m.accuracy, m.priority, m.move_damage_class_id, m.type_id, mn.name FROM pokemon_v2_move as m\n");
        //    sql = String.Format("{0} LEFT JOIN\n(SELECT def.move_id AS id, IFNULL(curr.name, def.name) AS name FROM pokemon_v2_movename def\n", sql);
        //    sql = String.Format("{0} LEFT JOIN pokemon_v2_movename curr ON def.move_id = curr.move_id AND def.language_id = 9 AND curr.language_id = {1}\n", sql, language);
        //    sql = String.Format("{0} GROUP BY def.move_id)\nAS mn ON m.id = mn.id\n", sql);
        //    sql = String.Format("{0} WHERE m.id = {1}", sql, id);
        //    DbDataReader reader = await ExecuteReaderAsync(sql, token);
        //    Move result = null;

        //    var mapper = new DatabaseMapper<Move>();
        //    result = await mapper.MapSingleObjectAsync(reader, token);
        //    int typeId = reader.GetInt32(reader.GetOrdinal("type_id"));
        //    result.Type = await LoadTypeAsync(typeId, language, version, token);
        //    int classId = reader.GetInt32(reader.GetOrdinal("move_damage_class_id"));
        //    result.DamageClass = await LoadDamageClassAsync(classId, language, token);
        //    sql = String.Format("SELECT id, power, pp, accuracy, type_id FROM pokemon_v2_movechange WHERE move_id = {0} and version_group_id =\n", id);
        //    sql = String.Format("{0} (SELECT version_group_id FROM pokemon_v2_version WHERE id = {1})", sql, version);
        //    DbDataReader reader2 = await ExecuteReaderAsync(sql, token);

        //    if (await reader2.ReadAsync(token))
        //    {
        //        if (!String.IsNullOrWhiteSpace(reader2["power"].ToString()))
        //            result.Power = reader2.GetInt32(reader2.GetOrdinal("power"));
        //        if (!String.IsNullOrWhiteSpace(reader2["pp"].ToString()))
        //            result.PowerPoints = reader2.GetInt32(reader2.GetOrdinal("pp"));
        //        if (!String.IsNullOrWhiteSpace(reader2["accuracy"].ToString()))
        //            result.Accuracy = reader2.GetInt32(reader2.GetOrdinal("accuracy"));
        //        if (!String.IsNullOrWhiteSpace(reader2["type_id"].ToString()))
        //        {
        //            typeId = reader2.GetInt32(reader.GetOrdinal("type_id"));
        //            result.Type = await LoadTypeAsync(typeId, language, version, token);
        //        }
        //    }

        //    return result;
        //}

        //public async Task<MoveLearnMethod> LoadMoveLearnMethodAsync(int id, int language, CancellationToken token)
        //{
        //    string sql = String.Format("SELECT mlm.id, mlmd.name, mlmd.description FROM pokemon_v2_movelearnmethod AS mlm\n");
        //    sql = String.Format("{0} LEFT JOIN\n(SELECT def.move_learn_method_id AS id, IFNULL(curr.name, def.name) AS name, IFNULL(curr.description, def.description) AS description FROM pokemon_v2_movelearnmethodname def\n", sql);
        //    sql = String.Format("{0} LEFT JOIN pokemon_v2_movelearnmethodname curr ON def.move_learn_method_id = curr.move_learn_method_id AND def.language_id = 9 AND curr.language_id = {1}\n", sql, language);
        //    sql = String.Format("{0} GROUP BY def.move_learn_method_id)\nAS mlmd ON mlm.id = mlmd.id\n", sql);
        //    sql = String.Format("{0} WHERE mlm.id = {1}", sql, id);
        //    DbDataReader reader = await ExecuteReaderAsync(sql, token);

        //    var mapper = new DatabaseMapper<MoveLearnMethod>();
        //    return await mapper.MapSingleObjectAsync(reader, token);
        //}

        //public async Task<List<PokemonMove>> LoadPokemonMoveSetAsync(int pokemon, int version, int language, CancellationToken token)
        //{
        //    string sql = String.Format("SELECT level, move_id, move_learn_method_id FROM pokemon_v2_pokemonmove\n");
        //    sql = String.Format("{0} WHERE pokemon_id = {1} AND version_group_id =\n", sql, pokemon);
        //    sql = String.Format("{0} (SELECT version_group_id FROM pokemon_v2_version WHERE id = {1})\n", sql, version);
        //    sql = String.Format("{0} ORDER BY move_learn_method_id, level, 'order', move_id", sql);
        //    DbDataReader reader = await ExecuteReaderAsync(sql, token);

        //    var result = new List<PokemonMove>();
        //    while (await reader.ReadAsync(token))
        //    {
        //        var element = new PokemonMove();                
        //        int moveId = reader.GetInt32(reader.GetOrdinal("move_id"));
        //        element.Move = await LoadMoveAsync(moveId, version, language, token);
        //        element.Level = reader.GetInt32(reader.GetOrdinal("level"));
        //        int learnMethodId = reader.GetInt32(reader.GetOrdinal("move_learn_method_id"));
        //        element.LearnMethod = await LoadMoveLearnMethodAsync(learnMethodId, language, token);
        //        result.Add(element);
        //    }
        //    return result;
        //}

        /// <summary>
        /// Disposes the object and closes the connection.
        /// </summary>
        public void Dispose()
        {
            Connection.Close();
        }
    }
}
