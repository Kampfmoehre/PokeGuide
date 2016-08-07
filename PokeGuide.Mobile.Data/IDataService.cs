using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using PokeGuide.Model;

namespace PokeGuide.Mobile.Data
{
    public interface IDataService
    {
        Task<IEnumerable<DisplayLanguage>> LoadLanguages(int displayLanguage, CancellationToken token);
    }
}
