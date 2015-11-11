using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

using PokeGuide.Model;
using PokeGuide.Model.Database;
using PokeGuide.Service.Interface;
using SQLite.Net.Interop;

namespace PokeGuide.Service
{
    public class StaticDataService : DataService, IStaticDataService
    {
        public StaticDataService(IStorageService storageService, ISQLitePlatform sqlitePlatform)
            : base(storageService, sqlitePlatform)
        { }

        public async Task<ObservableCollection<Language>> LoadLanguagesAsync(int displayLanguage, CancellationToken token)
        {
            string query = "SELECT ps.id, psn.name FROM pokemon_v2_language AS ps\n" +
                "LEFT JOIN\n(SELECT def.language_id AS id, IFNULL(curr.name, def.name) AS name FROM pokemon_v2_languagename def\n" +
                "LEFT JOIN pokemon_v2_languagename curr ON def.language_id = curr.language_id AND def.local_language_id = 9 AND curr.local_language_id = ?\n" +
                "GROUP BY def.language_id)\nAS psn ON ps.id = psn.id";
            IEnumerable<DbLanguage> list = await _connection.QueryAsync<DbLanguage>(token, query, new object[] { displayLanguage });
            return new ObservableCollection<Language>(list.Select(s => new Language { Id = s.Id, Name = s.Name }));
        }

        public async Task<ObservableCollection<GameVersion>> LoadVersionsAsync(int displayLanguage, CancellationToken token)
        {
            string query = "SELECT v.id, vn.name, v.version_group_id, vg.generation_id FROM pokemon_v2_version AS v\n" +
                 "LEFT JOIN pokemon_v2_versiongroup AS vg ON v.version_group_id = vg.id\n" +
                 "LEFT JOIN\n(SELECT def.version_id AS id, IFNULL(curr.name, def.name) AS name FROM pokemon_v2_versionname def\n" +
                 "LEFT JOIN pokemon_v2_versionname curr ON def.version_id = curr.version_id AND def.language_id = 9 AND curr.language_id = ?\n" +
                 "GROUP BY def.version_id)\nAS vn ON v.id = vn.id";
            IEnumerable<DbVersion> list = await _connection.QueryAsync<DbVersion>(token, query, new object[] { displayLanguage });
            return new ObservableCollection<GameVersion>(list.Select(s => new GameVersion
            {
                Generation = s.Generation,
                Id = s.Id,
                Name = s.Name,
                VersionGroup = s.VersionGroupId
            }));
        }

        public async Task<ObservableCollection<Ability>> LoadAbilitiesAsync(int displayLanguage, CancellationToken token)
        {
            try
            {
                string query = String.Format(@"
                    SELECT ab.id, abn.name, abft.flavor_text, abp.short_effect, abp.effect
                    FROM abilities AS ab
                    LEFT JOIN (SELECT e.ability_id AS id, COALESCE(o.name, e.name) AS name
                        FROM ability_names e
                        LEFT OUTER JOIN ability_names o ON e.ability_id = o.ability_id AND o.local_language_id = {0}
                        WHERE e.local_language_id = 9
                        GROUP BY e.ability_id)
                    AS abn ON ab.id = abn.id
                    LEFT JOIN (SELECT e.ability_id AS id, COALESCE(o.flavor_text, e.flavor_text) AS flavor_text, e.version_group_id
                        FROM ability_flavor_text e
                        LEFT OUTER JOIN ability_flavor_text o ON e.ability_id = o.ability_id AND o.language_id = {0}
                        WHERE e.language_id = 9 AND e.version_group_id = 15
                        GROUP BY e.ability_id)
                    AS abft ON ab.id = abft.id
                    LEFT JOIN (SELECT e.ability_id AS id, COALESCE(o.short_effect, e.short_effect) AS short_effect, COALESCE(o.effect, e.effect) AS effect
                        FROM ability_prose e
                        LEFT OUTER JOIN ability_prose o ON e.ability_id = o.ability_id AND o.local_language_id = {0}
                        WHERE e.local_language_id = 9
                        GROUP BY e.ability_id)
                    AS abp ON ab.id = abp.id
                    WHERE ab.is_main_series
                ", displayLanguage);
                IEnumerable<DbAbility> abilities = await _connection.QueryAsync<DbAbility>(token, query, new object[0]).ConfigureAwait(false);
                var result = new List<Ability>();
                foreach (DbAbility ability in abilities)
                {
                    var ab = new Ability
                    {
                        FlavorText = ability.FlavorText,
                        Id = ability.Id,
                        Name = ability.Name
                    };
                    ab.Description = ProzessAbilityText(ability.ShortEffect);
                    result.Add(ab);
                }
                return new ObservableCollection<Ability>(result);
            }
            catch (Exception)
            {
                return null;
            }
        }

        string ProzessAbilityText(string input)
        {
            string result = input;
            string nameRegexString = @"\[[\w\s]*\]";
            string identifierRegexString = @"{\w*:\w*}";
            var regex = new Regex(nameRegexString + identifierRegexString);
            foreach (Match match in regex.Matches(input))
            {
                string name = String.Empty;
                Match nameMatch = Regex.Match(match.Value, nameRegexString);
                name = nameMatch.Value;
                result = Regex.Replace(result, nameRegexString + identifierRegexString, name);
            }
            return result;
        }
    }
}