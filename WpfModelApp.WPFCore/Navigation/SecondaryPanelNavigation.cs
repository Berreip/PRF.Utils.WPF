using PRF.WPFCore.Navigation;
using WpfModelApp.WPFCore.Views.SecondaryView.SecondaryView1;

namespace WpfModelApp.WPFCore.Navigation
{
    /// <summary>
    /// Interface de navigation secondaire
    /// </summary>
    internal interface ISecondaryPanelNavigation : IWindowNavigator
    {
    }

    /// <inheritdoc cref="ISecondaryPanelNavigation" />
    internal class SecondaryPanelNavigation : WindowNavigator, ISecondaryPanelNavigation
    {
        public SecondaryPanelNavigation()
        {
            AddNavigationView<Secondary1View>("Secondary View");
        }
    }

}
