using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Views;

namespace PokeGuide.ViewModel
{
    public class MainViewModel : ViewModelBase, IMainViewModel
    {
        INavigationService _navigationService;
        RelayCommand _openPokemonViewCommand;
        RelayCommand _openSettingsViewCommand;

        public RelayCommand OpenPokemonViewCommand
        {
            get
            {
                if (_openPokemonViewCommand == null)
                {
                    _openPokemonViewCommand = new RelayCommand(() =>
                    {
                        _navigationService.NavigateTo("PokemonView");
                    });
                }
                return _openPokemonViewCommand;
            }
        }
        public RelayCommand OpenSettingsViewCommand
        {
            get {
                if (_openSettingsViewCommand == null)
                {
                    _openSettingsViewCommand = new RelayCommand(() =>
                    {
                        _navigationService.NavigateTo("PhoneSettings");
                    });
                }
                return _openSettingsViewCommand;
            }
        }
        public MainViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;
        }
    }
}