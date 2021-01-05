using System;
using System.Windows;
using System.Windows.Threading;

namespace WpfModelApp.WPFCore
{
    /// <summary>
    /// Classe de démarrage de l'application (il faut la setter comme telle dans les propriétés du projet la première fois)
    /// </summary>
    internal static class Startup
    {
        [STAThread]
        internal static void Main()
        {
            try
            {
                var container = new ModelAppBoot();
                var app = new App();
                //close the app when the main window is closed (default value is lastWindow)
                app.ShutdownMode = ShutdownMode.OnMainWindowClose;
                app.DispatcherUnhandledException += OnUnhandledException;
                AppDomain.CurrentDomain.UnhandledException += AppDomainOnUnhandledException;
                app.Exit += container.OnExit;
                app.InitializeComponent();

                app.Run(container.Run());
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Unhandled Exception (will exit after close): {ex} ");
            }
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
