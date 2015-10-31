using SQLite.Net.Attributes;

namespace PokeGuide.Model.Database
{
    [Table("pokemon_v2_locationarea")]
    public class DbLocationArea
    {
        [Column("id")]
        public int Id { get; set; }
        [Column("name")]
        public string Name { get; set; }
        [Column("location_id")]
        public int LocationId { get; set; }
    }
}
