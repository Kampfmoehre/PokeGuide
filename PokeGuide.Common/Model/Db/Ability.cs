using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;
using SQLite.Net.Attributes;

namespace PokeGuide.Model.Db
{
    [Table("abilities")]
    public class Ability : ObservableObject, ILocalisable
    {
        int _id;
        string _name;
        string _identifier;
        int _generationId;
        bool _isMainSeries;
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
        [Column("is_main_series")]
        public bool IsMainSeries
        {
            get { return _isMainSeries; }
            set { Set(() => IsMainSeries, ref _isMainSeries, value); }
        }
    }
}
