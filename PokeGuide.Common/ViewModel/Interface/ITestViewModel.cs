using System.Collections.ObjectModel;
using GalaSoft.MvvmLight.Command;
using PokeGuide.Model;

namespace PokeGuide.ViewModel.Interface
{
    public interface ITestViewModel
    {
        ObservableCollection<Model.Db.GameVersion> VersionsNew { get; set; }
        ObservableCollection<Model.Db.Ability> AbilitiesNew { get; set; }
        RelayCommand LoadVersionsNewCommand { get; }
        RelayCommand LoadAbilitiesNewCommand { get; }
        ObservableCollection<GameVersion> VersionsOld { get; set; }
        RelayCommand LoadVersionsOldCommand { get; }
        ObservableCollection<Ability> AbilitiesOld { get; set; }
        RelayCommand LoadAbilitiesOldCommand { get; }
        string TimeConsumedNew { get; set; }
        string TimeConsumedOld { get; set; }
    }
}
