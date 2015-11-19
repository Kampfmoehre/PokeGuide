using SQLite.Net.Attributes;

namespace PokeGuide.Core.Database
{
    [Table("move_damage_classes")]
    public class DbMoveDamageClass : DbName
    {
        [Column("identifier")]
        public string Identifier { get; set; }
        [Column("description")]
        public string Description { get; set; }
    }
}
