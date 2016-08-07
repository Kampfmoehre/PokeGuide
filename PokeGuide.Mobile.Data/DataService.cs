using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using PokeGuide.Core.Service;
using PokeGuide.Mobile.Data.Dto;
using PokeGuide.Model;
using SQLite.Net;
using SQLite.Net.Async;
using SQLite.Net.Interop;

namespace PokeGuide.Mobile.Data
{
    public class DataService : IDataService
    {
        IStorageService _storageService;
        ISQLitePlatform _sqlitePlatform;

        protected SQLiteAsyncConnection _connection;

        public DataService(IStorageService storageService, ISQLitePlatform platform)
        {
            _storageService = storageService;
            _sqlitePlatform = platform;
        }

        async Task InitializeAsync()
        {
            if (_connection == null)
            {
                string database = await _storageService.GetDatabasePathFromFileAsync("pokedex.sqlite");
                var conString = new SQLiteConnectionString(database, false);
                Func<SQLiteConnectionWithLock> function = () => new SQLiteConnectionWithLock(_sqlitePlatform, conString);
                _connection = new SQLiteAsyncConnection(function);
            }
        }

        public async Task<IEnumerable<DisplayLanguage>> LoadLanguages(int displayLanguage, CancellationToken token)
        {
            string query = String.Format(@"
SELECT l.id, ln.name, l.iso639
FROM languages AS l
LEFT JOIN (SELECT e.language_id AS id,  COALESCE(o.name, e.name) AS name
           FROM language_names e
           LEFT OUTER JOIN language_names o ON e.language_id = o.language_id AND o.local_language_id = {0}
           WHERE e.local_language_id = 9
           GROUP BY e.language_id)
AS ln ON l.id = ln.id
                ", displayLanguage);
            IEnumerable<DbLanguage> languages = await _connection.QueryAsync<DbLanguage>(token, query, new object[0]).ConfigureAwait(false);
            return languages.Select(s => new DisplayLanguage { Id = s.Id, Iso639 = s.Iso639, Name = s.Name });
        }
    }
}
