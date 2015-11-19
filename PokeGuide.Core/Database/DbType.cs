using SQLite.Net.Attributes;

namespace PokeGuide.Core.Database
{
    [Table("types")]
    public class DbType : DbName
    {
        [Column("identifier")]
        public string Identifier { get; set; }
        [Column("damage_class_id")]
        public int DamageClassId { get; set; }
        [Column("generation_id")]
        public int GenerationId { get; set; }
    }
}
