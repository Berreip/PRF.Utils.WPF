using System;

namespace PRF.WPFCore.BootStrappers
{
    /// <summary>
    /// Define a splash screen
    /// </summary>
    public interface ISplashScreen
    {
        void UpdateMessage(string message);
    }

    /// <summary>
    /// Define a splashScreen controller
    /// </summary>
    public interface ISplashController
    {
        event Action<string> OnUpdateMessage;
        event Action OnLoadingDoneSuccessfully;
        event Action<Exception> OnLoadingFailed;
    }
}