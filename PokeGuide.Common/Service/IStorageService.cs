using System.Threading.Tasks;

namespace PokeGuide.Service
{
    public interface IStorageService
    {
        Task<string> GetPathForFileAsync(string fileName);
        Task CopyDatabaseAsync(string fileName);        
    }
}