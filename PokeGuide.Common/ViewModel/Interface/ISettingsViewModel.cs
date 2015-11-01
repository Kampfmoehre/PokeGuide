using GalaSoft.MvvmLight.Command;

using PokeGuide.Model;

namespace PokeGuide.ViewModel.Interface
{
    /// <summary>
    /// Interface for settings view model
    /// </summary>
    public interface ISettingsViewModel
    {
        /// <summary>
        /// When the user changes the data display language
        /// </summary>
        /// <param name="newLanguage">The new language</param>
        void ChangeLanguage(Language newLanguage);
        /// <summary>
        /// Command for Back button
        /// </summary>
        RelayCommand NavigateBackCommand { get; }
    }
}
