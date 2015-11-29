using GalaSoft.MvvmLight.Command;

using PokeGuide.Core.Model;

namespace PokeGuide.Core.ViewModel.Interface
{
    /// <summary>
    /// Interface for main viewmodel
    /// </summary>
    public interface IMainViewModel
    {
        /// <summary>
        /// A selectable list of versions
        /// </summary>
        INotifyTaskCompletionCollection<GameVersion> VersionList { get; set; }
        /// <summary>
        /// Command that navigates to the pokemon view
        /// </summary>
        RelayCommand NavigateToPokemonCommand { get; }
        /// <summary>
        /// Command that navigates to the ability view
        /// </summary>
        RelayCommand NavigateToAbilitiesCommand { get; }
        /// <summary>
        /// Command that navigates to the move view
        /// </summary>
        RelayCommand NavigateToMovesCommand { get; }
    }
}
