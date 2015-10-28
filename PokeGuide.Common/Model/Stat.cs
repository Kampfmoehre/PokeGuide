namespace PokeGuide.Model
{
    public class Stat : ModelBase
    {
        int _effortValue;
        int _statValue;
        /// <summary>
        /// Sets and gets the 
        /// </summary>
        public int EffortValue
        {
            get { return _effortValue; }
            set { Set(() => EffortValue, ref _effortValue, value); }
        }
        /// <summary>
        /// Sets and gets the 
        /// </summary>
        public int StatValue
        {
            get { return _statValue; }
            set { Set(() => StatValue, ref _statValue, value); }
        }
    }
}
