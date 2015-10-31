using SQLite.Net.Attributes;

namespace PokeGuide.Model.Database
{
    [Table("pokemon_v2_encountermethod")]
    public class DbEncounterMethod
    {
        [Column("id")]
        public int Id { get; set; }
        [Column("name")]
        public string Name { get; set; }
    }
}
