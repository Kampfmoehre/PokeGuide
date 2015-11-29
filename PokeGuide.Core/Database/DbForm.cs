using SQLite.Net.Attributes;

namespace PokeGuide.Core.Database
{
    [Table("pokemon_forms")]
    public class DbForm : DbName
    {
        [Column("species_id")]
        public int SpeciesId { get; set; }
        [Column("genus")]
        public string Genus { get; set; }
        [Column("height")]
        public int Height { get; set; }
        [Column("weight")]
        public int Weight { get; set; }
        [Column("base_experience")]
        public int BaseExperience { get; set; }
        [Column("base_happiness")]
        public int BaseHappiness { get; set; }
        [Column("hatch_counter")]
        public int HatchCounter { get; set; }
        [Column("color_id")]
        public int ColorId { get; set; }
        [Column("color_name")]
        public string ColorName { get; set; }
        [Column("shape_id")]
        public int ShapeId { get; set; }
        [Column("shape_name")]
        public string ShapeName { get; set; }
        [Column("habitat_id")]
        public int Habitat { get; set; }
        [Column("habitat_name")]
        public string HabitatName { get; set; }
        [Column("capture_rate")]
        public int CaptureRate { get; set; }
        [Column("is_baby")]
        public bool IsBaby { get; set; }
        [Column("type1")]
        public int Type1Id { get; set; }
        [Column("type2")]
        public int? Type2Id { get; set; }
        [Column("ability1")]
        public int Ability1Id { get; set; }
        [Column("ability1_name")]
        public string Ability1Name { get; set; }
        [Column("ability2")]
        public int? Ability2Id { get; set; }
        [Column("ability2_name")]
        public string Ability2Name { get; set; }
        [Column("hidden_ability")]
        public int? HiddenAbility { get; set; }
        [Column("hidden_ability_name")]
        public string HiddenAbilityName { get; set; }
        [Column("rarity")]
        public int? ItemRarity { get; set; }
        [Column("item_id")]
        public int? ItemId { get; set; }
        [Column("item_name")]
        public string ItemName { get; set; }
        [Column("growth_rate_id")]
        public int GrowthRateId { get; set; }
        [Column("growth_rate_name")]
        public string GrowthRateName { get; set; }
        [Column("pokedex_id")]
        public int? PokedexId { get; set; }
        [Column("pokedex_number")]
        public int? PokedexNumber { get; set; }
        [Column("pokedex_name")]
        public string PokedexName { get; set; }
    }
}
