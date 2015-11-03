using SQLite.Net.Attributes;

namespace PokeGuide.Model.Database
{
    [Table("pokemon_v2_ability")]
    public class DbAbility
    {
        [Column("id")]
        public int Id { get; set; }
        [Column("name")]
        public string Name { get; set; }
        [Column("short_effect")]
        public string ShortEffect { get; set; }
        [Column("effect")]
        public string Effect { get; set; }
        [Column("flavor_text")]
        public string FlavorText { get; set; }
        [Column("identifier")]
        public string Identifier { get; set; }
        [Column("generation_id")]
        public int GenerationId { get; set; }
        [Column("is_main_series")]
        public bool IsMainSeries { get; set; }
    }
}
