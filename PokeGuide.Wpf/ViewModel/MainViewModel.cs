using System;
using System.Collections.ObjectModel;
using System.Linq;
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
        CancellationTokenSource _tokenSource;
        ObservableCollection<GameVersion> _versions;
        GameVersion _selectedVersion;
        ObservableCollection<Species> _species;
        Species _selectedSpecies;
        ObservableCollection<PokemonForm> _forms;
        PokemonForm _selectedForm;
        //ObservableCollection<MoveLearnElement> _moveSet;

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
                    LoadForms(value, SelectedVersion.Id);
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

        //Species _selectedPokemonIndex;
        ///// <summary>
        ///// Sets and gets the 
        ///// </summary>
        //public Species SelectedPokemonIndex
        //{
        //    get { return _selectedPokemonIndex; }
        //    set
        //    {
        //        Set(() => SelectedPokemonIndex, ref _selectedPokemonIndex, value);
        //        if (value != null)
        //            LoadPokemon(value.Id, SelectedVersion.Id);
        //    }
        //}



        ///// <summary>
        ///// Sets and gets the moveset
        ///// </summary>
        //public ObservableCollection<MoveLearnElement> MoveSet
        //{
        //    get { return _moveSet; }
        //    set { Set(() => MoveSet, ref _moveSet, value); }
        //}

        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        public MainViewModel(IDataService dataService)
        {
            _tokenSource = new CancellationTokenSource();
            _dataService = dataService;
            _dataService.LoadGameVersionsAsync((list, error) =>
            {
                if (error != null)
                {
                    return;
                }

                Versions = new ObservableCollection<GameVersion>(list);
                SelectedVersion = Versions.First();
            }, _tokenSource.Token);
        }

        void LoadAllSpecies(int generation)
        {
            _dataService.LoadAllSpeciesAsync(generation, (list, error) =>
            {
                if (error != null)
                    return;
                Species = new ObservableCollection<Species>(list);
                SelectedSpecies = Species.First();
            }, _tokenSource.Token);
        }
        void LoadForms(Species species, int versionGroup)
        {
            _dataService.LoadPokemonFormsAsync(species, versionGroup, (list, error) =>
            {
                if (error != null)
                    return;
                Forms = new ObservableCollection<PokemonForm>(list);
                SelectedForm = Forms.First();
            }, _tokenSource.Token);
        }

        //void LoadPokemon(int id, int version)
        //{
        //    _dataService.LoadPokemonAsync(id, version, (pokemon, error) =>
        //    {
        //        if (error != null)
        //            return;
        //        //SelectedPokemon = pokemon;
        //    }, _tokenSource.Token);
        //}

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