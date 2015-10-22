using System;
using System.Collections.Generic;
using System.Threading;

namespace PokeGuide.Wpf.Model
{
    /// <summary>
    /// Service for retrieving data from the database
    /// </summary>
    public interface IDataService
    {
        void GetData(Action<DataItem, Exception> callback);
        /// <summary>
        /// Loads a list of all game versions
        /// </summary>
        /// <param name="callback">The callback with the result</param>
        /// <param name="token">The cancellation token</param>
        void LoadGameVersionsAsync(Action<List<GameVersion>, Exception> callback, CancellationToken token);
        /// <summary>
        /// Loads a list of all Pokémon
        /// </summary>
        /// <param name="version">The version for which all Pokémon should be loaded</param>
        /// <param name="callback">The callback with the result</param>
        /// <param name="token">The cancellation token</param>
        void LoadAllSpeciesAsync(int generation, Action<List<Species>, Exception> callback, CancellationToken token);
        /// <summary>
        /// Loads all forms for a species
        /// </summary>
        /// <param name="species">The species</param>
        /// <param name="versionGroup">The ID of the version group</param>
        /// <param name="callback">The callback with the result</param>
        /// <param name="token">The cancellation token</param>
        void LoadPokemonFormsAsync(Species species, int versionGroup, Action<List<PokemonForm>, Exception> callback, CancellationToken token);        
        /// <summary>
        /// Loads a complete moveset for a Pokémon
        /// </summary>
        /// <param name="pokemon">The ID of the Pokémon</param>
        /// <param name="version">The version</param>
        /// <param name="callback">The callback with the result</param>
        /// <param name="token">The cancellation token</param>
        void LoadPokemonMoveSet(int pokemon, int version, Action<List<MoveLearnElement>, Exception> callback, CancellationToken token);
    }
}
