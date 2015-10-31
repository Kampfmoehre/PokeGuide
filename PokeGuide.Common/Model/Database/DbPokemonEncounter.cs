using SQLite.Net.Attributes;

namespace PokeGuide.Model.Database
{
    [Table("pokemon_v2_encounter")]
    public class DbPokemonEncounter
    {
        [Column("Id")]
        public int Id { get; set; }
        [Column("min_level")]
        public int MinLevel { get; set; }
        [Column("max_level")]
        public int MaxLevel { get; set; }
        [Column("rarity")]
        public int Rarity { get; set; }
        [Column("encounter_method_id")]
        public int EncounterMethodId { get; set; }
        [Column("location_area_id")]
        public int LocationAreaId { get; set; }
    }
}
