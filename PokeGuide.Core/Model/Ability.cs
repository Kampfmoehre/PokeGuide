namespace PokeGuide.Core.Model
{
    /// <summary>
    /// Represents an ability that a Pokémon can have
    /// </summary>
    public class Ability : ModelNameBase
    {
        string _ingameText;
        string _shortDescription;
        string _description;
        string _versionChangelog;
        /// <summary>
        /// Sets and gets the text that is displayed in the game
        /// </summary>
        public string IngameText
        {
            get { return _ingameText; }
            set { Set(() => IngameText, ref _ingameText, value); }
        }
        /// <summary>
        /// Sets and gets a summary of the description
        /// </summary>
        public string ShortDescription
        {
            get { return _shortDescription; }
            set { Set(() => ShortDescription, ref _shortDescription, value); }
        }
        /// <summary>
        /// Sets and gets the description
        /// </summary>
        public string Description
        {
            get { return _description; }
            set { Set(() => Description, ref _description, value); }
        }
        /// <summary>
        /// Sets and gets the changelog for a spezific version
        /// </summary>
        public string VersionChangelog
        {
            get { return _versionChangelog; }
            set { Set(() => VersionChangelog, ref _versionChangelog, value); }
        }
    }
}
