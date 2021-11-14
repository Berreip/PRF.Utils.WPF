using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace PRF.WPFCore
{
    /// <summary>
    /// Base class for all viewModel. In order to avoid memory leak in notifiable collection, it is advised to implement this class
    /// This class check that all raised properties exist on the current object
    /// </summary>
    public abstract class ViewModelBase : INotifyPropertyChanged
    {
        private readonly HashSet<string> _propertyHash;

        /// <inheritdoc />
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Constructor
        /// </summary>
        protected ViewModelBase()
        {
            // stocke les propriétés à la construction pour la vérification:
            _propertyHash = GetType()
                .GetProperties(BindingFlags.Instance | BindingFlags.Public)
                .Select(o => o.Name)
                .ToHashSet();
        }

        /// <summary>
        /// Notifie un changement de valeur d'une propriété notifiable
        /// </summary>
        protected void RaisePropertyChanged([CallerMemberName] string propertyName = null)
        {
            if (!_propertyHash.Contains(propertyName))
            {
                throw new InvalidOperationException($"Trying to raise notification on property \"{propertyName}\" which does not exists on type \"{GetType().Name}\"");
            }

            InvokeProperty(new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// Si la valeur a changé, met à jour l'ancienne valeur et notifie le changement de valeur de la propriété
        /// </summary>
        protected bool SetProperty<T>(ref T oldValue, T newValue, [CallerMemberName] string propertyName = null)
        {
            if (oldValue == null && newValue == null)
            {
                return false;
            }

            if (oldValue != null && oldValue.Equals(newValue))
            {
                return false;
            }

            oldValue = newValue;
            RaisePropertyChanged(propertyName);
            return true;
        }

        private void InvokeProperty(PropertyChangedEventArgs e)
        {
            PropertyChanged?.Invoke(this, e);
        }
    }
}