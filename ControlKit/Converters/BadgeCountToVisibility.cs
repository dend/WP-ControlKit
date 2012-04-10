using System;
using System.Windows;
using System.Windows.Data;

namespace ControlKit.Converters
{
    public class BadgeCountToVisibility : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            string countStr = value.ToString();
            if (countStr == "0")
                return Visibility.Collapsed;
            else
                return Visibility.Visible;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
