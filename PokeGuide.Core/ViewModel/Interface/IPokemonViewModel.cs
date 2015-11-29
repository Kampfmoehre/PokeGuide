using Nito.AsyncEx;

using PokeGuide.Core.Model;

namespace PokeGuide.Core.ViewModel.Interface
{
    /// <summary>
    /// Interface for the view of a Pokémon
    /// </summary>
    public interface IPokemonViewModel
    {
        /// <summary>
        /// A selectable list of Pokémon species
        /// </summary>
        INotifyTaskCompletionCollection<ModelNameBase> PokemonList { get; set; }
        /// <summary>
        /// A selectable list of Pokémon forms
        /// </summary>
        INotifyTaskCompletionCollection<ModelNameBase> FormList { get; set; }
        /// <summary>
        /// The currently selected Form
        /// </summary>
        INotifyTaskCompletion<PokemonForm> CurrentForm { get; set; }
    }
}
