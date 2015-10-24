using SQLite.Net.Attributes;

namespace PokeGuide.Model.Database
{
    [Table("pokemon_v2_pokemonform")]
    public class DbPokemonForm
    {
        [Column("id")]
        public int Id { get; set; }
        [Column("name")]
        public string Name { get; set; }
        [Column("height")]
        public int Height { get; set; }
        [Column("weight")]
        public int Weight { get; set; }
        [Column("base_experience")]
        public int BaseExperience { get; set; }
        [Column("type1")]
        public int Type1 { get; set; }
        [Column("type2")]
        public int? Type2 { get; set; }
        [Column("ability1")]
        public int Ability1 { get; set; }      
        [Column("ability2")]
        public int? Ability2 { get; set; }
        [Column("hidden_ability")]
        public int? HiddenAbility { get; set; }
    }
}
