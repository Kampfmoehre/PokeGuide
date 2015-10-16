using System.Data;

namespace PokeGuide.Data.Model
{
    public class Ability : ModelBase
    {
        public Ability()
        {
            
        }

        public Ability(DataRow row)
        {
            Effect = row["short_effect"].ToString();
            Description = row["effect"].ToString();
            FlavorText = row["flavor_text"].ToString();

        }
        public string Effect { get; set; }
        public string Description { get; set; }
        public string FlavorText { get; set; }
    }
}
