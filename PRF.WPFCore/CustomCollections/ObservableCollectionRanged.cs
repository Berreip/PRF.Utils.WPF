using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Data;
using System.Windows.Threading;

// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable InheritdocConsiderUsage
// ReSharper disable AutoPropertyCanBeMadeGetOnly.Local

namespace PRF.WPFCore.CustomCollections
{
    public interface IObservableCollectionRanged<T> : IList<T>, IReadOnlyList<T>, INotifyCollectionChanged, INotifyPropertyChanged
    {
        /// <summary>
        /// Ajoute tous les éléments fournis avec une seule notification finale
        /// </summary>
        /// <param name="items">la liste des éléments à ajouter</param>
        void AddRange(IEnumerable<T> items);

        /// <summary>
        /// Nettoie la collection et lui ajoute tous les éléments fournis (avec une seule notification finale)
        /// </summary>
        /// <param name="items">la liste des éléments à ajouter après le nettoyage</param>
        void Reset(IEnumerable<T> items);

        /// <summary>
        /// Ajoute les éléments demandés en gérant le différentiel (les éléments manquant sont ajouté,
        ///  les éléments en trop sont retirés, les autres sont laissé tel quel)
        /// </summary>
        /// <param name="elementsToAdd">la liste </param>
        void AddRangeDifferential(IEnumerable<T> elementsToAdd);
        
        new int Count { get; }
        new T this[int index] { get; set; }
    }

    /// <summary>
    /// Représente une implémentation de l'ObservableCollection qui supporte l'ajout ou le retrait d'un ensemble d'éléments sans notifier pour chacun 
    /// d'entre eux mais seulement une seule fois.
    /// </summary>
    public class ObservableCollectionRanged<T> : ObservableCollection<T>, IObservableCollectionRanged<T> where T : notnull
    {  
        private readonly object _syncCollection = new object();

        /// <summary>
        /// Créer une nouvelle ObservableCollectionRanged et lui spécifie si l'on souhaite déporter la gestion de la
        ///  synchronisation dans le thread UI (comportement par défaut)
        /// </summary>
        /// <param name="enableCollectionSynchronisation">détermine si l'on souhaite déporter la gestion de la
        ///  synchronisation dans le thread UI (true). Sinon, il faut préalablement se synchroniser dans le thread UI (false)</param>
        public ObservableCollectionRanged(bool enableCollectionSynchronisation = true)
        {
            EnableCollectionSync(enableCollectionSynchronisation);
        }

        /// <summary>
        /// Créer une nouvelle ObservableCollectionRanged à partir d'une énumération et lui spécifie si l'on souhaite déporter la gestion de la
        ///  synchronisation dans le thread UI (comportement par défaut)
        /// </summary>
        /// <param name="elements">une énumération d'éléments qui servira de base pour la création de l'ObservableCollectionRanged</param>
        /// <param name="enableCollectionSynchronisation">détermine si l'on souhaite déporter la gestion de la
        ///  synchronisation dans le thread UI (true). Sinon, il faut préalablement se synchroniser dans le thread UI (false)</param>
        public ObservableCollectionRanged(IEnumerable<T> elements, bool enableCollectionSynchronisation = true) : base(elements)
        {
            EnableCollectionSync(enableCollectionSynchronisation);
        }
        
        /// <summary>
        /// Ajoute tous les éléments fournis avec une seule notification finale
        /// </summary>
        /// <param name="items">la liste des éléments à ajouter</param>
        public void AddRange(IEnumerable<T> items)
        {
            CheckReentrancy();
            lock (_syncCollection)
            {
                AddRangeInternal(items);
            }
            NotifyReset();
        }


        /// <summary>
        /// Nettoie la collection et lui ajoute tous les éléments fournis (avec une seule notification finale)
        /// </summary>
        /// <param name="items">la liste des éléments à ajouter après le nettoyage</param>
        public void Reset(IEnumerable<T> items)
        {
            CheckReentrancy();
            lock (_syncCollection)
            {
                Items.Clear();
                AddRangeInternal(items);
            }
            NotifyReset();
        }

        /// <summary>
        /// Ajoute les éléments demandés en gérant le différentiel (les éléments manquant sont ajouté,
        ///  les éléments en trop sont retirés, les autres sont laissé tel quel)
        /// </summary>
        /// <param name="elementsToAdd">la liste </param>
        public void AddRangeDifferential(IEnumerable<T> elementsToAdd)
        {
            CheckReentrancy();
            lock (_syncCollection)
            {
                var isPresentDictionary = elementsToAdd.ToDictionary(o => o, o => false);
                var itemToRemoves = new List<T>();
                foreach (var item in Items)
                {
                    if (!isPresentDictionary.ContainsKey(item))
                    {
                        // ajoute à la liste des éléments à supprimer
                        itemToRemoves.Add(item);
                    }
                    else
                    {
                        // signale que l'élément est déjà présent
                        isPresentDictionary[item] = true;
                    }
                }

                // supprime les éléments signalés en tant que tel:
                foreach (var itemToRemove in itemToRemoves)
                {
                    Items.Remove(itemToRemove);
                }

                // ajoute les nouveaux
                AddRangeInternal(isPresentDictionary.Where(o => !o.Value).Select(o => o.Key));                
            }
            NotifyReset();
        }
        
        /// <summary>
        /// Add range without notification (to avoid a lock on Notify)
        /// </summary>
        private void AddRangeInternal(IEnumerable<T> items)
        {
            foreach (var item in items)
            {
                Items.Add(item);
            }
        }
        
        private void NotifyReset()
        {
            OnPropertyChanged(new PropertyChangedEventArgs(nameof(Count)));
            OnPropertyChanged(new PropertyChangedEventArgs("Item[]"));
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }

        private void EnableCollectionSync(bool enableCollectionSynchronisation)
        {
            if (enableCollectionSynchronisation)
            {
                if (!GetUiThread()?.CheckAccess() ?? false)
                {
                    // thrown an exception if not in the UIThread (this collection should be created in the UI Thread)
                    throw new InvalidOperationException($"Error: {nameof(ObservableCollectionRanged<T>)} should be created in the UiThread");
                }
                // Active la synchronisation de la collection d'objets de données
                BindingOperations.EnableCollectionSynchronization(this, _syncCollection);
            }
        }

        /// <summary>
        /// Override with locked access
        /// </summary>
        protected override void InsertItem(int index, T item)
        {
            lock (_syncCollection)
            {
                base.InsertItem(index, item);
            }
        }

        /// <summary>
        /// Override with locked access
        /// </summary>
        protected override void ClearItems()
        {
            lock (_syncCollection)
            {
                base.ClearItems();
            }
        }

        /// <summary>
        /// Override with locked access
        /// </summary>
        protected override void MoveItem(int oldIndex, int newIndex)
        {
            lock (_syncCollection)
            {
                base.MoveItem(oldIndex, newIndex);
            }
        }

        /// <summary>
        /// Override with locked access
        /// </summary>
        protected override void RemoveItem(int index)
        {
            lock (_syncCollection)
            {
                base.RemoveItem(index);
            }
        }

        /// <summary>
        /// Override with locked access
        /// </summary>
        protected override void SetItem(int index, T item)
        {
            lock (_syncCollection)
            {
                base.SetItem(index, item);
            }
        }

        private static Dispatcher GetUiThread() => Application.Current != null ? Application.Current.Dispatcher : Dispatcher.CurrentDispatcher;
    }
}
