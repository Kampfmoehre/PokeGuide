namespace PokeGuide.Model
{
    public class PokemonEvolution : ModelBase
    {
        SpeciesName _evolvesTo;
        int? _minLevel;
        string _evolutionTrigger;
        Item _evolutionItem;
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
    }
}
