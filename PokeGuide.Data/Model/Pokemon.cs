using System.Collections.Generic;

namespace PokeGuide.Data.Model
{
    public class Pokemon : ModelBase
    {
        public double Height { get; set; }
        public double Weight { get; set; }
        public Species Species { get; set; }        
        
        public EggGroup EggGroup1 { get; set; }
        public EggGroup EggGroup2 { get; set; }
        
        public List<Stat> Stats { get; set; }
    }
}
