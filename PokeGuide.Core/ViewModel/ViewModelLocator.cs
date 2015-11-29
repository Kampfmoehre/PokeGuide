using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;

using Microsoft.Practices.ServiceLocation;

using PokeGuide.Core.Design;
using PokeGuide.Core.Service;
using PokeGuide.Core.Service.Interface;
using PokeGuide.Core.ViewModel.Interface;

namespace PokeGuide.Core.ViewModel
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
                SimpleIoc.Default.Register<IDataService, DesginDataService>();
            }
            else
            {
                SimpleIoc.Default.Register<IDataService, DataService>();                
            }

            SimpleIoc.Default.Register<IMainViewModel, MainViewModel>();
            SimpleIoc.Default.Register<IAbilityViewModel, AbilityViewModel>();
            SimpleIoc.Default.Register<IMoveViewModel, MoveViewModel>();
            SimpleIoc.Default.Register<IPokemonViewModel, PokemonViewModel>();
        }

        public IMainViewModel Main
        {
            get { return ServiceLocator.Current.GetInstance<IMainViewModel>(); }
        }
        public IAbilityViewModel Ability
        {
            get { return ServiceLocator.Current.GetInstance<IAbilityViewModel>(); }
        }
        public IMoveViewModel Move
        {
            get { return ServiceLocator.Current.GetInstance<IMoveViewModel>(); }
        }
        public IPokemonViewModel Pokemon
        {
            get { return ServiceLocator.Current.GetInstance<IPokemonViewModel>(); }
        }
        
        public static void Cleanup()
        {
            // TODO Clear the ViewModels
        }
    }
}