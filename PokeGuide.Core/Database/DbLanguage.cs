using SQLite.Net.Attributes;

namespace PokeGuide.Core.Database
{
    [Table("languages")]
    public class DbLanguage : DbName
    {
        [Column("iso639")]
        public string Iso639 { get; set; }
    }
}
