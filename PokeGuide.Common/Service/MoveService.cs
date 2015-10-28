using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using PokeGuide.Model;
using PokeGuide.Model.Database;
using PokeGuide.Service.Interface;
using SQLite.Net.Interop;

namespace PokeGuide.Service
{
    public class MoveService : BaseDataService, IMoveService
    {
        public MoveService(IStorageService storageService, ISQLitePlatform sqlitePlatform) 
            : base(storageService, sqlitePlatform)
        { }

        public async Task<DamageClass> LoadDamageClassAsync(int id, int displayLanguage, CancellationToken token)
        {
            try
            {
                string query = "SELECT mdc.id, mdcd.name FROM pokemon_v2_movedamageclass mdc\n" +
                    "LEFT JOIN\n(SELECT e.move_damage_class_id AS id, COALESCE(o.description, e.description) AS name FROM pokemon_v2_movedamageclassdescription e\n" +
                    "LEFT OUTER JOIN pokemon_v2_movedamageclassdescription o ON e.move_damage_class_id = o.move_damage_class_id and o.language_id = ?\n" +
                    "WHERE e.language_id = 9\nGROUP BY e.move_damage_class_id)\nAS mdcd ON mdc.id = mdcd.id\n" +
                    "WHERE mdc.id = ?";
                IEnumerable<DbMoveDamageClass> damageClasses = await _connection.QueryAsync<DbMoveDamageClass>(token, query, new object[] { displayLanguage, id });
                DbMoveDamageClass damageClass = damageClasses.First();
                return new DamageClass { Id = damageClass.Id, Name = damageClass.Name };
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<Move> LoadMoveAsync(int id, GameVersion version, int displayLanguage, CancellationToken token)
        {
            try
            {
                string query = "SELECT m.id, mn.name, m.power, m.pp, m.accuracy, m.priority, m.move_damage_class_id, m.type_id FROM pokemon_v2_move m\n" +
                    "LEFT JOIN\n(SELECT e.move_id AS id, COALESCE(o.name, e.name) AS name FROM pokemon_v2_movename e\n" +
                    "LEFT OUTER JOIN pokemon_v2_movename o ON e.move_id = o.move_id and o.language_id = ?\n" +
                    "WHERE e.language_id = 9\nGROUP BY e.move_id)\nAS mn ON m.id = mn.id\n" +
                    "WHERE m.id = ? AND m.generation_id <= ?";
                IEnumerable<DbMove> moves = await _connection.QueryAsync<DbMove>(token, query, new object[] { displayLanguage, id, version.Generation });
                DbMove move = moves.First();
                var result = new Move
                {
                    Accuracy = move.Accuracy,
                    Id = move.Id,
                    Name = move.Name,
                    Power = move.Power,
                    PowerPoints = move.PowerPoints,
                    Priority = move.Priority
                };
                result.Type = await GetTypeAsync(move.Type, version);
                if (version.Generation > 3)
                    result.DamageClass = await LoadDamageClassAsync(move.MoveDamageClass, displayLanguage, token);
                else
                    result.DamageClass = await LoadDamageClassAsync(result.Type.DamageClassId, displayLanguage, token);
                
                return new Move();
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<MoveLearnMethod> LoadMoveLearnMethodAsync(int id, int displayLanguage, CancellationToken token)
        {
            try
            {
                string query = "SELECT mlm.id, mlmd.name FROM pokemon_v2_movelearnmethod mlm\n" +
                    "LEFT JOIN\n(SELECT e.move_learn_method_id AS id, COALESCE(o.name, e.name) AS name, COALESCE(o.description, e.description) AS description FROM pokemon_v2_movelearnmethodname e\n" +
                    "LEFT OUTER JOIN pokemon_v2_movelearnmethodname o ON e.move_learn_method_id = o.move_learn_method_id and o.language_id = ?\n" +
                    "WHERE e.language_id = 9\nGROUP BY e.move_learn_method_id)\nAS mlmd ON mlm.id = mlmd.id\n" +
                    "WHERE mlm.id = ?";
                IEnumerable<DbMoveLearnMethod> methods = await _connection.QueryAsync<DbMoveLearnMethod>(token, query, new object[] { displayLanguage, id });
                DbMoveLearnMethod method = methods.First();
                return new MoveLearnMethod
                {
                    Description = method.Description,
                    Id = method.Id,
                    Name = method.Name
                };
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<ObservableCollection<PokemonMove>> LoadMoveSetAsync(int pokemonId, GameVersion version, int displayLanguage, CancellationToken token)
        {
            try
            {
                List<DbPokemonMove> moveList = await _connection.Table<DbPokemonMove>().Where(w => w.PokemonId == pokemonId && w.VersionGroupId == version.VersionGroup).ToListAsync(token);

                var bag = new ConcurrentBag<PokemonMove>();
                var tasks = moveList.Select(async (m) =>
                {
                    var move = new PokemonMove
                    {
                        Id = m.Id,
                        Level = m.Level,
                        Order = m.Order
                    };
                    move.LearnMethod = await LoadMoveLearnMethodAsync(m.MoveLearnMethodId, displayLanguage, token);
                    move.Move = await LoadMoveAsync(m.MoveId, version, displayLanguage, token);
                    bag.Add(move);
                });
                await Task.WhenAll(tasks);
                return new ObservableCollection<PokemonMove>(bag.OrderBy(o => o.LearnMethod.Id).ThenBy(t => t.Order));
            }
            catch (Exception)
            {
                return new ObservableCollection<PokemonMove>();
            }
        }
    }
}
