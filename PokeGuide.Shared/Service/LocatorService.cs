using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;
using GalaSoft.MvvmLight.Views;

using Microsoft.Practices.ServiceLocation;
using PokeGuide.Core.Service.Interface;
using PokeGuide.View;

using SQLite.Net.Interop;
using SQLite.Net.Platform.WinRT;

namespace PokeGuide.Service
{
    /// <summary>
    /// Special locator for windows unviversal app (8.1)
    /// </summary>
    public class DeviceLocatorService
    {
        static DeviceLocatorService()
        {
            if (!ServiceLocator.IsLocationProviderSet)
                ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);


            if (ViewModelBase.IsInDesignModeStatic)
            { }
            else
            {
                if (!SimpleIoc.Default.IsRegistered<ISettingsService>())
                    SimpleIoc.Default.Register<ISettingsService, SettingsService>();
            }

            if (!SimpleIoc.Default.IsRegistered<IStorageService>())
                SimpleIoc.Default.Register<IStorageService, StorageService>(true);

            if (!SimpleIoc.Default.IsRegistered<ISQLitePlatform>())
                SimpleIoc.Default.Register<ISQLitePlatform, SQLitePlatformWinRT>(true);
            if (!SimpleIoc.Default.IsRegistered<INavigationService>())
            {
                INavigationService navigationService = CreateNavigationService();
                SimpleIoc.Default.Register<INavigationService>(() => navigationService);
            }
        }

        static INavigationService CreateNavigationService()
        {
            var navigationService = new NavigationService();
#if WINDOWS_PHONE_APP
            navigationService.Configure("PhoneSettings", typeof(PhoneSettings));
            navigationService.Configure("PokemonView", typeof(PokemonView));
#endif
            navigationService.Configure("AbilityView", typeof(AbilitiesView));
            return navigationService;
        }

        /// <summary>
        /// Call to clean up
        /// </summary>
        public static void Cleanup()
        { }
    }
}
