using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace MSOsu.View
{
    public class RoundConverter : IValueConverter
    {
        public static int Round = 4;
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Math.Round(((double)value), Round);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return DependencyProperty.UnsetValue;
        }
    }
}
