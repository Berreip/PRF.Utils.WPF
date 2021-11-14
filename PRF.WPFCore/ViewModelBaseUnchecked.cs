using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace PRF.WPFCore
{
    /// <summary>
    /// Base class for all viewModel. In order to avoid memory leak in notifiable collection, it is advised to implement this class
    /// This class DOES NOT check that the raised property exists on the current object
    /// </summary>
    public abstract class ViewModelBaseUnchecked : INotifyPropertyChanged
    {
        /// <inheritdoc />
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Notifie un changement de valeur d'une propriété notifiable
        /// </summary>
        protected void RaisePropertyChanged([CallerMemberName] string propertyName = null)
        {
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