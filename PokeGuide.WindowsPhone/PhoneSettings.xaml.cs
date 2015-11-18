
using Windows.Phone.UI.Input;
using Windows.UI.Xaml.Controls;

// Die Elementvorlage "Inhaltsdialog" ist unter "http://go.microsoft.com/fwlink/?LinkID=390556" dokumentiert.

namespace PokeGuide
{
    public sealed partial class PhoneSettings : Page
    {
        public PhoneSettings()
        {
            this.InitializeComponent();
            HardwareButtons.BackPressed += HardwareButtons_BackPressed;            
        }

        void HardwareButtons_BackPressed(object sender, BackPressedEventArgs e)
        {
            //var datacontext = DataContext as ISettingsViewModel;
            //if (datacontext != null)
            //{
            //    datacontext.NavigateBackCommand.Execute(null);
            //    e.Handled = true;
            //}
        }
    }
}
