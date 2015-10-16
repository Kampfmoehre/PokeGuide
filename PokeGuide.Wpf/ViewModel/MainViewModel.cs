using System.Collections.ObjectModel;
using System.Threading;
using GalaSoft.MvvmLight;
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
        readonly IDataService _dataService;
        string _welcomeTitle = string.Empty;
        ObservableCollection<GameVersion> _versions;
        ObservableCollection<Pokemon> _pokemon;
        CancellationTokenSource _tokenSource;

        /// <summary>
        /// Gets the WelcomeTitle property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public string WelcomeTitle
        {
            get
            {
                return _welcomeTitle;
            }
            set
            {
                Set(ref _welcomeTitle, value);
            }
        }


        private GameVersion _selectedVersion;

        /// <summary>
        /// Sets and gets the currently selected Version
        /// </summary>
        public GameVersion SelectedVersion
        {
            get { return _selectedVersion; }
            set {
                Set(() => SelectedVersion, ref _selectedVersion, value);
                LoadPokemon(value.Id);
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
        /// Sets and gets all Pokémon
        /// </summary>
        public ObservableCollection<Pokemon> Pokemon
        {
            get { return _pokemon; }
            set { Set(() => Pokemon, ref _pokemon, value); }
        }

        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        public MainViewModel(IDataService dataService)
        {
            _tokenSource = new CancellationTokenSource();
            _dataService = dataService;
            _dataService.GetData(
                (item, error) =>
                {
                    if (error != null)
                    {
                        // Report error here
                        return;
                    }

                    WelcomeTitle = item.Title;
                });
            _dataService.LoadGameVersionsAsync((list, error) =>
            {
                if (error != null)
                {
                    return;
                }

                Versions = new ObservableCollection<GameVersion>(list);
            }, _tokenSource.Token);
        }

        void LoadPokemon(int version)
        {
            _dataService.LoadAllPokemonAsync(version, (list, error) =>
            {
                if (error != null)
                    return;
                Pokemon = new ObservableCollection<Pokemon>(list);
            }, _tokenSource.Token);
        }

        public override void Cleanup()
        {
            // Clean up if needed 
            base.Cleanup();
            _tokenSource.Dispose();
        }
    }
}