using PRF.WPFCore;
using PRF.WPFCore.Commands;
using PRF.WPFCore.PopupManager;
using WpfModelApp.WPFCore.Config;
using WpfModelApp.WPFCore.Navigation;

namespace WpfModelApp.WPFCore.Views
{
    internal class MainWindowViewModel : ViewModelBase
    {
        private readonly IWindowsPopupManager<WpfModelAppEnumWindow> _windowsPopupManager;

        public IMainPanelNavigation MainPanelNavigation { get; }

        public ISecondaryPanelNavigation SecondaryPanelNavigation { get; }

        public IDelegateCommandLight OpenPopup1ShowDialogCommand { get; }

        public IDelegateCommandLight OpenPopup1ShowCommand { get; }

        public MainWindowViewModel(
            IMainPanelNavigation mainPanelNavigation,
            ISecondaryPanelNavigation secondaryPanelNavigation, 
            IWindowsPopupManager<WpfModelAppEnumWindow> windowsPopupManager)
        {
            _windowsPopupManager = windowsPopupManager;
            MainPanelNavigation = mainPanelNavigation;
            SecondaryPanelNavigation = secondaryPanelNavigation;

            OpenPopup1ShowDialogCommand = new DelegateCommandLight(ExecuteOpenPopup1ShowDialogCommand);
            OpenPopup1ShowCommand = new DelegateCommandLight(ExecuteOpenPopup1ShowCommand);
        }

        private void ExecuteOpenPopup1ShowCommand()
        {
            _windowsPopupManager.Show(WpfModelAppEnumWindow.Popup1);
        }

        private void ExecuteOpenPopup1ShowDialogCommand()
        {
            _windowsPopupManager.ShowDialog(WpfModelAppEnumWindow.Popup1);
        }
    }
}
