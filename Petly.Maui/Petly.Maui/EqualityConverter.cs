using System;
using System.Globalization;
using Microsoft.Maui.Controls;

namespace Petly.Maui
{
    // Порівнює два значення у MultiBinding і повертає true/false
    public sealed class EqualityConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values == null || values.Length < 2) return false;
            var a = values[0];
            var b = values[1];
            return Equals(a, b);
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
            => throw new NotSupportedException();
    }
}
