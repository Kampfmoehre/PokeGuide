﻿using System;

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
        /// Returns a query to select all types
        /// </summary>
        /// <param name="language">The ID of the language in which the names should be displayed</param>
        /// <returns>The query</returns>
        internal static string TypeListQuery(int language)
        {
            return String.Format(@"
                SELECT t.id, t.identifier, tn.name, t.damage_class_id, t.generation_id
                FROM types t
                LEFT JOIN (SELECT e.type_id AS id, COALESCE(o.name, e.name) AS name 
                           FROM type_names e
                           LEFT OUTER JOIN type_names o ON e.type_id = o.type_id and o.local_language_id = {0}
                           WHERE e.local_language_id = 9
                           GROUP BY e.type_id)
                AS tn ON t.id = tn.id
                ", language);
        }

        /// <summary>
        /// Returns a query to select all move damage classes
        /// </summary>
        /// <param name="language">The ID of the language in which the names and descriptions should be displayed</param>
        /// <returns>The query</returns>
        internal static string MoveDamageClassListQuery(int language)
        {
            return String.Format(@"
                SELECT mdc.id, mdc.identifier, mdcp.name, mdcp.description
                FROM move_damage_classes mdc
                LEFT JOIN (SELECT e.move_damage_class_id AS id, COALESCE(o.name, e.name) AS name, COALESCE(o.description, e.description) AS description  
                           FROM move_damage_class_prose e
                           LEFT OUTER JOIN move_damage_class_prose o ON e.move_damage_class_id = o.move_damage_class_id and o.local_language_id = {0}
                           WHERE e.local_language_id = 9
                           GROUP BY e.move_damage_class_id)
                AS mdcp ON mdc.id = mdcp.id
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
                    WHERE e.language_id = 9 AND e.version_group_id = {1}
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

        /// <summary>
        /// Returns a query that select all necessary information to display a move
        /// </summary>
        /// <param name="id">The ID of the move</param>
        /// <param name="versionGroupId">The ID of the version group for which to load the move</param>
        /// <param name="languageId">The ID of the language in which descriptions should be displayed</param>
        /// <returns>The query</returns>
        internal static string MoveQuery(int id, int versionGroupId, int languageId)
        {
            return String.Format(@"
                SELECT m.id, mn.name, m.type_id, m.power, m.pp, m.accuracy, m.priority, m.target_id, m.damage_class_id, mep.short_effect, mep.effect, mecp.effect_change, mft.flavor_text, m.effect_chance, mm.meta_category_id, mm.meta_ailment_id, mm.min_hits, mm.max_hits, mm.min_turns, mm.max_turns, mm.drain, mm.healing, mm.crit_rate, mm.ailment_chance, mm.flinch_chance, mm.stat_chance
                FROM moves m
                LEFT JOIN (SELECT e.move_id AS id, COALESCE(o.name, e.name) AS name
                           FROM move_names e
                           LEFT OUTER JOIN move_names o ON e.move_id = o.move_id AND o.local_language_id = {2}
                           WHERE e.local_language_id = 9
                           GROUP BY e.move_id)
                AS mn ON m.id = mn.id
                LEFT JOIN (SELECT e.move_effect_id AS id, COALESCE(o.short_effect, e.short_effect) AS short_effect, COALESCE(o.effect, e.effect) AS effect
                           FROM move_effect_prose e
                           LEFT OUTER JOIN move_effect_prose o ON e.move_effect_id = o.move_effect_id AND o.local_language_id = {2}
                           WHERE e.local_language_id = 9
                           GROUP BY e.move_effect_id)
                AS mep ON m.effect_id = mep.id
                LEFT JOIN (SELECT e.move_id AS id, COALESCE(o.flavor_text, e.flavor_text) AS flavor_text, e.version_group_id
                           FROM move_flavor_text e
                           LEFT OUTER JOIN move_flavor_text o ON e.move_id = o.move_id AND o.language_id = {2}
                           WHERE e.language_id = 9 AND e.version_group_id = {1}
                           GROUP BY e.move_id)
                AS mft ON m.id = mft.id
                LEFT JOIN move_effect_changelog mec ON mep.id = mec.effect_id AND mec.changed_in_version_group_id <= {1}
                LEFT JOIN (SELECT e.move_effect_changelog_id AS id, COALESCE(o.effect, e.effect) AS effect_change
                           FROM move_effect_changelog_prose e
                           LEFT OUTER JOIN move_effect_changelog_prose o ON e.move_effect_changelog_id = o.move_effect_changelog_id AND o.local_language_id = {2}
                           WHERE e.local_language_id = 9
                           GROUP BY e.move_effect_changelog_id)
                AS mecp ON mec.id = mecp.id
                LEFT JOIN move_meta mm ON m.id = mm.move_id
                WHERE m.id = {0}
                ORDER BY mec.changed_in_version_group_id DESC
                LIMIT 1
            ", id, versionGroupId, languageId);
        }

        /// <summary>
        /// Returns a query that loads information about Pokémon that can learn an ability
        /// </summary>
        /// <param name="abilityId">The ID of the ability</param>
        /// <param name="versionGroupId">The ID of the version group</param>
        /// <param name="languageId">The ID of the language in which to display the names</param>
        /// <returns>The query</returns>
        internal static string PokemonAbilityQuery(int abilityId, int versionGroupId, int languageId)
        {
            return String.Format(@"
                SELECT pa.pokemon_id, pa.slot, pa.is_hidden, COALESCE(pfn.form_name, psn.name) AS name
                FROM pokemon_abilities pa
                LEFT JOIN pokemon_forms pf ON pa.pokemon_id = pf.pokemon_id
                LEFT JOIN (SELECT e.pokemon_form_id AS id, COALESCE(o.form_name, e.form_name) AS form_name
                           FROM pokemon_form_names e
                           LEFT OUTER JOIN pokemon_form_names o ON e.pokemon_form_id = o.pokemon_form_id AND o.local_language_id = {2}
                           WHERE e.local_language_id = 9
                           GROUP BY e.pokemon_form_id)
                AS pfn ON pf.id = pfn.id
                LEFT JOIN pokemon p ON pa.pokemon_id = p.id
                LEFT JOIN pokemon_species ps ON p.species_id = ps.id
                LEFT JOIN (SELECT e.pokemon_species_id AS id, COALESCE(o.name, e.name) AS name
                           FROM pokemon_species_names e
                           LEFT OUTER JOIN pokemon_species_names o ON e.pokemon_species_id = o.pokemon_species_id AND o.local_language_id = {2}
                           WHERE e.local_language_id = 9
                           GROUP BY e.pokemon_species_id)
                AS psn ON ps.id = psn.id
                WHERE pa.ability_id = {0} AND pf.introduced_in_version_group_id <= {1}
                ", abilityId, versionGroupId, languageId);
        }
    }
}
