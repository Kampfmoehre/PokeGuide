using System;
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
        DateTime _debugStart;
        DateTime _debugEnd;

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
        RelayCommand _loadVersionCommand;
        RelayCommand _loadSpeciesCommand;
        RelayCommand _loadFormsCommand;
        RelayCommand _loadFormCommand;
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
                    Languages.PropertyChanged += Languages_propertyChanged;
                }
                Set(() => Languages, ref _languages, value);
                if (Languages != null)
                    Languages.PropertyChanged += Languages_propertyChanged;
            }
        }

        void Languages_propertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "IsSuccessfullyCompleted")
            {
                Languages.SelectedItemChanged += SelectedLanguageChanged;
                Languages.Result.SelectedItem = Languages.Result.Collection.First(f => f.Id == 6);
                //  ResetOperations();
                //  LoadVersionCommand.RaiseCanExecuteChanged();
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
                Versions = NotifyTaskCompletionCollection<Language>.Create(LoadVersionsAsync(e.NewItem.Id));
        }
        /// <summary>
        /// Sets and gets the
        /// </summary>
        public INotifyTaskCompletionCollection<GameVersion> Versions
        {
            get { return _versions ; }
            set
            {
                Set(() => Versions, ref _versions , value);
                if (Versions != null)
                {
                    Versions.PropertyChanged += (s, e) =>
                    {
                        if (e.PropertyName == "IsSuccessfullyCompleted")
                            LoadSpeciesCommand.RaiseCanExecuteChanged();
                    };
                }
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
                Set(() => SpeciesList, ref _speciesList, value);
                if (SpeciesList != null)
                {
                    SpeciesList.PropertyChanged += (s, e) =>
                    {
                        if (e.PropertyName == "IsSuccessfullyCompleted")
                            LoadFormsCommand.RaiseCanExecuteChanged();
                    };
                }
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
                Set(() => Forms, ref _forms, value);
                if (Forms != null)
                {
                    Forms.PropertyChanged += (s, e) =>
                    {
                        if (e.PropertyName == "IsSuccessfullyCompleted")
                            LoadFormCommand.RaiseCanExecuteChanged();
                    };
                }
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
        public RelayCommand LoadVersionCommand {
            get
            {
                if (_loadVersionCommand == null)
                {
                    _loadVersionCommand = new RelayCommand(() =>
                        Versions = NotifyTaskCompletion.Create(LoadVersionsAsync(Languages.Result.SelectedItem.Id)),
                        IsLanguageSet
                    );
                }
                return _loadVersionCommand;
            }
        }
        public RelayCommand LoadSpeciesCommand
        {
            get
            {
                if (_loadSpeciesCommand == null)
                {
                    _loadSpeciesCommand = new RelayCommand(() =>
                        SpeciesList = NotifyTaskCompletion.Create(LoadSpeciesAsync(Versions.Result.SelectedItem, Languages.Result.SelectedItem.Id)),
                        () => { return IsLanguageSet() && IsVersionSet(); }
                    );
                }
                return _loadSpeciesCommand;
            }
        }
        public RelayCommand LoadFormsCommand
        {
            get
            {
                if (_loadFormsCommand == null)
                {
                    _loadFormsCommand = new RelayCommand(() =>
                        Forms = NotifyTaskCompletion.Create(LoadFormsAsync(SpeciesList.Result.SelectedItem, Versions.Result.SelectedItem, Languages.Result.SelectedItem.Id)),
                        () => { return IsLanguageSet() && IsVersionSet() && SpeciesList.Result != null && SpeciesList.Result.SelectedItem != null; }
                    );
                }
                return _loadFormsCommand;
            }
        }
        public RelayCommand LoadFormCommand
        {
            get
            {
                if (_loadFormCommand == null)
                {
                    _loadFormCommand = new RelayCommand(() =>
                        {
                            _debugStart = DateTime.Now;
                            CurrentForm = NotifyTaskCompletion.Create(LoadFormAsync(Forms.Result.SelectedItem.Id, Versions.Result.SelectedItem, Languages.Result.SelectedItem.Id));
                            CurrentMoveSet = NotifyTaskCompletion.Create(LoadMoveSetAsync(Forms.Result.SelectedItem.Id, Versions.Result.SelectedItem, Languages.Result.SelectedItem.Id));

                        },
                        () => { return IsLanguageSet() && IsVersionSet() && Forms.Result != null && Forms.Result.SelectedItem != null; }
                    );
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
            Languages = NotifyTaskCompletion.Create(LoadLanguagesAsync(6));

            //Versions = new NotifyTaskCompletion<SelectableCollection<GameVersion>>(LoadVersions(6));

            //Versions.PropertyChanged += (s, e) =>
            //{
            //    Versions.Result.PropertyChanged += (s1, e1) =>
            //    {
            //        if (e1.PropertyName == "SelectedItem" && Versions.Result.SelectedItem != null)
            //            SpeciesList = new NotifyTaskCompletion<SelectableCollection<SpeciesName>>(LoadSpecies(Versions.Result.SelectedItem, 6));
            //    };
            //};
            if (IsInDesignMode)
            {
                Versions = NotifyTaskCompletion.Create(LoadVersionsAsync(6));
                SpeciesList = NotifyTaskCompletion.Create(LoadSpeciesAsync(null, 6));
                Forms = NotifyTaskCompletion.Create(LoadFormsAsync(new SpeciesName { Id = 1 }, null, 6));
                CurrentForm = NotifyTaskCompletion.Create(LoadFormAsync(1, null, 6));
                CurrentMoveSet = NotifyTaskCompletion.Create(LoadMoveSetAsync(6, null, 6));
            }
        }

        bool IsLanguageSet()
        {
            return Languages.Result != null && Languages.Result.SelectedItem != null;
        }
        bool IsVersionSet()
        {
            return Versions.Result != null && Versions.Result.SelectedItem != null;
        }

        async Task<SelectableCollection<Language>> LoadLanguagesAsync(int defaultLanguage)
        {
            return new SelectableCollection<Language>(await _staticDataService.LoadLanguagesAsync(defaultLanguage, _tokenSource.Token));
        }
        async Task<SelectableCollection<GameVersion>> LoadVersionsAsync(int language)
        {
            return new SelectableCollection<GameVersion>(await _staticDataService.LoadVersionsAsync(language, _tokenSource.Token));
        }
        async Task<SelectableCollection<SpeciesName>> LoadSpeciesAsync(GameVersion version, int language)
        {
            return new SelectableCollection<SpeciesName>(await _pokemonService.LoadAllSpeciesAsync(version, language, _tokenSource.Token));
        }
        async Task<SelectableCollection<PokemonForm>> LoadFormsAsync(SpeciesName species, GameVersion version, int language)
        {
            return new SelectableCollection<PokemonForm>(await _pokemonService.LoadFormsAsync(species, version, language, _tokenSource.Token));
        }
        async Task<PokemonForm> LoadFormAsync(int formid, GameVersion version, int language)
        {
            return await _pokemonService.LoadFormAsync(formid, version, language, _tokenSource.Token);
        }
        async Task<ObservableCollection<PokemonMove>> LoadMoveSetAsync(int formid, GameVersion version, int language)
        {
            var unused = await _pokemonService.LoadMoveSetAsync(formid, version, language, _tokenSource.Token);
            _debugEnd = DateTime.Now;
            TimeSpan span = _debugEnd - _debugStart;
            TimeConsumed = String.Format("{0} sek und {1} ms", span.Seconds, span.Milliseconds);
            return unused;
        }


        string _timeConsumed;
        /// <summary>
        /// Sets and gets the
        /// </summary>
        public string TimeConsumed
        {
            get { return _timeConsumed; }
            set { Set(() => TimeConsumed, ref _timeConsumed, value); }
        }

        void ResetOperations()
        {
            //_tokenSource.Cancel();
            _pokemonService.InitializeResources(Languages.Result.SelectedItem.Id, _tokenSource.Token);
        }
    }
}
