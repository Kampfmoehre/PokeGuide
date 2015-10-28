using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using PokeGuide.Model;
using PokeGuide.Service.Interface;
using Windows.UI.Core;

namespace PokeGuide.ViewModel
{
    public class MainViewModel : ViewModelBase, IMainViewModel
    {
        IStaticDataService _staticDataService;
        IPokemonService _pokemonService;
        IMoveService _moveService;
        CancellationTokenSource _tokenSource;
        ObservableCollection<Language> _languages;
        Language _selectedLanguage;
        ObservableCollection<GameVersion> _versions;
        GameVersion _selectedVersion;
        ObservableCollection<SpeciesName> _speciesList;
        SpeciesName _selectedSpecies;
        ObservableCollection<PokemonForm> _forms;
        PokemonForm _selectedForm;

        public ObservableCollection<Language> Languages
        {
            get { return _languages; }
            set { Set(() => Languages, ref _languages, value); }
        }
        public Language SelectedLanguage
        {
            get { return _selectedLanguage; }
            set
            {
                Set(() => SelectedLanguage, ref _selectedLanguage, value);
                if (value != null)
                    LoadAndAssignVersions(value.Id).ConfigureAwait(false);
            }
        }
        public ObservableCollection<GameVersion> Versions
        {
            get { return _versions; }
            set { Set(() => Versions, ref _versions, value); }
        }
        public GameVersion SelectedVersion
        {
            get { return _selectedVersion; }
            set
            {
                Set(() => SelectedVersion, ref _selectedVersion, value);
                SpeciesList = null;
                SelectedSpecies = null;
                if (value != null)
                    LoadAndAssignSpecies(value, SelectedLanguage.Id).ConfigureAwait(false);
            }
        }
        public ObservableCollection<SpeciesName> SpeciesList
        {
            get { return _speciesList; }
            set { Set(() => SpeciesList, ref _speciesList, value); }
        }
        public SpeciesName SelectedSpecies
        {
            get { return _selectedSpecies; }
            set
            {
                Set(() => SelectedSpecies, ref _selectedSpecies, value);
                Forms = null;
                SelectedFormName = null;
                if (value != null)
                    LoadAndAssignForms(value, SelectedVersion, SelectedLanguage.Id).ConfigureAwait(false);
            }
        }
        public ObservableCollection<PokemonForm> Forms
        {
            get { return _forms; }
            set { Set(() => Forms, ref _forms, value); }
        }
        public PokemonForm SelectedFormName
        {
            get { return _selectedForm; }
            set
            {
                Set(() => SelectedFormName, ref _selectedForm, value);
                if (value != null)
                    LoadAndAssignForm(value.Id, SelectedVersion, SelectedLanguage.Id).ConfigureAwait(false);
            }
        }

        PokemonForm _currentForm;
        /// <summary>
        /// Sets and gets the 
        /// </summary>
        public PokemonForm CurrentForm
        {
            get { return _currentForm; }
            set { Set(() => CurrentForm, ref _currentForm, value); }
        }        

        public RelayCommand<int> LoadLanguagesCommand { get; set; }

        readonly CoreDispatcher _dispatcher;

        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        public MainViewModel(IStaticDataService staticDataService, IPokemonService pokemonService, IMoveService moveService)
        {
            _dispatcher = CoreWindow.GetForCurrentThread().Dispatcher;
            _tokenSource = new CancellationTokenSource();
            _staticDataService = staticDataService;
            _pokemonService = pokemonService;
            _moveService = moveService;
            IsLoading = true;

            LoadLanguagesCommand = new RelayCommand<int>((value) =>
            {
                SelectedSpecies = SpeciesList.First(f => f.Id == value);
            });
            
            LoadAndAssignLanguages(6).ConfigureAwait(false);
            if (IsInDesignMode)
            {
                //Languages = new NotifyTaskCompletion<ObservableCollection<Language>>(
                //    new System.Threading.Tasks.Task<ObservableCollection<Language>>(() =>
                //        { return new ObservableCollection<Language> { new Language { Id = 6, Name = "Deutsch" } }; }));
                //Versions = new ObservableCollection<GameVersion>(new List<GameVersion> { new GameVersion { Generation = 6, Id = 12, Name = "Rubin", VersionGroup = 5 } });
                //SelectedVersion = Versions.First();
            }
            else
            {
                // Code runs "for real"
                
            }
        }

        async Task LoadAndAssignLanguages(int defaultLanguage)
        {
            Languages = await _staticDataService.LoadLanguagesAsync(defaultLanguage, _tokenSource.Token);
            SelectedLanguage = Languages.First(f => f.Id == 6);
        }
        async Task LoadAndAssignVersions(int displayLanguage)
        {
            Versions = await _staticDataService.LoadVersionsAsync(displayLanguage, _tokenSource.Token);
            //SelectedVersion = Versions.First();            
        }
        async Task LoadAndAssignSpecies(GameVersion version, int displayLanguage)
        {
            SpeciesList = await _pokemonService.LoadAllSpeciesAsync(version, displayLanguage, _tokenSource.Token);
            SelectedSpecies = SpeciesList.First();
        }
        async Task LoadAndAssignForms(SpeciesName species, GameVersion version, int displayLanguage)
        {
            IsLoading = true;
            Forms = await _pokemonService.LoadFormsAsync(species, version, displayLanguage, _tokenSource.Token);
            SelectedFormName = Forms.First();
            IsLoading = false;
        }
        async Task LoadAndAssignForm(int id, GameVersion version, int displayLanguage)
        {
            IsLoading = true;
            CurrentForm = await _pokemonService.LoadFormAsync(id, version, displayLanguage, _tokenSource.Token);
            
            CurrentForm.MoveSet = await _moveService.LoadMoveSetAsync(CurrentForm.Id, version, displayLanguage, _tokenSource.Token);
            IsLoading = false;
        }


        bool _isLoading;
        /// <summary>
        /// Sets and gets the 
        /// </summary>
        public bool IsLoading
        {
            get { return _isLoading; }
            set { Set(() => IsLoading, ref _isLoading, value); }
        }
    }
}