using GalaSoft.MvvmLight.Command;

namespace PokeGuide.ViewModel
{
    /// <summary>
    /// Interface for the main navigation page
    /// </summary>
    public interface IMainViewModel
    {        
        /// <summary>
        /// A command which navigates to the Pokémon view
        /// </summary>
        RelayCommand OpenPokemonViewCommand { get; }
        /// <summary>
        /// A command which opens the settings view
        /// </summary>
        RelayCommand OpenSettingsViewCommand { get; }
    }
}