using System;
using System.Collections.Concurrent;
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
    public class PokemonService : BaseDataService, IPokemonService
    {
        public PokemonService(IStorageService storageService, ISQLitePlatform sqlitePlatform) 
            : base(storageService, sqlitePlatform)
        { }

        public async Task<IEnumerable<Ability>> LoadAbilitiesAsync(int generation, int displayLanguage, CancellationToken token)
        {
            try
            {
                string query = String.Format(@"
                    SELECT ab.id, abn.name
                    FROM abilities AS ab
                    LEFT JOIN (SELECT e.ability_id AS id, COALESCE(o.name, e.name) AS name
                        FROM ability_names e
                        LEFT OUTER JOIN ability_names o ON e.ability_id = o.ability_id AND o.local_language_id = {0}
                        WHERE e.local_language_id = 9
                        GROUP BY e.ability_id)
                    AS abn ON ab.id = abn.id
                    WHERE ab.is_main_series AND ab.generation_id <= {1}
                ", displayLanguage, generation);
                IEnumerable<DbAbility> abilities = await _connection.QueryAsync<DbAbility>(token, query, new object[0]).ConfigureAwait(false);
                return abilities.Select(s => new Ability { Id = s.Id, Name = s.Name }).ToList();
            }
            catch (Exception)
            {
                return new List<Ability>();
            }
        }

        public async Task<Ability> LoadAbilityAsync(int id, int versionGroup, int displayLanguage, CancellationToken token)
        {
            try
            {
                string query = String.Format(@"
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
                ", id, versionGroup, displayLanguage);
                IEnumerable<DbAbility> abilities = await _connection.QueryAsync<DbAbility>(token, query, new object[0]).ConfigureAwait(false);
                DbAbility ability = abilities.FirstOrDefault();
                if (ability == null)
                    return null;

                var ab = new Ability
                {
                    FlavorText = ability.FlavorText,
                    Id = ability.Id,
                    Name = ability.Name
                };
                ab.Description = await ProzessAbilityText(ability.ShortEffect, displayLanguage, token);
                ab.Effect = await ProzessAbilityText(ability.Effect, displayLanguage, token);
                ab.EffectChange = await ProzessAbilityText(ability.EffectChange, displayLanguage, token);
                return ab;
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
                    return await LoadObjectNameByIdentifierAsync("moves", "move_names", "move_id", element, language, token);
                case "ability":
                    return await LoadObjectNameByIdentifierAsync("abilities", "ability_names", "ability_id", element, language, token);
                case "type":
                    return await LoadObjectNameByIdentifierAsync("types", "type_names", "type_id", element, language, token);
                case "pokemon":
                    return await LoadObjectNameByIdentifierAsync("pokemon_species", "pokemon_species_names", "pokemon_species_id", element, language, token);
                case "item":
                    return await LoadObjectNameByIdentifierAsync("items", "item_names", "item_id", element, language, token);
                default:
                    break;
            }
            return String.Empty;
        }

        async Task<string> LoadObjectNameByIdentifierAsync(string table, string nameTable, string idColumn, string identifier, int language, CancellationToken token)
        {
            try
            {
                string query = String.Format(@"
                    SELECT t.id, tn.name
                    FROM {0} t
                    LEFT JOIN (SELECT e.{2} AS id, COALESCE(o.name, e.name) AS name
                        FROM {1} e
                        LEFT OUTER JOIN {1} o ON e.{2} = o.{2} AND o.local_language_id = {3}
                        WHERE e.local_language_id = 9
                        GROUP BY e.{2})
                    AS tn ON t.id = tn.id
                    WHERE t.identifier = '{4}'
                ", table, nameTable, idColumn, language, identifier);
                IEnumerable<DbMove> objects = await _connection.QueryAsync<DbMove>(token, query, new object[0]).ConfigureAwait(false);
                DbMove result = objects.Single();
                return result.Name;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<PokedexEntry> LoadPokedexEntryAsync(int dexId, int speciesId, int displayLanguage, CancellationToken token)
        {
            try
            {
                var tableQuery = _connection.Table<DbPokemonDexNumber>().Where(w => w.PokedexId == dexId && w.PokemonSpeciesId == speciesId);
                List<DbPokemonDexNumber> entries = await tableQuery.ToListAsync(token);
                var result = new PokedexEntry { Id = dexId };
                if (entries.Any())
                {
                    result.DexNumber = entries.First().PokedexNumber;
                }
                return result;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<ObservableCollection<SpeciesName>> LoadAllSpeciesAsync(GameVersion version, int displayLanguage, CancellationToken token)
        {
            try
            {
                string query = "SELECT ps.id, psn.name, ps.generation_id FROM pokemon_v2_pokemonspecies AS ps\n" +
                    "LEFT JOIN\n(SELECT def.pokemon_species_id AS id, IFNULL(curr.name, def.name) AS name FROM pokemon_v2_pokemonspeciesname def\n" +
                    "LEFT JOIN pokemon_v2_pokemonspeciesname curr ON def.pokemon_species_id = curr.pokemon_species_id AND def.language_id = 9 AND curr.language_id = ?\n" +
                    "GROUP BY def.pokemon_species_id)\nAS psn ON ps.id = psn.id\nWHERE ps.generation_id <= ?";
                IEnumerable<DbPokemonSpecies> list = await _connection.QueryAsync<DbPokemonSpecies>(token, query, new object[] { displayLanguage, version.Generation });
                return new ObservableCollection<SpeciesName>(list.Select(s => new SpeciesName { Generation = s.GenerationId, Id = s.Id, Name = s.Name }));
            }
            catch (Exception)
            {
                return new ObservableCollection<SpeciesName>();
            }
        }

        public async Task<Species> LoadSpeciesAsync(int id, GameVersion version, int displayLanguage, CancellationToken token)
        {
            try
            {
                string query = "SELECT ps.id, psn.name, psn.genus, ps.gender_rate, ps.capture_rate, ps.base_happiness, ps.hatch_counter, ps.growth_rate_id FROM pokemon_v2_pokemonspecies AS ps\n" +
                    "LEFT JOIN\n(SELECT def.pokemon_species_id AS id, IFNULL(curr.name, def.name) AS name, IFNULL(curr.genus, def.genus) AS genus FROM pokemon_v2_pokemonspeciesname def\n" +
                    "LEFT JOIN pokemon_v2_pokemonspeciesname curr ON def.pokemon_species_id = curr.pokemon_species_id AND def.language_id = 9 AND curr.language_id = ?\n" +
                    "GROUP BY def.pokemon_species_id)\nAS psn ON ps.id = psn.id\n" +                    
                    "WHERE ps.id = ?";
                IEnumerable<DbPokemonSpecies> list = await _connection.QueryAsync<DbPokemonSpecies>(token, query, new object[] { displayLanguage, id });
                query = "SELECT p.id, pn.name FROM pokemon_v2_pokedex p\n" +
                    "LEFT JOIN\n(SELECT e.pokedex_id AS id, COALESCE(o.name, e.name) AS name FROM pokemon_v2_pokedexdescription e\n" +
                    "LEFT OUTER JOIN pokemon_v2_pokedexdescription o ON e.pokedex_id = o.pokedex_id and o.language_id = ?\n" +
                    "WHERE e.language_id = 9\nGROUP BY e.pokedex_id)\nAS pn ON p.id = pn.id\n" +
                    "LEFT JOIN pokemon_v2_pokedexversiongroup pvg ON p.id = pvg.pokedex_id\n" +
                    "WHERE pvg.version_group_id = ?";
                IEnumerable<DbPokedex> temp = await _connection.QueryAsync<DbPokedex>(token, query, new object[] { displayLanguage, version.VersionGroup });
                query = "SELECT egg_group_id FROM pokemon_v2_pokemonegggroup WHERE pokemon_species_id = ?";
                var eggGroups = new List<DbPokemonEggGroup>(await _connection.QueryAsync<DbPokemonEggGroup>(token, query, new object[] { id }));

                DbPokemonSpecies dbSpecies = list.First();
                DbPokedex dex = temp.First();

                var species = new Species
                {
                    BaseHappiness = dbSpecies.BaseHappiness,
                    CatchRate = dbSpecies.CaptureRate,
                    HatchCounter = dbSpecies.HatchCounter,
                    Id = dbSpecies.Id,
                    Name = dbSpecies.Name
                };
                species.GrowthRate = await GetGrowthRateAsync(dbSpecies.GrowthRateId);
                species.DexEntry = await LoadPokedexEntryAsync(dex.Id,  dbSpecies.Id, displayLanguage, token);
                species.DexEntry.Name = dex.Name;
                species.EggGroup1 = await LoadEggGroupAsync(eggGroups[0].EggGroupId, displayLanguage, token);
                if (eggGroups.Count > 1)
                    species.EggGroup2 = await LoadEggGroupAsync(eggGroups[1].EggGroupId, displayLanguage, token);
                //species.PossibleEvolutions = await LoadPossibleEvolutionsAsync(id, version, displayLanguage, token);
                species.GenderRate = CalculateGenderRate(dbSpecies.GenderRate);

                return species;
            }
            catch (Exception)
            {
                return null;
            }
        }

        GenderRate CalculateGenderRate(int femaleEights)
        {
            var rate = new GenderRate
            {
                Id = femaleEights
            };

            if (femaleEights == -1)
            {
                rate.Female = null;
                rate.Male = null;
            }
            else
            {
                rate.Female = ((double)femaleEights / 8) * 100;
                rate.Male = ((double)(8 - femaleEights) / 8) * 100;
            }
            return rate;
        }

        public async Task<ObservableCollection<PokemonForm>> LoadFormsAsync(SpeciesName species, GameVersion version, int displayLanguage, CancellationToken token)
        {
            try
            {
                string query = "SELECT pf.id, pfn.name FROM pokemon_v2_pokemonform pf\n" +
                    "LEFT JOIN pokemon_v2_pokemon p ON pf.pokemon_id = p.id\n" +
                    "LEFT JOIN\n(SELECT e.pokemon_form_id AS id, COALESCE(o.name, e.name) AS name FROM pokemon_v2_pokemonformname e\n" +
                    "LEFT OUTER JOIN pokemon_v2_pokemonformname o ON e.pokemon_form_id = o.pokemon_form_id and o.language_id = ?\n" +
                    "WHERE e.language_id = 9\nGROUP BY e.pokemon_form_id)\nAS pfn ON pf.id = pfn.id\n" +
                    "WHERE p.pokemon_species_id = ? AND pf.version_group_id <= ?";

                IEnumerable<DbPokemonForm> forms = await _connection.QueryAsync<DbPokemonForm>(token, query, new object[] { displayLanguage, species.Id, version.VersionGroup });
                return new ObservableCollection<PokemonForm>(forms.Select((s) =>
                {
                    var f = new PokemonForm { Id = s.Id };
                    if (String.IsNullOrWhiteSpace(s.Name))
                        f.Name = species.Name;
                        else
                        f.Name = s.Name;
                    return f;
                }));
            }
            catch (Exception)
            {
                return new ObservableCollection<PokemonForm>();
            }
        }

        

        public async Task<EggGroup> LoadEggGroupAsync(int id, int displayLanguage, CancellationToken token)
        {
            try
            {
                string query = "SELECT eg.id, egn.name FROM pokemon_v2_egggroup AS eg\n" +
                    "LEFT JOIN\n(SELECT e.egg_group_id AS id, COALESCE(o.name, e.name) AS name FROM pokemon_v2_egggroupname e\n" +
                    "LEFT OUTER JOIN pokemon_v2_egggroupname o ON e.egg_group_id = o.egg_group_id and o.language_id = ?\n" +
                    "WHERE e.language_id = 9\nGROUP BY e.egg_group_id)\nAS egn ON eg.id = egn.id\n" +
                    "WHERE egn.id = ?";
                IEnumerable <DbEggGroup> eggGroups = await _connection.QueryAsync<DbEggGroup>(token, query, new object[] { displayLanguage, id });
                DbEggGroup eggGroup = eggGroups.First();
                return new EggGroup { Id = eggGroup.Id, Name = eggGroup.Name };
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<ObservableCollection<Stat>> LoadPokemonStatsAsync(int formId, GameVersion version, int displayLanguage, CancellationToken token)
        {
            try
            {
                string query = "SELECT ps.pokemon_id, ps.stat_id, sn.name, ps.base_stat, ps.effort FROM pokemon_v2_pokemonstat ps\n" +
                    "LEFT JOIN pokemon_v2_stat s ON ps.stat_id = s.id\n" +
                    "LEFT JOIN\n(SELECT e.stat_id AS id, COALESCE(o.name, e.name) AS name FROM pokemon_v2_statname e\n" +
                    "LEFT OUTER JOIN pokemon_v2_statname o ON e.stat_id = o.stat_id and o.language_id = ?\n" +
                    "WHERE e.language_id = 9\nGROUP BY e.stat_id)\nAS sn ON s.id = sn.id\n" +
                    "WHERE ps.pokemon_id = ?";
                IEnumerable<DbPokemonStat> stats = await _connection.QueryAsync<DbPokemonStat>(token, query, new object[] { displayLanguage, formId });
                query = "SELECT psc.pokemon_id, psc.stat_id, sn.name, psc.base_stat FROM pokemon_v2_pokemonstatchange psc\n" +
                    "LEFT JOIN pokemon_v2_stat s ON psc.stat_id = s.id\n" +
                    "LEFT JOIN\n(SELECT e.stat_id AS id, COALESCE(o.name, e.name) AS name FROM pokemon_v2_statname e\n" +
                    "LEFT OUTER JOIN pokemon_v2_statname o ON e.stat_id = o.stat_id and o.language_id = ?\n" +
                    "WHERE e.language_id = 9\nGROUP BY e.stat_id)\nAS sn ON s.id = sn.id\n" +
                    "WHERE psc.pokemon_id = ? AND psc.min_generation_id >= ? AND psc.max_generation_id <= ?";
                IEnumerable<DbPokemonStat> statChanges = await _connection.QueryAsync<DbPokemonStat>(token, query, new object[] { displayLanguage, formId, version.Generation, version.Generation });                
                List<Stat> statTemp = stats.Select(s => new Stat
                {
                    EffortValue = s.Effort,
                    Id = s.StatId,
                    Name = s.Name,
                    StatValue = s.BaseStat
                }).ToList();
                foreach (DbPokemonStat stat in statChanges)
                {
                    Stat newStat = statTemp.FirstOrDefault(f => f.Id == stat.StatId);
                    if (newStat == null)
                        statTemp.Add(new Stat { Id = stat.StatId, Name = stat.Name, StatValue = stat.BaseStat });
                    else
                        newStat.StatValue = stat.BaseStat;
                }
                return new ObservableCollection<Stat>(statTemp.Where(w => w.StatValue > 0));
            }
            catch (Exception)
            {
                return new ObservableCollection<Stat>();
            }
        }

        public async Task<PokemonForm> LoadFormAsync(int formId, GameVersion version, int displayLanguage, CancellationToken token)
        {
            try
            {
                string query = "SELECT pf.id, pf.pokemon_id, p.pokemon_species_id, pfn.name, p.height, p.weight, p.base_experience, pt1.type_id AS type1, pt2.type_id AS type2, " +
                "pa1.ability_id AS ability1, pa2.ability_id AS ability2, pa3.ability_id AS hidden_ability, pi.rarity, pi.item_id FROM pokemon_v2_pokemonform pf\n" +
                "LEFT JOIN\n(SELECT e.pokemon_form_id AS id, COALESCE(o.name, e.name) AS name FROM pokemon_v2_pokemonformname e\n" +
                "LEFT OUTER JOIN pokemon_v2_pokemonformname o ON e.pokemon_form_id = o.pokemon_form_id and o.language_id = ?\n" +
                "WHERE e.language_id = 9\nGROUP BY e.pokemon_form_id)\nAS pfn ON pf.id = pfn.id\n" +
                "LEFT JOIN pokemon_v2_pokemon p ON pf.pokemon_id = p.id\n" +
                "LEFT JOIN pokemon_v2_pokemontype AS pt1 ON p.id = pt1.pokemon_id AND pt1.slot = 1\n" +
                "LEFT JOIN pokemon_v2_pokemontype AS pt2 ON p.id = pt2.pokemon_id AND pt2.slot = 2\n" +
                "LEFT JOIN pokemon_v2_pokemonability AS pa1 ON p.id = pa1.pokemon_id AND pa1.slot = 1\n" +
                "LEFT JOIN pokemon_v2_pokemonability AS pa2 ON p.id = pa2.pokemon_id AND pa2.slot = 2\n" +
                "LEFT JOIN pokemon_v2_pokemonability AS pa3 ON p.id = pa3.pokemon_id AND pa3.slot = 3\n" +
                "LEFT JOIN pokemon_v2_pokemonitem AS pi ON p.id = pi.pokemon_id AND pi.version_id = ?\n" +
                "WHERE pf.id = ?";
                IEnumerable<DbPokemonForm> forms = await _connection.QueryAsync<DbPokemonForm>(token, query, new object[] { displayLanguage, version.Id, formId });
                DbPokemonForm f = forms.First();

                var form = new PokemonForm
                {
                    BaseExperience = f.BaseExperience,
                    Height = Math.Round((double)f.Height / 10, 2),
                    HeldItemRarity = f.ItemRarity,
                    Id = f.Id,
                    Weight = Math.Round((double)f.Weight / 10, 2)
                };
                form.Species = await LoadSpeciesAsync(f.PokemonSpeciesId, version, displayLanguage, token);
                if (String.IsNullOrWhiteSpace(f.Name))
                    form.Name = form.Species.Name;
                else
                    form.Name = f.Name;

                // Handle Fairy before Gen 6
                if (version.Generation < 6 && f.Type1 == 18)
                    f.Type1 = 1;
                form.Type1 = await GetTypeAsync(f.Type1, version);
                if (f.Type2 != null)
                    form.Type2 = await GetTypeAsync((int)f.Type2, version);

                if (version.Generation >= 3)
                {
                    //form.Ability1 = await LoadAbilityAsync(f.Ability1, version, displayLanguage, token);
                    //if (f.Ability2 != null)
                    //    form.Ability2 = await LoadAbilityAsync((int)f.Ability2, version, displayLanguage, token);
                    //if (version.Generation >= 5 && f.HiddenAbility != null)
                    //    form.HiddenAbility = await LoadAbilityAsync((int)f.HiddenAbility, version, displayLanguage, token);
                }
                if (f.ItemId != null)
                    form.HeldItem = await LoadItemAsync((int)f.ItemId, displayLanguage, token);

                //form.Stats = await LoadPokemonStatsAsync(f.PokemonId, version, displayLanguage, token);

               return form;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<Move> LoadMoveAsync(int id, GameVersion version, int displayLanguage, CancellationToken token)
        {
            try
            {
                string query = "SELECT m.id, mn.name, IFNULL(mc.power, m.power) as power, IFNULL(mc.pp, m.pp) AS pp, IFNULL(mc.accuracy, m.accuracy) as accuracy,\n" +
                    " m.priority, m.move_damage_class_id, IFNULL(mc.type_id, m.type_id) AS type_id FROM pokemon_v2_move m\n" +
                    "LEFT JOIN\n(SELECT e.move_id AS id, COALESCE(o.name, e.name) AS name FROM pokemon_v2_movename e\n" +
                    "LEFT OUTER JOIN pokemon_v2_movename o ON e.move_id = o.move_id and o.language_id = ?\n" +
                    "WHERE e.language_id = 9\nGROUP BY e.move_id)\nAS mn ON m.id = mn.id\n" +
                    "LEFT JOIN pokemon_v2_movechange mc ON m.id = mc.move_id AND mc.version_group_id > ?\n" +
                    "WHERE m.id = ? AND m.generation_id <= ?";
                IEnumerable<DbMove> moves = await _connection.QueryAsync<DbMove>(token, query, new object[] { displayLanguage, version.VersionGroup, id, version.Generation });
                DbMove move = moves.First();
                var result = new Move
                {
                    Accuracy = move.Accuracy,
                    Id = move.Id,
                    Name = move.Name,
                    Power = move.Power,
                    PowerPoints = move.PowerPoints,
                    Priority = move.Priority
                };
                result.Type = await GetTypeAsync(move.Type, version);
                if (version.Generation > 3 || move.MoveDamageClass == 1)
                    result.DamageClass = await GetDamageClassAsync(move.MoveDamageClass);
                else
                    result.DamageClass = await GetDamageClassAsync(result.Type.DamageClassId);

                return result;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<MoveLearnMethod> LoadMoveLearnMethodAsync(int id, int displayLanguage, CancellationToken token)
        {
            try
            {
                string query = "SELECT mlm.id, mlmd.name FROM pokemon_v2_movelearnmethod mlm\n" +
                    "LEFT JOIN\n(SELECT e.move_learn_method_id AS id, COALESCE(o.name, e.name) AS name, COALESCE(o.description, e.description) AS description FROM pokemon_v2_movelearnmethodname e\n" +
                    "LEFT OUTER JOIN pokemon_v2_movelearnmethodname o ON e.move_learn_method_id = o.move_learn_method_id and o.language_id = ?\n" +
                    "WHERE e.language_id = 9\nGROUP BY e.move_learn_method_id)\nAS mlmd ON mlm.id = mlmd.id\n" +
                    "WHERE mlm.id = ?";
                IEnumerable<DbMoveLearnMethod> methods = await _connection.QueryAsync<DbMoveLearnMethod>(token, query, new object[] { displayLanguage, id });
                DbMoveLearnMethod method = methods.First();
                return new MoveLearnMethod
                {
                    Description = method.Description,
                    Id = method.Id,
                    Name = method.Name
                };
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<ObservableCollection<PokemonMove>> LoadMoveSetAsync(int pokemonId, GameVersion version, int displayLanguage, CancellationToken token)
        {
            try
            {
                List<DbPokemonMove> moveList = await _connection.Table<DbPokemonMove>().Where(w => w.PokemonId == pokemonId && w.VersionGroupId == version.VersionGroup).ToListAsync(token);

                var bag = new ConcurrentBag<PokemonMove>();
                var tasks = moveList.Select(async (m) =>
                {
                    var move = new PokemonMove
                    {
                        Id = m.Id,
                        Level = m.Level,
                        Order = m.Order
                    };
                    move.LearnMethod = await LoadMoveLearnMethodAsync(m.MoveLearnMethodId, displayLanguage, token);
                    move.Move = await LoadMoveAsync(m.MoveId, version, displayLanguage, token);
                    bag.Add(move);
                });
                await Task.WhenAll(tasks);
                return new ObservableCollection<PokemonMove>(bag.OrderBy(o => o.LearnMethod.Id).ThenBy(t => t.Level).ThenBy(t => t.Order));
            }
            catch (Exception)
            {
                return new ObservableCollection<PokemonMove>();
            }
        }

        public async Task<ObservableCollection<PokemonEvolution>> LoadPossibleEvolutionsAsync(int speciesId, GameVersion version, int displayLanguage, CancellationToken token)
        {
            try
            {
                string query = "SELECT ps.id, psn.name, pe.min_level, etn.name AS evolution_trigger, pe.evolution_item_id FROM pokemon_v2_pokemonspecies ps\n" +
                    "LEFT JOIN\n(SELECT def.pokemon_species_id AS id, IFNULL(curr.name, def.name) AS name FROM pokemon_v2_pokemonspeciesname def\n" +
                    "LEFT JOIN pokemon_v2_pokemonspeciesname curr ON def.pokemon_species_id = curr.pokemon_species_id AND def.language_id = 9 AND curr.language_id = ?\n" +
                    "GROUP BY def.pokemon_species_id)\nAS psn ON ps.id = psn.id\n" +
                    "LEFT JOIN pokemon_v2_pokemonevolution pe ON pe.evolved_species_id = ps.id\n" +
                    "LEFT JOIN pokemon_v2_evolutiontrigger et ON pe.evolution_trigger_id = et.id\n" +
                    "LEFT JOIN\n(SELECT e.evolution_trigger_id AS id, COALESCE(o.name, e.name) AS name FROM pokemon_v2_evolutiontriggername e\n" +
                    "LEFT OUTER JOIN pokemon_v2_evolutiontriggername o ON e.evolution_trigger_id = o.evolution_trigger_id and o.language_id = ?\n" +
                    "WHERE e.language_id = 9\nGROUP BY e.evolution_trigger_id)\nAS etn ON et.id = etn.id\n" +
                    "WHERE ps.evolves_from_species_id = ? AND ps.generation_id <= ?";
                IEnumerable<DbPokemonEvolution> evolutions = await _connection.QueryAsync<DbPokemonEvolution>(token, query, new object[] { displayLanguage, displayLanguage, speciesId, version.Generation });

                var result = new ObservableCollection<PokemonEvolution>();
                foreach (DbPokemonEvolution dbEvolution in evolutions)
                {
                    var evolution = new PokemonEvolution
                    {
                        EvolvesTo = new SpeciesName { Id = dbEvolution.Id, Name = dbEvolution.Name },
                        MinLevel = dbEvolution.MinLevel,                        
                        EvolutionTrigger = dbEvolution.EvolutionTrigger
                        
                    };
                    if (dbEvolution.EvolutionItemId != null)
                        evolution.EvolutionItem = await LoadItemAsync((int)dbEvolution.EvolutionItemId, displayLanguage, token);
                    result.Add(evolution);
                }
                return result;
            }
            catch (Exception)
            {
                return new ObservableCollection<PokemonEvolution>();
            }
        }

        public async Task<Item> LoadItemAsync(int id, int displayLanguage, CancellationToken token)
        {
            try
            {
                string query = "SELECT it.id, itn.name FROM pokemon_v2_item it\n" +
                    "LEFT JOIN\n(SELECT e.item_id AS id, COALESCE(o.name, e.name) AS name FROM pokemon_v2_itemname e\n" +
                    "LEFT OUTER JOIN pokemon_v2_itemname o ON e.item_id = o.item_id and o.language_id = ?\n" +
                    "WHERE e.language_id = 9\nGROUP BY e.item_id)\nAS itn ON it.id = itn.id\n" +
                    "WHERE it.id = ?";
                IEnumerable<DbItem> items = await _connection.QueryAsync<DbItem>(token, query, new object[] { displayLanguage, id });
                DbItem item = items.First();
                return new Item { Id = item.Id, Name = item.Name };
            }
            catch (Exception)
            {
                return null;
                throw;
            }
        }

        public async Task<ObservableCollection<PokemonEvolution>> LoadEvolutionGroupAsync(int speciesId, GameVersion version, int displayLanguage, CancellationToken token)
        {
            try
            {
                string query = "SELECT ps.id, psn.name, pe.min_level, etn.name AS evolution_trigger, pe.evolution_item_id, pe.location_id, ec.baby_trigger_item_id,\n" +
                    "pe.min_happiness, pe.time_of_day, pe.held_item_id, ps.is_baby FROM pokemon_v2_pokemonspecies ps\n" +
                    "LEFT JOIN\n(SELECT def.pokemon_species_id AS id, IFNULL(curr.name, def.name) AS name FROM pokemon_v2_pokemonspeciesname def\n" +
                    "LEFT JOIN pokemon_v2_pokemonspeciesname curr ON def.pokemon_species_id = curr.pokemon_species_id AND def.language_id = 9 AND curr.language_id = ?\n" +
                    "GROUP BY def.pokemon_species_id)\nAS psn ON ps.id = psn.id\n" +
                    "LEFT JOIN pokemon_v2_pokemonevolution pe ON pe.evolved_species_id = ps.id\n" +
                    "LEFT JOIN pokemon_v2_evolutiontrigger et ON pe.evolution_trigger_id = et.id\n" +
                    "LEFT JOIN\n(SELECT e.evolution_trigger_id AS id, COALESCE(o.name, e.name) AS name FROM pokemon_v2_evolutiontriggername e\n" +
                    "LEFT OUTER JOIN pokemon_v2_evolutiontriggername o ON e.evolution_trigger_id = o.evolution_trigger_id and o.language_id = ?\n" +
                    "WHERE e.language_id = 9\n\nGROUP BY e.evolution_trigger_id)\nAS etn ON et.id = etn.id\n" +
                    "LEFT JOIN pokemon_v2_evolutionchain ec ON ec.id = ps.evolution_chain_id\n" +
                    "WHERE ps.evolution_chain_id = (SELECT evolution_chain_id FROM pokemon_v2_pokemonspecies WHERE id = ?) AND ps.generation_id <= ?\n" +
                    "ORDER BY ps.'order'";
                IEnumerable<DbPokemonEvolution> evolutions = await _connection.QueryAsync<DbPokemonEvolution>(token, query, new object[] 
                {
                    displayLanguage,
                    displayLanguage,
                    speciesId,
                    version.Generation
                });
                var result = new ObservableCollection<PokemonEvolution>();
                foreach (DbPokemonEvolution evolution in evolutions)
                {
                    var evo = new PokemonEvolution
                    {
                        DayTime = evolution.TimeOfDay,
                        EvolutionTrigger = evolution.EvolutionTrigger,
                        EvolvesTo = new SpeciesName { Id = evolution.Id, Name = evolution.Name },                        
                        MinLevel = evolution.MinLevel,
                        MinHappiness = evolution.MinHappiness
                    };

                    if (evolution.IsBaby)
                        evo.EvolutionTrigger = "Zucht";

                    if (evolution.LocationId != null)
                    {
                        Location loc = await LoadLocationFromIdAsync((int)evolution.LocationId, version, displayLanguage, token);
                        if (loc == null)
                            continue;
                        evo.EvolutionLocation = loc;
                    }
                    if (evolution.EvolutionItemId != null)
                        evo.EvolutionItem = await LoadItemAsync((int)evolution.EvolutionItemId, displayLanguage, token);
                    else if (evolution.HeldItemId != null)
                        evo.EvolutionItem = await LoadItemAsync((int)evolution.HeldItemId, displayLanguage, token);
                    else if (evolution.IsBaby && evolution.BabyTriggerItemId != null)
                        evo.EvolutionItem = await LoadItemAsync((int)evolution.BabyTriggerItemId, displayLanguage, token);
                    result.Add(evo);
                }
                return result;
            }
            catch (Exception)
            {
                return new ObservableCollection<PokemonEvolution>();
            }
        }

        public async Task<Location> LoadLocationFromIdAsync(int id, GameVersion version, int displayLanguage, CancellationToken token)
        {
            try
            {
                string query = "SELECT loc.id, ln.name FROM pokemon_v2_location loc\n" +
                    "LEFT JOIN\n(SELECT e.location_id AS id, COALESCE(o.name, e.name) AS name FROM pokemon_v2_locationname e\n" +
                    "LEFT OUTER JOIN pokemon_v2_locationname o ON e.location_id = o.location_id and o.language_id = ?\n" +
                    "WHERE e.language_id = 9\nGROUP BY e.location_id)\nAS ln ON loc.id = ln.id\n" +
                    "LEFT JOIN pokemon_v2_region r ON r.id = loc.region_id\n" +
                    "LEFT JOIN pokemon_v2_versiongroupregion vgr ON r.id = vgr.region_id\n" +
                    "WHERE loc.id = ? AND vgr.version_group_id = ?";
                IEnumerable<DbLocation> locations = await _connection.QueryAsync<DbLocation>(token, query, new object[] { displayLanguage, id, version.VersionGroup });                
                DbLocation location = locations.FirstOrDefault();
                if (location == null)
                    return null;
                return new Location { Id = location.Id, Name = location.Name };
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<Location> LoadLocationFromAreaAsync(int areaId, GameVersion version, int displayLanguage, CancellationToken token)
        {
            try
            {
                string query = "SELECT la.id, lan.name, la.location_id FROM pokemon_v2_locationarea la\n" +
                        "LEFT JOIN\n(SELECT e.location_area_id AS id, COALESCE(o.name, e.name) AS name FROM pokemon_v2_locationareaname e\n" +
                        "LEFT OUTER JOIN pokemon_v2_locationareaname o ON e.location_area_id = o.location_area_id and o.language_id = ?\n" +
                        "WHERE e.language_id = 9\nGROUP BY e.location_area_id)\nAS lan ON la.id = lan.id\n" +                        
                        "WHERE la.id = ?";
                IEnumerable<DbLocationArea> areas = await _connection.QueryAsync<DbLocationArea>(token, query, new object[] { displayLanguage, areaId });
                DbLocationArea area = areas.First();
                Location location = await LoadLocationFromIdAsync(area.LocationId, version, displayLanguage, token);
                location.AreaId = area.Id;
                location.AreaName = area.Name;
                return location;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<ObservableCollection<PokemonLocation>> LoadPokemonEncountersAsync(int pokemonId, GameVersion version, int displayLanguage, CancellationToken token)
        {
            try
            {
                string query = "SELECT enc.id, MIN(enc.min_level) AS min_level, MAX(enc.max_level) AS max_level, enc.location_area_id, SUM(es.rarity) AS rarity, ecvm.encounter_condition_value_id, ec.id AS condition_id, es.encounter_method_id FROM pokemon_v2_encounter enc\n" +
                    "LEFT JOIN pokemon_v2_encounterslot es ON es.id = enc.encounter_slot_id\n" +
                    "LEFT JOIN pokemon_v2_encountermethod em ON em.id = es.encounter_method_id\n" +
                    "LEFT JOIN pokemon_v2_encounterconditionvaluemap ecvm ON ecvm.encounter_id = enc.id\n" +
                    "LEFT JOIN pokemon_v2_encounterconditionvalue ecv ON ecv.id = ecvm.encounter_condition_value_id\n" +
                    "LEFT JOIN pokemon_v2_encountercondition ec ON ecv.encounter_condition_id = ec.id\n" +
                    "WHERE enc.pokemon_id = ? AND enc.version_id = ?\n" +
                    "GROUP BY enc.location_area_id, es.encounter_method_id, ecvm.encounter_condition_value_id\n" +
                    "ORDER BY enc.location_area_id, ec.id, ecvm.encounter_condition_value_id";
                IEnumerable<DbPokemonEncounter> encounters = await _connection.QueryAsync<DbPokemonEncounter>(token, query, new object[] { pokemonId, version.Id });
                var result = new ObservableCollection<PokemonLocation>();
                foreach (DbPokemonEncounter encounter in encounters)
                {
                    PokemonLocation location = null;
                    //Check if there is another encounter on the same area with the same condition or null condition but another slot
                    PokemonLocation location2 = result.FirstOrDefault(f => f.Location.AreaId == encounter.LocationAreaId
                        && encounter.EncounterMethodId == f.EncounterMethod.Id && (encounter.EncounterConditionValueId == null || f.Conditions.Any() == false ||
                        (f.Conditions.Any() && f.Conditions.FirstOrDefault(x => x.Id == (int)encounter.EncounterConditionValueId) != null)));
                    // Merge encounter with same area and condition
                    if (location2 != null)
                    {
                        location2.Rarity += encounter.Rarity;
                        location = location2;
                    }
                    else
                        location = result.FirstOrDefault(f => f.Id == encounter.Id);

                    if (location == null)
                    {
                        location = new PokemonLocation
                        {
                            Id = encounter.Id,
                            MaxLevel = encounter.MaxLevel,
                            MinLevel = encounter.MinLevel,
                            Rarity = encounter.Rarity
                        };

                        location.EncounterMethod = await GetEncounterMethodAsync(encounter.EncounterMethodId);
                        location.Location = await LoadLocationFromAreaAsync(encounter.LocationAreaId, version, displayLanguage, token);
                    }

                    // Filter out double encounter conditions                    
                    if (encounter.EncounterConditionValueId != null && location.Conditions.FirstOrDefault(f => f.Id == (int)encounter.EncounterConditionValueId) == null)
                        //if (encounter.EncounterConditionValueId != null)
                        location.Conditions.Add(await GetEncounterConditionAsync((int)encounter.EncounterConditionValueId));

                    if (location2 == null)
                        result.Add(location);
                }
                return result;
            }
            catch (Exception)
            {
                return new ObservableCollection<PokemonLocation>();
            }
        }
    }
}
