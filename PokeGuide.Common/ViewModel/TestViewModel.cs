using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Nito.AsyncEx;
using PokeGuide.Model;
using PokeGuide.Service.Interface;
using PokeGuide.ViewModel.Interface;
using Windows.UI.Core;

namespace PokeGuide.ViewModel
{
    public class TestViewModel : ViewModelBase, ITestViewModel
    {
        CancellationTokenSource _tokenSource;
        ITestService _testService;
        ObservableCollection<Model.Db.GameVersion> _versionsNew;
        ObservableCollection<Model.Db.Ability> _abilitiesNew;
        ObservableCollection<GameVersion> _versionsOld;
        ObservableCollection<Ability> _abilitiesOld;
        ObservableCollection<Ability> _abilities;
        Species _speciesNew;
        Species _speciesOld;
        PokemonForm _formNew;
        PokemonForm _formOld;
        string _timeConsumedNew;
        string _timeConsumedOld;
        RelayCommand _loadVersionsNewCommand;
        RelayCommand _loadAbililitiesNewCommand;
        RelayCommand _loadVersionsOldCommand;
        RelayCommand _loadAbilitiesOldCommand;
        RelayCommand _loadSpeciesOldCommand;
        RelayCommand _loadSpeciesNewCommand;
        RelayCommand _loadFormOldCommand;
        RelayCommand _loadFormNewCommand;
        RelayCommand _loadAbilitiesCommand;
        private CoreDispatcher _dispatcher;

