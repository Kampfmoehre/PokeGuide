using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Nito.AsyncEx;
using PokeGuide.Model;
using PokeGuide.Service.Interface;

namespace PokeGuide.ViewModel
{
    public class PokemonViewModel : ViewModelBase, IPokemonViewModel
    {
        CancellationTokenSource _tokenSource;
        IStaticDataService _staticDataService;
        IPokemonService _pokemonService;
        IMoveService _moveService;
        INotifyTaskCompletionCollection<Language> _languages;
        INotifyTaskCompletionCollection<GameVersion> _versions;
        INotifyTaskCompletionCollection<SpeciesName> _speciesList;
        INotifyTaskCompletionCollection<PokemonForm> _forms;
        INotifyTaskCompletion<PokemonForm> _currentForm;
        INotifyTaskCompletion<ObservableCollection<PokemonMove>> _currentMoveSet;
        INotifyTaskCompletion<ObservableCollection<Stat>> _currentStats;
        INotifyTaskCompletion<ObservableCollection<PokemonEvolution>> _currentEvolutions;
        INotifyTaskCompletion<ObservableCollection<PokemonLocation>> _currentLocations;
        RelayCommand<int> _loadFormCommand;
        int? _cachedSpeciesId = 1;
        int? _cachedVersionid = 1;
        /// <summary>
        /// Sets and gets the
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
        void SelectedLanguageChanged(object sender, SelectedItemChangedEventArgs<Language> e)
        {
            Versions = null;
            SpeciesList = null;
            Forms = null;
            CurrentForm = null;
            CurrentMoveSet = null;
            if (e.NewItem != null)
            {
                ResetLanguage(e.NewItem.Id);
                Versions = NotifyTaskCompletionCollection<GameVersion>.Create(LoadVersionsAsync(e.NewItem.Id), _cachedVersionid);
            }
        }
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
            Forms = null;
            CurrentForm = null;
            CurrentMoveSet = null;
            if (e.NewItem != null)
            {
                SpeciesList = NotifyTaskCompletionCollection<SpeciesName>.Create(LoadSpeciesAsync(e.NewItem, Languages.SelectedItem.Id), _cachedSpeciesId);
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
            CurrentForm = null;
            CurrentMoveSet = null;
            CurrentEvolutions = null;
            CurrentLocations = null;
            if (e.NewItem != null)
            {
                Forms = NotifyTaskCompletionCollection<PokemonForm>.Create(LoadFormsAsync(e.NewItem, Versions.SelectedItem, Languages.SelectedItem.Id));
                CurrentEvolutions = NotifyTaskCompletion.Create(LoadEvolutionsAsync(e.NewItem.Id, Versions.SelectedItem, Languages.SelectedItem.Id));
                CurrentLocations = NotifyTaskCompletion.Create(LoadLocationsAsync(e.NewItem.Id, Versions.SelectedItem, Languages.SelectedItem.Id));                
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
                CurrentForm = NotifyTaskCompletion.Create(LoadFormAsync(e.NewItem.Id, Versions.SelectedItem, Languages.SelectedItem.Id));
                CurrentMoveSet = NotifyTaskCompletion.Create(LoadMoveSetAsync(e.NewItem.Id, Versions.SelectedItem, Languages.SelectedItem.Id));
                CurrentStats = NotifyTaskCompletion.Create(LoadStatsAsync(e.NewItem.Id, Versions.SelectedItem, Languages.SelectedItem.Id));                                
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

        public PokemonViewModel(IStaticDataService staticDataService, IPokemonService pokemonService, IMoveService moveService)
        {
            _tokenSource = new CancellationTokenSource();
            _staticDataService = staticDataService;
            _pokemonService = pokemonService;
            _moveService = moveService;
            ResetLanguage(6);            
            Languages = NotifyTaskCompletionCollection<Language>.Create(LoadLanguagesAsync(6), 6);

            if (IsInDesignMode)
            {
                Versions = NotifyTaskCompletionCollection<GameVersion>.Create(LoadVersionsAsync(6));
                SpeciesList = NotifyTaskCompletionCollection<SpeciesName>.Create(LoadSpeciesAsync(null, 6));
                Forms = NotifyTaskCompletionCollection<PokemonForm>.Create(LoadFormsAsync(new SpeciesName { Id = 1 }, null, 6));
                CurrentForm = NotifyTaskCompletion.Create(LoadFormAsync(1, null, 6));
                CurrentMoveSet = NotifyTaskCompletion.Create(LoadMoveSetAsync(6, null, 6));
                CurrentStats = NotifyTaskCompletion.Create(LoadStatsAsync(6, null, 6));
                CurrentEvolutions = NotifyTaskCompletion.Create(LoadEvolutionsAsync(6, null, 6));
                CurrentLocations = NotifyTaskCompletion.Create(LoadLocationsAsync(6, null, 6));
            }
        }

        void ResetLanguage(int displayLanguage)
        {
            _pokemonService.InitializeResources(displayLanguage, _tokenSource.Token);
        }

        async Task<ObservableCollection<Language>> LoadLanguagesAsync(int defaultLanguage)
        {
            return await _staticDataService.LoadLanguagesAsync(defaultLanguage, _tokenSource.Token);
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
