using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows;

// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable InheritdocConsiderUsage
// ReSharper disable AutoPropertyCanBeMadeGetOnly.Local
namespace PRF.WPFCore.UiWorkerThread
{
    /// <summary>
    /// Encapsule le lancement d'une tache dans le thread UI avec un try/catch + message
    /// </summary>
    public static class AsyncWrapperUi
    {
        /// <summary>
        /// The global default callback in cas of exception (to override if wanted
        /// </summary>
        public static Action<Exception> OnErrorCallBack { get; set; } = e =>
        {
            Trace.TraceError($"Error: {e}");
            MessageBox.Show($"Error: {e}");
        };
        
        /// <summary>
        /// Lance une action en asynchrone dans le Thread UI et capture l'exception pour afficher un message
        /// </summary>
        public static async Task DispatchUiAndWrapAsync(Action toDo, Action? onFinally = null)
        {
            try
            {
                await UiThreadDispatcher.ExecuteOnUIAsync(toDo).ConfigureAwait(false);
            }
            catch (Exception e)
            {
                OnErrorCallBack?.Invoke(e);
            }
            finally
            {
                InvokeFinally(onFinally);
            }
        }
        
        /// <summary>
        /// Lance une action en asynchrone dans le Thread UI et capture l'exception pour afficher un message
        /// </summary>
        public static async Task DispatchUiAndWrapAsync(Func<Task> toDo, Action? onFinally = null)
        {
            try
            {
                await UiThreadDispatcher.ExecuteOnUIAsync(toDo).ConfigureAwait(false);
            }
            catch (Exception e)
            {
                OnErrorCallBack?.Invoke(e);
            }
            finally
            {
                InvokeFinally(onFinally);
            }
        }
        
        /// <summary>
        /// Lance une action en asynchrone dans le Thread UI et capture l'exception pour afficher un message
        /// </summary>
        public static async Task<T?> DispatchUiAndWrapAsync<T>(Func<T> toDo, Action? onFinally = null)
        {
            try
            {
                return await UiThreadDispatcher.ExecuteOnUIAsync(toDo).ConfigureAwait(false);
            }
            catch (Exception e)
            {
                OnErrorCallBack?.Invoke(e);
                return default;
            }
            finally
            {
                InvokeFinally(onFinally);
            }
        }
        
        /// <summary>
        /// Lance une action en asynchrone dans le Thread UI et capture l'exception pour afficher un message
        /// </summary>
        public static async Task<T?> DispatchUiAndWrapAsync<T>(Func<Task<T>> toDo, Action? onFinally = null)
        {
            try
            {
                return await UiThreadDispatcher.ExecuteOnUIAsync(toDo).ConfigureAwait(false);
            }
            catch (Exception e)
            {
                OnErrorCallBack?.Invoke(e);
                return default;
            }
            finally
            {
                InvokeFinally(onFinally);
            }
        }

        /// <summary>
        /// Start and wrap with a try catch a sync action in the ui thread
        /// </summary>
        public static void DispatchUiAndWrap(Action toDo, Action? onFinally = null)
        {
            try
            {
                UiThreadDispatcher.ExecuteOnUI(toDo);
            }
            catch (Exception e)
            {
                OnErrorCallBack?.Invoke(e);
            }
            finally
            {
                onFinally?.Invoke();
            }
        }
        
        private static void InvokeFinally(Action? onFinally)
        {
            try
            {
                onFinally?.Invoke();
            }
            catch (Exception e)
            {
                OnErrorCallBack?.Invoke(e);
            }
        }
    }
}
