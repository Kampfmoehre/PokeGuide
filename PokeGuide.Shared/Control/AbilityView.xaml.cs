using PokeGuide.Model;

using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// Die Elementvorlage "Benutzersteuerelement" ist unter http://go.microsoft.com/fwlink/?LinkId=234236 dokumentiert.

namespace PokeGuide.Control
{
    public sealed partial class AbilityView : UserControl
    {
        public AbilityView()
        {
            this.InitializeComponent();
            DataContextChanged += AbilityView_DataContextChanged;
        }

        void AbilityView_DataContextChanged(FrameworkElement sender, DataContextChangedEventArgs args)
        {
            if (DataContext == null)
                return;

            var ability = DataContext as Ability;
            if (ability == null)
                return;

            PlaceholderTextConverter.ConvertTextWithPlaceholders(txtEffect, ability.Effect);
            PlaceholderTextConverter.ConvertTextWithPlaceholders(txtEffectChange, ability.EffectChange);
            PlaceholderTextConverter.ConvertTextWithPlaceholders(txtShortEffect, ability.Description);
        }
    }
}
