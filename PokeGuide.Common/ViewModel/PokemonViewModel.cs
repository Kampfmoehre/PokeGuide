using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using GalaSoft.MvvmLight.Views;
using Nito.AsyncEx;
using PokeGuide.Model;
using PokeGuide.Service.Interface;
using Windows.Storage;

namespace PokeGuide.ViewModel
{
    public class PokemonViewModel : ViewModelBase, IPokemonViewModel
    {
        CancellationTokenSource _tokenSource;
        IStaticDataService _staticDataService;
        IPokemonService _pokemonService;
        INavigationService _navigationService;
        INotifyTaskCompletionCollection<GameVersion> _versions;
        INotifyTaskCompletionCollection<SpeciesName> _speciesList;
        INotifyTaskCompletionCollection<PokemonForm> _forms;
        INotifyTaskCompletion<PokemonForm> _currentForm;
        INotifyTaskCompletion<ObservableCollection<PokemonMove>> _currentMoveSet;
        INotifyTaskCompletion<ObservableCollection<Stat>> _currentStats;
        INotifyTaskCompletion<ObservableCollection<PokemonEvolution>> _currentEvolutions;
        INotifyTaskCompletion<ObservableCollection<PokemonLocation>> _currentLocations;
        RelayCommand<int> _loadFormCommand;
        RelayCommand _openSettingsCommand;
        RelayCommand _navigateBackCommand;
        int _currentLanguage;
        int? _cachedSpeciesId = 1;
        int? _cachedVersionid = 1;
        /// <summary>
        /// Sets and gets the
        /// </summary>
        public INotifyTaskCompletionCollection<GameVersion> Versions
        {
            get { return _versions ; }
            set
            {
                if (Versions != null)
                    Versions.SelectedItemChanged -= Versions_SelectedItemChanged;
                Set(() => Versions, ref _versions , value);
                if (Versions != null)
                    Versions.SelectedItemChanged += Versions_SelectedItemChanged;
            }
        }

        void Versions_SelectedItemChanged(object sender, SelectedItemChangedEventArgs<GameVersion> e)
        {
            SpeciesList = null;
            if (e.NewItem != null)
            {
                SpeciesList = NotifyTaskCompletionCollection<SpeciesName>.Create(LoadSpeciesAsync(e.NewItem, _currentLanguage), _cachedSpeciesId);
                _cachedVersionid = e.NewItem.Id;
            }
        }

        /// <summary>
        /// Sets and gets the
        /// </summary>
        public INotifyTaskCompletionCollection<SpeciesName> SpeciesList
        {
            get { return _speciesList; }
            set
            {
                if (SpeciesList != null)
                    SpeciesList.SelectedItemChanged -= SpeciesList_SelectedItemChanged;
                Set(() => SpeciesList, ref _speciesList, value);
                if (SpeciesList != null)
                    SpeciesList.SelectedItemChanged += SpeciesList_SelectedItemChanged;
            }
        }

        void SpeciesList_SelectedItemChanged(object sender, SelectedItemChangedEventArgs<SpeciesName> e)
        {
            Forms = null;
            CurrentEvolutions = null;
            CurrentLocations = null;
            if (e.NewItem != null)
            {
                Forms = NotifyTaskCompletionCollection<PokemonForm>.Create(LoadFormsAsync(e.NewItem, Versions.SelectedItem, _currentLanguage));
                CurrentEvolutions = NotifyTaskCompletion.Create(LoadEvolutionsAsync(e.NewItem.Id, Versions.SelectedItem, _currentLanguage));
                CurrentLocations = NotifyTaskCompletion.Create(LoadLocationsAsync(e.NewItem.Id, Versions.SelectedItem, _currentLanguage));                
                _cachedSpeciesId = e.NewItem.Id;
            }
        }

        /// <summary>
        /// Sets and gets the
        /// </summary>
        public INotifyTaskCompletionCollection<PokemonForm> Forms
        {
            get { return _forms; }
            set
            {
                if (Forms != null)
                    Forms.SelectedItemChanged -= Forms_SelectedItemChanged;
                Set(() => Forms, ref _forms, value);                
                if (Forms != null)
                    Forms.SelectedItemChanged += Forms_SelectedItemChanged;
            }
        }

        void Forms_SelectedItemChanged(object sender, SelectedItemChangedEventArgs<PokemonForm> e)
        {
            CurrentForm = null;
            CurrentMoveSet = null;
            CurrentStats = null;
            if (e.NewItem != null)
            {
                CurrentForm = NotifyTaskCompletion.Create(LoadFormAsync(e.NewItem.Id, Versions.SelectedItem, _currentLanguage));
                CurrentMoveSet = NotifyTaskCompletion.Create(LoadMoveSetAsync(e.NewItem.Id, Versions.SelectedItem, _currentLanguage));
                CurrentStats = NotifyTaskCompletion.Create(LoadStatsAsync(e.NewItem.Id, Versions.SelectedItem, _currentLanguage));                                
            }
        }

