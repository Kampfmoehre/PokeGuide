using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using PokeGuide.Data;

namespace PokeGuide.Wpf.Model
{
    public class DataService : IDataService
    {
        string _database;
        int _language;

        public DataService()
        {
            _database = "database.sqlite3";
            _language = 6;
        }

        public void GetData(Action<DataItem, Exception> callback)
        {
            // Use this to connect to the actual data service

            var item = new DataItem("Welcome to MVVM Light");
            callback(item, null);
        }

        public async void LoadAllPokemonAsync(int version, Action<List<Pokemon>, Exception> callback, CancellationToken token)
        {
            try
            {
                List<Data.Model.Pokemon> list = null;
                using (var loader = new DataLoader(_database))
                {
                    list = await loader.LoadAllPokemonAsync(version, _language, token);
                }
                callback(list.Select(s => new Pokemon { Id = s.Id, Name = s.Name }).ToList(), null);
            }
            catch (Exception ex)
            {
                callback(null, ex);
            }
        }

        public async void LoadGameVersionsAsync(Action<List<GameVersion>, Exception> callback, CancellationToken token)
        {
            try
            {
                List<Data.Model.GameVersion> result = null;
                using (var loader = new DataLoader(_database))
                {
                    result = await loader.LoadGamesAsync(_language, token);
                }
                var resultList = new List<GameVersion>();
                foreach (Data.Model.GameVersion version in result)
                {
                    resultList.Add(new GameVersion { Id = version.Id, Name = version.Name });
                }
                callback(resultList, null);
            }
            catch (Exception ex)
            {
                callback(null, ex);
            }
        }
    }
}