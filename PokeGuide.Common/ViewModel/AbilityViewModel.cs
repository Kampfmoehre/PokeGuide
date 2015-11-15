using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;

using GalaSoft.MvvmLight.Views;

using Nito.AsyncEx;

using PokeGuide.Model;
using PokeGuide.Service.Interface;
using PokeGuide.ViewModel.Interface;

namespace PokeGuide.ViewModel
{
    public class AbilityViewModel : ViewModel, IAbilityViewModel, INavigable
    {
        INavigationService _navigationService;
        IPokemonService _pokemonService;
        CancellationTokenSource _tokenSource;
        INotifyTaskCompletionCollection<Ability> _abilities;
        INotifyTaskCompletion<Ability> _selectedAbility;
        int _cachedAbilityId;

        /// <summary>
        /// Sets and gets the 
        /// </summary>
        public INotifyTaskCompletionCollection<Ability> Abilities
        {
            get { return _abilities; }
            set
            {
                if (Abilities != null)
                    Abilities.SelectedItemChanged -= SelectedAbilityChanged;
                Set(() => Abilities, ref _abilities, value);
                if (Abilities != null)  
                    Abilities.SelectedItemChanged += SelectedAbilityChanged;
            }
        }

        /// <summary>
        /// Sets and gets the 
        /// </summary>
        public INotifyTaskCompletion<Ability> SelectedAbility
        {
            get { return _selectedAbility; }
            set { Set(() => SelectedAbility, ref _selectedAbility, value); }
        }

        public AbilityViewModel(INavigationService navigationService, IPokemonService pokemonService, IStaticDataService staticDataService) : base(staticDataService)
        {
            _tokenSource = new CancellationTokenSource();
            _navigationService = navigationService;
            _pokemonService = pokemonService;
            _cachedAbilityId = 1;
        }

        protected override void ChangeLanguage(Language newLanguage)
        {
            Abilities = null;
            base.ChangeLanguage(newLanguage);
            //if (newLanguage != null && CurrentVersion != null)
                
        }

        protected override void ChangeVersion(GameVersion newVersion)
        {
            SelectedAbility = null;
            base.ChangeVersion(newVersion);
            if (newVersion != null)
                Abilities = NotifyTaskCompletionCollection<Ability>.Create(LoadAbilitiesAsync(CurrentVersion.VersionGroup, CurrentLanguage), _cachedAbilityId);
            //SelectedAbilityChanged(this, new SelectedItemChangedEventArgs<Ability>(null, new Ability { Id = _cachedAbilityId }));
        }

        void SelectedAbilityChanged(object sender, SelectedItemChangedEventArgs<Ability> e)
        {
            if (e.NewItem != null)
            {
                _cachedAbilityId = e.NewItem.Id;
                SelectedAbility = NotifyTaskCompletion.Create<Ability>(LoadAbilityAsync(e.NewItem.Id, CurrentVersion.VersionGroup, CurrentLanguage));
            }
        }

        async Task<ObservableCollection<Ability>> LoadAbilitiesAsync(int generation, int languageId)
        {
            IEnumerable<Ability> abilities = await _pokemonService.LoadAbilitiesAsync(generation, languageId, _tokenSource.Token);
            return new ObservableCollection<Ability>(abilities);
        }

        async Task<Ability> LoadAbilityAsync(int id, int versionGroupId, int languageId)
        {
            return await _pokemonService.LoadAbilityAsync(id, versionGroupId, languageId, _tokenSource.Token);
        }

        public void CleanUp()
        {
            if (Abilities != null)
                Abilities.SelectedItemChanged -= SelectedAbilityChanged;
        }

        public void Activate(object parameter)
        {
            if (parameter != null)
                _cachedAbilityId = (int)parameter;
        }

        public void Deactivate(object parameter)
        {
            throw new NotImplementedException();
        }
    }
}
