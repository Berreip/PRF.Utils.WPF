using System.Diagnostics;
using PRF.WPFCore;
using PRF.WPFCore.Commands;
using PRF.WPFCore.Diagnostic;

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
            //
            // System.Diagnostics.Debug.SetProvider();
            // DebugProvider
        }

        private void ExecuteDebugFailCommand()
        {
            DebugCore.Fail("Manual debug fail");
        }

        private void ExecuteDebugAssertCommand()
        {
            DebugCore.Assert(this.GetType() == typeof(IDelegateCommandLight), "expected type is not expected");
        }
    }
}