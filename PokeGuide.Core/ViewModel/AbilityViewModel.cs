using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

using Nito.AsyncEx;

using PokeGuide.Core.Event;
using PokeGuide.Core.Model;
using PokeGuide.Core.Service.Interface;
using PokeGuide.Core.ViewModel.Interface;

namespace PokeGuide.Core.ViewModel
{
    /// <summary>
    /// View model for viewing abilities
    /// </summary>
    public class AbilityViewModel : SingleObjectViewModel, IAbilityViewModel, INavigable
    {
        IDataService _dataService;
        INotifyTaskCompletionCollection<ModelNameBase> _abilityList;
        INotifyTaskCompletion<Ability> _currentAbility;
        INotifyTaskCompletionCollection<PokemonAbility> _pokemonList;
        int? _cachedAbilityId;

        /// <summary>
        /// Initializes a new instance of the <see cref="AbilityViewModel"/> class
        /// </summary>
        /// <param name="dataService">The service that is responsible for data loading</param>
        public AbilityViewModel(IDataService dataService) : base()
        {
            _dataService = dataService;
            AbilityList = NotifyTaskCompletionCollection<ModelNameBase>.Create(LoadAbilitiesAsync, _cachedAbilityId);
        }

        /// <summary>
        /// Sets and gets the the list of abilities
        /// </summary>
        public INotifyTaskCompletionCollection<ModelNameBase> AbilityList
        {
            get { return _abilityList; }
            set
            {
                if (AbilityList != null)
                    AbilityList.SelectedItemChanged -= SelectedAbilityChanged;
                Set(() => AbilityList, ref _abilityList, value);
                if (AbilityList != null)
                    AbilityList.SelectedItemChanged += SelectedAbilityChanged;
            }
        }
        /// <summary>
        /// Sets and gets the current selected ability
        /// </summary>
        public INotifyTaskCompletion<Ability> CurrentAbility
        {
            get { return _currentAbility; }
            set { Set(() => CurrentAbility, ref _currentAbility, value); }
        }
        /// <summary>
        /// Sets and gets the 
        /// </summary>
        public INotifyTaskCompletionCollection<PokemonAbility> PokemonList
        {
            get { return _pokemonList; }
            set { Set(() => PokemonList, ref _pokemonList, value); }
        }

        public void Activate(object param)
        {
            int abilityId = 0;
            if (param != null && Int32.TryParse(param.ToString(), out abilityId))
            {
                _cachedAbilityId = abilityId;
                AbilityList.SelectItem(abilityId);
            }
        }
        public void Deactivate(object param)
        { }

        /// <summary>
        /// Reacts to changes of the current version
        /// </summary>
        /// <param name="newVersion">The new version</param>
        protected override void ChangeVersion(GameVersion newVersion)
        {
            base.ChangeVersion(newVersion);
            AbilityList = NotifyTaskCompletionCollection<ModelNameBase>.Create(LoadAbilitiesAsync, _cachedAbilityId);
        }
        void SelectedAbilityChanged(object sender, SelectedItemChangedEventArgs<ModelNameBase> e)
        {
            CurrentAbility = null;
            PokemonList = null;
            if (e.NewItem != null)
            {
                CurrentAbility = NotifyTaskCompletion.Create(LoadAbilityByIdAsync(e.NewItem.Id));
                PokemonList = NotifyTaskCompletionCollection<PokemonAbility>.Create(LoadPokemonByAbilityAsync(e.NewItem.Id));
            }
        }

        async Task<ObservableCollection<ModelNameBase>> LoadAbilitiesAsync()
        {
            IEnumerable<ModelNameBase> list = await _dataService.LoadAbilityNamesAsync(CurrentLanguage.Id, CurrentVersion.Generation, TokenSource.Token);
            return new ObservableCollection<ModelNameBase>(list);
        }        
        async Task<Ability> LoadAbilityByIdAsync(int id)
        {
            return await _dataService.LoadAbilityByIdAsync(id, CurrentVersion.VersionGroup, CurrentLanguage.Id, TokenSource.Token);
        }
        async Task<ObservableCollection<PokemonAbility>> LoadPokemonByAbilityAsync(int abilityId)
        {
            IEnumerable<PokemonAbility> list = await _dataService.LoadPokemonByAbilityAsync(abilityId, CurrentVersion.VersionGroup, CurrentLanguage.Id, TokenSource.Token);
            return new ObservableCollection<PokemonAbility>(list);
        }
    }
}
