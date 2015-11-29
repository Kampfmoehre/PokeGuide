namespace PokeGuide.Core.Model
{
    /// <summary>
    /// Represents an entry in a Pokédex
    /// </summary>
    public class PokedexEntry : ModelNameBase
    {
        int? _dexNumber;
        string _dexDescription;
        /// <summary>
        /// Sets and gets the number that the Pokémon has in this Pokédex
        /// </summary>
        public int? DexNumber
        {
            get { return _dexNumber; }
            set { Set(() => DexNumber, ref _dexNumber, value); }
        }
        /// <summary>
        /// Sets and gets the Pokémon description that is in the Pokédex
        /// </summary>
        public string DexDescription
        {
            get { return _dexDescription; }
            set { Set(() => DexDescription, ref _dexDescription, value); }
        }
    }
}
