using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using GalaSoft.MvvmLight;

using PokeGuide.Model;
using PokeGuide.Service;

namespace PokeGuide.ViewModel
{
    public class MainViewModel : ViewModelBase, IMainViewModel
    {
        bool _isLoading;
        IStaticDataService _staticDataService;
        IPokemonService _pokemonService;
        ObservableCollection<GameVersion> _versions;
        GameVersion _selectedVersion;        
        CancellationTokenSource _tokenSource;
        ObservableCollection<SpeciesName> _speciesList;
        Species _selectedSpecies;
        SpeciesName _selectedSpeciesName;
        ObservableCollection<Language> _languages;
        Language _selectedLanguage;
        ObservableCollection<PokemonForm> _forms;
        PokemonForm _selectedForm;

        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        public MainViewModel(IStaticDataService staticDataService, IPokemonService pokemonService)
        {
            IsLoading = true;
            _tokenSource = new CancellationTokenSource();
            _staticDataService = staticDataService;
            _pokemonService = pokemonService;

            Languages = Task.Run(() => this.LoadLanguagesAsync(6)).Result;
            SelectedLanguage = Languages.First(f => f.Id == 6);
            Versions = Task.Run(() => this.LoadVersionsAsync(SelectedLanguage.Id)).Result;
            SpeciesList = Task.Run(() => this.LoadSpeciesNamesAsync(SelectedLanguage.Id)).Result;
            SelectedVersion = Versions.FirstOrDefault();
            if (IsInDesignMode)
            {
                Versions = new ObservableCollection<GameVersion>(new List<GameVersion> { new GameVersion { Generation = 6, Id = 12, Name = "Rubin", VersionGroup = 5 } });
                SelectedVersion = Versions.First();
            }
            ////else
            ////{
            ////    // Code runs "for real"
            ////}
            IsLoading = false;
        }

        /// <summary>
        /// Sets and gets the 
        /// </summary>
        public bool IsLoading
        {
            get { return _isLoading; }
            set { Set(() => IsLoading, ref _isLoading, value); }
        }
        /// <summary>
        /// Sets and gets the 
        /// </summary>
        public ObservableCollection<GameVersion> Versions
        {
            get { return _versions; }
            set { Set(() => Versions, ref _versions, value); }
        }
        /// <summary>
        /// Sets and gets the 
        /// </summary>
        public GameVersion SelectedVersion
        {
            get { return _selectedVersion; }
            set
            {
                Set(() => SelectedVersion, ref _selectedVersion, value);
                RaisePropertyChanged("SpeciesList");
            }
        }
        /// <summary>
        /// Sets and gets the 
        /// </summary>
        public ObservableCollection<SpeciesName> SpeciesList
        {
            get { return new ObservableCollection<SpeciesName>(_speciesList.Where(w => w.Generation == SelectedVersion.Generation)); }
            set { Set(() => SpeciesList, ref _speciesList, value); }
        }
        /// <summary>
        /// Sets and gets the 
        /// </summary>
        public Species SelectedSpecies
        {
            get { return _selectedSpecies; }
            set { Set(() => SelectedSpecies, ref _selectedSpecies, value); }
        }
        /// <summary>
        /// Sets and gets the 
        /// </summary>
        public SpeciesName SelectedSpeciesName
        {
            get { return _selectedSpeciesName; }
            set
            {
                Set(() => SelectedSpeciesName, ref _selectedSpeciesName, value);
                Task.Run(() => LoadFormsAsync(value.Id, SelectedVersion, SelectedLanguage.Id));
            }
        }
        /// <summary>
        /// Sets and gets the 
        /// </summary>
        public ObservableCollection<Language> Languages
        {
            get { return _languages; }
            set { Set(() => Languages, ref _languages, value); }
        }
        /// <summary>
        /// Sets and gets the 
        /// </summary>
        public Language SelectedLanguage
        {
            get { return _selectedLanguage; }
            set { Set(() => SelectedLanguage, ref _selectedLanguage, value); }
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

        async Task<ObservableCollection<Language>> LoadLanguagesAsync(int displayLanguage)
        {
            return new ObservableCollection<Language>(await _staticDataService.LoadLanguagesAsync(displayLanguage, _tokenSource.Token));
        }
        async Task<ObservableCollection<GameVersion>> LoadVersionsAsync(int displayLanguage)
        {
            return new ObservableCollection<GameVersion>(await _staticDataService.LoadVersionsAsync(displayLanguage, _tokenSource.Token));            
        }
        async Task<ObservableCollection<SpeciesName>> LoadSpeciesNamesAsync(int displayLanguage)
        {
            return new ObservableCollection<SpeciesName>(await _pokemonService.LoadAllSpeciesAsync(displayLanguage, _tokenSource.Token));
        }
        async Task LoadFormsAsync(int id, GameVersion version, int displayLanguage)
        {
            //IsLoading = true;
            Forms = new ObservableCollection<PokemonForm>(await _pokemonService.LoadFormsAsync(id, version, displayLanguage, _tokenSource.Token));
            SelectedForm = Forms.First();
            //IsLoading = false;
        }
    }
}