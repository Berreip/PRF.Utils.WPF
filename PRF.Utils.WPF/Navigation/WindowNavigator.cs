using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using PRF.Utils.WPF.BootStrappers;
using PRF.Utils.WPF.Commands;

namespace PRF.Utils.WPF.Navigation
{
    /// <summary>
    /// Gestionnaire de navigation pour un panel donné. Il est lié à plusieurs commandes et vues
    /// </summary>
    public interface IWindowNavigator : IEnumerable<KeyValuePair<Type, INavigationCommand>>
    {
        /// <summary>
        /// Le panel affiché pour ce navigateur
        /// </summary>
        NavigablePanelDependencyData NavigablePanelData { get; }

        /// <summary>
        /// On demande la navigation vers la première vue de la liste
        /// </summary>
        void NavigateToFirstView();

        /// <summary>
        /// Demande l'affichage de la vue correspondant au type donné
        /// </summary>
        void RequestMainPanelNavigation<T>() where T : INavigableView;

        /// <summary>
        /// Demande l'affichage de la vue correspondant au type donné
        /// </summary>
        void RequestMainPanelNavigation(Type targetType);
    }

    /// <inheritdoc cref="IWindowNavigator" />
    public abstract class WindowNavigator : NotifierBase, IWindowNavigator
    {
        private readonly Dictionary<Type, INavigationCommand> _commandsReference = new Dictionary<Type, INavigationCommand>();
        private readonly object _key = new object();
        private NavigablePanelDependencyData _navigablePanelData;

        /// <summary>
        /// Ajoute des boutons de commande et une vue à ce panneau de navigation (permet de lier plusieurs boutons à l'affichage d'une même zone)
        /// </summary>
        /// <typeparam name="T">le type de la vue navigable</typeparam>
        /// <param name="commandName">le nom de la commande pour un éventuel affichage</param>
        /// <param name="canExecute">la condition d'execution éventuelle</param>
        protected INavigationCommand AddNavigationView<T>(string commandName, Func<bool> canExecute = null) where T : INavigableView
        {
            var navigationCommand = new NavigationCommand(typeof(T), commandName, UpdateNavigablePanelData, canExecute);
            _commandsReference.Add(typeof(T), navigationCommand);
            return navigationCommand;
        }
        
        /// <inheritdoc/>
        public void NavigateToFirstView()
        {
            SelectButtonAndNavigate(_commandsReference.Values.First());
        }

        /// <inheritdoc/>
        public void RequestMainPanelNavigation<T>() where T : INavigableView
        {
            RequestMainPanelNavigation(typeof(T));
        }

        /// <inheritdoc/>
        public void RequestMainPanelNavigation(Type targetType)
        {
            if (targetType == null || !_commandsReference.TryGetValue(targetType, out var command))
            {
                // si le type souhaité n'est plus une page ou n'existe plus on lance une exception
                throw new InvalidOperationException($"Navigation to type {targetType} is not possible. Are you sure you registered this type before? ");
            }
            SelectButtonAndNavigate(command);
        }

        private void SelectButtonAndNavigate(INavigationCommand command)
        {
            lock (_key)
            {
                foreach (var otherCommand in _commandsReference.Values.Where(o => o != command && o.IsSelected))
                {
                    otherCommand.IsSelected = false;
                }
                command.IsSelected = true;
                UpdateNavigablePanelData(command.RegisteredType);
            }
        }

        private void UpdateNavigablePanelData(Type targetType)
        {
            NavigablePanelData = new NavigablePanelDependencyData(targetType);
        }


        /// <inheritdoc/>
        public NavigablePanelDependencyData NavigablePanelData
        {
            get => _navigablePanelData;
            private set
            {
                _navigablePanelData = value;
                Notify();
            }
        }

        /// <inheritdoc/>
        public IEnumerator<KeyValuePair<Type, INavigationCommand>> GetEnumerator() => _commandsReference.GetEnumerator();

        /// <inheritdoc/>
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        
        private sealed class NavigationCommand : NotifierBase, INavigationCommand
        {
            private bool _isSelected;

            public event Action<bool> IsSelectedChanged;

            public NavigationCommand(Type navigationViewType, string name, Action<Type> execute, Func<bool> canExecute = null)
            {
                Name = name;
                RegisteredType = navigationViewType;
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
                    if (SetProperty(ref _isSelected, value))
                    {
                        RaiseIsSelectedChanged(value);
                    }
                }
            }

            private void RaiseIsSelectedChanged(bool isSelected)
            {
                IsSelectedChanged?.Invoke(isSelected);
            }
        }
    }


    /// <summary>
    /// Classe permettant d'injecter un panel 
    /// </summary>
    public static class PanelWindowResolver
    {
        /// <summary>
        /// Retrieve the panel
        /// </summary>
        public static NavigablePanelDependencyData GetPanel(DependencyObject obj)
        {
            return (NavigablePanelDependencyData)obj.GetValue(PanelProperty);
        }

        /// <summary>
        /// Set the panel
        /// </summary>
        public static void SetPanel(DependencyObject obj, NavigablePanelDependencyData value)
        {
            obj.SetValue(PanelProperty, value);
        }

        /// <summary>
        /// The panel itself
        /// </summary>
        public static readonly DependencyProperty PanelProperty = DependencyProperty.RegisterAttached(
            "Panel", typeof(NavigablePanelDependencyData), typeof(PanelWindowResolver),
            new PropertyMetadata(new NavigablePanelDependencyData(), OnPanelChanged));


        private static void OnPanelChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is ContentControl content && e.NewValue is NavigablePanelDependencyData dependencyData) try
                {
                    INavigableView previousNavigableView = null;
                    if (content.HasContent && content.Content is INavigableView pView)
                    {
                        previousNavigableView = pView;

                        // dans le cas où l'on souhaite naviguer vers la page elle même, on ne CHANGE PAS la page mais on appele 
                        // la méthode NavigateToItSelfRequested et on quitte
                        if (previousNavigableView.GetType() == dependencyData.ViewType)
                        {
                            previousNavigableView.NavigateToItSelfRequested();
                            return;
                        }
                    }

                    var navigableView = ContainerHolder.Container.Resolve<INavigableView>(dependencyData.ViewType);

                    // préviens la nouvelle vue que l'on y rentre
                    navigableView.NavigateToCurrentRequested();
                    // set le contenu:
                    content.Content = navigableView;

                    // préviens l'ancienne vue que l'on la quitte (après qu'elle ne soit plus affichée)
                    previousNavigableView?.NavigateFromCurrentRequested();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"ERREUR: {ex}");
                }
        }

    }

    /// <summary>
    /// DependencyData used for keeping track of the displayed panel
    /// </summary>
    public class NavigablePanelDependencyData
    {
        /// <summary>
        /// The displayed view type
        /// </summary>
        public Type ViewType { get; }

        /// <summary>
        /// Empty constructor
        /// </summary>
        public NavigablePanelDependencyData() { }

        /// <summary>
        /// Guve the displayed view type
        /// </summary>
        public NavigablePanelDependencyData(Type type)
        {
            ViewType = type;
        }
    }
}

