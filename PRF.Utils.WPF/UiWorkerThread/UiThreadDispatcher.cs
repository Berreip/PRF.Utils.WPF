using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable InheritdocConsiderUsage
// ReSharper disable AutoPropertyCanBeMadeGetOnly.Local

namespace PRF.Utils.WPF.UiWorkerThread
{
    /// <summary>
    /// Classe de dispatch des action dans le thread UI
    /// </summary>
    public static class UiThreadDispatcher
    {
        /// <summary>
        ///  Execute une action dans le thread d'affichage en synchrone
        /// </summary>
        /// <param name="action"> l'action à exécuter </param>
        /// <param name="prio"> la priorité de dispatch (ne modifier qu'en connaissance de cause)</param>
        public static void Dispatch(Action action, DispatcherPriority prio = DispatcherPriority.Normal)
        {
            var uiThread = Application.Current != null ? Application.Current.Dispatcher : Dispatcher.CurrentDispatcher;

            if (uiThread.CheckAccess()) //if we are already in the UI thread, invoke action
                action();
            else
            {
                //otherwise dispatch in the ui thread
                uiThread.Invoke(action, prio);
            }
        }
        
        /// <summary>
        ///  Execute une action dans le thread d'affichage en asynchrone et donne la main pour récupérer le résultat
        /// </summary>
        /// <param name="func"> la Func à exécuter </param>
        /// <param name="prio"> la priorité de dispatch (ne modifier qu'en connaissance de cause)</param>
        public static async Task<T> DispatchAsyncWithReturn<T>(Func<T> func, DispatcherPriority prio = DispatcherPriority.Normal)
        {
            var uiThread = Application.Current != null ? Application.Current.Dispatcher : Dispatcher.CurrentDispatcher;

            //dispatch in the ui thread or invoke in current if already there
            return uiThread.CheckAccess()
                ? func()
                : await uiThread.InvokeAsync(func, prio);
        }

        /// <summary>
        ///  Execute une action dans le thread d'affichage en asynchrone et donne la main pour connaitre l'état de l'appel
        /// </summary>
        /// <param name="action"> l'action à exécuter </param>
        /// <param name="prio"> la priorité de dispatch (ne modifier qu'en connaissance de cause)</param>
        public static async Task DispatchAsync(Action action, DispatcherPriority prio = DispatcherPriority.Normal)
        {
            var uiThread = Application.Current != null ? Application.Current.Dispatcher : Dispatcher.CurrentDispatcher;

            //dispatch in the ui thread or invoke in current if already there
            if (uiThread.CheckAccess())
            {
                action();
            }
            else
            {
                await uiThread.InvokeAsync(action, prio);
            }
        }
    }
}