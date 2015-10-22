using System;
using System.Collections.Generic;

namespace PokeGuide.Data.Model
{
    /// <summary>
    /// Base class for all database models
    /// </summary>
    public class ModelBase
    {
        public ModelBase()
        {
            
        }

        /// <summary>
        /// The id of the object
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// The (localized) name of the object
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Returns a list of mappings
        /// </summary>
        /// <returns>A list of mappings</returns>
        internal virtual List<Mapping> GetMappings()
        {
            return new List<Mapping>
            {
                new Mapping { Column = "Id", PropertyName = "Id", TypeToCast = typeof(Int32) },
                new Mapping { Column = "Name", PropertyName = "Name", TypeToCast = typeof(String) }
            };
        }
        /// <summary>
        /// Get the query to retrieve data from the database
        /// </summary>
        /// <returns></returns>
        internal virtual string GetListQuery()
        {
            return String.Empty;
        }
        internal virtual string GetSingleQuery()
        {
            return String.Empty;
        }
    }
}
