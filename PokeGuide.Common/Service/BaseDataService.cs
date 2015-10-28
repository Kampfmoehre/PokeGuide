using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Nito.AsyncEx;
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
            
        }

        public AsyncLazy<IEnumerable<ElementType>> TypeList { get; private set; }
        public AsyncLazy<IEnumerable<GrowthRate>> GrowthRates { get; private set; }
        public AsyncLazy<IEnumerable<DamageClass>> DamageClasses { get; private set; }

        public void InitializeResources(int displayLanguage, CancellationToken token)
        {
            TypeList = new AsyncLazy<IEnumerable<ElementType>>(async () =>
            {
                return await LoadTypesAsync(displayLanguage, token);
            });
            GrowthRates = new AsyncLazy<IEnumerable<GrowthRate>>(async () =>
            {
                return await LoadGrowthRatesAsync(displayLanguage, token);
            });
            DamageClasses = new AsyncLazy<IEnumerable<DamageClass>>(async () =>
            {
                return await LoadDamageClassesAsync(displayLanguage, token);
            });
        }

        public async Task<ElementType> GetTypeAsync(int id, GameVersion version)
        {
            IEnumerable<ElementType> types = await TypeList;
            return types.FirstOrDefault(f => f.Id == id && f.Generation <= version.Generation);
        }

        public async Task<GrowthRate> GetGrowthRateAsync(int id)
        {
            IEnumerable<GrowthRate> rates = await GrowthRates;
            return rates.FirstOrDefault(f => f.Id == id);
        }

        public async Task<DamageClass> GetDamageClassAsync(int id)
        {
            IEnumerable<DamageClass> classes = await DamageClasses;
            return classes.FirstOrDefault(f => f.Id == id);
        }

        public async Task<IEnumerable<ElementType>>LoadTypesAsync(int displayLanguage, CancellationToken token)
        {
            try
            {
                string query = "SELECT t.id, tn.name, t.move_damage_class_id, t.generation_id FROM pokemon_v2_type t\n" +
                    "LEFT JOIN\n(SELECT e.type_id AS id, COALESCE(o.name, e.name) AS name FROM pokemon_v2_typename e\n" +
                    "LEFT OUTER JOIN pokemon_v2_typename o ON e.type_id = o.type_id and o.language_id = ?\n" +
                    "WHERE e.language_id = 9\nGROUP BY e.type_id)\nAS tn ON t.id = tn.id";
                    //"WHERE t.generation_id <= ?";
                    IEnumerable<DbType> types = await _connection.QueryAsync<DbType>(token, query, new object[] { displayLanguage });
                    return types.Select(s => new ElementType
                    {
                        DamageClassId = s.MoveDamageClassId,
                        Generation = s.GenerationId,
                        Id = s.Id,
                        Name = s.Name
                    });
            }
            catch (Exception)
            {
                return new List<ElementType>();
            }
        }

        public async Task<IEnumerable<GrowthRate>> LoadGrowthRatesAsync(int displayLanguage, CancellationToken token)
        {
            try
            {
                string query = "SELECT gr.id, grd.name FROM pokemon_v2_growthrate gr\n" +
                    "LEFT JOIN\n(SELECT e.growth_rate_id AS id, COALESCE(o.description, e.description) AS name FROM pokemon_v2_growthratedescription e\n" +
                    "LEFT OUTER JOIN pokemon_v2_growthratedescription o ON e.growth_rate_id = o.growth_rate_id and o.language_id = ?\n" +
                    "WHERE e.language_id = 9\nGROUP BY e.growth_rate_id)\nAS grd ON gr.id = grd.id";
                    //"WHERE gr.id = ?";
                IEnumerable<DbGrowthRate> growthRates = await _connection.QueryAsync<DbGrowthRate>(token, query, new object[] { displayLanguage });
                return growthRates.Select(s => new GrowthRate { Id = s.Id, Name = s.Name });
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<IEnumerable<DamageClass>> LoadDamageClassesAsync(int displayLanguage, CancellationToken token)
        {
            try
            {
                string query = "SELECT mdc.id, mdcd.name FROM pokemon_v2_movedamageclass mdc\n" +
                    "LEFT JOIN\n(SELECT e.move_damage_class_id AS id, COALESCE(o.description, e.description) AS name FROM pokemon_v2_movedamageclassdescription e\n" +
                    "LEFT OUTER JOIN pokemon_v2_movedamageclassdescription o ON e.move_damage_class_id = o.move_damage_class_id and o.language_id = ?\n" +
                    "WHERE e.language_id = 9\nGROUP BY e.move_damage_class_id)\nAS mdcd ON mdc.id = mdcd.id";
                IEnumerable<DbMoveDamageClass> damageClasses = await _connection.QueryAsync<DbMoveDamageClass>(token, query, new object[] { displayLanguage });
                return damageClasses.Select(s => new DamageClass { Id = s.Id, Name = s.Name });
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}