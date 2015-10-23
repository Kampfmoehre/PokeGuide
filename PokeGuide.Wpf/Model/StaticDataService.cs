using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace PokeGuide.Wpf.Model
{
    class StaticDataService : IStaticDataService
    {
        readonly string _database;
        public StaticDataService(string database)
        {
            _database = database;
        }

        public async Task<List<GameVersion>> LoadGameVersionAsync(int language, IProgress<double> progress, CancellationToken token)
        {
            var dl = new Data.DataLoader(_database);
            List<Data.Model.GameVersion> versions = await dl.LoadGamesAsync(language, progress, token);
            return versions.Select(s => new GameVersion
            {
                Generation = s.Generation,
                Id = s.Id,
                Name = s.Name,
                VersionGroup = s.VersionGroupId
            }).ToList();
        }

        public async Task<List<Language>> LoadLanguages(int language, CancellationToken token)
        {
            var dl = new Data.DataLoader(_database);
            List<Data.Model.Language> languages = await dl.LoadLanguagesAsync(language, token);
            return languages.Select(s => new Language
            {
                Id = s.Id,
                Name = s.Name
            }).ToList();
        }
    }
}
