using System.Threading.Tasks;

using PokeGuide.Core.Model;

namespace PokeGuide.Core.Service.Interface
{
    /// <summary>
    /// Interface for device specific settings handling 
    /// </summary>
    public interface ISettingsService
    {
        /// <summary>
        /// Loads current settings
        /// </summary>
        /// <returns>The settings</returns>
        Task<Settings> LoadSettings();
        /// <summary>
        /// Saves changes to current settings
        /// </summary>
        /// <param name="settings">The settings</param>
        void SaveSettings(Settings settings);
    }
}
