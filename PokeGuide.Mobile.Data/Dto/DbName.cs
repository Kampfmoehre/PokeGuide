using SQLite.Net.Attributes;

namespace PokeGuide.Mobile.Data.Dto
{
    /// <summary>
    /// Simple base model with ID and name properties
    /// </summary>
    class DbName
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
