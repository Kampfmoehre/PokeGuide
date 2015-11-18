using System;
using System.Diagnostics;
using System.Threading.Tasks;

using PokeGuide.Core.Service.Interface;

using Windows.ApplicationModel;
using Windows.Storage;

namespace PokeGuide.Service
{
    /// <summary>
    /// Storage service for Windows 8.1 universal apps
    /// </summary>
    public class StorageService : IStorageService
    {
        /// <summary>
        /// Installs a fresh copy of the database to the local folder
        /// </summary>
        /// <param name="fileName">The name of the database file</param>
        /// <returns></returns>
        public async Task CopyDatabaseAsync(string fileName)
        {
            bool databaseExists = false;
            StorageFile file = null;
            try
            {
                file = await ApplicationData.Current.LocalFolder.GetFileAsync(fileName);
                databaseExists = true;
            }
            catch (Exception)
            {
                databaseExists = false;
            }

            if (databaseExists)
                await file.DeleteAsync(StorageDeleteOption.PermanentDelete);
            try
            {
                StorageFile databaseFile = await Package.Current.InstalledLocation.GetFileAsync(fileName);
                await databaseFile.CopyAsync(ApplicationData.Current.LocalFolder);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Database not found");
                Debug.WriteLine(ex.Message);
            }
        }

        /// <summary>
        /// Ensures that the database is installed and returns the path to the database
        /// </summary>
        /// <param name="fileName">The name of the database file</param>
        /// <returns>The path to the database file</returns>
        public async Task<string> GetDatabasePathForFileAsync(string fileName)
        {
            await CopyDatabaseAsync(fileName);
            StorageFile file = await ApplicationData.Current.LocalFolder.CreateFileAsync(fileName, CreationCollisionOption.OpenIfExists);

            return file.Path;
        }
    }
}
