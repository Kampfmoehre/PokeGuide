using System;

namespace PokeGuide.Data.Model
{
    /// <summary>
    /// Data to map query result to a data object
    /// </summary>
    internal class Mapping
    {
        /// <summary>
        /// The sql column of the query result
        /// </summary>
        public string Column { get; set; }
        /// <summary>
        /// The name of the property in which the value should be stored
        /// </summary>
        public string PropertyName { get; set; }
        /// <summary>
        /// The type of the property in which the value should be stored
        /// </summary>
        public Type TypeToCast { get; set; }
    }
}
