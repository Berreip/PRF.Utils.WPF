using System;
using System.Windows;

namespace PRF.WPFCore.Commands
{
    internal static class CommandHelpers
    {
        public static T CheckCastParameter<T>(this object parameter)
        {
            try
            {
                return (T)parameter;
            }
            catch (Exception e)
            {
                throw new InvalidCastException("the parameter provided to the DelegateCommandLight is not of the expected type", e);
            }
        }

        public static void ManageErrorOnCommand(this Exception ex, Action<Exception> exceptionCallBack)
        {
            if (exceptionCallBack != null)
            {
                exceptionCallBack(ex);
            }
            else
            {
                MessageBox.Show($"error in command: {ex}");
            }
        }
    }
}
