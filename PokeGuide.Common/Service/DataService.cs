using System.Threading.Tasks;
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
            //Task.Factory.StartNew(() => { InitializeAsync(); }).Wait();
            Task.Run(() => this.InitializeAsync()).Wait();
            //Initialization = InitializeAsync();
        }

        public Task Initialization { get; set; }

        async Task InitializeAsync()
        {
            if (_connection == null)
            {
                string database = await _storageService.GetPathForFileAsync("database.sqlite3");
                _connection = new SQLiteAsyncConnection(() => new SQLiteConnectionWithLock(_sqlitePlatform, new SQLiteConnectionString(database, false)));
            }
        }

        public void Cleanup()
        {
            // ToDo clear all connections
        }
    }
}
