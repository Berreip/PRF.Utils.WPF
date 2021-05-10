using PRF.WPFCore;

namespace WpfModelApp.WPFCore.SplashScreen
{
    public sealed class SplashScreenViewModel : NotifierBase
    {
        private string _message;

        public string Message
        {
            get => _message;
            set => SetProperty(ref _message, value);
        }
    }
}