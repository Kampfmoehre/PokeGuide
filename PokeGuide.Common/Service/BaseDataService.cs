using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using PokeGuide.Model;
using PokeGuide.Model.Database;
using PokeGuide.Service.Interface;
using SQLite.Net.Interop;

namespace PokeGuide.Service
{
    public class BaseDataService : DataService, IBaseDataService
    {
        public BaseDataService(IStorageService storageService, ISQLitePlatform sqlitePlatform)
            : base(storageService, sqlitePlatform)
        {
            TypeList = new AsyncLazy<IEnumerable<ElementType>>(async () =>
            {
                return await LoadTypesAsync(6, null);
            });
        }

        public async Task<ElementType> LoadTypeAsync(int id, GameVersion version, int displayLanguage, CancellationToken token)
        {
            try
            {
                string query = "SELECT t.id, tn.name, t.move_damage_class_id FROM pokemon_v2_type t\n" +
                    "LEFT JOIN\n(SELECT e.type_id AS id, COALESCE(o.name, e.name) AS name FROM pokemon_v2_typename e\n" +
                    "LEFT OUTER JOIN pokemon_v2_typename o ON e.type_id = o.type_id and o.language_id = ?\n" +
                    "WHERE e.language_id = 9\nGROUP BY e.type_id)\nAS tn ON t.id = tn.id\n" +
                    "WHERE t.id = ? AND t.generation_id <= ?";
                IEnumerable<DbType> types = await _connection.QueryAsync<DbType>(token, query, new object[] { displayLanguage, id, version.Generation });
                DbType type = types.First(); // await _connection.ExecuteScalarAsync<DbType>(token, query, new object[] { displayLanguage, id, version.Generation });
                if (type == null)
                    return null;
                return new ElementType { DamageClassId = type.MoveDamageClassId, Id = type.Id, Name = type.Name };
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<ElementType> GetTypeAsync(int id, GameVersion version)
        {
            return await TypeList.FirstOrDefault(f => f.Id == id && f.Generation <= version.Generation);
        }

        public async Task<IEnumerable<ElementType>>LoadTypesAsync(int displayLanguage, CancellationToken token)
        {
            try
            {
                string query = "SELECT t.id, tn.name, t.move_damage_class_id, t.generation_id FROM pokemon_v2_type t\n" +
                    "LEFT JOIN\n(SELECT e.type_id AS id, COALESCE(o.name, e.name) AS name FROM pokemon_v2_typename e\n" +
                    "LEFT OUTER JOIN pokemon_v2_typename o ON e.type_id = o.type_id and o.language_id = ?\n" +
                    "WERE e.language_id = 9\nGROUP BY e.type_id)\nAS tn ON t.id = tn.id";
                    //"WHERE t.generation_id <= ?";
                    IEnumerable<DbType> types = await _connection.QueryAsync<DbType>(token, query, new object[] { displayLanguage, id, version.Generation });
                    return types.Select(s => new ElementType
                    {
                        DamageClassId = s.MoveDamageClassId,
                        Generation = s.GenerationId,
                        Id = s.id,
                        Name = s.Name
                    });
            }
            catch (System.Exception)
            {
                return new List<ElementType>();
            }
        }
        // see https://github.com/StephenCleary/AsyncEx/wiki/NotifyTaskCompletion
        public AsyncLazy<IEnumerable<ElementType>> TypeList  {get; private set; }
    }
}