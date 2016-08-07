namespace PokeGuide.Model
{
    public class DisplayLanguage : ModelNameBase
    {
        string _iso639;
        /// <summary>
        /// Sets and gets the language abbreviation for ISO 639
        /// </summary>
        public string Iso639
        {
            get { return _iso639; }
            set { Set(() => Iso639, ref _iso639, value); }
        }
    }
}
