using SQLite.Net.Attributes;

namespace PokeGuide.Core.Database
{
    /// <summary>
    /// Simple table with id and name column for most lists
    /// </summary>
    public class DbName
    {
        /// <summary>
        /// Gets or sets the ID of the object
        /// </summary>
        [Column("id")]
        public int Id { get; set; }
        /// <summary>
        /// Gets or sets the localized name of the object
        /// </summary>
        [Column("name")]
        public string Name { get; set; }
    }
}
