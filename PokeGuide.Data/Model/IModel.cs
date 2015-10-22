using System.Collections.Generic;

namespace PokeGuide.Data.Model
{
    interface IModel
    {
        List<Mapping> GetMappings();
        string GetQuery();
    }
}
