using SQLite.Net.Attributes;

namespace PokeGuide.Mobile.Data.Dto
{
    [Table("languages")]
    class DbLanguage : DbName
    {
        [Column("iso639")]
        public string Iso639 { get; set; }
    }
}
