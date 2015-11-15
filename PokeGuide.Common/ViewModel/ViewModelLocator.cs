using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;

using Microsoft.Practices.ServiceLocation;

using PokeGuide.Design;
using PokeGuide.Service;
using PokeGuide.Service.Interface;
using PokeGuide.ViewModel.Interface;

namespace PokeGuide.ViewModel
{
    /// <summary>
    /// This class contains static references to all the view models in the
    /// application and provides an entry point for the bindings.
    /// </summary>
    public class ViewModelLocator
    {
        /// <summary>
        /// Initializes a new instance of the ViewModelLocator class.
        /// </summary>
        public ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

            if (ViewModelBase.IsInDesignModeStatic)
            {
                // Create design time view services and models
                SimpleIoc.Default.Register<IStaticDataService, DesignStaticDataService>();
                SimpleIoc.Default.Register<IPokemonService, DesignPokemonService>();
                SimpleIoc.Default.Register<IMoveService, DesignMoveService>();
            }
            else
            {
                // Create run time view services and models
                SimpleIoc.Default.Register<IStaticDataService, StaticDataService>();
                SimpleIoc.Default.Register<IPokemonService, PokemonService>();
                SimpleIoc.Default.Register<IMoveService, MoveService>();
                SimpleIoc.Default.Register<ITestService, TestService>();                
            }

            SimpleIoc.Default.Register<IMainViewModel, MainViewModel>();
            SimpleIoc.Default.Register<IPokemonViewModel, PokemonViewModel>();
            SimpleIoc.Default.Register<ISettingsViewModel, SettingsViewModel>();
            SimpleIoc.Default.Register<ITestViewModel, TestViewModel>();
            SimpleIoc.Default.Register<IAbilityViewModel, AbilityViewModel>();
        }

        /// <summary>
        /// The main view model
        /// </summary>
        public IMainViewModel MainViewModel
        {
            get { return ServiceLocator.Current.GetInstance<IMainViewModel>(); }
        }
        /// <summary>
        /// Gets the pokemon view model
        /// </summary>
        public IPokemonViewModel PokemonViewModel
        {
            get { return ServiceLocator.Current.GetInstance<IPokemonViewModel>(); }
        }
        /// <summary>
        /// Gets the settings view model
        /// </summary>
        public ISettingsViewModel SettingsViewModel
        {
            get { return ServiceLocator.Current.GetInstance<ISettingsViewModel>(); }
        }
        public ITestViewModel TestViewModel
        {
            get { return ServiceLocator.Current.GetInstance<ITestViewModel>(); }
        }        
        public IAbilityViewModel AbilityViewModel
        {
            get { return ServiceLocator.Current.GetInstance<IAbilityViewModel>(); }
        }
        /// <summary>
        /// Cleans up resources that are not needed
        /// </summary>
        public static void Cleanup()
        {
        }
    }
}