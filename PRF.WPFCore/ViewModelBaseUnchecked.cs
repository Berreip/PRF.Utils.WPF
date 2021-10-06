using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace PRF.WPFCore
{
    /// <summary>
    /// Classe de base pour toutes les classes notifiantes. Cette version ne vérifie pas que la propriété notifiante existe bien. 
    /// Celà permet d'éviter la réflexion pour contruire le hash de vérification dans les cas où cette étape serait impactante (très très peu probable)
    /// Afin d'eviter les fuites mémoires, il est conseillé de dériver de cette classe dans tous les ViewModel et tous les adapteurs
    /// (ou d'implémenter INotifyPropertyChanged)
    /// </summary>
    public abstract class NotifierBaseUnchecked : INotifyPropertyChanged
    {
        /// <inheritdoc />
        /// <summary>
        /// L'évènement de changement des propriétés notifiantes
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Notifie un changement de valeur d'une propriété notifiable
        /// </summary>
        protected void RaisePropertyChanged([CallerMemberName] string propertyName = null)
        {
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
