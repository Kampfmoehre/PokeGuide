using SQLite.Net.Attributes;

namespace PokeGuide.Model.Database
{
    [Table("pokemon_v2_pokemonmove")]
    class DbPokemonMove
    {
        [Column("id")]
        public int Id { get; set; }
        [Column("order")]
        public int? Order { get; set; }
        [Column("level")]
        public int Level { get; set; }
        [Column("move_id")]
        public int MoveId { get; set; }
        [Column("pokemon_id")]
        public int PokemonId { get; set; }
        [Column("version_group_id")]
        public int VersionGroupId { get; set; }
        [Column("move_learn_method_id")]
        public int MoveLearnMethodId { get; set; }
    }
}
