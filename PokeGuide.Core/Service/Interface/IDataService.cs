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
        /// Initializes loading lists of much needed data that are cached
        /// </summary>
        /// <param name="displayLanguage">The ID of the language in which to display names</param>
        /// <param name="token">A token to cancel the operation</param>
        void InitializeResources(int displayLanguage, CancellationToken token);
        /// <summary>
        /// Loads a language from the database by its ID
        /// </summary>
        /// <param name="id">The ID of the language</param>
        /// <param name="displayLanguage">The ID of the language in which to display the name</param>
        /// <param name="token">A token to cancel the operation</param>
        /// <returns>A <see cref="DisplayLanguage"/> object</returns>
        Task<DisplayLanguage> LoadLanguageByIdAsync(int id, int displayLanguage, CancellationToken token);
        /// <summary>
        /// Loads A version by its ID
        /// </summary>
        /// <param name="id">The ID of the version</param>
        /// <param name="language">The ID of the language in which to display the name</param>
        /// <param name="token">A token to cancel the operation</param>
        /// <returns>A <see cref="GameVersion"/> object</returns>
        Task<GameVersion> LoadVersionByIdAsync(int id, int language, CancellationToken token);
        /// <summary>
        /// Loads a list of all versions
        /// </summary>
        /// <param name="language">The ID of the language in which to display the names</param>
        /// <param name="token">A token to cancel the operation</param>
        /// <returns>A list of <see cref="GameVersion"/> objects</returns>
        Task<IEnumerable<GameVersion>> LoadVersionsAsync(int language, CancellationToken token);
        /// <summary>
        /// Loads a list of all abilities that occur in a given generation, containing their ID and a localized name
        /// </summary>
        /// <param name="language">The ID of the language in which to display the names</param>
        /// <param name="generation">The ID of the generation</param>
        /// <param name="token">A token to cancel the operation</param>
        /// <returns>A list of <see cref="ModelNameBase"/> with ability IDs and names</returns>
        Task<IEnumerable<ModelNameBase>> LoadAbilityNamesAsync(int language, int generation, CancellationToken token);
        /// <summary>
        /// Loads a single ability by its ID for a version group
        /// </summary>
        /// <param name="id">The ID of the ability</param>
        /// <param name="versionGroup">The ID of the version group</param>
        /// <param name="language">The ID of the language in which to display text<</param>
        /// <param name="token">A token to cancel the operation</param>
        /// <returns>An <see cref="Ability"/> or <c>null</c> if no move with this ID exists in the given version group</returns>
        Task<Ability> LoadAbilityByIdAsync(int id, int versionGroup, int language, CancellationToken token);
        /// <summary>
        /// Loads a list of all moves that occur in a given generation, containing their ID and a localized name
        /// </summary>
        /// <param name="languageId">The ID of the language in which to display the names</param>
        /// <param name="generation">The ID of the generation</param>
        /// <param name="token">A token to cancel the operation</param>
        /// <returns>A list of <see cref="ModelNameBase"/> with move IDs and names</returns>
        Task<IEnumerable<ModelNameBase>> LoadMoveNamesAsync(int language, int generation, CancellationToken token);
        /// <summary>
        /// Loads a single move by its ID for a version
        /// </summary>
        /// <param name="id">The ID of the move</param>
        /// <param name="version">A <see cref="GameVersion"/> object which contains the version</param>
        /// <param name="language">The ID of the language in which to display text</param>
        /// <param name="token">A token to cancel the operation</param>
        /// <returns>A <see cref="Move"/> or <c>null</c> if no move with this ID exists in the given version</returns>
        Task<Move> LoadMoveByIdAsync(int id, GameVersion version, int language, CancellationToken token);
        /// <summary>
        /// Loads a list of Pokémon that can have an ability
        /// </summary>
        /// <param name="abilityId">The ID of the ability</param>
        /// <param name="versionGroupId">The ID of the version group</param>
        /// <param name="language">The ID of the language in which to display the names of the Pokémon</param>
        /// <param name="token">A token to cancel the operation</param>
        /// <returns>A list of <see cref="PokemonAbility"/> objects</returns>
        Task<IEnumerable<PokemonAbility>> LoadPokemonByAbilityAsync(int abilityId, int versionGroupId, int language, CancellationToken token);
        /// <summary>
        /// Loads a list of Pokémon that occur in a given generation
        /// </summary>
        /// <param name="generation">The ID of the generation</param>
        /// <param name="language">The ID of the language in which to display to name</param>
        /// <param name="token">A token to cancel the operation</param>
        /// <returns>A list of <see cref="ModelNameBase"/> with Pokémon IDs and names</returns>
        Task<IEnumerable<ModelNameBase>> LoadPokemonAsync(int generation, int language, CancellationToken token);
        /// <summary>
        /// Loads a list of Pokémon forms that occur in a given version group
        /// </summary>
        /// <param name="versionGroupId">The ID of the version group</param>
        /// <param name="language">The ID of the language</param>
        /// <param name="token">A token to cancel the operation</param>
        /// <returns>A list of <see cref="ModelNameBase"/> with Pokémon forms names and IDs</returns>
        Task<IEnumerable<ModelNameBase>> LoadPokemonFormsAsync(int versionGroupId, int language, CancellationToken token);
        /// <summary>
        /// Loads a Pokémon form by its ID
        /// </summary>
        /// <param name="id">The ID of the Pokémon form</param>
        /// <param name="version">The version</param>
        /// <param name="language">The ID of the language in which to display all names</param>
        /// <param name="token">A token to cancel the operation</param>
        /// <returns>A <see cref="PokemonForm"/> object</returns>
        Task<PokemonForm> LoadPokemonFormByIdAsync(int id, GameVersion version, int language, CancellationToken token);
    }
}
