using System;
using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;

using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using GalaSoft.MvvmLight.Views;

using PokeGuide.Model;
using PokeGuide.Service.Interface;
using PokeGuide.ViewModel.Interface;

using Windows.Storage;

namespace PokeGuide.ViewModel
{
    /// <summary>
    /// Viewmodel for any settings view
    /// </summary>
    public class SettingsViewModel : ViewModelBase, ISettingsViewModel
    {
        CancellationTokenSource _tokenSource;
        IStaticDataService _staticDataService;
        INavigationService _navigationService;
        INotifyTaskCompletionCollection<Language> _languages;
        RelayCommand _navigateBackCommand;
        /// <summary>
        /// Sets and gets the list of available data languages
        /// </summary>
        public INotifyTaskCompletionCollection<Language> Languages
        {
            get { return _languages; }
            set
            {
                if (Languages != null)
                {
                    Languages.SelectedItemChanged -= SelectedLanguageChanged;
                }
                Set(() => Languages, ref _languages, value);
                if (Languages != null)
                    Languages.SelectedItemChanged += SelectedLanguageChanged;
            }
        }
        /// <summary>
        /// Gets or sets the current app settings
        /// </summary>
        public ApplicationDataContainer AppSettings { get; set; }
        /// <summary>
        /// Navigates back
        /// </summary>
        public RelayCommand NavigateBackCommand
        {
            get
            {
                if (_navigateBackCommand == null)
                    _navigateBackCommand = new RelayCommand(() => _navigationService.GoBack());
                return _navigateBackCommand;
            }
        }

        /// <summary>
        /// Initializes a new instance of the SettingsViewModel
        /// </summary>
        /// <param name="staticDataService">A service implemtation</param>
        public SettingsViewModel(IStaticDataService staticDataService, INavigationService navigationService)
        {
            _tokenSource = new CancellationTokenSource();
            _staticDataService = staticDataService;
            _navigationService = navigationService;

            int displayLanguage = 6;
            AppSettings = ApplicationData.Current.LocalSettings;
            object lang = AppSettings.Values["displayLanguage"];
            if (lang != null)
                displayLanguage = Convert.ToInt32(lang);

            Languages = NotifyTaskCompletionCollection<Language>.Create(LoadLanguagesAsync(displayLanguage), displayLanguage);
        }

        /// <summary>
        /// Handles changing of selected language
        /// </summary>
        /// <param name="sender">The sender</param>
        /// <param name="e">Event args</param>
        void SelectedLanguageChanged(object sender, SelectedItemChangedEventArgs<Language> e)
        {
            if (e.NewItem != null && e.OldItem != null)
            {                
                AppSettings.Values["displayLanguage"] = e.NewItem.Id;
                ChangeLanguage(e.NewItem);
            }
        }

        /// <summary>
        /// Loads the languages asynchronously
        /// </summary>
        /// <param name="defaultLanguage">THe ID of the default language</param>
        /// <returns></returns>
        async Task<ObservableCollection<Language>> LoadLanguagesAsync(int defaultLanguage)
        {
            return await _staticDataService.LoadLanguagesAsync(defaultLanguage, _tokenSource.Token);
        }

        /// <summary>
        /// Handles change of language and sends the message
        /// </summary>
        /// <param name="newLanguage">The new language</param>
        public void ChangeLanguage(Language newLanguage)
        {
            Messenger.Default.Send<Language>(newLanguage);
        }
    }
}
