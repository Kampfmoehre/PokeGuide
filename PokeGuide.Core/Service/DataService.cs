using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Nito.AsyncEx;
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

        public void InitializeResources(int displayLanguage, CancellationToken token)
        {
            TypeList = new AsyncLazy<IEnumerable<ElementType>>(async () => { return await LoadTypesAsync(displayLanguage, token); });
            DamageClassList = new AsyncLazy<IEnumerable<DamageClass>>(async () => { return await LoadDamageClassesAsync(displayLanguage, token); });
        }

        public AsyncLazy<IEnumerable<ElementType>> TypeList { get; private set; }
        public AsyncLazy<IEnumerable<DamageClass>> DamageClassList { get; private set; }

        async Task<ElementType> GetTypeById(int id)
        {
            IEnumerable<ElementType> types = await TypeList;
            return types.Single(s => s.Id == id);
        }
        async Task<ElementType> GetTypeById(string identifier)
        {
            IEnumerable<ElementType> types = await TypeList;
            return types.Single(s => s.Identifier == identifier);
        }
        async Task<DamageClass> GetDamageClassById(int id)
        {
            IEnumerable<DamageClass> classes = await DamageClassList;
            return classes.Single(s => s.Id == id);
        }
        async Task<DamageClass> GetDamageClassById(string identifier)
        {
            IEnumerable<DamageClass> classes = await DamageClassList;
            return classes.Single(s => s.Identifier == identifier);
        }

        async Task<IEnumerable<ElementType>> LoadTypesAsync(int displayLanguage, CancellationToken token)
        {
            try
            {
                string query = Queries.TypeListQuery(displayLanguage);
                IEnumerable<DbType> types = await _connection.QueryAsync<DbType>(token, query, new object[0]).ConfigureAwait(false);
                return types.Select(s => new ElementType
                {
                    DamageClassId = s.DamageClassId,
                    Generation = s.GenerationId,                    
                    Id = s.Id,
                    Identifier = s.Identifier,
                    Name = s.Name
                });
            }
            catch (Exception)
            {
                return new List<ElementType>();
            }
        }

        async Task<IEnumerable<DamageClass>> LoadDamageClassesAsync(int language, CancellationToken token)
        {
            try
            {
                string query = Queries.MoveDamageClassListQuery(language);
                IEnumerable<DbMoveDamageClass> damageClasses = await _connection.QueryAsync<DbMoveDamageClass>(token, query, new object[0]).ConfigureAwait(false);
                return damageClasses.Select(s => new DamageClass { Description = s.Description, Id = s.Id, Identifier = s.Identifier, Name = s.Name });
            }
            catch (Exception)
            {
                return new List<DamageClass>();
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
                result.ShortDescription = await ProzessPlaceholderText(ability.ShortEffect, language, token);
                result.Description = await ProzessPlaceholderText(ability.Effect, language, token);
                result.VersionChangelog = await ProzessPlaceholderText(ability.EffectChange, language, token);
                return result;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<IEnumerable<ModelNameBase>> LoadMoveNamesAsync(int language, int generation, CancellationToken token)
        {
            try
            {
                string query = Queries.LocalizedNameQuery(language, "move");
                query = String.Format(@"{0}
                    WHERE t.generation_id <= {1}
                    ", query, generation);
                IEnumerable<DbName> moves = await _connection.QueryAsync<DbName>(token, query, new object[0]).ConfigureAwait(false);
                return moves.Select(s => new ModelNameBase { Id = s.Id, Name = s.Name });
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<Move> LoadMoveByIdAsync(int id, GameVersion version, int language, CancellationToken token)
        {
            try
            {
                string query = Queries.MoveQuery(id, version.VersionGroup, language);
                IEnumerable<DbMove> moves = await _connection.QueryAsync<DbMove>(token, query, new object[0]).ConfigureAwait(false);
                DbMove move = moves.Single();
                var result = new Move
                {
                    Accuracy = move.Accuracy,
                    Id = move.Id,
                    IngameText = move.FlavorText,
                    Name = move.Name,
                    Power = move.Power,
                    PowerPoints = move.PowerPoints,
                    Priority = move.Priority
                };
                result.Description = await ProzessPlaceholderText(move.Effect, language, token);
                result.ShortDescription = await ProzessPlaceholderText(move.ShortEffect, language, token);                
                result.VersionChangelog = await ProzessPlaceholderText(move.EffectChange, language, token);

                result.Type = await GetTypeById(move.TypeId);
                int damageClassId = move.DamageClassId;
                if (version.Generation <= 3)
                    damageClassId = result.Type.DamageClassId;
                result.DamageCategory = await GetDamageClassById(damageClassId);

                return result;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<IEnumerable<PokemonAbility>> LoadPokemonByAbilityAsync(int abilityId, int versionGroupId, int language, CancellationToken token)
        {
            try
            {
                string query = Queries.PokemonAbilityQuery(abilityId, versionGroupId, language);
                IEnumerable<DbPokemonAbility> pokemonAbilities = await _connection.QueryAsync<DbPokemonAbility>(token, query, new object[0]).ConfigureAwait(false);
                return pokemonAbilities.Select(s => new PokemonAbility
                {
                    IsHidden = s.IsHidden,
                    Pokemon = new ModelNameBase { Id = s.PokemonId, Name = s.Name },
                    Slot = s.Slot
                });
            }
            catch (Exception)
            {
                return null;
            }
        }

        async Task<string> ProzessPlaceholderText(string input, int language, CancellationToken token)
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
                    ElementType elementType = await GetTypeById(element);
                    return elementType.Name;
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
