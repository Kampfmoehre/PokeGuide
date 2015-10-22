using System;
using System.Collections.Generic;

namespace PokeGuide.Data.Model
{
    public class Ability : ModelBase
    {
        public string Effect { get; set; }
        public string Description { get; set; }
        public string FlavorText { get; set; }

        internal override List<Mapping> GetMappings()
        {
            List<Mapping> result = base.GetMappings();
            result.Add(new Mapping { Column = "short_effect", PropertyName = "Effect", TypeToCast = typeof(String) });
            result.Add(new Mapping { Column = "effect", PropertyName = "Description", TypeToCast = typeof(String) });
            result.Add(new Mapping { Column = "flavor_text", PropertyName = "FlavorText", TypeToCast = typeof(String) });
            return result;
        }
        internal override string GetListQuery()
        {
            return "SELECT a.id, an.name, ad.short_effect, ad.effect, af.flavor_text FROM pokemon_v2_ability AS a\n" +
                "LEFT JOIN\n(SELECT def.ability_id AS id, IFNULL(curr.name, def.name) AS name FROM pokemon_v2_abilityname def\n" +
                "LEFT JOIN pokemon_v2_abilityname curr ON def.ability_id = curr.ability_id AND def.language_id = 9 AND curr.language_id = {0}\n" +
                "GROUP BY def.ability_id)\nAS an ON a.id = an.id\n" +
                "LEFT JOIN\n(SELECT def.ability_id AS id, IFNULL(curr.short_effect, def.short_effect) AS short_effect, IFNULL(curr.effect, def.effect) AS effect FROM pokemon_v2_abilitydescription def\n" +
                "LEFT JOIN pokemon_v2_abilitydescription curr ON def.ability_id = curr.ability_id AND def.language_id = 9 AND curr.language_id = {0}\n" +
                "GROUP BY def.ability_id)\nAS ad ON a.id = ad.id\n" +
                "LEFT JOIN\n(SELECT def.ability_id AS id, vg.id as version, IFNULL(curr.flavor_text, def.flavor_text) AS flavor_text FROM pokemon_v2_abilityflavortext def\n" +
                "LEFT JOIN pokemon_v2_versiongroup vg ON def.version_group_id = vg.id AND vg.id = {1}\n" +
                "LEFT JOIN pokemon_v2_abilityflavortext curr ON def.ability_id = curr.ability_id AND def.language_id = 9 AND curr.language_id = {0}\n" +
                "GROUP BY def.id)\nAS af ON a.id = af.id\nWHERE af.version IS NOT NULL";
        }
        internal override string GetSingleQuery()
        {
            return GetListQuery() + " AND a.id = {2}";
        }
    }
}
