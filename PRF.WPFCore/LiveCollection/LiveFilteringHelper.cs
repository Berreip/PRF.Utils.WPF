using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Data;
using System.Windows.Threading;

// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable InheritdocConsiderUsage
// ReSharper disable AutoPropertyCanBeMadeGetOnly.Local

namespace PRF.WPFCore.LiveCollection
{
    /// <summary>
    /// Externalise l'activation des filtres
    /// </summary>
    public static class LiveFilteringHelper
    {
        /// <summary>
        /// Permet d'activer le filtrage dynamique (la collection est filtrée si certaines de ses propriété changent)
        /// </summary>
        /// <param name="collectionView">la collection que l'on souhaite filtrer dynamiquement</param>
        /// <param name="livefilterProps">les champs dont les modifications déclenchent un filtrage</param>
        public static void ActivateLiveFiltering(this ICollectionView collectionView, params string[] livefilterProps)
        {
            // On vérifie qu'on est dans le thread UI pour faciliter l'utilisation: les Collections seront très probablement
            // consommées depuis des vues, et l'ItemSource DOIT être créé dans le mm thread. Autant vérifier dès maintenant que l'on est
            // dans le thread UI afin d'éviter des exceptions peu claires plus tard.
            var uiThread = Application.Current != null ? Application.Current.Dispatcher : Dispatcher.CurrentDispatcher;
            if (!uiThread.CheckAccess())
            {
                throw new InvalidOperationException("You must use ActivateLiveFiltering in the Ui Thread. Ideally, in the constructor of a ViewModel");
            }

            if (collectionView is ListCollectionView listView)
            {
                ActivateLiveFiltering(listView, livefilterProps);
            }
        }

        // ReSharper disable once ParameterTypeCanBeEnumerable.Local
        private static void ActivateLiveFiltering(ListCollectionView collectionView, string[] livefilterProps)
        {
            using (collectionView.DeferRefresh())
            {
                foreach (var lfp in livefilterProps)
                {
                    collectionView.LiveFilteringProperties.Add(lfp);
                }
                collectionView.IsLiveFiltering = true;
            }
        }
        /// <summary>
        /// Permet d'activer le tri dynamique (la collection est triée si certaines de ses propriété changent)
        /// </summary>
        /// <param name="collectionView">la collection que l'on souhaite trier dynamiquement</param>
        /// <param name="livesortProps">les champs dont les modifications déclenchent un tri</param>
        public static void ActivateLiveSorting(this ICollectionView collectionView, params string[] livesortProps)
        {
            // On vérifie qu'on est dans le thread UI pour faciliter l'utilisation: les Collections seront très probablement
            // consommées depuis des vues, et l'ItemSource DOIT être créé dans le mm thread. Autant vérifier dès maintenant que l'on est
            // dans le thread UI afin d'éviter des exceptions peu claires plus tard.
            var uiThread = Application.Current != null ? Application.Current.Dispatcher : Dispatcher.CurrentDispatcher;
            if (!uiThread.CheckAccess())
            {
                throw new InvalidOperationException("You must use ActivateLiveSorting in the Ui Thread. Ideally, in the constructor of a ViewModel");
            }

            if (collectionView is ListCollectionView listView)
            {
                ActivateLiveSorting(listView, livesortProps);
            }
        }

        // ReSharper disable once UnusedMember.Local
        // ReSharper disable once ParameterTypeCanBeEnumerable.Local
        private static void ActivateLiveSorting(ListCollectionView collectionView, string[] livesortProps)
        {
            using (collectionView.DeferRefresh())
            {
                foreach (var lfp in livesortProps)
                {
                    collectionView.LiveSortingProperties.Add(lfp);
                }
                collectionView.IsLiveSorting = true;
            }
        }
    }
}
