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
        public IDelegateCommandLight DebugFailBigTextCommand { get; }

        public Secondary1ViewModel()
        {
            DebugAssertCommand = new DelegateCommandLight(ExecuteDebugAssertCommand);
            DebugFailCommand = new DelegateCommandLight(ExecuteDebugFailCommand);
            DebugFailBigTextCommand = new DelegateCommandLight(ExecuteDebugFailBigTextCommand);
        }

        private void ExecuteDebugFailBigTextCommand()
        {
            DebugCore.Fail(@"Lorem ipsum dolor sit amet, consectetur adipiscing elit. 

Pellentesque ut diam nec mauris bibendum malesuada. Nullam pellentesque efficitur dolor ut lobortis. 
Curabitur et risus nec dui faucibus aliquet. Nam scelerisque bibendum condimentum. Sed laoreet ornare justo, non pretium nisl viverra sed. Donec malesuada non enim eget pulvinar. Integer iaculis nisi id nibh tempor imperdiet. Quisque nulla est, euismod eu blandit a, feugiat ut justo. Nam fermentum eleifend ipsum a commodo. Proin erat nibh, fermentum et dapibus ut, auctor vel lectus. Nunc a faucibus tellus. Nulla nec placerat justo. ");
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