using SQLite.Net.Attributes;

namespace PokeGuide.Core.Database
{
    [Table("versions")]
    public class DbVersion : DbName
    {
        [Column("version_group_id")]
        public int VersionGroup { get; set; }
        [Column("generation_id")]
        public int Generation { get; set; }
    }
}
