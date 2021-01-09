using System;
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
    public static class WrapperCoreUI
    {
        /// <summary>
        /// Lance une action en asynchrone dans le Thread UI et capture l'exception pour afficher un message
        /// </summary>
        public static async Task DispatchAsync(Action toDo, Action onFinally = null)
        {
            try
            {
                await UiThreadDispatcher.ExecuteOnUIAsync(toDo).ConfigureAwait(false);
            }
            catch (Exception e)
            {
                MessageBox.Show($"Error: {e}");
            }
            finally
            {
                InvokeFinally(onFinally);
            }
        }

        private static void InvokeFinally(Action onFinally)
        {
            try
            {
                onFinally?.Invoke();
            }
            catch (Exception e)
            {
                MessageBox.Show($"Error On finally: {e}");
            }
        }

        /// <summary>
        /// Lance une action en synchrone dans le Thread UI et capture l'exception pour afficher un message
        /// </summary>
        public static void Dispatch(Action toDo, Action onFinally = null)
        {
            try
            {
                UiThreadDispatcher.ExecuteOnUI(toDo);
            }
            catch (Exception e)
            {
                MessageBox.Show($"Error: {e}");
            }
            finally
            {
                onFinally?.Invoke();
            }
        }
    }
}
