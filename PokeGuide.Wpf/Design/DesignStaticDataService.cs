using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using PokeGuide.Wpf.Model;

namespace PokeGuide.Wpf.Design
{
    class DesignStaticDataService : IStaticDataService
    {
        public async Task<List<GameVersion>> LoadGameVersionAsync(int language, IProgress<double> progress, CancellationToken token)
        {
            progress.Report(33);
            return await Task.Factory.StartNew(() =>
            {
                return new List<GameVersion>
                {
                    new GameVersion
                    {
                        Generation = 6,
                        Id = 23,
                        Name = "X",
                        VersionGroup = 15
                    }
                };
            });
        }

        public async Task<List<Language>> LoadLanguages(int language, CancellationToken token)
        {
            return await Task.Factory.StartNew(() =>
            {
                return new List<Language>
                {
                    new Language { Id = 6, Name = "Deutsch" },
                    new Language { Id = 9, Name = "Englisch" }
                };
            });
        }
    }
}
