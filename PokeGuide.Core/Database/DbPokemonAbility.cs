using SQLite.Net.Attributes;

namespace PokeGuide.Core.Database
{
    [Table("pokemon_abilities")]
    public class DbPokemonAbility : DbName
    {
        [Column("pokemon_id")]
        public int PokemonId { get; set; }
        [Column("slot")]
        public int Slot { get; set; }
        [Column("is_hidden")]
        public bool IsHidden { get; set; }
    }
}
