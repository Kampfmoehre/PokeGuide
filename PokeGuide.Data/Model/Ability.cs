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
            return "SELECT a.id, an.name, ad.short_effect, ad.effect, aft.flavor_text FROM pokemon_v2_ability AS a\n" +
                "LEFT JOIN\n(SELECT def.ability_id AS id, IFNULL(curr.name, def.name) AS name FROM pokemon_v2_abilityname def\n" +
                "LEFT JOIN pokemon_v2_abilityname curr ON def.ability_id = curr.ability_id AND def.language_id = 9 AND curr.language_id = {0}\n" +
                "GROUP BY def.ability_id)\nAS an ON a.id = an.id\n" +
                "LEFT JOIN\n(SELECT def.ability_id AS id, IFNULL(curr.short_effect, def.short_effect) AS short_effect, IFNULL(curr.effect, def.effect) AS effect FROM pokemon_v2_abilitydescription def\n" +
                "LEFT JOIN pokemon_v2_abilitydescription curr ON def.ability_id = curr.ability_id AND def.language_id = 9 AND curr.language_id = {0}\n" +
                "GROUP BY def.ability_id)\nAS ad ON a.id = ad.id\n" +
                "LEFT JOIN\n(SELECT e.ability_id AS id, COALESCE(o.flavor_text, e.flavor_text) AS flavor_text, e.version_group_id FROM pokemon_v2_abilityflavortext e\n" +
                "LEFT OUTER JOIN pokemon_v2_abilityflavortext o ON e.ability_id = o.ability_id and o.language_id = {0} AND o.version_group_id = {1}\n" +
                "WHERE e.language_id = 9 AND e.version_group_id = {1}\nGROUP BY e.ability_id)\nAS aft ON a.id = aft.id\n" +
                "WHERE aft.version_group_id = {1}";
        }
        internal override string GetSingleQuery()
        {
            return GetListQuery() + " AND a.id = {2}";
        }
        internal override string GetCountQuery()
        {
            return "SELECT count(a.id) FROM pokemon_v2_ability AS a\n" +
                "LEFT JOIN pokemon_v2_abilityflavortext aft ON a.id = aft.ability_id\n" +
                "LEFT JOIN pokemon_v2_versiongroup vg ON aft.version_group_id = vg.id\n" +
                "WHERE aft.version_group_id IS NOT NULL AND aft.version_group_id = {1}";
        }
    }
}
