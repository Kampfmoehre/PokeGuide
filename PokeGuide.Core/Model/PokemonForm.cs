namespace PokeGuide.Core.Model
{
    /// <summary>
    /// Represents a Pokémon form
    /// </summary>
    public class PokemonForm : ModelNameBase
    {
        ModelNameBase _species;
        string _genus;
        int _height;
        int _weight;
        int _baseExperience;
        int _baseHappiness;
        int _hatchCounter;
        PokemonColor _color;
        ModelUriBase _shape;
        ModelNameBase _habitat;
        int _captureRate;
        bool _isBaby;
        ElementType _type1;
        ElementType _type2;
        ModelNameBase _ability1;
        ModelNameBase _ability2;
        ModelNameBase _hiddenAbility;
        int? _itemRarity;
        ModelNameBase _heldItem;
        ModelNameBase _growthRate;
        PokedexEntry _dexEntry;
        /// <summary>
        /// Sets and gets the underlying species
        /// </summary>
        public ModelNameBase Species
        {
            get { return _species; }
            set { Set(() => Species, ref _species, value); }
        }
        /// <summary>
        /// Sets and gets the genus as stated in the Pokédex
        /// </summary>
        public string Genus
        {
            get { return _genus; }
            set { Set(() => Genus, ref _genus, value); }
        }
        /// <summary>
        /// Sets and gets the height of the Pokémon
        /// </summary>
        public int Height
        {
            get { return _height; }
            set { Set(() => Height, ref _height, value); }
        }
        /// <summary>
        /// Sets and gets the weight of the Pokémon
        /// </summary>
        public int Weight
        {
            get { return _weight; }
            set { Set(() => Weight, ref _weight, value); }
        }
        /// <summary>
        /// Sets and gets the base experience that is granted upon defeating this Pokémon
        /// </summary>
        public int BaseExperience
        {
            get { return _baseExperience; }
            set { Set(() => BaseExperience, ref _baseExperience, value); }
        }
        /// <summary>
        /// Sets and gets the happiness that the Pokémon has when catched
        /// </summary>
        public int BaseHappiness
        {
            get { return _baseHappiness; }
            set { Set(() => BaseHappiness, ref _baseHappiness, value); }
        }
        /// <summary>
        /// Sets and gets the counter until this Pokémon hatches
        /// </summary>
        public int HatchCounter
        {
            get { return _hatchCounter; }
            set { Set(() => HatchCounter, ref _hatchCounter, value); }
        }
        /// <summary>
        /// Sets and gets the color as stated in the Pokédex
        /// </summary>
        public PokemonColor Color
        {
            get { return _color; }
            set { Set(() => Color, ref _color, value); }
        }
        /// <summary>
        /// Sets and gets the shape as stated in the Pokédex
        /// </summary>
        public ModelUriBase Shape
        {
            get { return _shape; }
            set { Set(() => Shape, ref _shape, value); }
        }
        /// <summary>
        /// Sets and gets the habitat as stated in the Pokédex
        /// </summary>
        public ModelNameBase Habitat
        {
            get { return _habitat; }
            set { Set(() => Habitat, ref _habitat, value); }
        }
        /// <summary>
        /// Sets and gets the rate at which this Pokémon can be captured with full HP with a Pokéball
        /// </summary>
        public int CaptureRate
        {
            get { return _captureRate; }
            set { Set(() => CaptureRate, ref _captureRate, value); }
        }
        /// <summary>
        /// Sets and gets if the Pokémon is a baby Pokémon
        /// </summary>
        public bool IsBaby
        {
            get { return _isBaby; }
            set { Set(() => IsBaby, ref _isBaby, value); }
        }
        /// <summary>
        /// Sets and gets the first type of the Pokémon
        /// </summary>
        public ElementType Type1
        {
            get { return _type1; }
            set { Set(() => Type1, ref _type1, value); }
        }
        /// <summary>
        /// Sets and gets the second type of the Pokémon
        /// </summary>
        public ElementType Type2
        {
            get { return _type2; }
            set { Set(() => Type2, ref _type2, value); }
        }
        /// <summary>
        /// Sets and gets the first ability of the Pokémon
        /// </summary>
        public ModelNameBase Ability1
        {
            get { return _ability1; }
            set { Set(() => Ability1, ref _ability1, value); }
        }
        /// <summary>
        /// Sets and gets the second ability of the Pokémon
        /// </summary>
        public ModelNameBase Ability2
        {
            get { return _ability2; }
            set { Set(() => Ability2, ref _ability2, value); }
        }
        /// <summary>
        /// Sets and gets the hidden ability of the Pokémon
        /// </summary>
        public ModelNameBase HiddenAbility
        {
            get { return _hiddenAbility; }
            set { Set(() => HiddenAbility, ref _hiddenAbility, value); }
        }
        /// <summary>
        /// Sets and gets the rarity whith which a wild Pokémon will held the item
        /// </summary>
        public int? ItemRarity
        {
            get { return _itemRarity; }
            set { Set(() => ItemRarity, ref _itemRarity, value); }
        }
        /// <summary>
        /// Sets and gets the item that wild Pokémon can held
        /// </summary>
        public ModelNameBase HeldItem
        {
            get { return _heldItem; }
            set { Set(() => HeldItem, ref _heldItem, value); }
        }
        /// <summary>
        /// Sets and gets the rate with which the Pokémon will gain new levels
        /// </summary>
        public ModelNameBase GrowthRate
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
