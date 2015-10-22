namespace PokeGuide.Wpf.Model
{
    public class Species : ModelBase
    {
        string _genus;
        int _captureRate;
        int _baseHappiness;
        int _hatchCounter;
        /// <summary>
        /// Sets and gets the 
        /// </summary>
        public string Genus
        {
            get { return _genus; }
            set { Set(() => Genus, ref _genus, value); }
        }        
        /// <summary>
        /// Sets and gets the 
        /// </summary>
        public int CaptureRate
        {
            get { return _captureRate; }
            set { Set(() => CaptureRate, ref _captureRate, value); }
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
        public int HatchCounter
        {
            get { return _hatchCounter; }
            set { Set(() => HatchCounter, ref _hatchCounter, value); }
        }
    }
}
