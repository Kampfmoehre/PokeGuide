namespace PokeGuide.Data.Model
{
    public class Pokemon : ModelBase
    {
        public string Name { get; set; }
        public double Height { get; set; }
        public double Weight { get; set; }
        public string Genus { get; set; }
        public int CaptureRate { get; set; }
        public int BaseHappiness { get; set; }
        public int HatchCounter { get; set; }
    }
}
