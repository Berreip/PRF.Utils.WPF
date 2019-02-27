using System;
using System.Globalization;
using System.IO;
using System.Windows.Data;

// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable InheritdocConsiderUsage
// ReSharper disable AutoPropertyCanBeMadeGetOnly.Local

namespace PRF.Utils.WPF.Converters
{
    /// <summary>
    /// Convertisseur Permettant de convertir un DirectoryInfo en string correspondant à son Name
    /// </summary>
    public class DirectoryInfoToStringConverter : IValueConverter
    {
        /// <summary>
        /// Fait la conversion
        /// </summary>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var dir = value as DirectoryInfo;
            return dir?.Name ?? string.Empty;
        }

        /// <summary>
        /// Fait la conversion inverse
        /// </summary>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // la conversion retour n'est pas implémenté pour les noms simples car on ne peut retrouver le chemin complet
            throw new NotImplementedException();
        }
    }
}
