namespace PokeGuide.Data.Model
{
    public class Pokemon : ModelBase
    {
        public double Height { get; set; }
        public double Weight { get; set; }
        public string Genus { get; set; }
        public int CaptureRate { get; set; }
        public int BaseHappiness { get; set; }
        public int HatchCounter { get; set; }
        public Ability Ability1 { get; set; }
        public Ability Ability2 { get; set; }
        public Ability HiddenAbility { get; set; }
        public EggGroup EggGroup1 { get; set; }
        public EggGroup EggGroup2 { get; set; }
    }
}
