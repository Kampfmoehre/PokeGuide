using System.Collections.ObjectModel;
using PokeGuide.Model;

namespace PokeGuide.ViewModel
{
    public interface IPokemonViewModel
    {
        NotifyTaskCompletion<SelectableCollection<GameVersion>> Versions { get; set; }
        NotifyTaskCompletion<SelectableCollection<SpeciesName>> SpeciesList { get; set; }
        NotifyTaskCompletion<SelectableCollection<PokemonForm>> Forms { get; set; }
    }
}