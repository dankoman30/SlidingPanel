using System.Globalization;
using System.Windows.Data;
using System.Windows;

namespace WpfApp2.Converters
{
    public class ItemSpacingToMarginConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values.Length > 0 && values[0] is double spacing)
            {
                return new Thickness(0, 0, spacing, 0);
            }
            return new Thickness(0);
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
