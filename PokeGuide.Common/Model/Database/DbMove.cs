using SQLite.Net.Attributes;

namespace PokeGuide.Model.Database
{
    [Table("pokemon_v2_move")]
    public class DbMove
    {
        [Column("id")]
        public int Id { get; set; }
        [Column("name")]
        public string Name { get; set; }
        [Column("power")]
        public int? Power { get; set; }
        [Column("pp")]
        public int PowerPoints { get; set; }
        [Column("accuracy")]
        public int Accuracy { get; set; }
        [Column("priority")]
        public int Priority { get; set; }
        [Column("move_damage_class_id")]
        public int MoveDamageClass { get; set; }
        [Column("type_id")]
        public int Type { get; set; }
    }
}
