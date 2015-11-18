using System.Threading.Tasks;

namespace PokeGuide.Core.Service.Interface
{
    /// <summary>
    /// Interface for device specific storage service
    /// </summary>
    public interface IStorageService
    {
        /// <summary>
        /// Should ensure that the database is installed and return the path to the database
        /// </summary>
        /// <param name="fileName">The name of the database file</param>
        /// <returns>The path to the database</returns>
        Task<string> GetDatabasePathForFileAsync(string fileName);
        /// <summary>
        /// Should ensure if the latest copy of the database is installed
        /// </summary>
        /// <param name="fileName">The name of the file</param>
        /// <returns></returns>
        Task CopyDatabaseAsync(string fileName);
    }
}
