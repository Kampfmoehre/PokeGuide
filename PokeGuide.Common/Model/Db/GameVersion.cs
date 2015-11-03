using GalaSoft.MvvmLight;

using SQLite.Net.Attributes;
using SQLiteNetExtensions.Attributes;

namespace PokeGuide.Model.Db
{
    [Table("versions")]
    public class GameVersion : ObservableObject, ILocalisable
    {
        int _id;
        string _name;
        int _versionGroupId;
        VersionGroup _versionGroup;
        /// <summary>
        /// Sets and gets the
        /// </summary>
        [PrimaryKey, Column("id")]
        public int Id
        {
            get { return _id; }
            set { Set(() => Id, ref _id, value); }
        }
        /// <summary>
        /// Sets and gets the
        /// </summary>
        [Column("name")]
        public string Name
        {
            get { return _name; }
            set { Set(() => Name, ref _name, value); }
        }
        /// <summary>
        /// Sets and gets the
        /// </summary>
        [ForeignKey(typeof(VersionGroup)), Column("version_group_id")]
        public int VersionGroupId
        {
            get { return _versionGroupId; }
            set { Set(() => VersionGroupId, ref _versionGroupId, value); }
        }
        /// <summary>
        /// Sets and gets the
        /// </summary>
        [ManyToOne]
        public VersionGroup VersionGroup
        {
            get { return _versionGroup; }
            set { Set(() => VersionGroup, ref _versionGroup, value); }
        }
    }
}
