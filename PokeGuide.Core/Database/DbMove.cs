using SQLite.Net.Attributes;

namespace PokeGuide.Core.Database
{
    [Table("moves")]
    public class DbMove : DbName
    {
        [Column("type_id")]
        public int TypeId { get; set; }
        [Column("power")]
        public int? Power { get; set; }
        [Column("pp")]
        public int PowerPoints { get; set; }
        [Column("accuracy")]
        public int? Accuracy { get; set; }
        [Column("priority")]
        public int Priority { get; set; }
        [Column("target_id")]
        public int TargetId { get; set; }
        [Column("damage_class_id")]
        public int DamageClassId { get; set; }
        [Column("short_effect")]
        public string ShortEffect { get; set; }
        [Column("effect")]
        public string Effect { get; set; }
        [Column("effect_change")]
        public string EffectChange { get; set; }
        [Column("flavor_text")]
        public string FlavorText { get; set; }
        [Column("effect_chance")]
        public int? EffectChance { get; set; }
        [Column("meta_category_id")]
        public int MetaCategoryId { get; set; }
        [Column("meta_ailment_id")]
        public int metaAilmentId { get; set; }
        [Column("min_hits")]
        public int? MinHits { get; set; }
        [Column("max_hits")]
        public int? MaxHits { get; set; }
        [Column("min_turns")]
        public int? MinTurns { get; set; }
        [Column("max_turns")]
        public int? MaxTurns { get; set; }
        [Column("drain")]
        public int Drain { get; set; }
        [Column("healing")]
        public int Healing { get; set; }
        [Column("crit_rate")]
        public int CritRate { get; set; }
        [Column("ailment_chance")]
        public int AilmentChance { get; set; }
        [Column("flinch_chance")]
        public int FlinchChance { get; set; }
        [Column("stat_chance")]
        public int StatChance { get; set; }
    }
}
