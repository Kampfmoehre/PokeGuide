using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;
using Microsoft.Practices.ServiceLocation;
using SQLite.Net.Interop;
using SQLite.Net.Platform.WinRT;

namespace PokeGuide.Service
{
    public class DeviceLocatorService
    {
        static DeviceLocatorService()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);
            if (ViewModelBase.IsInDesignModeStatic)
            { }
            else
            { }

            if (!SimpleIoc.Default.IsRegistered<IStorageService>())
                SimpleIoc.Default.Register<IStorageService, StorageService>();

            if (!SimpleIoc.Default.IsRegistered<ISQLitePlatform>())
                SimpleIoc.Default.Register<ISQLitePlatform, SQLitePlatformWinRT>();
        }

        public static void Cleanup()
        { }
    }
}
