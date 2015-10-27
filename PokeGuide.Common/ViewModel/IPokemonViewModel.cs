using System.Collections.ObjectModel;
using GalaSoft.MvvmLight.Command;
using PokeGuide.Model;

namespace PokeGuide.ViewModel
{
    public interface IPokemonViewModel
    {
        NotifyTaskCompletion<SelectableCollection<Language>> Languages { get; set; }
        NotifyTaskCompletion<SelectableCollection<GameVersion>> Versions { get; set; }
        NotifyTaskCompletion<SelectableCollection<SpeciesName>> SpeciesList { get; set; }
        NotifyTaskCompletion<SelectableCollection<PokemonForm>> Forms { get; set; }
        NotifyTaskCompletion<PokemonForm> CurrentForm { get; set; }
        NotifyTaskCompletion<ObservableCollection<PokemonMove>> CurrentMoveSet { get; set; }
        RelayCommand LoadVersionCommand { get; }
        RelayCommand LoadSpeciesCommand { get; }
        RelayCommand LoadFormsCommand { get; }
        RelayCommand LoadFormCommand { get; }
    }
}