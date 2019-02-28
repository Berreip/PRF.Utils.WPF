using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using PRF.Utils.CoreComponents.Extensions;

namespace PRF.Utils.WPF
{
    /// <summary>
    /// Classe de base pour toutes les classes notifiante. 
    /// Afin d'eviter les fuites mémoires, il est conseillé de dériver de cette classe dans tous les ViewModel et tous les adapteurs
    /// (ou d'implémenter INotifyPropertyChanged)
    /// </summary>
    public abstract class NotifierBase : INotifyPropertyChanged
    {
        private readonly HashSet<string> _propertyHash;

        /// <inheritdoc />
        /// <summary>
        /// L'évènement de changement des propriétés notifiantes
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Constructeur
        /// </summary>
        protected NotifierBase()
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
        protected void Notify([CallerMemberName] string propertyName = null)
        {
            if (!_propertyHash.Contains(propertyName))
            {
                throw new InvalidOperationException($"Trying to raise notification on property \"{propertyName}\" which does not exists on type \"{GetType().Name}\"");
            }
            InvokeProperty(new PropertyChangedEventArgs(propertyName));
        }

        private void InvokeProperty(PropertyChangedEventArgs e)
        {
            var handler = PropertyChanged;
            try
            {
                handler?.Invoke(this, e);
            }
            catch
            {
                //uniquement pour prévenir les rares cas de plantage (bug du framework) en cas de notification d'une propriété mise à jour de puis un thread de background 
                //et surveillée par le liveshapping d'une collectionview 
                try
                {
                    //du coup on re-essaye:
                    handler?.Invoke(this, e);
                }
                catch
                {
                    //et on avale si ca replante une 2e fois
                }
            }
        }
    }
}
