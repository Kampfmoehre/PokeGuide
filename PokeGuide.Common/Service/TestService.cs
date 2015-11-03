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
    }
}
