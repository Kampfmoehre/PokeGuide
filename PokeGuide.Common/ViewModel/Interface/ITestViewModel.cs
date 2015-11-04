using System.Collections.ObjectModel;
using GalaSoft.MvvmLight.Command;
using Nito.AsyncEx;
using PokeGuide.Model;

namespace PokeGuide.ViewModel.Interface
{
    public interface ITestViewModel
    {
        ObservableCollection<Model.Db.GameVersion> VersionsNew { get; set; }
        ObservableCollection<Model.Db.Ability> AbilitiesNew { get; set; }
        ObservableCollection<GameVersion> VersionsOld { get; set; }        
        ObservableCollection<Ability> AbilitiesOld { get; set; }
        Species SpeciesNew { get; set; }
        Species SpeciesOld { get; set; }
        PokemonForm FormNew { get; set; }        
        PokemonForm FormOld { get; set; }
        RelayCommand LoadVersionsNewCommand { get; }
        RelayCommand LoadAbilitiesNewCommand { get; }
        RelayCommand LoadAbilitiesOldCommand { get; }
        RelayCommand LoadSpeciesNewCommand { get; }        
        RelayCommand LoadSpeciesOldCommand { get; }
        RelayCommand LoadFormNewCommand { get; }
        RelayCommand LoadFormOldCommand { get; }
        string TimeConsumedNew { get; set; }
        string TimeConsumedOld { get; set; }
    }
}
