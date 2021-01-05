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
    /// Convertie la valeur d'un int (typiquement pour une progressBar) en visibility
    /// </summary>
    public class ProgressBarValueToVisibilityConverter : IValueConverter
    {
        /// <summary>
        /// Fait la conversion
        /// </summary>
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            try
            {
                return value == null || (int)value > 0
                    ? Visibility.Visible
                    : Visibility.Collapsed;
            }
            catch
            {
                return Visibility.Visible;
            }
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
