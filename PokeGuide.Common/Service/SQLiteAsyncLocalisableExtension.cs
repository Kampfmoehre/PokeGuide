using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using PokeGuide.Model;

using SQLite.Net.Async;

namespace PokeGuide.Service
{
    public static class SQLiteAsyncLocalisableExtension
    {
        /// <summary>
        /// Retrieves a list of items and loads their names in a given language
        /// </summary>
        /// <typeparam name="T">The type, must implement <see cref="ILocalisable"/></typeparam>
        /// <param name="con">The sqlite connection</param>
        /// <param name="function">The method to retrieve the list</param>
        /// <param name="token">A token to cancel the operation</param>
        /// <param name="table">The name of the table where the names are stored</param>
        /// <param name="idColumn">The name of the column that is referenzing the item to localize</param>
        /// <param name="language">The ID of the language in which the name should be localized</param>
        /// <param name="fallbackLanguage">The ID of the language in which the name is loaded, when the language does not exist</param>
        /// <returns>A list of T</returns>
        public static async Task<IEnumerable<T>> LocalizeItemsAsnyc<T>(this SQLiteAsyncConnection con, Func<Task<List<T>>> function, CancellationToken token, string table, string idColumn, int language, int fallbackLanguage = 9) where T : class, ILocalisable, new()
        {
            List<T> items = await function();
            return await LocalizeItemsAsync<T>(con, items, token, table, idColumn, language, fallbackLanguage);
        }

        /// <summary>
        /// Loads names for a list of items
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="con"></param>
        /// <param name="items"></param>
        /// <param name="token"></param>
        /// <param name="table"></param>
        /// <param name="idColumn"></param>
        /// <param name="language"></param>
        /// <param name="fallbackLanguage"></param>
        /// <returns></returns>
        public static async Task<IEnumerable<T>> LocalizeItemsAsync<T>(this SQLiteAsyncConnection con, IEnumerable<T> items, CancellationToken token, string table, string idColumn, int language, int fallbackLanguage = 9) where T : class, ILocalisable, new()
        {
            string query = String.Format("SELECT e.{1} AS id, COALESCE(o.name, e.name) AS name FROM {0} e\nLEFT OUTER JOIN {0} " +
              "o ON e.{1} = o.{1} and o.local_language_id = {2}\nWHERE e.local_language_id = {3}\nGROUP BY e.{1}",
              table, idColumn, language, fallbackLanguage);
            IEnumerable<T> temp = await con.QueryAsync<T>(token, query, new object[0]);
            foreach (T item in items)
            {
                item.Name = temp.First(f => f.Id == item.Id).Name;
            }
            return items;
        }
    }
}
