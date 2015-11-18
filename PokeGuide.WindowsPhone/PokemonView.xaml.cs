
using Windows.Phone.UI.Input;
using Windows.UI.Xaml.Controls;

// Die Elementvorlage "Standardseite" ist unter "http://go.microsoft.com/fwlink/?LinkID=390556" dokumentiert.

namespace PokeGuide
{
    /// <summary>
    /// Eine leere Seite, die eigenständig verwendet werden kann oder auf die innerhalb eines Frames navigiert werden kann.
    /// </summary>
    public sealed partial class PokemonView : Page
    {
        public PokemonView()
        {
            this.InitializeComponent();
            HardwareButtons.BackPressed += HardwareButtons_BackPressed;
        }

        void HardwareButtons_BackPressed(object sender, BackPressedEventArgs e)
        {
            //var datacontext = DataContext as IPokemonViewModel;
            //if (datacontext != null)
            //{
            //    datacontext.NavigateBackCommand.Execute(null);
            //    e.Handled = true;
            //}
        }
    }
}
