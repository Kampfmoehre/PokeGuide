using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using PokeGuide.Core.Model;

namespace PokeGuide.Core.Service.Interface
{
    /// <summary>
    /// Interface for the service that loads all data from the database
    /// </summary>
    public interface IDataService
    {
        /// <summary>
        /// Loads a language from the database by its ID
        /// </summary>
        /// <param name="id">The ID of the language</param>
        /// <param name="displayLanguage">The ID of the language in which to display the name</param>
        /// <param name="token">A token to cancel to operation</param>
        /// <returns>A <see cref="DisplayLanguage"/> object</returns>
        Task<DisplayLanguage> LoadLanguageByIdAsync(int id, int displayLanguage, CancellationToken token);
        /// <summary>
        /// Loads A version by its ID
        /// </summary>
        /// <param name="id">The ID of the version</param>
        /// <param name="language">The ID of the language in which to display the name</param>
        /// <param name="token">A token to cancel to operation</param>
        /// <returns>A <see cref="GameVersion"/> object</returns>
        Task<GameVersion> LoadVersionByIdAsync(int id, int language, CancellationToken token);
        /// <summary>
        /// Loads a list of all versions
        /// </summary>
        /// <param name="language">The ID of the language in which to display the names</param>
        /// <param name="token">A token to cancel to operation</param>
        /// <returns>A list of <see cref="GameVersion"/> objects</returns>
        Task<IEnumerable<GameVersion>> LoadVersionsAsync(int language, CancellationToken token);
        /// <summary>
        /// Loads a list of all abilities that occur in a given generation, containing their ID and a localized name
        /// </summary>
        /// <param name="language">The ID of the language in which to display the names</param>
        /// <param name="generation">The ID of the generation</param>
        /// <param name="token">A token to cancel to operation</param>
        /// <returns>A list of <see cref="ModelNameBase"/> with ability IDs and names</returns>
        Task<IEnumerable<ModelNameBase>> LoadAbilityNamesAsync(int language, int generation, CancellationToken token);
        /// <summary>
        /// Loads a single ability by its ID for a version group
        /// </summary>
        /// <param name="id">The ID of the ability</param>
        /// <param name="versionGroup">The ID of the version group</param>
        /// <param name="language">The ID of the language in which to display the name<</param>
        /// <param name="token">A token to cancel to operation</param>
        /// <returns>A <see cref="Ability"/> object</returns>
        Task<Ability> LoadAbilityByIdAsync(int id, int versionGroup, int language, CancellationToken token);        
    }
}
