using SQLite.Net.Attributes;

namespace PokeGuide.Model.Database
{
    [Table("pokemon_v2_encounterconditionvalue")]
    public class DbEncounterCondition
    {
        [Column("id")]
        public int Id { get; set; }
        [Column("name")]
        public string Name { get; set; }
        [Column("encounter_id")]
        public int EncounterId { get; set; }
        [Column("encounter_name")]
        public string EncounterName { get; set; }
    }
}
