using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;
using PokeGuide.Model;
using PokeGuide.Service;

namespace PokeGuide.Design
{
    public class DesignStaticDataService : IStaticDataService
    {
        public void Cleanup()
        { }

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

        public Task<ObservableCollection<GameVersion>> LoadVersionsAsync(int displayLanguage, CancellationToken token)
        {
            var tcs = new TaskCompletionSource<ObservableCollection<GameVersion>>();
            tcs.SetResult(new ObservableCollection<GameVersion>
            {
                new GameVersion { Generation = 6, Id = 23, Name = "X", VersionGroup = 15},
                new GameVersion { Generation = 6, Id = 24, Name = "Y", VersionGroup = 15}
            });
            return tcs.Task;
        }
    }
}
