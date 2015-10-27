using System;
using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;
using PokeGuide.Model;
using PokeGuide.Service.Interface;

namespace PokeGuide.Design
{
    public class DesignMoveService : IMoveService
    {
        public Task<DamageClass> LoadDamageClassAsync(int id, int displayLanguage, CancellationToken token)
        {
            var tcs = new TaskCompletionSource<DamageClass>();
            tcs.SetResult(new DamageClass { Id = 1, Name = "Physisch" });
            return tcs.Task;
        }

        public Task<Move> LoadMoveAsync(int id, GameVersion version, int displayLanguage, CancellationToken token)
        {
            var tcs = new TaskCompletionSource<Move>();
            tcs.SetResult(new Move
            {
                Accuracy = 90,
                DamageClass = LoadDamageClassAsync(1, displayLanguage, token).Result,
                Id = 2,
                Name = "Flügelschlag",
                Power = 60,
                PowerPoints = 15,
                Priority = 0,
                Type = new ElementType { Id = 1, Name = "Flug" }
            });
            return tcs.Task;
        }

        public Task<MoveLearnMethod> LoadMoveLearnMethodAsync(int id, int displayLanguage, CancellationToken token)
        {
            var tcs = new TaskCompletionSource<MoveLearnMethod>();
            tcs.SetResult(new MoveLearnMethod { Id = 1, Name = "Level" });
            return tcs.Task;
        }

        public Task<ObservableCollection<PokemonMove>> LoadMoveSetAsync(int pokemonId, GameVersion version, int displayLanguage, CancellationToken token)
        {
            var tcs = new TaskCompletionSource<ObservableCollection<PokemonMove>>();
            tcs.SetResult(new ObservableCollection<PokemonMove>
            {
                new PokemonMove
                {
                    LearnMethod = new MoveLearnMethod { Id = 1, Name = "Level" },
                    Level = 12,
                    Move = LoadMoveAsync(1, version, displayLanguage, token).Result
                }
            });
            return tcs.Task;
        }
    }
}