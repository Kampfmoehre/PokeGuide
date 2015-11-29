using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;

using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using GalaSoft.MvvmLight.Views;

using PokeGuide.Core.Event;
using PokeGuide.Core.Model;
using PokeGuide.Core.Service.Interface;
using PokeGuide.Core.ViewModel.Interface;

namespace PokeGuide.Core.ViewModel
{
    public class MainViewModel : ViewModelBase, IMainViewModel
    {
        CancellationTokenSource _tokenSource;
        INavigationService _navigationService;
        IDataService _dataService;
        ISettingsService _settingsService;
        INotifyTaskCompletionCollection<GameVersion> _versionList;
        RelayCommand _navigateToAbilitiesCommand;
        RelayCommand _navigateToMovesCommand;
        RelayCommand _navigateToPokemonCommand;
        DisplayLanguage _currentLanguage;
        int _cachedVersionId;

        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        public MainViewModel(INavigationService navigationService, IDataService dataService, ISettingsService settingsService)
        {
            _tokenSource = new CancellationTokenSource();
            _navigationService = navigationService;
            _dataService = dataService;
            _settingsService = settingsService;

            Messenger.Default.Register<DisplayLanguage>(this, ChangeLanguage);

            Task.Factory.StartNew(async () =>
            {
                Settings settings = await settingsService.LoadSettings();
                _cachedVersionId = settings.CurrentVersion.Id;
                Messenger.Default.Send<DisplayLanguage>(settings.CurrentLanguage);
            });
        }
        /// <summary>
        /// Sets and gets the list of all versions
        /// </summary>
        public INotifyTaskCompletionCollection<GameVersion> VersionList
        {
            get { return _versionList; }
            set
            {
                Set(() => VersionList, ref _versionList, value);
                if (VersionList != null)
                    VersionList.SelectedItemChanged += SelectedVersionChanged;
            }
        }
        public RelayCommand NavigateToAbilitiesCommand
        {
            get
            {
                if (_navigateToAbilitiesCommand == null)
                    _navigateToAbilitiesCommand = new RelayCommand(() => _navigationService.NavigateTo("AbilityView"));
                return _navigateToAbilitiesCommand;
            }
        }
        public RelayCommand NavigateToMovesCommand
        {
            get
            {
                if (_navigateToMovesCommand == null)
                    _navigateToMovesCommand = new RelayCommand(() => _navigationService.NavigateTo("MoveView"));
                return _navigateToMovesCommand;
            }
        }
        public RelayCommand NavigateToPokemonCommand
        {
            get
            {
                if (_navigateToPokemonCommand == null)
                    _navigateToPokemonCommand = new RelayCommand(() => _navigationService.NavigateTo("PokemonView"));
                return _navigateToPokemonCommand;
            }
        }

        void ChangeLanguage(DisplayLanguage newLanguage)
        {
            _currentLanguage = newLanguage;
            VersionList = NotifyTaskCompletionCollection<GameVersion>.Create(LoadVersionsAsync, _cachedVersionId);
        }

        void SelectedVersionChanged(object sender, SelectedItemChangedEventArgs<GameVersion> e)
        {
            if (e.NewItem != null)
                Messenger.Default.Send<GameVersion>(e.NewItem);
        }

        async Task<ObservableCollection<GameVersion>> LoadVersionsAsync()
        {
            IEnumerable<GameVersion> versions = await _dataService.LoadVersionsAsync(_currentLanguage.Id, _tokenSource.Token);
            return new ObservableCollection<GameVersion>(versions);
        }
    }
}