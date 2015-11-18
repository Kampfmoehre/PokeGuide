using System;

namespace PokeGuide.Core.Database
{
    static class Queries
    {
        /// <summary>
        /// Returns a query that selects all elements with localized names
        /// </summary>
        /// <param name="language">The ID of the language</param>
        /// <param name="name">The name of the element</param>
        /// <param name="table">The name of the table in which the elements are</param>
        /// <param name="nameTable">The name of the table in which the element names are</param>
        /// <param name="idColumn">The name of the column which is used in the name table to reference the element</param>
        /// <returns>The query</returns>
        internal static string LocalizedNameQuery(int language, string name, string table = "", string nameTable = "", string idColumn = "")
        {
            if (String.IsNullOrEmpty(table))
                table = String.Format("{0}s", name);
            if (String.IsNullOrEmpty(nameTable))
                nameTable = String.Format("{0}_names", name);
            if (String.IsNullOrEmpty(idColumn))
                idColumn = String.Format("{0}_id", name);

            return String.Format(@"
                SELECT t.id, tn.name
                FROM {0} AS t
                LEFT JOIN (SELECT e.{1} AS id, COALESCE(o.name, e.name) AS name
                    FROM {2} e
                    LEFT OUTER JOIN {2} o ON e.{1} = o.{1} AND o.local_language_id = {3}
                    WHERE e.local_language_id = 9
                    GROUP BY e.{1})
                AS tn ON t.id = tn.id
                ", table, idColumn, nameTable, language);
        }

        /// <summary>
        /// Returns a query to retrieve all versions
        /// </summary>
        /// <param name="language">The ID of the language</param>
        /// <returns>The query</returns>
        internal static string VersionListQuery(int language)
        {
            return String.Format(@"
                SELECT v.id, vn.name, v.version_group_id, vg.generation_id
                FROM versions AS v
                LEFT JOIN version_groups AS vg ON v.version_group_id = vg.id
                LEFT JOIN (SELECT e.version_id AS id, COALESCE(o.name, e.name) AS name FROM version_names e
                           LEFT OUTER JOIN version_names o ON e.version_id = o.version_id and o.local_language_id = 6
                           WHERE e.local_language_id = 9
                           GROUP BY e.version_id)
                AS vn ON v.id = vn.id
                ", language);
        }

        /// <summary>
        /// Returns a query to select a single ability by its ID
        /// </summary>
        /// <param name="id">The ID of the ability</param>
        /// <param name="versionGroupId">The ID of the version group</param>
        /// <param name="languageId">The ID of the language</param>
        /// <returns>The query to retrieve the ability</returns>
        internal static string AbilityQuery(int id, int versionGroupId, int languageId)
        {
            return String.Format(@"
                SELECT ab.id, abn.name, abft.flavor_text, abp.short_effect, abp.effect, acp.effect_change
                FROM abilities AS ab
                LEFT JOIN (SELECT e.ability_id AS id, COALESCE(o.name, e.name) AS name
                    FROM ability_names e
                    LEFT OUTER JOIN ability_names o ON e.ability_id = o.ability_id AND o.local_language_id = {2}
                    WHERE e.local_language_id = 9
                    GROUP BY e.ability_id)
                AS abn ON ab.id = abn.id
                LEFT JOIN (SELECT e.ability_id AS id, COALESCE(o.flavor_text, e.flavor_text) AS flavor_text, e.version_group_id
                    FROM ability_flavor_text e
                    LEFT OUTER JOIN ability_flavor_text o ON e.ability_id = o.ability_id AND o.language_id = {2}
                    WHERE e.language_id = 9 AND e.version_group_id = 15
                    GROUP BY e.ability_id)
                AS abft ON ab.id = abft.id
                LEFT JOIN (SELECT e.ability_id AS id, COALESCE(o.short_effect, e.short_effect) AS short_effect, COALESCE(o.effect, e.effect) AS effect
                    FROM ability_prose e
                    LEFT OUTER JOIN ability_prose o ON e.ability_id = o.ability_id AND o.local_language_id = {2}
                    WHERE e.local_language_id = 9
                    GROUP BY e.ability_id)
                AS abp ON ab.id = abp.id
                LEFT JOIN ability_changelog ac ON ab.id = ac.ability_id AND ac.changed_in_version_group_id <= {1}
                LEFT JOIN (SELECT e.ability_changelog_id AS id, COALESCE(o.effect, e.effect) AS effect_change
                    FROM ability_changelog_prose e
                    LEFT OUTER JOIN ability_changelog_prose o ON e.ability_changelog_id = o.ability_changelog_id AND o.local_language_id = {2}
                    WHERE e.local_language_id = 9
                    GROUP BY e.ability_changelog_id)
                AS acp ON ac.id = acp.id
                WHERE ab.is_main_series AND ab.id = {0}
                ORDER BY ac.changed_in_version_group_id DESC
                LIMIT 1
                ", id, versionGroupId, languageId);
        }
    }
}
