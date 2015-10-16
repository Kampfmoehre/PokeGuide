using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Reflection;
using System.Threading.Tasks;

using PokeGuide.Data.Model;

namespace PokeGuide.Data
{
    /// <summary>
    /// Extension for Modelbase to map query results to model
    /// </summary>
    /// <typeparam name="T">The type of the model</typeparam>
    public class DatabaseMapper<T> where T : ModelBase, new()
    {
        /// <summary>
        /// Maps query result to a model
        /// </summary>
        /// <param name="reader">The database reader from the query</param>
        /// <param name="mapping">The mapping</param>
        /// <returns>A list with a model for each row of the reader</returns>
        internal List<T> MapFromQuery(DbDataReader reader, IEnumerable<Mapping> mapping)
        {
            var result = new List<T>();
            while (reader.Read())
            {
                var objectToWrite = new T();
                foreach (Mapping map in mapping)
                {
                    PropertyInfo prop = objectToWrite.GetType().GetProperty(map.PropertyName);
                    Type t = map.TypeToCast;
                    object temp = Convert.ChangeType(reader[map.Column], t);
                    prop.SetValue(objectToWrite, temp);                    
                }
                result.Add(objectToWrite);
            }

            return result;
        }

        internal async Task<T> MapSingleObject(DbDataReader reader, IEnumerable<Mapping> mapping)
        {
            T objectToWrite = null;
            if (await reader.ReadAsync())
            {
                objectToWrite = new T();
                foreach (Mapping map in mapping)
                {
                    PropertyInfo prop = objectToWrite.GetType().GetProperty(map.PropertyName);
                    Type t = map.TypeToCast;
                    object temp = Convert.ChangeType(reader[map.Column], t);
                    prop.SetValue(objectToWrite, temp);
                }
            }
            return objectToWrite;
        }
    }
}
