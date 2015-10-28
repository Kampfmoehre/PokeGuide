namespace PokeGuide.Model
{
    public class Species : SpeciesName
    {
        int _hatchCounter;
        int _baseHappiness;
        int _catchRate;
        PokedexEntry _dexEntry;
        GrowthRate _growthRate;
        EggGroup _eggGroup1;
        EggGroup _eggGroup2;

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
        /// <summary>
        /// Sets and gets the 
        /// </summary>
        public EggGroup EggGroup1
        {
            get { return _eggGroup1; }
            set { Set(() => EggGroup1, ref _eggGroup1, value); }
        }
        /// <summary>
        /// Sets and gets the 
        /// </summary>
        public EggGroup EggGroup2
        {
            get { return _eggGroup2; }
            set { Set(() => EggGroup2, ref _eggGroup2, value); }
        }
    }
}