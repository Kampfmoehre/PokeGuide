using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace PokeGuide.Wpf.Model
{
    /// <summary>
    /// Interface for static data loading
    /// </summary>
    public interface IStaticDataService
    {
        /// <summary>
        /// Loads all languages
        /// </summary>
        /// <param name="language">The language to localize the names</param>
        /// <param name="token">A token to cancel the operation</param>
        /// <returns>A list of languages</returns>
        Task<List<Language>> LoadLanguages(int language, CancellationToken token);
        /// <summary>
        /// Loads a list of all game versions
        /// </summary>
        /// <param name="language">The ID of the language</param>
        /// <param name="progress">An interface to track progress</param>
        /// <param name="token">A token to cancel the operation</param>
        /// <returns>A list of all game versions</returns>
        Task<List<GameVersion>> LoadGameVersionAsync(int language, IProgress<double> progress, CancellationToken token);
    }
}
