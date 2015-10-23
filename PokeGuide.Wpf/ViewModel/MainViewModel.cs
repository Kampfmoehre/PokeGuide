using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Threading;
using PokeGuide.Wpf.Model;

namespace PokeGuide.Wpf.ViewModel
{
    /// <summary>
    /// This class contains properties that the main View can data bind to.
    /// <para>
    /// See http://www.mvvmlight.net
    /// </para>
    /// </summary>
    public class MainViewModel : ViewModelBase
    {
        Progress<double> _progress;
        double __currentProgress;
        string _currentAction;
        readonly IStaticDataService _staticDataService;
        readonly IPokemonService _pokemonService;
        CancellationTokenSource _tokenSource;
        ObservableCollection<Language> _languages;
        ObservableCollection<GameVersion> _versions;
        GameVersion _selectedVersion;
        ObservableCollection<Species> _species;
        Species _selectedSpecies;
        ObservableCollection<PokemonForm> _forms;
        PokemonForm _selectedForm;
        RelayCommand _cancelLoadingCommand;

        /// <summary>
        /// Gets the MyCommand.
        /// </summary>
        public RelayCommand CancelLoadingCommand
        {
            get
            {
                return _cancelLoadingCommand ?? (_cancelLoadingCommand = new RelayCommand(() =>
                    {
                        _tokenSource.Cancel();
                    },
                    () => true));
            }
        }
        /// <summary>
        /// Sets and gets the 
        /// </summary>
        public double CurrentProgress
        {
            get { return __currentProgress; }
            set { Set(() => CurrentProgress, ref __currentProgress, value); }
        }                
        /// <summary>
        /// Sets and gets the 
        /// </summary>
        public string CurrentAction
        {
            get { return _currentAction; }
            set { Set(() => CurrentAction, ref _currentAction, value); }
        }
        /// <summary>
        /// Sets and gets the 
        /// </summary>
        public ObservableCollection<Language> Languages
        {
            get { return _languages; }
            set { Set(() => Languages, ref _languages, value); }
        }
        Language _selectedLanguage;
        /// <summary>
        /// Sets and gets the 
        /// </summary>
        public Language SelectedLanguage
        {
            get { return _selectedLanguage; }
            set
            {
                Set(() => SelectedLanguage, ref _selectedLanguage, value);
                if (value != null)
                    LoadVersions(value.Id);
            }
        }
        /// <summary>
        /// Sets and gets versions
        /// </summary>
        public ObservableCollection<GameVersion> Versions
        {
            get { return _versions; }
            set { Set(() => Versions, ref _versions, value); }
        }
        /// <summary>
        /// Sets and gets the currently selected Version
        /// </summary>
        public GameVersion SelectedVersion
        {
            get { return _selectedVersion; }
            set
            {
                Set(() => SelectedVersion, ref _selectedVersion, value);
                if (value == null)
                    SelectedSpecies = null;
                else
                    LoadAllSpecies(value.Generation);
            }
        }
        /// <summary>
        /// Sets and gets all Pokémon
        /// </summary>
        public ObservableCollection<Species> Species
        {
            get { return _species; }
            set { Set(() => Species, ref _species, value); }
        }
        /// <summary>
        /// Sets and gets the currently selected Pokémon
        /// </summary>
        public Species SelectedSpecies
        {
            get { return _selectedSpecies; }
            set
            {
                Set(() => SelectedSpecies, ref _selectedSpecies, value);
                if (value == null)
                    Forms = null;
                else
                    LoadForms(value, SelectedVersion.VersionGroup);
            }
        }        
        /// <summary>
        /// Sets and gets the 
        /// </summary>
        public ObservableCollection<PokemonForm> Forms
        {
            get { return _forms; }
            set { Set(() => Forms, ref _forms, value); }
        }        
        /// <summary>
        /// Sets and gets the 
        /// </summary>
        public PokemonForm SelectedForm
        {
            get { return _selectedForm; }
            set { Set(() => SelectedForm, ref _selectedForm, value); }
        }

        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        public MainViewModel(IStaticDataService staticDataService, IPokemonService pokemonService)
        {
            _tokenSource = new CancellationTokenSource();
            _staticDataService = staticDataService;
            _pokemonService = pokemonService;
            _progress = new Progress<double>();
            _progress.ProgressChanged += (s, e) => CurrentProgress = e;
            
            Task.Factory.StartNew(async () =>
            {
                List<Language> languages = await _staticDataService.LoadLanguages(6, _tokenSource.Token);
                
                DispatcherHelper.CheckBeginInvokeOnUI(() =>
                {
                    if (_tokenSource.IsCancellationRequested)
                        return;
                    Languages = new ObservableCollection<Language>(languages);
                    SelectedLanguage = Languages.First(f => f.Id == 6);                    
                });
            });
        }

        async void LoadVersions(int language)
        {
            CurrentAction = "Loading games ...";
            List<GameVersion> versions = await _staticDataService.LoadGameVersionAsync(language, _progress, _tokenSource.Token);
            DispatcherHelper.CheckBeginInvokeOnUI(() =>
            {
                if (_tokenSource.IsCancellationRequested)
                    return;
                Versions = new ObservableCollection<GameVersion>(versions);
                SelectedVersion = Versions.First();
                CurrentAction = "";
            });
        }

        async void LoadAllSpecies(int generation)
        {
            CurrentAction = "Loading species ...";
            List<Species> species = await _pokemonService.LoadAllSpeciesAsync(generation, SelectedLanguage.Id, _progress, _tokenSource.Token);
            DispatcherHelper.CheckBeginInvokeOnUI(() =>
            {
                if (_tokenSource.IsCancellationRequested)
                    return;
                Species = new ObservableCollection<Species>(species);
                SelectedSpecies = Species.First();
                CurrentAction = "";
            });
        }
        async void LoadForms(Species species, int versionGroup)
        {
            CurrentAction = "Loading forms ...";
            List<PokemonForm> forms = await _pokemonService.LoadPokemonFormsAsync(species, versionGroup, SelectedLanguage.Id, _progress, _tokenSource.Token);
            DispatcherHelper.CheckBeginInvokeOnUI(() =>
            {
                if (_tokenSource.IsCancellationRequested)
                    return;
                Forms = new ObservableCollection<PokemonForm>(forms);
                SelectedForm = Forms.First();
                CurrentAction = "";
            });
        }        

        //void LoadMoveSet(int pokemon, int version)
        //{
        //    _dataService.LoadPokemonMoveSet(pokemon, version, (list, error) =>
        //    {
        //        if (error != null)
        //            return;
        //        MoveSet = new ObservableCollection<MoveLearnElement>(list);
        //    }, _tokenSource.Token);
        //}

        public override void Cleanup()
        {
            // Clean up if needed 
            base.Cleanup();
            _tokenSource.Dispose();
        }
    }
}