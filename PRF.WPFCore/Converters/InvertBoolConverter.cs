using System;
using System.Globalization;
using System.Windows.Data;

// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable InheritdocConsiderUsage
// ReSharper disable AutoPropertyCanBeMadeGetOnly.Local

namespace PRF.WPFCore.Converters
{
    /// <summary>
    /// Convertisseur Permettant d'inverser un booléen
    /// </summary>
    public class InvertBoolConverter : IValueConverter
    {
        /// <summary>
        /// Fait la conversion
        /// </summary>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (targetType != typeof(bool))
                throw new InvalidCastException("value is not a boolean");

            // ReSharper disable once PossibleNullReferenceException
            return !(bool)value;
        }

        /// <summary>
        /// Fait la conversion inverse
        /// </summary>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (targetType != typeof(bool))
                throw new InvalidCastException("value is not a boolean");

            // ReSharper disable once PossibleNullReferenceException
            return !(bool)value;
        }
    }
}
