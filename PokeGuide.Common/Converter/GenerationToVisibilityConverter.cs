using System;
using System.Globalization;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace PokeGuide.Converter
{
    /// <summary>
    /// Converts a game generation to a visibility.
    /// This will hide an element if the given generation is below the value given parameter
    /// </summary>
    class GenerationToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var info = new CultureInfo(language);
            int actualGen = System.Convert.ToInt32(value, info);
            int minimumGen = System.Convert.ToInt32(parameter, info);
            if (actualGen < minimumGen)
                return Visibility.Collapsed;

            return Visibility.Visible;
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            int actualGen = System.Convert.ToInt32(value, culture);
            int minimumGen = System.Convert.ToInt32(parameter, culture);
            if (actualGen < minimumGen)
                return Visibility.Collapsed;

            return Visibility.Visible;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
