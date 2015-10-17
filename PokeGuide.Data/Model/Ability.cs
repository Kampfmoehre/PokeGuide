using System;
using System.Collections.Generic;

namespace PokeGuide.Data.Model
{
    public class Ability : ModelBase
    {
        public string Effect { get; set; }
        public string Description { get; set; }
        public string FlavorText { get; set; }

        internal override List<Mapping> GetMappings()
        {
            List<Mapping> result = base.GetMappings();
            result.Add(new Mapping { Column = "short_effect", PropertyName = "Effect", TypeToCast = typeof(String) });
            result.Add(new Mapping { Column = "effect", PropertyName = "Description", TypeToCast = typeof(String) });
            result.Add(new Mapping { Column = "flavor_text", PropertyName = "FlavorText", TypeToCast = typeof(String) });
            return result;
        }
    }
}
