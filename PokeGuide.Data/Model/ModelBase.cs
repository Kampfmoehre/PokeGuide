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
        public string Name { get; set; }
        internal virtual List<Mapping> GetMappings()
        {
            return new List<Mapping>
            {
                new Mapping { Column = "Id", PropertyName = "Id", TypeToCast = typeof(Int32) },
                new Mapping { Column = "Name", PropertyName = "Name", TypeToCast = typeof(String) }
            };
        }
    }
}
