using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;

using Microsoft.Practices.ServiceLocation;

using PokeGuide.Core.Service;

using SQLite.Net.Interop;
using SQLite.Net.Platform.WinRT;

namespace PokeGuide.Mobile.Service
{
    /// <summary>
    /// Special locator for Windows 8.1 Universal apps
    /// </summary>
    class DeviceLocatorService
    {
        static DeviceLocatorService()
        {
            if (!ServiceLocator.IsLocationProviderSet)
                ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);


            if (ViewModelBase.IsInDesignModeStatic)
            { }
            else
            {
                //if (!SimpleIoc.Default.IsRegistered<ISettingsService>())
                //    SimpleIoc.Default.Register<ISettingsService, SettingsService>();
            }

            if (!SimpleIoc.Default.IsRegistered<IStorageService>())
                SimpleIoc.Default.Register<IStorageService, StorageService>(true);

            if (!SimpleIoc.Default.IsRegistered<ISQLitePlatform>())
                SimpleIoc.Default.Register<ISQLitePlatform, SQLitePlatformWinRT>(true);
            //if (!SimpleIoc.Default.IsRegistered<INavigationService>())
            {
                //INavigationService navigationService = CreateNavigationService();
                //SimpleIoc.Default.Register<INavigationService>(() => navigationService);
            }
        }

        public static void Cleanup()
        { }
    }
}
