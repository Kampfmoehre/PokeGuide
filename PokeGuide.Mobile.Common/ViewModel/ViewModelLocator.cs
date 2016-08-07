using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;
using Microsoft.Practices.ServiceLocation;
using PokeGuide.Mobile.Data;

namespace PokeGuide.Mobile.Common.ViewModel
{
    public class ViewModelLocator
    {
        public ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

            if (ViewModelBase.IsInDesignModeStatic)
            {

            }
            else
            {
                SimpleIoc.Default.Register<IDataService, DataService>();
            }

            SimpleIoc.Default.Register<IMainViewModel, MainViewModel>();
        }

        public IMainViewModel MainViewModel
        {
            get { return ServiceLocator.Current.GetInstance<IMainViewModel>(); }
        }

        public static void Cleanup()
        { }
    }
}
