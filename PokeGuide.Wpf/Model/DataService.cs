using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

        public async void LoadAllSpeciesAsync(int generation, Action<List<Species>, Exception> callback, CancellationToken token)
        {
            try
            {
                List<Data.Model.Species> list = null;
                using (var loader = new DataLoader(_database))
                {
                    list = await loader.LoadAllSpeciesAsync(generation, _language, token);
                }
                callback(list.Select(s => new Species
                {
                    BaseHappiness = s.BaseHappiness,
                    CaptureRate = s.CaptureRate,
                    Genus = s.Genus,
                    HatchCounter = s.HatchCounter,
                    Id = s.Id,                    
                    Name = s.Name                    
                }).ToList(), null);
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

                List<GameVersion> resultList = result.Select(s => new GameVersion
                {
                    Generation = s.Generation,
                    Id = s.Id,
                    Name = s.Name,
                    VersionGroup = s.VersionGroupId
                }).ToList();
                callback(resultList, null);
            }
            catch (Exception ex)
            {
                callback(null, ex);
            }
        }

        public async void LoadPokemonMoveSet(int pokemon, int version, Action<List<MoveLearnElement>, Exception> callback, CancellationToken token)
        {
            try
            {
                List<Data.Model.PokemonMove> result = null;
                using (var loader = new DataLoader(_database))
                {
                    //result = await loader.LoadPokemonMoveSetAsync(pokemon, version, _language, token);
                }
                callback(result.Select(s => new MoveLearnElement
                {
                    Id = s.Id,
                    LearnMethod = new MoveLearnMethod
                    {
                        Description = s.LearnMethod.Description,
                        Id = s.LearnMethod.Id,
                        Name = s.LearnMethod.Name
                    },
                    Level = s.Level,
                    Move = new Move
                    {
                        Accuracy = s.Move.Accuracy,
                        DamageClass = new DamageClass { Id = s.Move.DamageClass.Id, Name = s.Move.DamageClass.Name },
                        Id = s.Move.Id,
                        Name = s.Move.Name,
                        Power = s.Move.Power,
                        PowerPoints = s.Move.PowerPoints,
                        Priority = s.Move.Priority,
                        Type = new ElementType { Id = s.Move.Type.Id, Name = s.Move.Type.Name }
                    }
                }).ToList(), null);
            }
            catch (Exception ex)
            {
                callback(null, ex);
            }
        }

        public async void LoadPokemonFormsAsync(Species species, int versionGroup, Action<List<PokemonForm>, Exception> callback, CancellationToken token)
        {
            try
            {
                List<Data.Model.PokemonForm> result = null;
                using (var loader = new DataLoader(_database))
                {
                    result = await loader.LoadFormsAsync(species.Id, versionGroup, _language, token);
                }
                List<PokemonForm> list = result.Select(s => new PokemonForm
                {
                    Ability1 = s.Ability1 == null ? null : new Ability
                    {
                        Description = s.Ability1.Description,
                        Effect = s.Ability1.Effect,
                        FlavorText = s.Ability1.FlavorText,
                        Id = s.Ability1.Id,
                        Name = s.Ability1.Name
                    },
                    Ability2 = s.Ability2 == null ? null : new Ability
                    {
                        Description = s.Ability2.Description,
                        Effect = s.Ability2.Effect,
                        FlavorText = s.Ability2.FlavorText,
                        Id = s.Ability2.Id,
                        Name = s.Ability2.Name
                    },
                    BaseExperience = s.BaseExperience,
                    Height = s.Height,
                    HiddenAbility = s.HiddenAbility == null ? null : new Ability
                    {
                        Description = s.HiddenAbility.Description,
                        Effect = s.HiddenAbility.Effect,
                        FlavorText = s.HiddenAbility.FlavorText,
                        Id = s.HiddenAbility.Id,
                        Name = s.HiddenAbility.Name
                    },
                    Id = s.Id,
                    Name = s.Name,
                    Species = species,
                    Type1 = new ElementType { Id = s.Type1.Id, Name = s.Type1.Name },
                    Type2 = s.Type2 == null ? null : new ElementType { Id = s.Type2.Id, Name = s.Type2.Name },
                    Weight = s.Weight
                }).ToList();
                callback(list, null);
            }
            catch (Exception ex)
            {
                callback(null, ex);
            }
        }
    }
}