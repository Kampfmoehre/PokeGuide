using System.Collections.ObjectModel;
using GalaSoft.MvvmLight.Command;
using PokeGuide.Model;

namespace PokeGuide.ViewModel
{
    public interface IMainViewModel
    {
        ObservableCollection<Language> Languages { get; set; }
        //NotifyTaskCompletion<ObservableCollection<Language>> Languages { get; set; }
        Language SelectedLanguage { get; set; }
        ObservableCollection<GameVersion> Versions { get; set; }
        //NotifyTaskCompletion<ObservableCollection<GameVersion>> Versions { get; set; }
        GameVersion SelectedVersion { get; set; }
        ObservableCollection<SpeciesName> SpeciesList { get; set; }
        //NotifyTaskCompletion<ObservableCollection<SpeciesName>> SpeciesList { get; set; }
        SpeciesName SelectedSpecies { get; set; }
        ObservableCollection<PokemonForm> Forms { get; set; }
        //NotifyTaskCompletion<ObservableCollection<PokemonForm>> Forms { get; set; }
        PokemonForm SelectedForm { get; set; }
        RelayCommand LoadLanguagesCommand { get; set; }
        bool IsLoading { get; set; }
    }
}