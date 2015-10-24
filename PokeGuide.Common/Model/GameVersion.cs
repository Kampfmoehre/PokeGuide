using SQLite.Net.Attributes;

namespace PokeGuide.Model
{
    [Table("pokemon_v2_version")]
    public class GameVersion : ModelBase
    {
        int _versionGroup;
        int _generation;
        /// <summary>
        /// Sets and gets the 
        /// </summary>
        [Column("version_group_id")]
        public int VersionGroup
        {
            get { return _versionGroup; }
            set { Set(() => VersionGroup, ref _versionGroup, value); }
        }
        /// <summary>
        /// Sets and gets the 
        /// </summary>
        [Column("generation_id")]
        public int Generation
        {
            get { return _generation; }
            set { Set(() => Generation, ref _generation, value); }
        }
    }
}
