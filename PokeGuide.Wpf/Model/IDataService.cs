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
        void LoadAllPokemonAsync(int version, Action<List<Pokemon>, Exception> callback, CancellationToken token);
    }
}
