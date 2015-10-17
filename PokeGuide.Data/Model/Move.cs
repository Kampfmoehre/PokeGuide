using System;
using System.Collections.Generic;

namespace PokeGuide.Data.Model
{
    public class Move : ModelBase
    {
        public int? Power { get; set; }
        public int PowerPoints { get; set; }
        public int? Accuracy { get; set; }
        public int Priority { get; set; }
        public DamageClass DamageClass { get; set; }
        public ElementType Type { get; set; }
        internal override List<Mapping> GetMappings()
        {
            List<Mapping> result = base.GetMappings();
            
            result.Add(new Mapping { Column = "power", PropertyName = "Power", TypeToCast = typeof(Nullable<Int32>) });
            result.Add(new Mapping { Column = "pp", PropertyName = "PowerPoints", TypeToCast = typeof(Int32) });
            result.Add(new Mapping { Column = "accuracy", PropertyName = "Accuracy", TypeToCast = typeof(Nullable<Int32>) });
            result.Add(new Mapping { Column = "priority", PropertyName = "Priority", TypeToCast = typeof(Int32) });
            return result;
        }
    }
}
