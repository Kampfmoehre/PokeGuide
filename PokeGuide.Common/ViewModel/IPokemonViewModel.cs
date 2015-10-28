using System.Collections.ObjectModel;
using GalaSoft.MvvmLight.Command;
using Nito.AsyncEx;
using PokeGuide.Model;

namespace PokeGuide.ViewModel
{
    public interface IPokemonViewModel
    {        
        INotifyTaskCompletion<SelectableCollection<Language>> Languages { get; set; }
        INotifyTaskCompletion<SelectableCollection<GameVersion>> Versions { get; set; }
        INotifyTaskCompletion<SelectableCollection<SpeciesName>> SpeciesList { get; set; }
        INotifyTaskCompletion<SelectableCollection<PokemonForm>> Forms { get; set; }
        INotifyTaskCompletion<PokemonForm> CurrentForm { get; set; }
        INotifyTaskCompletion<ObservableCollection<PokemonMove>> CurrentMoveSet { get; set; }
        RelayCommand LoadVersionCommand { get; }
        RelayCommand LoadSpeciesCommand { get; }
        RelayCommand LoadFormsCommand { get; }
        RelayCommand LoadFormCommand { get; }
        string TimeConsumed { get; set; }
    }
}