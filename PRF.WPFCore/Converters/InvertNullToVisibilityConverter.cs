using System;
using System.Windows;
using System.Windows.Data;

// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable InheritdocConsiderUsage
// ReSharper disable AutoPropertyCanBeMadeGetOnly.Local

namespace PRF.WPFCore.Converters
{
    /// <summary>
    /// L'inverse de NullToVisibilityConverter : Collapsed si l'élément n'est pas null et Visible s'il est null
    /// </summary>
    public class InvertNullToVisibilityConverter : IValueConverter
    {
        /// <summary>
        /// Fait la conversion
        /// </summary>
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return value == null ? Visibility.Visible : Visibility.Collapsed;
        }

        /// <summary>
        /// Fait la conversion inverse
        /// </summary>
        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
