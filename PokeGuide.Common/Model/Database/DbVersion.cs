using SQLite.Net.Attributes;

namespace PokeGuide.Model.Database
{
    [Table("pokemon_v2_version")]
    public class DbVersion
    {
        [Column("id")]
        public int Id { get; set; }
        [Column("name")]
        public string Name { get; set; }
        [Column("version_group_id")]
        public int VersionGroupId { get; set; }
        [Column("generation_id")]
        public int Generation { get; set; }
    }
}
