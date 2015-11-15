using System;
using System.Threading;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;

using PokeGuide.Model;
using PokeGuide.Service.Interface;
using PokeGuide.ViewModel.Interface;
using Windows.Storage;

namespace PokeGuide.ViewModel
{
    public class ViewModel : ViewModelBase
    {
        protected CancellationTokenSource TokenSource {get; private set;}        
        protected int CurrentLanguage { get; set; }
        protected GameVersion CurrentVersion { get; set; }
        IStaticDataService _staticDataService;

        public ViewModel(IStaticDataService staticDataService)
        {
            TokenSource = new CancellationTokenSource();
            _staticDataService = staticDataService;

            var settings = ApplicationData.Current.LocalSettings;
            object lang = settings.Values["displayLanguage"];
            if (lang != null)
                CurrentLanguage = Convert.ToInt32(lang);
            else
                CurrentLanguage = 6;
            object version = settings.Values["currentVersion"];
            int versionId = 0;
            if (version != null)
                versionId = Convert.ToInt32(version);

            Messenger.Default.Register<Language>(this, (language) => ChangeLanguage(language));
            Messenger.Default.Register<GameVersion>(this, (gameVersion) => ChangeVersion(gameVersion));
            ChangeLanguage(new Language { Id = CurrentLanguage });
            if (versionId > 0)
            {
                Task.Run(async () =>
                {
                    GameVersion newVersion = await LoadVersionAsync(Convert.ToInt32(versionId), CurrentLanguage);
                    ChangeVersion(newVersion);
                });
            }
        }

        async Task<GameVersion> LoadVersionAsync(int id, int language)
        {
            return await _staticDataService.LoadVersionAsync(id, language, TokenSource.Token);
        }

        protected virtual void ChangeLanguage(Language newLanguage)
        {
            CurrentLanguage = newLanguage.Id;
        }

        protected virtual void ChangeVersion(GameVersion newVersion)
        {
            CurrentVersion = newVersion;
        }
    }
}
