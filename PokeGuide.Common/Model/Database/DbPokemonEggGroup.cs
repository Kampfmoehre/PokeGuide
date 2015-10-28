using SQLite.Net.Attributes;

namespace PokeGuide.Model.Database
{
    [Table("pokemon_v2_pokemonegggroup")]
    public class DbPokemonEggGroup
    {
        [Column("id")]
        public int Id { get; set; }
        [Column("pokemon_species_id")]
        public int PokemonSpeciesId { get; set; }
        [Column("egg_group_id")]
        public int EggGroupId { get; set; }
    }
}
