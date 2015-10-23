using System;
using System.Collections.Generic;

namespace PokeGuide.Data.Model
{
    public class Species : ModelBase
    {
        public string Genus { get; set; }
        public int CaptureRate { get; set; }
        public int BaseHappiness { get; set; }
        public int HatchCounter { get; set; }

        internal override List<Mapping> GetMappings()
        {
            List<Mapping> mappings = base.GetMappings();
            mappings.Add(new Mapping { Column = "genus", PropertyName = "Genus", TypeToCast = typeof(String) });
            mappings.Add(new Mapping { Column = "capture_rate", PropertyName = "CaptureRate", TypeToCast = typeof(Int32) });
            mappings.Add(new Mapping { Column = "base_happiness", PropertyName = "BaseHappiness", TypeToCast = typeof(Int32) });
            mappings.Add(new Mapping { Column = "hatch_counter", PropertyName = "HatchCounter", TypeToCast = typeof(Int32) });
            return mappings;
        }

        internal override string GetListQuery()
        {
            return "SELECT ps.id, psn.name, psn.genus, ps.gender_rate, ps.capture_rate, ps.base_happiness, ps.hatch_counter FROM pokemon_v2_pokemonspecies AS ps\n" +
                "LEFT JOIN\n(SELECT def.pokemon_species_id AS id, IFNULL(curr.name, def.name) AS name, IFNULL(curr.genus, def.genus) AS genus FROM pokemon_v2_pokemonspeciesname def\n" + 
                "LEFT JOIN pokemon_v2_pokemonspeciesname curr ON def.pokemon_species_id = curr.pokemon_species_id AND def.language_id = 9 AND curr.language_id = {0}\n" +
                "GROUP BY def.pokemon_species_id)\nAS psn ON ps.id = psn.id\n" +
                "WHERE ps.generation_id <= {1}";
        }
        internal override string GetSingleQuery()
        {
            return GetListQuery() + " AND ps.id = {2}";
        }
        internal override string GetCountQuery()
        {
            return "SELECT count(ps.id) FROM pokemon_v2_pokemonspecies AS ps\n" +
                "WHERE ps.generation_id <= {1}";
        }
    }
}
