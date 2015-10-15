namespace PokeGuide.Data.Model
{
    public class Ability : ModelBase
    {
        public string Name { get; set; }
        public string Effect { get; set; }
        public string Description { get; set; }
        public string FlavorText { get; set; }
    }
}
