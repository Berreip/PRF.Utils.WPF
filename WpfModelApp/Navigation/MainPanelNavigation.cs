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
    internal interface IMainPanelNavigation : IWindowNavigator
    {
        /// <summary>
        /// La liste des commandes
        /// </summary>
        ICollectionView Commands { get; }
    }

    /// <inheritdoc cref="IMainPanelNavigation" />
    internal class MainPanelNavigation : WindowNavigator, IMainPanelNavigation
    {
        private ObservableCollectionRanged<INavigationCommand> _commandsObs;

        /// <inheritdoc />
        public ICollectionView Commands { get; }

        public MainPanelNavigation()
        {
            Commands = ObservableCollectionSource.GetDefaultView(out _commandsObs);
            _commandsObs.Add(AddNavigationView<View1View>("Vue 1"));
            _commandsObs.Add(AddNavigationView<View2View>("Vue 2"));
        }
    }

}
