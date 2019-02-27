using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Windows.Data;

// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable InheritdocConsiderUsage
// ReSharper disable AutoPropertyCanBeMadeGetOnly.Local

namespace PRF.Utils.WPF.CustomCollections
{
    /// <summary>
    /// Représente une implémentation de l'ObservableCollection qui supporte l'ajout ou le retrait d'un ensemble d'éléments sans notifier pour chacun 
    /// d'entre eux mais seulement une seule fois.
    /// </summary>
    public class ObservableCollectionRanged<T> : ObservableCollection<T>
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
            if (enableCollectionSynchronisation)
            {
                // Active la synchronisation de la collection d'objets de données
                BindingOperations.EnableCollectionSynchronization(this, _syncCollection);
            }
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
            if (enableCollectionSynchronisation)
            {
                // Active la synchronisation de la collection d'objets de données
                BindingOperations.EnableCollectionSynchronization(this, _syncCollection);
            }
        }

        /// <summary>
        /// Ajoute tous les éléments fournis avec une seule notification finale
        /// </summary>
        /// <param name="items">la liste des éléments à ajouter</param>
        public void AddRange(IEnumerable<T> items)
        {
            foreach (var item in items)
            {
                Add(item);
            }
            OnPropertyChanged(new PropertyChangedEventArgs(nameof(Count)));
            OnPropertyChanged(new PropertyChangedEventArgs("Item[]"));
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }

        /// <summary>
        /// Nettoie la collection et lui ajoute tous les éléments fournis (avec une seule notification finale)
        /// </summary>
        /// <param name="items">la liste des éléments à ajouter après le nettoyage</param>
        public void Reset(IEnumerable<T> items)
        {
            Items.Clear();
            AddRange(items);
        }

        /// <summary>
        /// Ajoute les éléments demandés en gérant le différentiel (les éléments manquant sont ajouté,
        ///  les éléments en trop sont retirés, les autres sont laissé tel quel)
        /// </summary>
        /// <param name="elementsToAdd">la liste </param>
        public void AddRangeDifferential(IEnumerable<T> elementsToAdd)
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
                Remove(itemToRemove);
            }
            // ajoute les nouveaux
            foreach (var newItem in isPresentDictionary.Where(o => !o.Value))
            {
                Add(newItem.Key);
            }

            OnPropertyChanged(new PropertyChangedEventArgs(nameof(Count)));
            OnPropertyChanged(new PropertyChangedEventArgs("Item[]"));
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }
    }
}
