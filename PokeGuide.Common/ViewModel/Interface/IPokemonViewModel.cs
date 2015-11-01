using System.Collections.ObjectModel;
using GalaSoft.MvvmLight.Command;
using Nito.AsyncEx;
using PokeGuide.Model;

namespace PokeGuide.ViewModel
{
    /// <summary>
    /// The view model for the default Pokémon view
    /// </summary>
    public interface IPokemonViewModel
    {
        /// <summary>
        /// A list of versions in which one can be selected
        /// </summary>
        INotifyTaskCompletionCollection<GameVersion> Versions { get; set; }
        /// <summary>
        /// A list of species in which one can be selected
        /// </summary>
        INotifyTaskCompletionCollection<SpeciesName> SpeciesList { get; set; }
        /// <summary>
        /// A list of Pokémon forms in which one can be selected
        /// </summary>
        INotifyTaskCompletionCollection<PokemonForm> Forms { get; set; }
        /// <summary>
        /// Information about the currently selected Pokémon form
        /// </summary>
        INotifyTaskCompletion<PokemonForm> CurrentForm { get; set; }
        /// <summary>
        /// Information about all moves that the currently selected Pokémon can learn
        /// </summary>
        INotifyTaskCompletion<ObservableCollection<PokemonMove>> CurrentMoveSet { get; set; }
        /// <summary>
        /// A list of the stats of the currently selected Pokémon
        /// </summary>
        INotifyTaskCompletion<ObservableCollection<Stat>> CurrentStats { get; set; }
        /// <summary>
        /// A list of the possible evolutions of the currently selected Pokémon
        /// </summary>
        INotifyTaskCompletion<ObservableCollection<PokemonEvolution>> CurrentEvolutions { get; set; }
        /// <summary>
        /// A list of all locations the currently selected Pokémon can be caught
        /// </summary>
        INotifyTaskCompletion<ObservableCollection<PokemonLocation>> CurrentLocations { get; set; }
        RelayCommand<int> LoadFormCommand { get; }
        /// <summary>
        /// A command to open the settings page
        /// </summary>
        RelayCommand OpenSettingsCommand { get; }
        /// <summary>
        /// Command for Back button
        /// </summary>
        RelayCommand NavigateBackCommand { get; }
    }
}