using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Data;

namespace PRF.WPFCore.CustomCollections
{
    /// <summary>
    /// Classe permettant de générer une ICollectionView ayant en backingField une ObservableCollectionRanged
    /// </summary>
    public static class ObservableCollectionSource
    {
        /// <summary>
        /// Return the default view for a newly created ObservableCollectionRanged
        /// </summary>
        /// <param name="enableCollectionSynchronisation">Determine if the crossThread manipulation for the underlying collection will be enabled</param>
        /// <returns>the default view</returns>
        public static ICollectionView GetDefaultView<T>(bool enableCollectionSynchronisation = true) where T : notnull
        {
            return GetDefaultView<T>(out _, enableCollectionSynchronisation);
        }

        /// <summary>
        /// Return the default view for a newly created ObservableCollectionRanged
        /// </summary>
        /// <param name="elements">list of items to add in the Collection</param>
        /// <param name="enableCollectionSynchronisation">Determine if the crossThread manipulation for the underlying collection will be enabled</param>
        /// <returns>the default view</returns>
        public static ICollectionView GetDefaultView<T>(IEnumerable<T> elements, bool enableCollectionSynchronisation = true) where T : notnull
        {
            return GetDefaultView(elements, out _, enableCollectionSynchronisation);
        }

        /// <summary>
        /// Return the default view for a newly created ObservableCollectionRanged and the created collection as out parameter
        /// </summary>
        /// <param name="col">the newly created ObservableCollectionRanged</param>
        /// <param name="enableCollectionSynchronisation">Determine if the crossThread manipulation for the underlying collection will be enabled</param>
        /// <returns>the default view</returns>
        public static ICollectionView GetDefaultView<T>(out ObservableCollectionRanged<T> col, bool enableCollectionSynchronisation = true) where T : notnull
        {
            col = new ObservableCollectionRanged<T>(enableCollectionSynchronisation);
            return CollectionViewSource.GetDefaultView(col);
        }

        /// <summary>
        /// Return the default view for a newly created ObservableCollectionRanged (from an enumeration of elemetnts)
        /// and the created collection as out parameter
        /// </summary>
        /// <param name="elements">list of items to add in the Collection</param>
        /// <param name="col">the newly created ObservableCollectionRanged</param>
        /// <param name="enableCollectionSynchronisation">Determine if the crossThread manipulation for the underlying collection will be enabled</param>
        /// <returns>the default view</returns>
        public static ICollectionView GetDefaultView<T>(IEnumerable<T> elements, out ObservableCollectionRanged<T> col, bool enableCollectionSynchronisation = true) where T : notnull
        {
            col = new ObservableCollectionRanged<T>(elements, enableCollectionSynchronisation);
            return CollectionViewSource.GetDefaultView(col);
        }
    }
}
