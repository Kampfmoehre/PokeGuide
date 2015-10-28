using SQLite.Net.Attributes;

namespace PokeGuide.Model.Database
{
    [Table("pokemon_v2_type")]
    public class DbType
    {
        [Column("id")]
        public int Id { get; set; }
        [Column("name")]
        public string Name { get; set; }
        [Column("move_damage_class_id")]
        public int MoveDamageClassId { get; set; }
        [Column("generation_id")]
        public int GenerationId { get; set; }
    }
}
