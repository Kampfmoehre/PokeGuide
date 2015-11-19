using Nito.AsyncEx;

using PokeGuide.Core.Model;

namespace PokeGuide.Core.ViewModel.Interface
{
    /// <summary>
    /// Interface for a viewmodel that contains a selectable list of moves
    /// </summary>
    public interface IMoveViewModel
    {
        /// <summary>
        /// The selectable list to display
        /// </summary>
        INotifyTaskCompletionCollection<ModelNameBase> MoveList { get; set; }
        /// <summary>
        /// The ability that was selected in the list
        /// </summary>
        INotifyTaskCompletion<Move> CurrentMove { get; set; }
    }
}
