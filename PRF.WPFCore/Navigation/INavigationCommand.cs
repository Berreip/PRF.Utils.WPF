using System;
using System.ComponentModel;
using PRF.WPFCore.Commands;

namespace PRF.WPFCore.Navigation
{
    /// <summary>
    /// Interface des commandes de navigation du menu principal
    /// </summary>
    public interface INavigationCommand : INotifyPropertyChanged
    {
        /// <summary>
        /// Le nom du boutton
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Le type de vue enregistrée
        /// </summary>
        Type RegisteredType { get; }

        /// <summary>
        /// La commande lié à ce bouton
        /// </summary>
        IDelegateCommandLight Command { get; }

        /// <summary>
        /// Renvoie true si le bouton est actuellement sélectionné
        /// </summary>
        bool IsSelected { get; set; }

        /// <summary>
        /// Event raised when a command IsSelected change
        /// </summary>
        event Action<bool> IsSelectedChanged;
    }
}