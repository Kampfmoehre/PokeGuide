namespace PokeGuide.Core.Model
{
    /// <summary>
    /// Represents a single Pokémon game
    /// </summary>
    public class GameVersion : ModelNameBase
    {
        int _versionGroup;
        int _generation;
        /// <summary>
        /// Sets and gets the group in which the version can be categorized
        /// </summary>
        public int VersionGroup
        {
            get { return _versionGroup; }
            set { Set(() => VersionGroup, ref _versionGroup, value); }
        }
        /// <summary>
        /// Sets and gets the generation in which the game is
        /// </summary>
        public int Generation
        {
            get { return _generation; }
            set { Set(() => Generation, ref _generation, value); }
        }
    }
}
