using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;
using PRF.Utils.CoreComponents.Async;

namespace PRF.WPFCore.Helpers
{
    /// <summary>
    /// Allow to dispatch a callback and display a message in a message box on exception
    /// </summary>
    public static class WrapperCoreMessageBox
    {
        /// <summary>
        /// Dispatch a callback in a new task and display a message in a message box on exception
        /// </summary>
        public static async Task DispatchAndWrapAsync(Action action, Action? onFinally = null, [CallerMemberName] string methodName = "")
        {
            await AsyncWrapperBase.DispatchAndWrapAsyncBase(
                action,
                e => OnError(e, methodName),
                onFinally).ConfigureAwait(false);
        }

        /// <summary>
        /// Dispatch a callback in a new task and display a message in a message box on exception
        /// </summary>
        public static async Task DispatchAndWrapAsync<T>(Func<T> action, Action? onFinally = null, [CallerMemberName] string methodName = "")
        {
            await AsyncWrapperBase.DispatchAndWrapAsyncBase(
                action,
                e => OnError(e, methodName),
                onFinally).ConfigureAwait(false);
        }

        /// <summary>
        /// Dispatch a callback in a new task and display a message in a message box on exception
        /// </summary>
        public static async Task DispatchAndWrapAsync(Func<Task> action, Action? onFinally = null, [CallerMemberName] string methodName = "")
        {
            await AsyncWrapperBase.DispatchAndWrapAsyncBase(
                action,
                e => OnError(e, methodName),
                onFinally).ConfigureAwait(false);
        }


        /// <summary>
        /// Dispatch a callback in a new task and display a message in a message box on exception
        /// </summary>
        public static async Task DispatchAndWrapAsync<T>(Func<Task<T>> action, Action? onFinally = null, [CallerMemberName] string methodName = "")
        {
            await AsyncWrapperBase.DispatchAndWrapAsyncBase(
                action,
                e => OnError(e, methodName),
                onFinally).ConfigureAwait(false);
        }

        private static void OnError(Exception e, string methodName)
        {
            MessageBox.Show($"Error in method [{methodName}]:  {e}");
        }
    }
}
