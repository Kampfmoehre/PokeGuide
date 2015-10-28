namespace PokeGuide.Model
{
    public class PokedexEntry : ModelBase
    {
        int? _dexNumber;
        /// <summary>
        /// Sets and gets the 
        /// </summary>
        public int? DexNumber
        {
            get { return _dexNumber; }
            set { Set(() => DexNumber, ref _dexNumber, value); }
        }
    }
}
