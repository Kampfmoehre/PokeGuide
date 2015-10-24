using SQLite.Net.Attributes;

namespace PokeGuide.Model.Database
{
    [Table("pokemon_v2_language")]
    public class DbLanguage
    {
        [Column("id")]
        public int Id { get; set; }
        [Column("name")]
        public string Name { get; set; }
        [Column("iso639")]
        public string Iso639 { get; set; }
        [Column("iso3166")]
        public string Iso3166 { get; set; }
        [Column("official")]
        public bool IsOfficial { get; set; }
    }
}
