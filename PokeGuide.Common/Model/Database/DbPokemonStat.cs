using SQLite.Net.Attributes;

namespace PokeGuide.Model.Database
{
    [Table("pokemon_v2_pokemonstat")]
    public class DbPokemonStat
    {
        [Column("stat_id")]
        public int StatId { get; set; }
        [Column("name")]
        public string Name { get; set; }
        [Column("pokemon_id")]
        public int PokemonId { get; set; }
        [Column("base_stat")]
        public int BaseStat { get; set; }
        [Column("effort")]
        public int Effort { get; set; }
    }
}
