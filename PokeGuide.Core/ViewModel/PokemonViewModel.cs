using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

using GalaSoft.MvvmLight.Views;

using Nito.AsyncEx;

using PokeGuide.Core.Event;
using PokeGuide.Core.Model;
using PokeGuide.Core.Service.Interface;
using PokeGuide.Core.ViewModel.Interface;

namespace PokeGuide.Core.ViewModel
{
    public class PokemonViewModel : SingleObjectViewModel, IPokemonViewModel, INavigable
    {
        INavigationService _navigationService;
        IDataService _dataService;
        INotifyTaskCompletionCollection<ModelNameBase> _pokemonList;
        INotifyTaskCompletionCollection<ModelNameBase> _formList;
        INotifyTaskCompletion<PokemonForm> _currentForm;
        int _cachedSpeciesId;
        int _cachedFormId;

        public PokemonViewModel(INavigationService navigationService, IDataService dataService) : base()
        {
            _navigationService = navigationService;
            _dataService = dataService;
        }
        /// <summary>
        /// Sets and gets the 
        /// </summary>
        public INotifyTaskCompletionCollection<ModelNameBase> PokemonList
        {
            get { return _pokemonList; }
            set
            {
                if (PokemonList != null)
                    PokemonList.SelectedItemChanged -= SelectedSpeciesChanged;
                Set(() => PokemonList, ref _pokemonList, value);
                if (PokemonList != null)
                    PokemonList.SelectedItemChanged += SelectedSpeciesChanged;
            }
        }
        /// <summary>
        /// Sets and gets the 
        /// </summary>
        public INotifyTaskCompletionCollection<ModelNameBase> FormList
        {
            get { return _formList; }
            set
            {
                Set(() => FormList, ref _formList, value);
                if (FormList != null)
                    FormList.SelectedItemChanged += SelectedFormChanged;
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

        public void Activate(object parameter)
        {
            throw new NotImplementedException();
        }

        public void Deactivate(object parameter)
        {
            throw new NotImplementedException();
        }

        protected override void ChangeVersion(GameVersion newVersion)
        {
            PokemonList = null;
            base.ChangeVersion(newVersion);
            PokemonList = NotifyTaskCompletionCollection<ModelNameBase>.Create(LoadPokemonAsync(), _cachedSpeciesId);
        }

        void SelectedSpeciesChanged(object sender, SelectedItemChangedEventArgs<ModelNameBase> e)
        {
            FormList = null;
            if (e.NewItem != null)
            {
                _cachedSpeciesId = e.NewItem.Id;
                FormList = NotifyTaskCompletionCollection<ModelNameBase>.Create(LoadFormsAsync(), _cachedFormId);
            }
        }
        void SelectedFormChanged(object sender, SelectedItemChangedEventArgs<ModelNameBase> e)
        {
            CurrentForm = null;
            if (e.NewItem != null)
            {
                _cachedFormId = e.NewItem.Id;
                CurrentForm = NotifyTaskCompletion.Create<PokemonForm>(LoadFormByIdAsync(e.NewItem.Id));
            }
        }

        async Task<ObservableCollection<ModelNameBase>> LoadPokemonAsync()
        {
            IEnumerable<ModelNameBase> list = await _dataService.LoadPokemonAsync(CurrentVersion.Generation, CurrentLanguage.Id, TokenSource.Token);
            return new ObservableCollection<ModelNameBase>(list);
        }
        async Task<ObservableCollection<ModelNameBase>> LoadFormsAsync()
        {
            IEnumerable<ModelNameBase> list = await _dataService.LoadPokemonFormsAsync(CurrentVersion.VersionGroup, CurrentLanguage.Id, TokenSource.Token);
            return new ObservableCollection<ModelNameBase>(list);
        }
        async Task<PokemonForm> LoadFormByIdAsync(int id)
        {
            return await _dataService.LoadPokemonFormByIdAsync(id, CurrentVersion, CurrentLanguage.Id, TokenSource.Token);
        }
    }
}
