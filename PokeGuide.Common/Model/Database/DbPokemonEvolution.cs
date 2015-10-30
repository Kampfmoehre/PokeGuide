﻿using SQLite.Net.Attributes;

namespace PokeGuide.Model.Database
{
    [Table("pokemon_v2_pokemonevolution")]
    public class DbPokemonEvolution
    {
        [Column("id")]
        public int Id { get; set; }
        [Column("name")]
        public string Name { get; set; }
        [Column("min_level")]
        public int? MinLevel { get; set; }
        [Column("evolution_trigger")]
        public string EvolutionTrigger { get; set; }
        [Column("evolution_item_id")]
        public int? EvolutionItemId { get; set; }
    }
}
