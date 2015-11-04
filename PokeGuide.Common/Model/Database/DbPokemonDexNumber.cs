using SQLite.Net.Attributes;

namespace PokeGuide.Model.Database
{
    [Table("pokemon_dex_numbers")]
    public class DbPokemonDexNumber
    {
        [Column("id")]
        public int Id { get; set; }
        [Column("pokedex_number")]
        public int PokedexNumber { get; set; }
        [Column("pokemon_species_id")]
        public int PokemonSpeciesId { get; set; }
        [Column("pokedex_id")]
        public int PokedexId { get; set; }
    }
}
