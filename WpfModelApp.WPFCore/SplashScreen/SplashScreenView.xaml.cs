using System.Windows;
using PRF.WPFCore.BootStrappers;

namespace WpfModelApp.WPFCore.SplashScreen
{
    public sealed partial class SplashScreenView : Window, ISplashScreen
    {
        private readonly SplashScreenViewModel _splashVm;

        public SplashScreenView(SplashScreenViewModel dataContext)
        {
            _splashVm = dataContext;
            DataContext = dataContext;
            InitializeComponent();
        }

        /// <inheritdoc />
        public void UpdateMessage(string message)
        {
            _splashVm.Message = message;
        }
    }
}