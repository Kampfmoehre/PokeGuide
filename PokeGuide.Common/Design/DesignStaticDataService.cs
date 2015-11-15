﻿using System;
using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;
using PokeGuide.Model;
using PokeGuide.Service.Interface;

namespace PokeGuide.Design
{
    public class DesignStaticDataService : IStaticDataService
    {
        public void Cleanup()
        { }

        public Task<ObservableCollection<Ability>> LoadAbilitiesAsync(int displayLanguage, CancellationToken token)
        {
            var tcs = new TaskCompletionSource<ObservableCollection<Ability>>();
            tcs.SetResult(new ObservableCollection<Ability>
            {
                new Ability { Id = 6, Name = "Duftnote" },
                new Ability { Id = 9, Name = "Niesel" }
            });
            return tcs.Task;
        }

        public Task<ObservableCollection<Language>> LoadLanguagesAsync(int displayLanguage, CancellationToken token)
        {
            var tcs = new TaskCompletionSource<ObservableCollection<Language>>();
            tcs.SetResult(new ObservableCollection<Language>
            {
                new Language { Id = 6, Name = "Deutsch" },
                new Language { Id = 9, Name = "Englisch" }
            });
            return tcs.Task;
        }

        public Task<GameVersion> LoadVersionAsync(int id, int displayLanguage, CancellationToken token)
        {
            var tcs = new TaskCompletionSource<GameVersion>();
            tcs.SetResult(new GameVersion
            {
                Id = 6, Name = "Platin"
            });
            return tcs.Task;
        }

        public Task<ObservableCollection<GameVersion>> LoadVersionsAsync(int displayLanguage, CancellationToken token)
        {
            var tcs = new TaskCompletionSource<ObservableCollection<GameVersion>>();
            tcs.SetResult(new ObservableCollection<GameVersion>
            {
                new GameVersion { Generation = 1, Id = 1, Name = "Rot", VersionGroup = 1},
                new GameVersion { Generation = 6, Id = 23, Name = "X", VersionGroup = 15},
                new GameVersion { Generation = 6, Id = 24, Name = "Y", VersionGroup = 15}
            });
            return tcs.Task;
        }
    }
}
