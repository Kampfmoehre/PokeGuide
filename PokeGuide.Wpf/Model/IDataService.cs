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
        void LoadGameVersionsAsync(Action<List<GameVersion>, Exception> callback, CancellationToken token);
        /// <summary>
        /// Loads a list of all Pokémon
        /// </summary>
        /// <param name="version">The version for which all Pokémon should be loaded</param>
        /// <param name="callback">The callback with the result</param>
        /// <param name="token">The cancellation token</param>
        void LoadAllPokemonAsync(int version, Action<List<Pokemon>, Exception> callback, CancellationToken token);
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
