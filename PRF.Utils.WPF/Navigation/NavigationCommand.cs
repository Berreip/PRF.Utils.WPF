using System;
using System.ComponentModel;
using PRF.Utils.WPF.Commands;

namespace PRF.Utils.WPF.Navigation
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
    }

    /// <inheritdoc cref="INavigationCommand"/>
    internal class NavigationCommand<T> : NotifierBase, INavigationCommand where T : INavigableView
    {
        private bool _isSelected;

        public NavigationCommand(string name, Action<Type> execute, Func<bool> canExecute = null)
        {
            Name = name;
            RegisteredType = typeof(T);
            Command = canExecute == null
                ? new DelegateCommandLight(() => execute(RegisteredType))
                : new DelegateCommandLight(() => execute(RegisteredType), canExecute);
        }

        /// <inheritdoc />
        public IDelegateCommandLight Command { get; }

        /// <inheritdoc />
        public string Name { get; }

        /// <inheritdoc />
        public Type RegisteredType { get; }

        /// <inheritdoc />
        public bool IsSelected
        {
            get => _isSelected;
            set
            {
                if (_isSelected == value) return;
                _isSelected = value;
                Notify();
            }
        }

    }
}