        /// <summary>
        /// Sets and gets the 
        /// </summary>
        public ObservableCollection<Model.Db.GameVersion> VersionsNew
        {
            get { return _versionsNew; }
            set { Set(() => VersionsNew, ref _versionsNew, value); }
        }
        /// <summary>
        /// Sets and gets the 
        /// </summary>
        public ObservableCollection<GameVersion> VersionsOld
        {
            get { return _versionsOld; }
            set { Set(() => VersionsOld, ref _versionsOld, value); }
        }
        /// <summary>
        /// Sets and gets the 
        /// </summary>
        public ObservableCollection<Model.Db.Ability> AbilitiesNew
        {
            get { return _abilitiesNew; }
            set { Set(() => AbilitiesNew, ref _abilitiesNew, value); }
        }
        /// <summary>
        /// Sets and gets the 
        /// </summary>
        public ObservableCollection<Ability> AbilitiesOld
        {
            get { return _abilitiesOld; }
            set { Set(() => AbilitiesOld, ref _abilitiesOld, value); }
        }
        /// <summary>
        /// Sets and gets the 
        /// </summary>
        public ObservableCollection<Ability> Abilities
        {
            get { return _abilities; }
            set { Set(() => Abilities, ref _abilities, value); }
        }
        /// <summary>
        /// Sets and gets the 
        /// </summary>
        public Species SpeciesNew
        {
            get { return _speciesNew; }
            set { Set(() => SpeciesNew, ref _speciesNew, value); }
        }
        /// <summary>
        /// Sets and gets the 
        /// </summary>
        public Species SpeciesOld
        {
            get { return _speciesOld; }
            set { Set(() => SpeciesOld, ref _speciesOld, value); }
        }        
        /// <summary>
        /// Sets and gets the 
        /// </summary>
        public string TimeConsumedNew
        {
            get { return _timeConsumedNew; }
            set { Set(() => TimeConsumedNew, ref _timeConsumedNew, value); }
        }
        /// <summary>
        /// Sets and gets the 
        /// </summary>
        public string TimeConsumedOld
        {
            get { return _timeConsumedOld; }
            set { Set(() => TimeConsumedOld, ref _timeConsumedOld, value); }
        }
        /// <summary>
        /// Sets and gets the 
        /// </summary>
        public PokemonForm FormNew
        {
            get { return _formNew; }
            set { Set(() => FormNew, ref _formNew, value); }
        }
        /// <summary>
        /// Sets and gets the 
        /// </summary>
        public PokemonForm FormOld
        {
            get { return _formOld; }
            set { Set(() => FormOld, ref _formOld, value); }
        }
        public RelayCommand LoadVersionsNewCommand
        {
            get
            {
                if (_loadVersionsNewCommand == null)
                    _loadVersionsNewCommand = new RelayCommand(async () => 
                    {
                        DateTime start = DateTime.Now;
                        VersionsNew = await LoadVersionsNewAsync(6);
                        DateTime end = DateTime.Now;
                        TimeSpan span = end - start;
                        TimeConsumedNew = span.TotalMilliseconds.ToString();
                    });
                return _loadVersionsNewCommand;
            }
        }
        public RelayCommand LoadVersionsOldCommand
        {
            get
            {
                if (_loadVersionsOldCommand == null)
                    _loadVersionsOldCommand = new RelayCommand(async () => 
                    {
                        DateTime start = DateTime.Now;
                        VersionsOld = await LoadVersionsOldAsync(6);
                        DateTime end = DateTime.Now;
                        TimeSpan span = end - start;
                        TimeConsumedOld = span.TotalMilliseconds.ToString();
                    });
                return _loadVersionsOldCommand;
            }
        }
        public RelayCommand LoadAbilitiesNewCommand
        {
            get
            {
                if (_loadAbililitiesNewCommand == null)
                    _loadAbililitiesNewCommand = new RelayCommand(async () =>
                    {
                        DateTime start = DateTime.Now;
                        AbilitiesNew = await LoadAbilitiesNewAsync(6);
                        DateTime end = DateTime.Now;
                        TimeSpan span = end - start;
                        TimeConsumedNew = span.TotalMilliseconds.ToString();
                    });
                return _loadAbililitiesNewCommand;
            }
        }
        public RelayCommand LoadAbilitiesOldCommand
        {
            get
            {
                if (_loadAbilitiesOldCommand == null)
                    _loadAbilitiesOldCommand = new RelayCommand(async () =>
                    {
                        DateTime start = DateTime.Now;
                        AbilitiesOld = await LoadAbilitiesOldAsync(6);
                        DateTime end = DateTime.Now;
                        TimeSpan span = end - start;
                        TimeConsumedOld = span.TotalMilliseconds.ToString();
                    });
                return _loadAbilitiesOldCommand;
            }
        }
        public RelayCommand LoadSpeciesNewCommand
        {
            get
            {
                if (_loadSpeciesNewCommand == null)
                    _loadSpeciesNewCommand = new RelayCommand(async () =>
                    {
                        var sw = Stopwatch.StartNew();
                        SpeciesNew = await LoadSpeciesNew(227, 15, 6);
                        //Species temp = await LoadSpeciesNew(227, 15, 6);                        
                        //await _dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                        //{
                        //    SpeciesNew = temp;
                        //});
                        sw.Stop();
                        TimeConsumedNew = String.Format("Loaded Species {0:d} in {1:0.00} seconds", 227, (double)sw.ElapsedMilliseconds / 1000);
                    });
                return _loadSpeciesNewCommand;
            }
        }
        public RelayCommand LoadSpeciesOldCommand
        {
            get
            {
                if (_loadSpeciesOldCommand == null)
                    _loadSpeciesOldCommand = new RelayCommand(async () =>
                    {
                        var sw = Stopwatch.StartNew();
                        SpeciesOld = await LoadSpeciesOld(227, 15, 6);
                        //SpeciesOld = await LoadSpeciesOld(227, 15, 6);
                        sw.Stop();
                        TimeConsumedOld = String.Format("Loaded Species {0:d} in {1:0.00} seconds", 227, (double)sw.ElapsedMilliseconds / 1000);
                    });
                return _loadSpeciesOldCommand;
            }
        }
        public RelayCommand LoadFormNewCommand
        {
            get
            {
                if (_loadFormNewCommand == null)
                    _loadFormNewCommand = new RelayCommand(async () =>
                    {
                        var sw = Stopwatch.StartNew();
                        FormNew = await LoadFormNewAsync(270, new GameVersion { Generation = 6, Id = 25, VersionGroup = 15 }, 6);
                        sw.Stop();
                        TimeConsumedNew = String.Format("Loaded Form {0:d} in {1:0.00} seconds", 270, (double)sw.ElapsedMilliseconds / 1000);
                    });
                return _loadFormNewCommand;
            }
        }
        public RelayCommand LoadFormOldCommand
        {
            get
            {
                if (_loadFormOldCommand == null)
                    _loadFormOldCommand = new RelayCommand(async () =>
                    {
                        var sw = Stopwatch.StartNew();
                        FormOld = await LoadFormOldAsync(270, new GameVersion { Generation = 6, Id = 25, VersionGroup = 15 }, 6);
                        sw.Stop();
                        TimeConsumedOld = String.Format("Loaded Form {0:d} in {1:0.00} seconds", 270, (double)sw.ElapsedMilliseconds / 1000);
                    });
                return _loadFormOldCommand;
            }
        }
        public RelayCommand LoadAbilitiesCommand
        {
            get
            {
                if (_loadAbilitiesCommand == null)
                    _loadAbilitiesCommand = new RelayCommand(async () =>
                    {
                        var sw = Stopwatch.StartNew();
                        Abilities = await LoadAbilitiesAsync(6);
                        sw.Stop();
                        TimeConsumedNew = String.Format("Loaded {0:d} Abilities in {1:0.00} seconds", Abilities.Count, (double)sw.ElapsedMilliseconds / 1000);
                    });
                return _loadAbilitiesCommand;
            }
        }
        public TestViewModel(ITestService testService)
        {
            _tokenSource = new CancellationTokenSource();
            _testService = testService;
            _dispatcher = CoreWindow.GetForCurrentThread().Dispatcher;
            _testService.InitializeResources(6, _tokenSource.Token);
        }

