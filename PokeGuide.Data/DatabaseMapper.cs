using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SQLite;
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
        SQLiteConnection _connection;

        internal DatabaseMapper(SQLiteConnection connection)
        {
            _connection = connection;
        }

        /// <summary>
        /// Fires the query from the model and maps the result in a list of model instances
        /// </summary>
        /// <param name="queryArgs">Arguments for the query</param>
        /// <param name="token">The cancellation token</param>        
        /// <returns>A list of model instances</returns>
        internal async Task<List<T>> MapFromQueryAsync(object[] queryArgs, IProgress<double> progress, CancellationToken token)
        {
            double listCount = 0;
            if (progress != null)
                listCount = await GetListCount(queryArgs, token);
            string query = new T().GetListQuery();
            query = String.Format(query, queryArgs);
            var command = new SQLiteCommand(query, _connection);
            DbDataReader reader = await command.ExecuteReaderAsync(token);            
            return await MapFromQueryAsync(reader, listCount, progress, token);
        }

        /// <summary>
        /// Retrieves the count of results of a query
        /// </summary>
        /// <param name="queryArgs"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        internal async Task<double> GetListCount(object[] queryArgs, CancellationToken token)
        {
            string query = new T().GetCountQuery();
            var command = new SQLiteCommand(String.Format(query, queryArgs), _connection);
            object result = await command.ExecuteScalarAsync(token);
            return Convert.ToDouble(result);
        }

        /// <summary>
        /// Maps query result to a model
        /// </summary>
        /// <param name="reader">The database reader from the query</param>
        /// <param name="token">The cancellation token</param>
        /// <returns>A list with a model for each row of the reader</returns>
        internal async Task<List<T>> MapFromQueryAsync(DbDataReader reader, double totalCount, IProgress<double> progress, CancellationToken token)
        {
            var result = new List<T>();
            int processed = 0;
            while (await reader.ReadAsync(token))
            {
                T objectToWrite = FillObject(reader);
                result.Add(objectToWrite);
                processed++;
                if (progress != null)
                {
                    progress.Report(Math.Round(processed / totalCount * 100, 1));
                    // Slowdown for Progress indication
                    //Thread.Sleep(250);
                }
            }

            return result;
        }

        /// <summary>
        /// Fires the query from the model and maps the result to an instance of the model
        /// </summary>
        /// <param name="queryArgs">Arguments for the query</param>
        /// <param name="token">The cancellation token</param>
        /// <returns>An instance of a model</returns>
        internal async Task<T> MapSingleObjectAsync(object[] queryArgs, CancellationToken token)
        {
            string query = new T().GetSingleQuery();
            query = String.Format(query, queryArgs);
            var command = new SQLiteCommand(query, _connection);
            DbDataReader reader = await command.ExecuteReaderAsync(token);
            return await MapSingleObjectAsync(reader, token);
        }

        /// <summary>
        /// Maps a single object from a query result
        /// </summary>
        /// <param name="reader">The database reader with the query result</param>
        /// <param name="token"></param>
        /// <returns>The object with data from the query</returns>
        internal async Task<T> MapSingleObjectAsync(DbDataReader reader, CancellationToken token)
        {
            T objectToWrite = null;
            if (await reader.ReadAsync(token))
            {
                objectToWrite = FillObject(reader);
            }
            return objectToWrite;
        }

        /// <summary>
        /// Fills an object with data from a reader
        /// </summary>
        /// <param name="reader">The reader with the data</param>
        /// <returns>The object</returns>
        T FillObject(DbDataReader reader)
        {
            //var objectToWrite = new T();
            T objectToWrite = Activator.CreateInstance<T>();
            foreach (Mapping map in objectToWrite.GetMappings())
            {
                PropertyInfo prop = objectToWrite.GetType().GetProperty(map.PropertyName);
                Type t = map.TypeToCast;
                object value = reader[map.Column];
                //SetObjectProperty(ref objectToWrite, prop, t, value);

                if (value != null && value != DBNull.Value)
                {
                    if (IsSubclassOfRawGeneric(typeof(ModelBase), prop.PropertyType))
                    {
                        var subObject = Activator.CreateInstance(prop.PropertyType);
                        PropertyInfo subProp = subObject.GetType().GetProperty("Id");
                        object tempy = Convert.ChangeType(value, t, CultureInfo.InvariantCulture);
                        subProp.SetValue(subObject, tempy);
                        //SetObjectProperty(ref subObject, subProp, t, value);
                        t = prop.PropertyType;
                        value = subObject;
                    }

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

        static bool IsSubclassOfRawGeneric(Type generic, Type toCheck)
        {
            while (toCheck != null && toCheck != typeof(object))
            {
                var cur = toCheck.IsGenericType ? toCheck.GetGenericTypeDefinition() : toCheck;
                if (generic == cur)
                {
                    return true;
                }
                toCheck = toCheck.BaseType;
            }
            return false;
        }

        void SetObjectProperty(ref object objectToWrite, PropertyInfo property, Type t, object value)
        {
            if (value != null && value != DBNull.Value)
            {
                Type nullableType = Nullable.GetUnderlyingType(property.PropertyType);
                if (nullableType != null)
                {
                    t = nullableType;
                }
                object temp = Convert.ChangeType(value, t, CultureInfo.InvariantCulture);
                property.SetValue(objectToWrite, temp);
            }
        }
    }
}
