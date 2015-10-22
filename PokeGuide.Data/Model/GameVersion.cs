using System;
using System.Collections.Generic;

namespace PokeGuide.Data.Model
{
    public class GameVersion : ModelBase
    {
        public int VersionGroupId { get; set; }
        public int Generation { get; set; }

        internal override List<Mapping> GetMappings()
        {
            List<Mapping> mappings = base.GetMappings();
            mappings.Add(new Mapping { Column = "version_group_id", PropertyName = "VersionGroupId", TypeToCast = typeof(Int32) });
            mappings.Add(new Mapping { Column = "generation", PropertyName = "Generation", TypeToCast = typeof(Int32) });
            return mappings;
        }

        internal override string GetListQuery()
        {
            return "SELECT v.id, vn.name, v.version_group_id, vg.generation_id AS generation FROM pokemon_v2_version AS v\n" +
               "LEFT JOIN pokemon_v2_versiongroup AS vg ON v.version_group_id = vg.id\n" +
               "LEFT JOIN\n(SELECT def.version_id AS id, IFNULL(curr.name, def.name) AS name FROM pokemon_v2_versionname def\n" +
               "LEFT JOIN pokemon_v2_versionname curr ON def.version_id = curr.version_id AND def.language_id = 9 AND curr.language_id = {0}\n" +
               "GROUP BY def.version_id)\nAS vn ON v.id = vn.id";
        }
    }
}
