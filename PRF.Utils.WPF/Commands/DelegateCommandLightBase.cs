using System;
using System.Threading.Tasks;
using PRF.Utils.WPF.UiWorkerThread;

namespace PRF.Utils.WPF.Commands
{
    /// <inheritdoc />
    public abstract class DelegateCommandLightBase : IDelegateCommandLight
    {
        /// <inheritdoc />
        public async Task RaiseCanExecuteChanged()
        {
            await UiThreadDispatcher.DispatchAsync(() => CanExecuteChanged?.Invoke(this, EventArgs.Empty)).ConfigureAwait(false);
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