        async Task<ObservableCollection<Model.Db.GameVersion>> LoadVersionsNewAsync(int language)
        {
            IEnumerable<Model.Db.GameVersion> versions = await _testService.LoadVersionsNewAsync(language, _tokenSource.Token);
            return new ObservableCollection<Model.Db.GameVersion>(versions);
        }
        async Task<ObservableCollection<GameVersion>> LoadVersionsOldAsync(int language)
        {
            IEnumerable<GameVersion> versions = await _testService.LoadVersionsOldAsync(language, _tokenSource.Token);
            return new ObservableCollection<GameVersion>(versions);
        }
        async Task<ObservableCollection<Model.Db.Ability>> LoadAbilitiesNewAsync(int language)
        {
            IEnumerable<Model.Db.Ability> versions = await _testService.LoadAbilitiesNewAsync(6, language, _tokenSource.Token);
            return new ObservableCollection<Model.Db.Ability>(versions);
        }
        async Task<ObservableCollection<Ability>> LoadAbilitiesOldAsync(int language)
        {
            IEnumerable<Ability> versions = await _testService.LoadAbilitiesOldAsync(6, language, _tokenSource.Token);
            return new ObservableCollection<Ability>(versions);
        }
        async Task<Species> LoadSpeciesNew(int id, int versionGroupId, int language)
        {
            return await _testService.LoadSpeciesByIdAsync(id, versionGroupId, language, _tokenSource.Token);
        }
        async Task<Species> LoadSpeciesOld(int id, int versionGroupId, int language)
        {
            return await _testService.LoadSpeciesByIdOldAsync(id, versionGroupId, language, _tokenSource.Token);
        }
        async Task<PokemonForm> LoadFormNewAsync(int id, GameVersion version, int language)
        {
            return await _testService.LoadFormByIdAsync(id, version, language, _tokenSource.Token);
        }
        async Task<PokemonForm> LoadFormOldAsync(int id, GameVersion version, int language)
        {
            return await _testService.LoadFormByIdOldAsync(id, version, language, _tokenSource.Token);
        }
        async Task<ObservableCollection<Ability>> LoadAbilitiesAsync(int language)
        {
            List<Ability> abilities = await _testService.LoadAbilitiesAsync(language, _tokenSource.Token);
            return new ObservableCollection<Ability>(abilities);
        }
    }
}
