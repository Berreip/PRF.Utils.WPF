using System;
using System.Threading.Tasks;
using System.Windows;

namespace PRF.Utils.WPF.Helpers
{
    /// <summary>
    /// Classe utilitaire pour rendre al main dès que possible en entrée d'une commande
    /// </summary>
    public static class WrapperCore
    {
        /// <summary>
        /// Lance en arrière plan une action (utile surtout en entrée d'une commande)
        /// </summary>
        /// <param name="action">l'action à exécuter</param>
        /// <param name="onFinally">l'action en finally éventuelle</param>
        /// <returns></returns>
        public static async Task WrapAsync(Action action, Action onFinally = null)
        {
            await Task.Run(() =>
            {
                try
                {
                    action.Invoke();
                }
                catch (Exception e)
                {
                    MessageBox.Show($"Error: {e}");
                }
                finally
                {
                    InvokeFinally(onFinally);
                }
            }).ConfigureAwait(false);
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
    }
}
