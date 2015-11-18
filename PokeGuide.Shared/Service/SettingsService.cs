using System;
using System.Threading;
using System.Threading.Tasks;

using PokeGuide.Core.Model;
using PokeGuide.Core.Service.Interface;

using Windows.Storage;

namespace PokeGuide.Service
{
    class SettingsService : ISettingsService
    {
        const string languageSettingsKey = "currentLanguage";
        const string versionSettingsKey = "currentVersion";
        IDataService _dataService;

        public SettingsService(IDataService dataService)
        {
            _dataService = dataService;
        }

        public async Task<Settings> LoadSettings()
        {
            var settings = ApplicationData.Current.LocalSettings;
            object languageSetting = settings.Values[languageSettingsKey];
            // ToDo map current culture to default language
            int languageId = 6;
            if (languageSetting != null)
                languageId = Convert.ToInt32(languageSetting);
            object versionSetting = settings.Values[versionSettingsKey];
            int versionId = 25;
            if (versionSetting != null)
                versionId = Convert.ToInt32(versionSetting);

            var result = new Settings();
            var source = new CancellationTokenSource();
            result.CurrentLanguage = await LoadLanguage(languageId, source.Token);            
            result.CurrentVersion = await LoadVersion(versionId, languageId, source.Token);
            return result;
        }

        public void SaveSettings(Settings settings)
        {
            var localSettings = ApplicationData.Current.LocalSettings;
            localSettings.Values[languageSettingsKey] = settings.CurrentLanguage.Id;
            localSettings.Values[versionSettingsKey] = settings.CurrentVersion.Id;
        }

        async Task<DisplayLanguage> LoadLanguage(int id, CancellationToken token)
        {
            return await _dataService.LoadLanguageByIdAsync(id, id, token);
        }
        async Task<GameVersion> LoadVersion(int id, int language, CancellationToken token)
        {
            return await _dataService.LoadVersionByIdAsync(id, language, token);
        }
    }
}
