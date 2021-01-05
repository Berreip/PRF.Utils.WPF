using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Text;
using System.Windows.Controls;
using System.Windows.Data;

// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable InheritdocConsiderUsage
// ReSharper disable AutoPropertyCanBeMadeGetOnly.Local

namespace PRF.WPFCore.Converters
{
    /// <summary>
    /// Utile pour les règles de validation
    /// </summary>
    public class ValidationContentConverter : IValueConverter
    {
        /// <summary>
        /// Fait la conversion
        /// </summary>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var ve = (ReadOnlyObservableCollection<ValidationError>)value;
            if (ve == null || ve.Count <= 0) return "Valid value";

            var sb = new StringBuilder();
            foreach (var validationError in ve)
            {
                sb.AppendLine(validationError.ErrorContent.ToString());
            }
            var result = sb.ToString();
            return result.Length >= 2
                ? result.Remove(result.Length - 2, 2)
                : result;
        }

        /// <summary>
        /// Fait la conversion inverse
        /// </summary>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
