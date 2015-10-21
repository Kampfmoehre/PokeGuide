using System;
using System.Collections.Generic;
using System.Threading;
using PokeGuide.Wpf.Model;

namespace PokeGuide.Wpf.Design
{
    public class DesignDataService : IDataService
    {
        public void GetData(Action<DataItem, Exception> callback)
        {
            // Use this to create design time data

            var item = new DataItem("Welcome to MVVM Light [design]");
            callback(item, null);
        }

        public void LoadAllPokemonAsync(int version, Action<List<Pokemon>, Exception> callback, CancellationToken token)
        {
            var list = new List<Pokemon>
            {
                new Pokemon { Id = 1, Name = "Bisasam" },
                new Pokemon { Id = 2, Name = "Glumanda" },
                new Pokemon { Id = 3, Name = "Schiggy" }
            };
            callback(list, null);
        }

        public void LoadGameVersionsAsync(Action<List<GameVersion>, Exception> callback, CancellationToken token)
        {
            var list = new List<GameVersion>
            {
                new GameVersion { Id = 1, Name = "Rot" },
                new GameVersion { Id = 2, Name = "Blau" },
                new GameVersion { Id = 3, Name = "Gelb" }
            };
            callback(list, null);
        }

        public void LoadPokemonMoveSet(int pokemon, int version, Action<List<MoveLearnElement>, Exception> callback, CancellationToken token)
        {
            var list = new List<MoveLearnElement>
            {
                new MoveLearnElement
                {
                    LearnMethod = new MoveLearnMethod { Description = "By Level Up", Id = 1, Name = "Level Up" },
                    Level = 1,
                    Move = new Move
                    {
                        Accuracy = 100,
                        DamageClass = new DamageClass { Id = 1, Name = "Physisch" },
                        Id = 17,
                        Name = "Flügelschlag",
                        Power = 60,
                        PowerPoints = 15,
                        Priority = 0,
                        Type = new ElementType { Id = 1, Name = "Flug" }
                    }
                }
            };
            callback(list, null);
        }
    }
}