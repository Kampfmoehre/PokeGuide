using System;

namespace PokeGuide.Model
{
    public class PokemonEvolution : ModelBase
    {
        SpeciesName _evolvesTo;
        int? _minLevel;
        string _evolutionTrigger;
        Item _evolutionItem;
        Location _EvolutionLocation;
        int? _minHappiness;
        string _dayTime;

        /// <summary>
        /// Sets and gets the 
        /// </summary>
        public SpeciesName EvolvesTo
        {
            get { return _evolvesTo; }
            set { Set(() => EvolvesTo, ref _evolvesTo, value); }
        }
        /// <summary>
        /// Sets and gets the 
        /// </summary>
        public int? MinLevel
        {
            get { return _minLevel; }
            set { Set(() => MinLevel, ref _minLevel, value); }
        }
        /// <summary>
        /// Sets and gets the 
        /// </summary>
        public string EvolutionTrigger
        {
            get { return _evolutionTrigger; }
            set { Set(() => EvolutionTrigger, ref _evolutionTrigger, value); }
        }
        /// <summary>
        /// Sets and gets the 
        /// </summary>
        public Item EvolutionItem
        {
            get { return _evolutionItem; }
            set { Set(() => EvolutionItem, ref _evolutionItem, value); }
        }
        /// <summary>
        /// Sets and gets the 
        /// </summary>
        public Location EvolutionLocation
        {
            get { return _EvolutionLocation; }
            set { Set(() => EvolutionLocation, ref _EvolutionLocation, value); }
        }
        /// <summary>
        /// Sets and gets the 
        /// </summary>
        public int? MinHappiness
        {
            get { return _minHappiness; }
            set { Set(() => MinHappiness, ref _minHappiness, value); }
        }
        /// <summary>
        /// Sets and gets the 
        /// </summary>
        public string DayTime
        {
            get { return _dayTime; }
            set { Set(() => DayTime, ref _dayTime, value); }
        }
        /// <summary>
        /// Sets and gets the 
        /// </summary>
        public string EvolutionReason
        {
            get
            {
                string ret = String.Empty;
                if (EvolutionLocation != null)
                    ret += EvolutionLocation.Name;
                if (EvolutionItem != null)
                    ret += EvolutionItem.Name;
                if (MinHappiness != null)
                    ret += "Zufriedenheit";
                if (!String.IsNullOrWhiteSpace(DayTime))
                {
                    if (String.IsNullOrWhiteSpace(ret))
                        ret = DayTime;
                    else
                        ret = String.Format("{0} {1}", ret, DayTime);
                }
                return ret;                
            }
        }
    }
}
