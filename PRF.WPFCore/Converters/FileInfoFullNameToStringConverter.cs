using System;
using System.Globalization;
using System.IO;
using System.Windows.Data;

// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable InheritdocConsiderUsage
// ReSharper disable AutoPropertyCanBeMadeGetOnly.Local

namespace PRF.WPFCore.Converters
{
    /// <summary>
    /// Convertisseur Permettant de convertir un FileInfo en string (en utilisant FullName)
    /// </summary>
    public class FileInfoFullNameToStringConverter : IValueConverter
    {
        /// <summary>
        /// Fait la conversion
        /// </summary>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var dir = value as FileInfo;
            return dir?.FullName ?? string.Empty;
        }

        /// <summary>
        /// Fait la conversion inverse
        /// </summary>
        public object? ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var str = value as string;
            return string.IsNullOrEmpty(str) ? null : new FileInfo(str);
        }
    }
}
