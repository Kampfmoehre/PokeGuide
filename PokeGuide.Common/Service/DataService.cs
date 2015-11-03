using System.Threading.Tasks;
using PokeGuide.Service.Interface;
using SQLite.Net;
using SQLite.Net.Async;
using SQLite.Net.Interop;

namespace PokeGuide.Service
{
    public class DataService : IDataService
    {
        IStorageService _storageService;
        ISQLitePlatform _sqlitePlatform;
        protected SQLiteAsyncConnection _connection;

        public DataService(IStorageService storageService, ISQLitePlatform sqlitePlatform)
        {
            _storageService = storageService;
            _sqlitePlatform = sqlitePlatform;
            Task.Run(() => this.InitializeAsync()).Wait();
        }

        public Task Initialization { get; set; }

        async Task InitializeAsync()
        {
            if (_connection == null)
            {
                string database = await _storageService.GetPathForFileAsync("pokedex.sqlite");
                _connection = new SQLiteAsyncConnection(() => new SQLiteConnectionWithLock(_sqlitePlatform, new SQLiteConnectionString(database, false)));
            }
        }

        public void Cleanup()
        {
            // ToDo clear all connections
        }
    }
}
