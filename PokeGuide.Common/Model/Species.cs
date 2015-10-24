namespace PokeGuide.Model
{
    public class Species : SpeciesName
    {
        int _hatchCounter;
        int _baseHappiness;
        int _catchRate;
        PokedexEntry _dexEntry;
        GrowthRate _growthRate;

        /// <summary>
        /// Sets and gets the 
        /// </summary>
        public int HatchCounter
        {
            get { return _hatchCounter; }
            set { Set(() => HatchCounter, ref _hatchCounter, value); }
        }
        /// <summary>
        /// Sets and gets the 
        /// </summary>
        public int BaseHappiness
        {
            get { return _baseHappiness; }
            set { Set(() => BaseHappiness, ref _baseHappiness, value); }
        }
        /// <summary>
        /// Sets and gets the 
        /// </summary>
        public int CatchRate
        {
            get { return _catchRate; }
            set { Set(() => CatchRate, ref _catchRate, value); }
        }
        /// <summary>
        /// Sets and gets the 
        /// </summary>
        //[Column("growth_rate_id")]
        public GrowthRate GrowthRate
        {
            get { return _growthRate; }
            set { Set(() => GrowthRate, ref _growthRate, value); }
        }
        /// <summary>
        /// Sets and gets the 
        /// </summary>
        public PokedexEntry DexEntry
        {
            get { return _dexEntry; }
            set { Set(() => DexEntry, ref _dexEntry, value); }
        }
    }
}