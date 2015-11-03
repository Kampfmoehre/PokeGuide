using GalaSoft.MvvmLight;

using SQLite.Net.Attributes;

namespace PokeGuide.Model.Db
{
    [Table("version_groups")]
    public class VersionGroup : ObservableObject
    {
        int _id;
        string _identifier;
        int _generationId;
        int _order;
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
        [Column("identifier")]
        public string Identifier
        {
            get { return _identifier; }
            set { Set(() => Identifier, ref _identifier, value); }
        }
        /// <summary>
        /// Sets and gets the
        /// </summary>
        [Column("generation_id")]
        public int GenerationId
        {
            get { return _generationId; }
            set { Set(() => GenerationId, ref _generationId, value); }
        }
        /// <summary>
        /// Sets and gets the
        /// </summary>
        [Column("order")]
        public int Order
        {
            get { return _order; }
            set { Set(() => Order, ref _order, value); }
        }
    }
}