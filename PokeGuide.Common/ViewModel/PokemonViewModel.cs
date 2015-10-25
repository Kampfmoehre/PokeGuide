using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using PokeGuide.Model;
using PokeGuide.Service;
using PokeGuide.ViewModel;

namespace PokeGuide.ViewModel
{    
    public class PokemonViewModel : ViewModelBase, IPokemonViewModel
    {
        CancellationTokenSource _tokenSource;
        IStaticDataService _staticDataService;
        IPokemonService _pokemonService;
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

        public PokemonViewModel(IStaticDataService staticDataService, IPokemonService pokemonService)
        {
            _tokenSource = new CancellationTokenSource();
            _staticDataService = staticDataService;
            _pokemonService = pokemonService;



            Versions = new NotifyTaskCompletion<SelectableCollection<GameVersion>>(LoadVersions(6));
            
            Versions.PropertyChanged += (s, e) =>
            {
                Versions.Result.PropertyChanged += (s1, e1) =>
                {
                    if (e1.PropertyName == "SelectedItem" && Versions.Result.SelectedItem != null)
                        SpeciesList = new NotifyTaskCompletion<SelectableCollection<SpeciesName>>(LoadSpecies(Versions.Result.SelectedItem, 6));
                };
                if (e.PropertyName == "IsSuccessfullyCompleted")
                {
                    
                }
                    
                    
            };            
        }

        async Task<SelectableCollection<GameVersion>> LoadVersions(int language)
        {
            return new SelectableCollection<GameVersion>(await _staticDataService.LoadVersionsAsync(language, _tokenSource.Token));
        }
        async Task<SelectableCollection<SpeciesName>> LoadSpecies(GameVersion version, int language)
        {
            return new SelectableCollection<SpeciesName>(await _pokemonService.LoadAllSpeciesAsync(version, language, _tokenSource.Token));
        }
    }
}
