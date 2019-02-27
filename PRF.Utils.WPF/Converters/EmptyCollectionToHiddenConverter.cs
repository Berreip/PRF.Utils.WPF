using System;
using System.Collections;
using System.Windows;
using System.Windows.Data;

// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable InheritdocConsiderUsage
// ReSharper disable AutoPropertyCanBeMadeGetOnly.Local

namespace PRF.Utils.WPF.Converters
{
    /// <summary>
    /// Rend un élément visible si la liste bindée est non vide
    /// </summary>
    public class EmptyCollectionToHiddenConverter : IValueConverter
    {
        /// <summary>
        /// Fait la conversion
        /// </summary>
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            switch (value)
            {
                case null:
                    return Visibility.Collapsed; // cachée si null
                case ListCollectionView collectionView:
                    return collectionView.Count == 0 ? Visibility.Collapsed : Visibility.Visible;
                case ICollection collection:
                    return collection.Count == 0 ? Visibility.Collapsed : Visibility.Visible;
            }

            return Visibility.Visible;
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
