using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

using PokeGuide.Core.Database;
using PokeGuide.Core.Model;
using PokeGuide.Core.Service.Interface;

using SQLite.Net;
using SQLite.Net.Async;
using SQLite.Net.Interop;

namespace PokeGuide.Core.Service
{
    public class DataService : IDataService
    {
        IStorageService _storageService;
        ISQLitePlatform _sqlitePlatform;
        protected SQLiteAsyncConnection _connection;

        public DataService(IStorageService storageService, ISQLitePlatform sqlitePlatform)
        {
            _storageService = storageService;
            _sqlitePlatform = sqlitePlatform;
        }

        async Task InitializeAsync()
        {
            if (_connection == null)
            {
                string database = await _storageService.GetDatabasePathForFileAsync("pokedex.sqlite");                
                _connection = new SQLiteAsyncConnection(() => new SQLiteConnectionWithLock(_sqlitePlatform, new SQLiteConnectionString(database, false)));
            }
        }

        public async Task<DisplayLanguage> LoadLanguageByIdAsync(int id, int displayLanguage, CancellationToken token)
        {
            try
            {
                string query = Queries.LocalizedNameQuery(displayLanguage, "language");
                query = String.Format(@"{0}
                WHERE t.id = {1}
                ", query, id);
                IEnumerable<DbLanguage> languages = await _connection.QueryAsync<DbLanguage>(token, query, new object[0]).ConfigureAwait(false);
                DbLanguage language = languages.Single();
                return new DisplayLanguage { Id = language.Id, Iso639 = language.Iso639, Name = language.Name };
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<GameVersion> LoadVersionByIdAsync(int id, int language, CancellationToken token)
        {
            try
            {
                string query = Queries.VersionListQuery(language);
                query = String.Format(@"{0}
                    WHERE v.id = {1}
                    ", query, id);
                IEnumerable<DbVersion> versions = await _connection.QueryAsync<DbVersion>(token, query, new object[0]).ConfigureAwait(false);
                DbVersion version = versions.Single();
                return new GameVersion { Generation = version.Generation, Id = version.Id, Name = version.Name, VersionGroup = version.VersionGroup };
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<IEnumerable<ModelNameBase>> LoadAbilityNamesAsync(int language, int generation, CancellationToken token)
        {
            try
            {
                string query = Queries.LocalizedNameQuery(language, "ability", "abilities");
                query = String.Format(@"{0}
                    WHERE t.generation_id <= {1}
                    ", query, generation);
                IEnumerable<DbName> list = await _connection.QueryAsync<DbName>(token, query, new object[0]).ConfigureAwait(false);
                return list.Select(s => new ModelNameBase { Id = s.Id, Name = s.Name });
            }
            catch (Exception)
            {
                return new List<ModelNameBase>();
            }
        }

        public async Task<IEnumerable<GameVersion>> LoadVersionsAsync(int language, CancellationToken token)
        {
            try
            {
                string query = Queries.VersionListQuery(language);
                IEnumerable<DbVersion> versions = await _connection.QueryAsync<DbVersion>(token, query, new object[0]).ConfigureAwait(false);
                return versions.Select(s => new GameVersion
                {
                    Generation = s.Generation,
                    Id = s.Id,
                    Name = s.Name,
                    VersionGroup = s.VersionGroup
                });
            }
            catch (Exception)
            {
                return new List<GameVersion>();
            }
        }

        public async Task<Ability> LoadAbilityByIdAsync(int id, int versionGroup, int language, CancellationToken token)
        {
            try
            {
                string query = Queries.AbilityQuery(id, versionGroup, language);
                IEnumerable<DbAbility> abilities = await _connection.QueryAsync<DbAbility>(token, query, new object[0]).ConfigureAwait(false);
                DbAbility ability = abilities.Single();

                var result = new Ability
                {
                    Id = ability.Id,
                    IngameText = ability.FlavorText,
                    Name = ability.Name
                };
                result.ShortDescription = await ProzessAbilityText(ability.ShortEffect, language, token);
                result.Description = await ProzessAbilityText(ability.Effect, language, token);
                result.VersionChangelog = await ProzessAbilityText(ability.EffectChange, language, token);
                return result;
            }
            catch (Exception)
            {
                return null;
            }
        }

        async Task<string> ProzessAbilityText(string input, int language, CancellationToken token)
        {
            if (String.IsNullOrWhiteSpace(input))
                return String.Empty;
            string result = input;
            string nameRegexString = @"\[\]";
            string identifierRegexString = @"{\w*:\w*-*\w*}";
            var regex = new Regex(nameRegexString + identifierRegexString);
            var processed = new List<string>();
            foreach (Match match in regex.Matches(input))
            {
                if (processed.Contains(match.Value))
                    continue;

                Match nameMatch = Regex.Match(match.Value, nameRegexString);
                Match identifierMatch = Regex.Match(match.Value, identifierRegexString);
                string identifier = identifierMatch.Value.Trim(new char[] { '{', '}' });
                string name = await GetDefaultNameForIdentifier(identifier, language, token);
                result = result.Replace(match.Value, "[" + name + "]" + identifierMatch.Value);
                processed.Add(match.Value);
            }
            return result;
        }

        async Task<string> GetDefaultNameForIdentifier(string identifier, int language, CancellationToken token)
        {
            int splitIndex = identifier.IndexOf(':');
            string type = identifier.Remove(splitIndex);
            string element = identifier.Substring(splitIndex + 1);

            switch (type)
            {
                case "mechanic":
                    switch (element)
                    {
                        case "rain":
                            return "Regen";
                        case "speed":
                            return "Initiative";
                        case "hp":
                            return "KP";
                        case "weather":
                            return "Wetter";
                        case "paralysis":
                            return "Paralyse";
                        case "evasion":
                            return "Fluchtwert";
                        case "infatuation":
                            return "Anziehung";
                        case "accuracy":
                            return "Genauigkeit";
                        case "sleep":
                            return "Schlaf";
                        case "poison":
                            return "Vergiftung";
                        case "confusion":
                            return "Verwirrung";
                        case "attack":
                            return "Angriff";
                        case "burn":
                            return "Verbrennung";
                        case "sandstorm":
                            return "Sandsturm";
                        case "pp":
                            return "AP";
                        case "fog":
                            return "Nebel";
                        case "defense":
                            return "Verteidigung";
                        case "hail":
                            return "Hagel";
                        case "status-ailment":
                            return "Statusänderung";
                        case "status-ailments":
                            return "Statusänderungen";
                        case "stat-modifier":
                        case "stat-modifiers":
                            return "Statuswerteänderung";
                        case "special-attack":
                            return "Spezialangriff";
                        case "strong-sunlight":
                            return "Starkes Sonnenlicht";
                        case "hatch-counter":
                            return "Schlüpfzähler";
                        case "step-cycle":
                            return "Schrittzyklus";
                        case "regular-damage":
                            return "Regulärer Schaden";
                        default:
                            break;
                    }
                    break;
                case "move":
                    return await LoadObjectNameByIdentifierAsync(element, language, token, "move", "", "", "");
                case "ability":
                    return await LoadObjectNameByIdentifierAsync(element, language, token, "ability", "abilities", "", "");
                case "type":
                    return await LoadObjectNameByIdentifierAsync(element, language, token, "type", "", "", "");
                case "pokemon":
                    return await LoadObjectNameByIdentifierAsync(element, language, token, "pokemon_species", "pokemon_species", "", "");
                case "item":
                    return await LoadObjectNameByIdentifierAsync(element, language, token, "item", "", "", "");
                default:
                    break;
            }
            return String.Empty;
        }

        async Task<string> LoadObjectNameByIdentifierAsync(string identifier, int language, CancellationToken token, string name, string table, string nameTable, string idColumn)
        {
            try
            {
                string query = Queries.LocalizedNameQuery(language, name, table, nameTable, idColumn);
                query = String.Format(@"{0}
                    WHERE t.identifier = '{4}'
                    ", query, identifier);
                IEnumerable<DbName> objects = await _connection.QueryAsync<DbName>(token, query, new object[0]).ConfigureAwait(false);
                DbName result = objects.FirstOrDefault();
                if (result == null)
                    return null;
                return result.Name;
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
