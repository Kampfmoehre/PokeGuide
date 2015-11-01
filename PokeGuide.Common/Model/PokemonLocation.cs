using System;
using System.Collections.Generic;
using System.Linq;

namespace PokeGuide.Model
{
    public class PokemonLocation : ModelBase
    {
        public PokemonLocation()
        {
            Conditions = new List<EncounterCondition>();
        }
        Location _location;
        double _rarity;
        int _minLevel;
        int _maxLevel;
        EncounterMethod _encounterMethod;
        List<EncounterCondition> _conditions;
        /// <summary>
        /// Sets and gets the 
        /// </summary>
        public Location Location
        {
            get { return _location; }
            set { Set(() => Location, ref _location, value); }
        }
        /// <summary>
        /// Sets and gets the 
        /// </summary>
        public double Rarity
        {
            get { return _rarity; }
            set { Set(() => Rarity, ref _rarity, value); }
        }
        /// <summary>
        /// Sets and gets the 
        /// </summary>
        public int MinLevel
        {
            get { return _minLevel; }
            set { Set(() => MinLevel, ref _minLevel, value); }
        }
        /// <summary>
        /// Sets and gets the 
        /// </summary>
        public int MaxLevel
        {
            get { return _maxLevel; }
            set { Set(() => MaxLevel, ref _maxLevel, value); }
        }
        /// <summary>
        /// Sets and gets the 
        /// </summary>
        public EncounterMethod EncounterMethod
        {
            get { return _encounterMethod; }
            set { Set(() => EncounterMethod, ref _encounterMethod, value); }
        }
        /// <summary>
        /// Sets and gets the 
        /// </summary>
        public List<EncounterCondition> Conditions
        {
            get { return _conditions; }
            set { Set(() => Conditions, ref _conditions, value); }
        }
        public string ConditionText
        {
            get
            {
                if (Conditions.Any())
                    return String.Join(", ", Conditions.Select(s => s.Name));
                return String.Empty;
            }
        }
    }
}
