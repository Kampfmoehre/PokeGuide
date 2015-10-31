namespace PokeGuide.Model
{
    public class GenderRate : ModelBase
    {
        double? _female;
        /// <summary>
        /// Sets and gets the 
        /// </summary>
        public double? Female
        {
            get { return _female; }
            set { Set(() => Female, ref _female, value); }
        }
        double? _male;
        /// <summary>
        /// Sets and gets the 
        /// </summary>
        public double? Male
        {
            get { return _male; }
            set { Set(() => Male, ref _male, value); }
        }
    }
}
