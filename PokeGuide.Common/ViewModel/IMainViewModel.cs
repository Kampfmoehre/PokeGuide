using System.Collections.ObjectModel;

using PokeGuide.Model;

namespace PokeGuide.ViewModel
{
    public interface IMainViewModel
    {
        ObservableCollection<GameVersion> Versions { get; set; }
        GameVersion SelectedVersion { get; set; }
        ObservableCollection<SpeciesName> SpeciesList { get; set; }        
        SpeciesName SelectedSpeciesName { get; set; }
        ObservableCollection<Language> Languages { get; set; }
        Language SelectedLanguage { get; set; }
        ObservableCollection<PokemonForm> Forms { get; set; }
        PokemonForm SelectedForm { get; set; }
        bool IsLoading { get; set; }
    }
}