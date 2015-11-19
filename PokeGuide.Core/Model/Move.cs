namespace PokeGuide.Core.Model
{
    /// <summary>
    /// Represents a move
    /// </summary>
    public class Move : ModelNameBase
    {
        int? _power;
        int? _accuracy;
        int _priority;
        int _powerPoints;
        ElementType _type;
        DamageClass _damageCategory;
        string _ingameText;
        string _shortDescription;
        string _description;
        string _versionChangelog;
        /// <summary>
        /// Sets and gets the 
        /// </summary>
        public int? Power
        {
            get { return _power; }
            set { Set(() => Power, ref _power, value); }
        }
        /// <summary>
        /// Sets and gets the 
        /// </summary>
        public int? Accuracy
        {
            get { return _accuracy; }
            set { Set(() => Accuracy, ref _accuracy, value); }
        }
        /// <summary>
        /// Sets and gets the 
        /// </summary>
        public int Priority
        {
            get { return _priority; }
            set { Set(() => Priority, ref _priority, value); }
        }
        /// <summary>
        /// Sets and gets the 
        /// </summary>
        public int PowerPoints
        {
            get { return _powerPoints; }
            set { Set(() => PowerPoints, ref _powerPoints, value); }
        }
        /// <summary>
        /// Sets and gets the 
        /// </summary>
        public ElementType Type
        {
            get { return _type; }
            set { Set(() => Type, ref _type, value); }
        }        
        /// <summary>
        /// Sets and gets the 
        /// </summary>
        public DamageClass DamageCategory
        {
            get { return _damageCategory; }
            set { Set(() => DamageCategory, ref _damageCategory, value); }
        }
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
