using System;
using System.ComponentModel;
using PRF.Utils.WPF.CustomCollections;
using PRF.Utils.WPF.Navigation;
using WpfModelApp.Views.MainView.View1;
using WpfModelApp.Views.MainView.View2;

namespace WpfModelApp.Navigation
{
    /// <summary>
    /// Interface de navigation du panneau principal
    /// </summary>
    internal interface IMainPanelNavigation : IWindowNavigator, IDisposable
    {
        /// <summary>
        /// La liste des commandes
        /// </summary>
        ICollectionView Commands { get; }

        /// <summary>
        /// Indique si l'on souhaite afficher le panneau de sélection des menus
        /// </summary>
        bool ShouldDisplayMenu { get; set; }
    }

    /// <inheritdoc cref="IMainPanelNavigation" />
    internal class MainPanelNavigation : WindowNavigator, IMainPanelNavigation
    {
        private readonly ObservableCollectionRanged<INavigationCommand> _commandsObs;
        private bool _displayMenu;

        /// <inheritdoc />
        public ICollectionView Commands { get; }
        
        public MainPanelNavigation()
        {
            Commands = ObservableCollectionSource.GetDefaultView(out _commandsObs);
            _commandsObs.Add(AddNavigationView<View1View>("Vue 1"));
            _commandsObs.Add(AddNavigationView<View2View>("Vue 2"));
            foreach (var cmd in _commandsObs)
            {
                cmd.IsSelectedChanged += OnSelectionChanged;
            }
        }

        /// <inheritdoc />
        public bool ShouldDisplayMenu
        {
            get => _displayMenu;
            set => SetProperty(ref _displayMenu, value);
        }

        private void OnSelectionChanged(bool isSelected)
        {
            ShouldDisplayMenu = false;
        }

        public void Dispose()
        {
            foreach (var cmd in _commandsObs)
            {
                cmd.IsSelectedChanged -= OnSelectionChanged;
            }
        }
    }

}