        /// <summary>
        /// Sets and gets the
        /// </summary>
        public INotifyTaskCompletion<PokemonForm> CurrentForm
        {
            get { return _currentForm; }
            set { Set(() => CurrentForm, ref _currentForm, value); }
        }
        /// <summary>
        /// Sets and gets the
        /// </summary>
        public INotifyTaskCompletion<ObservableCollection<PokemonMove>> CurrentMoveSet
        {
            get { return _currentMoveSet; }
            set { Set(() => CurrentMoveSet, ref _currentMoveSet, value); }
        }
        /// <summary>
        /// Sets and gets the 
        /// </summary>
        public INotifyTaskCompletion<ObservableCollection<Stat>> CurrentStats
        {
            get { return _currentStats; }
            set { Set(() => CurrentStats, ref _currentStats, value); }
        }
        /// <summary>
        /// Sets and gets the 
        /// </summary>
        public INotifyTaskCompletion<ObservableCollection<PokemonEvolution>> CurrentEvolutions
        {
            get { return _currentEvolutions; }
            set { Set(() => CurrentEvolutions, ref _currentEvolutions, value); }
        }
        /// <summary>
        /// Sets and gets the 
        /// </summary>
        public INotifyTaskCompletion<ObservableCollection<PokemonLocation>> CurrentLocations
        {
            get { return _currentLocations; }
            set { Set(() => CurrentLocations, ref _currentLocations, value); }
        }

        public RelayCommand<int> LoadFormCommand
        {
            get
            {
                if (_loadFormCommand == null)
                {
                    _loadFormCommand = new RelayCommand<int>((id) =>
                    {
                        SpeciesList.SelectedItem = SpeciesList.Collection.First(f => f.Id == id);
                    });
                }
                return _loadFormCommand;
            }
        }
        public RelayCommand OpenSettingsCommand
        {
            get
            {
                if (_openSettingsCommand == null)
                {
                    _openSettingsCommand = new RelayCommand(() =>
                    {
                        _navigationService.NavigateTo("PhoneSettings");
                    });
                }
                return _openSettingsCommand;
            }
        }
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

        public PokemonViewModel(IStaticDataService staticDataService, IPokemonService pokemonService, INavigationService navigationService)
        {
            _tokenSource = new CancellationTokenSource();
            _staticDataService = staticDataService;
            _pokemonService = pokemonService;
            _navigationService = navigationService;

            var settings = ApplicationData.Current.LocalSettings;
            object lang = settings.Values["displayLanguage"];
            if (lang != null)
                _currentLanguage = Convert.ToInt32(lang);
            else
                _currentLanguage = 6;

            ChangeLanguage(new Language { Id = _currentLanguage });
            Messenger.Default.Register<Language>(this, (language) => ChangeLanguage(language));

            if (IsInDesignMode)
            {
                Versions = NotifyTaskCompletionCollection<GameVersion>.Create(LoadVersionsAsync(_currentLanguage));
                SpeciesList = NotifyTaskCompletionCollection<SpeciesName>.Create(LoadSpeciesAsync(null, _currentLanguage));
                Forms = NotifyTaskCompletionCollection<PokemonForm>.Create(LoadFormsAsync(new SpeciesName { Id = 1 }, null, _currentLanguage));
                CurrentForm = NotifyTaskCompletion.Create(LoadFormAsync(1, null, _currentLanguage));
                CurrentMoveSet = NotifyTaskCompletion.Create(LoadMoveSetAsync(6, null, _currentLanguage));
                CurrentStats = NotifyTaskCompletion.Create(LoadStatsAsync(6, null, _currentLanguage));
                CurrentEvolutions = NotifyTaskCompletion.Create(LoadEvolutionsAsync(6, null, _currentLanguage));
                CurrentLocations = NotifyTaskCompletion.Create(LoadLocationsAsync(6, null, _currentLanguage));
            }
        }

        void ChangeLanguage(Language newLanguage)
        {
            _currentLanguage = newLanguage.Id;
            Versions = null;
            SpeciesList = null;
            Forms = null;
            CurrentForm = null;
            CurrentMoveSet = null;
            if (newLanguage != null)
            {
                ResetLanguage(newLanguage.Id);
                Versions = NotifyTaskCompletionCollection<GameVersion>.Create(LoadVersionsAsync(newLanguage.Id), _cachedVersionid);
            }
        }

        void ResetLanguage(int displayLanguage)
        {
            _pokemonService.InitializeResources(displayLanguage, _tokenSource.Token);
        }

        
        async Task<ObservableCollection<GameVersion>> LoadVersionsAsync(int language)
        {
            return await _staticDataService.LoadVersionsAsync(language, _tokenSource.Token);
        }
        async Task<ObservableCollection<SpeciesName>> LoadSpeciesAsync(GameVersion version, int language)
        {
            return await _pokemonService.LoadAllSpeciesAsync(version, language, _tokenSource.Token);
        }
        async Task<ObservableCollection<PokemonForm>> LoadFormsAsync(SpeciesName species, GameVersion version, int language)
        {
            return await _pokemonService.LoadFormsAsync(species, version, language, _tokenSource.Token);
        }
        async Task<PokemonForm> LoadFormAsync(int formid, GameVersion version, int language)
        {
            return await _pokemonService.LoadFormAsync(formid, version, language, _tokenSource.Token);
        }
        async Task<ObservableCollection<PokemonMove>> LoadMoveSetAsync(int formid, GameVersion version, int language)
        {
            return await _pokemonService.LoadMoveSetAsync(formid, version, language, _tokenSource.Token);
        }
        async Task<ObservableCollection<Stat>> LoadStatsAsync(int formId, GameVersion version, int language)
        {
            return await _pokemonService.LoadPokemonStatsAsync(formId, version, language, _tokenSource.Token);
        }
        async Task<ObservableCollection<PokemonEvolution>> LoadEvolutionsAsync(int speciesId, GameVersion version, int language)
        {
            return await _pokemonService.LoadEvolutionGroupAsync(speciesId, version, language, _tokenSource.Token);
        }
        async Task<ObservableCollection<PokemonLocation>> LoadLocationsAsync(int pokemonId, GameVersion version, int language)
        {
            return await _pokemonService.LoadPokemonEncountersAsync(pokemonId, version, language, _tokenSource.Token);
        }
    }
}
