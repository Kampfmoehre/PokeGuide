using PokeGuide.Core.Model;

using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

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

            PlaceholderTextConverter.ConvertTextWithPlaceholders(txtEffect, ability.Description);
            PlaceholderTextConverter.ConvertTextWithPlaceholders(txtEffectChange, ability.VersionChangelog);
            PlaceholderTextConverter.ConvertTextWithPlaceholders(txtShortEffect, ability.ShortDescription);
        }
    }
}
