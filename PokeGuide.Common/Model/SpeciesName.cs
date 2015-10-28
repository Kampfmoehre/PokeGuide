namespace PokeGuide.Model
{
    public class SpeciesName : ModelBase
    {
        int _generation;
        /// <summary>
        /// Sets and gets the 
        /// </summary>
        public int Generation
        {
            get { return _generation; }
            set { Set(() => Generation, ref _generation, value); }
        }
    }
}
