using System.Diagnostics;
using PRF.WPFCore;
using PRF.WPFCore.Commands;

namespace WpfModelApp.WPFCore.Views.SecondaryView.SecondaryView1
{
    internal class Secondary1ViewModel : ViewModelBase
    {
        public IDelegateCommandLight DebugAssertCommand { get; }
        public IDelegateCommandLight DebugFailCommand { get; }

        public Secondary1ViewModel()
        {
            DebugAssertCommand = new DelegateCommandLight(ExecuteDebugAssertCommand);
            DebugFailCommand = new DelegateCommandLight(ExecuteDebugFailCommand);

        }

        private void ExecuteDebugFailCommand()
        {
            Debug.Fail("Manual debug fail");
        }

        private void ExecuteDebugAssertCommand()
        {
            Debug.Assert(this.GetType() == typeof(IDelegateCommandLight), "expected type is not expected");
        }
    }
}