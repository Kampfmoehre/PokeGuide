using System;
using System.Collections.Generic;

namespace PokeGuide.Data.Model
{
    public class Stat : ModelBase
    {
        public int StatValue { get; set; }
        public int EffortValue { get; set; }

        internal override List<Mapping> GetMappings()
        {
            return new List<Mapping>
            {
                new Mapping { Column = "stat_id", PropertyName = "Id", TypeToCast = typeof(Int32) },
                new Mapping { Column = "name", PropertyName = "Name", TypeToCast = typeof(String) },
                new Mapping { Column = "base_stat", PropertyName = "StatValue", TypeToCast = typeof(Int32) },
                new Mapping { Column = "effort", PropertyName = "EffortValue", TypeToCast = typeof(Int32) }
            };
        }
    }
}
