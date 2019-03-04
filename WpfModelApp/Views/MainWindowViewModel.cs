using PRF.Utils.WPF;
using PRF.Utils.WPF.Commands;
using PRF.Utils.WPF.PopupManager;
using WpfModelApp.Config;
using WpfModelApp.Navigation;

namespace WpfModelApp.Views
{
    internal class MainWindowViewModel : NotifierBase
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
