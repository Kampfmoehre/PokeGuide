namespace PokeGuide.Core.Model
{
    /// <summary>
    /// Represents a language that can be selected by the user in which all data should be displayed
    /// </summary>
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
