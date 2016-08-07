using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;
using PokeGuide.Mobile.Data;

namespace PokeGuide.Mobile.Common.ViewModel
{
    public class MainViewModel : ViewModelBase, IMainViewModel
    {
        CancellationTokenSource TokenSource { get; set; }
        IDataService _dataService;

        public MainViewModel(IDataService dataService)
        {
            TokenSource = new CancellationTokenSource();
            _dataService = dataService;
        }

        public void LoadLanguages(int displayLanguage)
        {

        }
    }
}
