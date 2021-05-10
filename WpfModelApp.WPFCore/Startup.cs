using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using PRF.WPFCore.BootStrappers;
using WpfModelApp.WPFCore.SplashScreen;

namespace WpfModelApp.WPFCore
{
    /// <summary>
    /// Classe de démarrage de l'application (il faut la setter comme telle dans les propriétés du projet la première fois)
    /// </summary>
    internal static class Startup
    {
        [STAThread]
        internal static int Main()
        {
            try
            {
                var container = new ModelAppBoot();
                var app = new App(); // Application.Current.Dispatcher.Thread.ManagedThreadId; is set
                
                //close the app when the main window is closed (default value is lastWindow)
                app.ShutdownMode = ShutdownMode.OnMainWindowClose;
                app.DispatcherUnhandledException += OnUnhandledException;
                AppDomain.CurrentDomain.UnhandledException += AppDomainOnUnhandledException;

                var controller = new SplashController();
                app.Exit += container.OnExit;
                app.Startup += controller.OnStart;
                app.InitializeComponent();

                var i = app.Run(container.RunWithSplashScreen<SplashScreenView>(controller, w => OnLoadingSuccessful(w, app)));
                
                return i;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Unhandled Exception (will exit after close): {ex} ");
                return -1;
            }
        }

        private static void OnLoadingSuccessful(Window mainWindow, App app)
        {
            app.MainWindow = mainWindow;
            mainWindow.Show();
        }

        private static void AppDomainOnUnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            MessageBox.Show($@"AppDomainOnUnhandledException error in {sender}: Exception - {e}");
        }

        private static void OnUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            MessageBox.Show($"OnUnhandledException error: {e.Exception}");
        }
    }
}
