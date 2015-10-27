using System;
using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
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
        NotifyTaskCompletion<SelectableCollection<GameVersion>> _versions;
        NotifyTaskCompletion<SelectableCollection<SpeciesName>> _speciesList;
        NotifyTaskCompletion<SelectableCollection<PokemonForm>> _forms;
        /// <summary>
        /// Sets and gets the 
        /// </summary>
        public NotifyTaskCompletion<SelectableCollection<GameVersion>> Versions
        {
            get { return _versions ; }
            set { Set(() => Versions, ref _versions , value); }
        }
        /// <summary>
        /// Sets and gets the 
        /// </summary>
        public NotifyTaskCompletion<SelectableCollection<SpeciesName>> SpeciesList
        {
            get { return _speciesList; }
            set { Set(() => SpeciesList, ref _speciesList, value); }
        }
        /// <summary>
        /// Sets and gets the 
        /// </summary>
        public NotifyTaskCompletion<SelectableCollection<PokemonForm>> Forms
        {
            get { return _forms; }
            set { Set(() => Forms, ref _forms, value); }
        }
        NotifyTaskCompletion<SelectableCollection<Language>> _languages;
        /// <summary>
        /// Sets and gets the 
        /// </summary>
        public NotifyTaskCompletion<SelectableCollection<Language>> Languages
        {
            get { return _languages; }
            set
            {
                Set(() => Languages, ref _languages, value);
                LoadVersionCommand.RaiseCanExecuteChanged();
            }
        }
        NotifyTaskCompletion<PokemonForm> _currentForm;
        /// <summary>
        /// Sets and gets the 
        /// </summary>
        public NotifyTaskCompletion<PokemonForm> CurrentForm
        {
            get { return _currentForm; }
            set { Set(() => CurrentForm, ref _currentForm, value); }
        }
        NotifyTaskCompletion<ObservableCollection<PokemonMove>> _currentMoveSet;
        /// <summary>
        /// Sets and gets the 
        /// </summary>
        public NotifyTaskCompletion<ObservableCollection<PokemonMove>> CurrentMoveSet
        {
            get { return _currentMoveSet; }
            set { Set(() => CurrentMoveSet, ref _currentMoveSet, value); }
        }
        RelayCommand _loadVersionCommand;
        public RelayCommand LoadVersionCommand {
            get
            {
                if (_loadVersionCommand == null)
                {
                    _loadVersionCommand = new RelayCommand(() =>
                        Versions = new NotifyTaskCompletion<SelectableCollection<GameVersion>>(LoadVersionsAsync(Languages.Result.SelectedItem.Id)),
                        IsLanguageSet
                    );
                }
                return _loadVersionCommand;
            }
        }
        RelayCommand _loadSpeciesCommand;
        public RelayCommand LoadSpeciesCommand
        {
            get
            {
                if (_loadSpeciesCommand == null)
                {
                    _loadSpeciesCommand = new RelayCommand(() =>
                        SpeciesList = new NotifyTaskCompletion<SelectableCollection<SpeciesName>>(LoadSpeciesAsync(Versions.Result.SelectedItem, Languages.Result.SelectedItem.Id)),
                        () => { return IsLanguageSet() && IsVersionSet(); }
                    );
                }
                return _loadSpeciesCommand;
            }
        }
        RelayCommand _loadFormsCommand;
        public RelayCommand LoadFormsCommand
        {
            get
            {
                if (_loadFormsCommand == null)
                {
                    _loadFormsCommand = new RelayCommand(() =>
                        Forms = new NotifyTaskCompletion<SelectableCollection<PokemonForm>>(LoadFormsAsync(SpeciesList.Result.SelectedItem.Id, Versions.Result.SelectedItem, Languages.Result.SelectedItem.Id)),
                        () => { return IsLanguageSet() && IsVersionSet() && SpeciesList.Result != null && SpeciesList.Result.SelectedItem != null; }
                    );
                }
                return _loadFormsCommand;
            }
        }
        RelayCommand _loadFormCommand;
        public RelayCommand LoadFormCommand
        {
            get
            {
                if (_loadFormsCommand == null)
                {
                    _loadFormCommand = new RelayCommand(() =>
                        {
                            CurrentForm = new NotifyTaskCompletion<PokemonForm>(LoadFormAsync(Forms.Result.SelectedItem.Id, Versions.Result.SelectedItem, Languages.Result.SelectedItem.Id));
                            CurrentMoveSet = new NotifyTaskCompletion<ObservableCollection<PokemonMove>>(LoadMoveSetAsync(Forms.Result.SelectedItem.Id, Versions.Result.SelectedItem, Languages.Result.SelectedItem.Id));
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
            Languages = new NotifyTaskCompletion<SelectableCollection<Language>>(LoadLanguagesAsync(6));            
                 
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
                Versions = new NotifyTaskCompletion<SelectableCollection<GameVersion>>(LoadVersionsAsync(6));
                SpeciesList = new NotifyTaskCompletion<SelectableCollection<SpeciesName>>(LoadSpeciesAsync(null, 6));
                Forms = new NotifyTaskCompletion<SelectableCollection<PokemonForm>>(LoadFormsAsync(1, null, 6));
                CurrentForm = new NotifyTaskCompletion<PokemonForm>(LoadFormAsync(1, null, 6));
                CurrentMoveSet = new NotifyTaskCompletion<ObservableCollection<PokemonMove>>(LoadMoveSetAsync(6, null, 6));
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
        async Task<SelectableCollection<PokemonForm>> LoadFormsAsync(int speciesId, GameVersion version, int language)
        {
            return new SelectableCollection<PokemonForm>(await _pokemonService.LoadFormsAsync(speciesId, version, language, _tokenSource.Token));
        }
        async Task<PokemonForm> LoadFormAsync(int formid, GameVersion version, int language)
        {
            return await _pokemonService.LoadFormAsync(formid, version, language, _tokenSource.Token);
        }
        async Task<ObservableCollection<PokemonMove>> LoadMoveSetAsync(int formid, GameVersion version, int language)
        {
            return await _moveService.LoadMoveSetAsync(formid, version, language, _tokenSource.Token);
        }
    }
}
