using System.Threading.Tasks;

namespace PokeGuide.Core.Service
{
    /// <summary>
    /// Interface for device specific storage services
    /// </summary>
    public interface IStorageService
    {
        /// <summary>
        /// Should ensure that the database is installed and return the path to the database
        /// </summary>
        /// <param name="fileName">The name of the database file</param>
        /// <returns>The full path to the database</returns>
        Task<string> GetDatabasePathFromFileAsync(string fileName);
        /// <summary>
        /// Should ensure that the latest copy of the database is installed
        /// </summary>
        /// <param name="fileName">The path to the database</param>
        Task CopyDatabaseAsync(string fileName);
    }
}
