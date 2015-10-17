using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Globalization;
using System.Reflection;
using System.Threading;
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
        /// <returns>A list with a model for each row of the reader</returns>
        internal List<T> MapFromQuery(DbDataReader reader)
        {
            var result = new List<T>();
            while (reader.Read())
            {
                T objectToWrite = FillObject(reader);
                result.Add(objectToWrite);
            }

            return result;
        }

        internal async Task<T> MapSingleObject(DbDataReader reader, CancellationToken token)
        {
            T objectToWrite = null;
            if (await reader.ReadAsync(token))
            {
                objectToWrite = FillObject(reader);
            }
            return objectToWrite;
        }

        T FillObject(DbDataReader reader)
        {
            var objectToWrite = new T();
            foreach (Mapping map in objectToWrite.GetMappings())
            {
                PropertyInfo prop = objectToWrite.GetType().GetProperty(map.PropertyName);
                Type t = map.TypeToCast;
                Object value = reader[map.Column];
                if (value != null)
                {
                    Type nullableType = Nullable.GetUnderlyingType(prop.PropertyType);
                    if (nullableType != null)
                    {
                        t = nullableType;
                    }
                    object temp = Convert.ChangeType(value, t, CultureInfo.InvariantCulture);
                    prop.SetValue(objectToWrite, temp);
                }
            }
            return objectToWrite;
        }
    }
}
