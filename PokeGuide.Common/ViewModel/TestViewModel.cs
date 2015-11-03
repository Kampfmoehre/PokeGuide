using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;

using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;

using PokeGuide.Model;
using PokeGuide.Service.Interface;
using PokeGuide.ViewModel.Interface;

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
        string _timeConsumedNew;
        string _timeConsumedOld;
        RelayCommand _loadVersionsNewCommand;
        RelayCommand _loadAbililitiesNewCommand;
        RelayCommand _loadVersionsOldCommand;
        RelayCommand _loadAbilitiesOldCommand;
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
        public TestViewModel(ITestService testService)
        {
            _tokenSource = new CancellationTokenSource();
            _testService = testService;
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
    }
}
