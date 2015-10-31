using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokeGuide.Model
{
    public class PokemonLocation : ModelBase
    {
        Location _location;
        double _rarity;
        int _minLevel;
        int _maxLevel;
        EncounterMethod _encounterMethod;
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
    }
}
