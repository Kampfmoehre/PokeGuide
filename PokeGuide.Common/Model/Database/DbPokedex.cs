using SQLite.Net.Attributes;

namespace PokeGuide.Model.Database
{
    [Table("pokemon_v2_pokedex")]
    public class DbPokedex
    {
        [Column("id")]
        public int Id { get; set; }
        [Column("name")]
        public string Name { get; set; }
        [Column("is_main_series")]
        public int IsMainSeries { get; set; }
    }
}
