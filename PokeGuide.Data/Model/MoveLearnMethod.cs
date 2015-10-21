using System;
using System.Collections.Generic;

namespace PokeGuide.Data.Model
{
    public class MoveLearnMethod : ModelBase
    {
        public string Description { get; set; }

        internal override List<Mapping> GetMappings()
        {
            List<Mapping> mappings = base.GetMappings();
            mappings.Add(new Mapping { Column = "description", PropertyName = "Description", TypeToCast = typeof(String) });
            return mappings;
        }
    }
}
