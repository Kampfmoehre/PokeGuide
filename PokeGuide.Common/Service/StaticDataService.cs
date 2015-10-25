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
    public class StaticDataService : DataService, IStaticDataService
    {
        public StaticDataService(IStorageService storageService, ISQLitePlatform sqlitePlatform)
            : base(storageService, sqlitePlatform)
        { }

        public async Task<ObservableCollection<Language>> LoadLanguagesAsync(int displayLanguage, CancellationToken token)
        {
            string query = "SELECT ps.id, psn.name FROM pokemon_v2_language AS ps\n" +
                "LEFT JOIN\n(SELECT def.language_id AS id, IFNULL(curr.name, def.name) AS name FROM pokemon_v2_languagename def\n" +
                "LEFT JOIN pokemon_v2_languagename curr ON def.language_id = curr.language_id AND def.local_language_id = 9 AND curr.local_language_id = ?\n" +
                "GROUP BY def.language_id)\nAS psn ON ps.id = psn.id";
            IEnumerable<DbLanguage> list = await _connection.QueryAsync<DbLanguage>(token, query, new object[] { displayLanguage });
            return new ObservableCollection<Language>(list.Select(s => new Language { Id = s.Id, Name = s.Name }));
        }

        public async Task<ObservableCollection<GameVersion>> LoadVersionsAsync(int displayLanguage, CancellationToken token)
        {
            string query = "SELECT v.id, vn.name, v.version_group_id, vg.generation_id FROM pokemon_v2_version AS v\n" +
                 "LEFT JOIN pokemon_v2_versiongroup AS vg ON v.version_group_id = vg.id\n" +
                 "LEFT JOIN\n(SELECT def.version_id AS id, IFNULL(curr.name, def.name) AS name FROM pokemon_v2_versionname def\n" +
                 "LEFT JOIN pokemon_v2_versionname curr ON def.version_id = curr.version_id AND def.language_id = 9 AND curr.language_id = ?\n" +
                 "GROUP BY def.version_id)\nAS vn ON v.id = vn.id";
            IEnumerable<DbVersion> list = await _connection.QueryAsync<DbVersion>(token, query, new object[] { displayLanguage });
            return new ObservableCollection<GameVersion>(list.Select(s => new GameVersion
            {
                Generation = s.Generation,
                Id = s.Id,
                Name = s.Name,
                VersionGroup = s.VersionGroupId
            }));
        }
    }
}