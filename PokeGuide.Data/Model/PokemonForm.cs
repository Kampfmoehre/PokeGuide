using System;
using System.Collections.Generic;

namespace PokeGuide.Data.Model
{
    public class PokemonForm : ModelBase
    {
        public Species Species { get; set; }
        public int Height { get; set; }
        public int Weight { get; set; }
        public int BaseExperience { get; set; }
        public ElementType Type1 { get; set; }
        public ElementType Type2 { get; set; }
        public Ability Ability1 { get; set; }
        public Ability Ability2 { get; set; }
        public Ability HiddenAbility { get; set; }

        internal override List<Mapping> GetMappings()
        {
            List<Mapping> mappings = base.GetMappings();
            mappings.Add(new Mapping { Column = "height", PropertyName = "Height", TypeToCast = typeof(Int32) });
            mappings.Add(new Mapping { Column = "weight", PropertyName = "Weight", TypeToCast = typeof(Int32) });
            mappings.Add(new Mapping { Column = "base_experience", PropertyName = "BaseExperience", TypeToCast = typeof(Int32) });
            mappings.Add(new Mapping { Column = "type1", PropertyName = "Type1", TypeToCast = typeof(Int32) });
            mappings.Add(new Mapping { Column = "type2", PropertyName = "Type2", TypeToCast = typeof(Int32) });
            mappings.Add(new Mapping { Column = "type2", PropertyName = "Type2", TypeToCast = typeof(Int32) });
            mappings.Add(new Mapping { Column = "ability1", PropertyName = "Ability1", TypeToCast = typeof(Int32) });
            mappings.Add(new Mapping { Column = "ability2", PropertyName = "Ability2", TypeToCast = typeof(Int32) });
            mappings.Add(new Mapping { Column = "hidden_ability", PropertyName = "HiddenAbility", TypeToCast = typeof(Int32) });
            return mappings;
        }

        internal override string GetListQuery()
        {
            return "SELECT pf.id, pfn.form AS name, p.height, p.weight, p.base_experience, pt1.type_id AS type1, pt2.type_id AS type2, " +
                "pa1.ability_id AS ability1, pa2.ability_id AS ability2, pa3.ability_id AS hidden_ability FROM pokemon_v2_pokemonform AS pf\n" +
                "LEFT JOIN pokemon_v2_pokemon AS p ON p.id = pf.pokemon_id\n" +
                "LEFT JOIN pokemon_v2_pokemonspecies AS ps ON ps.id = p.pokemon_species_id\n" +
                "LEFT JOIN\n(SELECT def.pokemon_form_id AS id, IFNULL(curr.name, def.name) AS form, IFNULL(curr.pokemon_name, def.pokemon_name) AS name FROM pokemon_v2_pokemonformname def\n" +
                "LEFT JOIN pokemon_v2_pokemonformname curr ON def.pokemon_form_id = curr.pokemon_form_id AND def.language_id = 9 AND curr.language_id = {0}\n" +                
                "GROUP BY def.pokemon_form_id)\nAS pfn ON pf.id = pfn.id\n" +
                "LEFT JOIN pokemon_v2_pokemontype AS pt1 ON p.id = pt1.pokemon_id AND pt1.slot = 1\n" +
                "LEFT JOIN pokemon_v2_pokemontype AS pt2 ON p.id = pt2.pokemon_id AND pt2.slot = 2\n" +
                "LEFT JOIN pokemon_v2_pokemonability AS pa1 ON p.id = pa1.pokemon_id AND pa1.slot = 1\n" +
                "LEFT JOIN pokemon_v2_pokemonability AS pa2 ON p.id = pa2.pokemon_id AND pa2.slot = 2\n" +
                "LEFT JOIN pokemon_v2_pokemonability AS pa3 ON p.id = pa3.pokemon_id AND pa3.slot = 3\n" +
                "WHERE ps.id = {1} AND pf.version_group_id <= {2}\nORDER BY pf.'order'";
        }

        internal override string GetCountQuery()
        {
            return "SELECT count(pf.id) FROM pokemon_v2_pokemonform AS pf\n" +
                "LEFT JOIN pokemon_v2_pokemon AS p ON p.id = pf.pokemon_id\n" +
                "LEFT JOIN pokemon_v2_pokemonspecies AS ps ON ps.id = p.pokemon_species_id\n" +
                "WHERE ps.id = {1} AND pf.version_group_id <= {2}";
        }
    }
}
