using System;
using PRF.Utils.WPF.UiWorkerThread;

namespace PRF.Utils.WPF.Commands
{
    /// <inheritdoc />
    public abstract class DelegateCommandLightBase : IDelegateCommandLight
    {
        /// <inheritdoc />
        public void RaiseCanExecuteChanged()
        {
            UiThreadDispatcher.DispatchAsync(() => CanExecuteChanged?.Invoke(this, EventArgs.Empty));
        }

        /// <inheritdoc />
        public void Execute()
        {
            Execute(null);
        }

        /// <inheritdoc />
        public bool CanExecute()
        {
            return CanExecute(null);
        }

        /// <inheritdoc />
        public abstract bool CanExecute(object parameter);

        /// <inheritdoc />
        public abstract void Execute(object parameter);

        /// <inheritdoc />
        public event EventHandler CanExecuteChanged;
    }
}