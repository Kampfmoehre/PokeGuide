using SQLite.Net.Attributes;

namespace PokeGuide.Model.Database
{
    [Table("pokemon_v2_pokemonspecies")]
    public class DbPokemonSpecies
    {
        [Column("id")]
        public int Id { get; set; }
        [Column("name")]
        public string Name { get; set; }
        [Column("genus")]
        public string Genus { get; set; }
        [Column("order")]
        public int Order { get; set; }
        [Column("gender_rate")]
        public int GenderRate { get; set; }
        [Column("capture_rate")]
        public int CaptureRate { get; set; }
        [Column("base_happiness")]
        public int BaseHappiness { get; set; }
        [Column("is_baby")]
        public int IsBaby { get; set; }
        [Column("hatch_counter")]
        public int HatchCounter { get; set; }
        [Column("has_gender_differences")]
        public int HasGenderDifferences { get; set; }
        [Column("forms_switchable")]
        public int FormsSwitchable { get; set; }
        [Column("evolution_chain_id")]
        public int EvolutionChainId { get; set; }
        [Column("evolves_from_species_id")]
        public int EvolvesFromSpeciesId { get; set; }
        [Column("generation_id")]
        public int GenerationId { get; set; }
        [Column("pokemon_color_id")]
        public int PokemonColorId { get; set; }
        [Column("pokemon_habitat_id")]
        public int PokemonHabitatId { get; set; }
        [Column("pokemon_shape_id")]
        public int PokemonShapeId { get; set; }
        [Column("growth_rate_id")]
        public int GrowthRateId { get; set; }
        [Column("growth_rate")]
        public string GrowthRate { get; set; }
        [Column("pokedex_number")]
        public int? PokedexNumber { get; set; }
        [Column("dex_name")]
        public string PokedexName { get; set; }
        [Column("dex_id")]
        public int? PokedexId { get; set; }
    }
}
