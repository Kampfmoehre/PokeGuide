using SQLite.Net.Attributes;

namespace PokeGuide.Model.Database
{
    [Table("pokemon_v2_location")]
    public class DbLocation
    {
        [Column("id")]
        public int Id { get; set; }
        [Column("name")]
        public string Name { get; set; }
        [Column("region_id")]
        public int RegionId { get; set; }
    }
}
