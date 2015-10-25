using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.Storage;

namespace PokeGuide.Service
{
    public class StorageService : IStorageService
    {
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

        public async Task<string> GetPathForFileAsync(string fileName)
        {
            await CopyDatabaseAsync(fileName);
            StorageFile file = await ApplicationData.Current.LocalFolder.CreateFileAsync(fileName, CreationCollisionOption.OpenIfExists);

            return file.Path;
        }
    }
}
