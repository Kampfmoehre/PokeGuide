namespace PokeGuide.Wpf.Model
{
    /// <summary>
    /// Represents a version of a game
    /// </summary>
    public class GameVersion : ModelBase
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
        int _versionGroup;
        /// <summary>
        /// Sets and gets the 
        /// </summary>
        public int VersionGroup
        {
            get { return _versionGroup; }
            set { Set(() => VersionGroup, ref _versionGroup, value); }
        }
    }
}
