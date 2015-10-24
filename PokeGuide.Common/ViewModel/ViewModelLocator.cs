using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;
using Microsoft.Practices.ServiceLocation;
using PokeGuide.Design;
using PokeGuide.Service;

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
            }
            else
            {
                // Create run time view services and models
                SimpleIoc.Default.Register<IStaticDataService, StaticDataService>();
                SimpleIoc.Default.Register<IPokemonService, PokemonService>();
            }

            SimpleIoc.Default.Register<IMainViewModel, MainViewModel>();
        }

        public IMainViewModel MainViewModel
        {
            get
            {
                return ServiceLocator.Current.GetInstance<IMainViewModel>();
            }
        }
        
        public static void Cleanup()
        {
            // TODO Clear the ViewModels
        }
    }
}