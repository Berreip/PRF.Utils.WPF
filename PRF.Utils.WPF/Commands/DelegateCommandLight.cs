using PRF.Utils.WPF.Helpers;
using PRF.Utils.WPF.UiWorkerThread;
using System;
using System.Threading.Tasks;
using System.Windows.Input;
using PRF.Utils.CoreComponents.Async;
using System.Windows;

namespace PRF.Utils.WPF.Commands
{
    /// <summary>
    /// define the most basic command that have a manual RaiseCanExecuteChanged
    /// </summary>
    public interface IDelegateCommandLightBase : ICommand
    {
        /// <summary>
        /// Request an evaluation of the CanExecute of the given command
        /// </summary>
        Task RaiseCanExecuteChanged();
    }

    /// <summary>
    /// basic commands without parameters
    /// </summary>
    public interface IDelegateCommandLight : IDelegateCommandLightBase
    {
        /// <summary>
        /// CanExecute
        /// </summary>
        bool CanExecute();

        /// <summary>
        /// Execute
        /// </summary>
        void Execute();
    }

    /// <summary>
    /// basic commands with parameter
    /// </summary>
    public interface IDelegateCommandLight<T> : IDelegateCommandLightBase
    {
        /// <summary>
        /// CanExecute with the correct expected type
        /// </summary>
        bool CanExecute(T parameter);

        /// <summary>
        /// Execute with the correct expected type
        /// </summary>
        void Execute(T parameter);
    }

    /// <summary>
    /// Async command with parameters
    /// </summary>
    public interface IDelegateCommandLightAsync<T> : IDelegateCommandLightBase
    {
        /// <summary>
        /// CanExecute with the correct expected type
        /// </summary>
        bool CanExecute(T parameter);
        
        /// <summary>
        /// Execute with the correct expected type
        /// </summary>
        Task ExecuteAsync(T parameter);
    }

    /// <summary>
    /// Async command without parameters
    /// </summary>
    public interface IDelegateCommandLightAsync : IDelegateCommandLightBase
    {
        /// <summary>
        /// CanExecute
        /// </summary>
        bool CanExecute();

        /// <summary>
        /// Execute async
        /// </summary>
        Task ExecuteAsync();
    }

    /// <inheritdoc />
    public abstract class DelegateCommandLightBase : IDelegateCommandLightBase
    {
        /// <inheritdoc />
        public async Task RaiseCanExecuteChanged()
        {
            await UiThreadDispatcher.DispatchAsync(() => CanExecuteChanged?.Invoke(this, EventArgs.Empty)).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public abstract bool CanExecute(object parameter);

        /// <inheritdoc />
        public abstract void Execute(object parameter);

        /// <inheritdoc />
        public event EventHandler CanExecuteChanged;
    }

    /// <inheritdoc />
    public abstract class DelegateCommandLightWithoutParameterBase : DelegateCommandLightBase, IDelegateCommandLight
    {
        private readonly Func<bool> _canExecute;

        /// <summary>
        /// Constructor of a parameterless command with an optional can execute
        /// </summary>
        protected DelegateCommandLightWithoutParameterBase(Func<bool> canExecute = null)
        {
            _canExecute = canExecute;
        }

        /// <inheritdoc />
        public bool CanExecute() => _canExecute == null || _canExecute();

        /// <inheritdoc />
        public abstract void Execute();

        /// <inheritdoc />
        public override bool CanExecute(object parameter) => CanExecute();

        /// <inheritdoc />
        public override void Execute(object parameter) => Execute();
    }

    /// <inheritdoc />
    public abstract class DelegateCommandLightWithParameterBase<T> : DelegateCommandLightBase, IDelegateCommandLight<T>
    {
        private readonly Func<T, bool> _canExecute;

        /// <summary>
        /// Constructor of a command with parameter and an optional can execute
        /// </summary>
        protected DelegateCommandLightWithParameterBase(Func<T, bool> canExecute)
        {
            _canExecute = canExecute;
        }

        /// <inheritdoc />
        public bool CanExecute(T parameter) => _canExecute == null || _canExecute(parameter);

        /// <inheritdoc />
        public abstract void Execute(T parameter);

        /// <inheritdoc />
        public override bool CanExecute(object parameter) => CanExecute(parameter.CheckCastParameter<T>());

        /// <inheritdoc />
        public override void Execute(object parameter) => Execute(parameter.CheckCastParameter<T>());
    }

    /// <inheritdoc />
    public sealed class DelegateCommandLight : DelegateCommandLightWithoutParameterBase
    {
        private readonly Action _execute;
        private readonly Func<Task> _executeAsync;
        private readonly Action<Exception> _onErrorOnAsync;

        /// <inheritdoc />
        public DelegateCommandLight(Action execute, Func<bool> canExecute = null) : base(canExecute)
        {
            _execute = execute;
        }

        /// <summary>
        /// constructor of an async command. Watch out, it is kind of a trick as it avoid that replacing a
        /// ctor DelegateCommandLight(() => ...) by an async one : DelegateCommandLight(async() => ....) leads to a major bug
        /// (cast of the second one to Action and no way to grab again the await part)
        /// Please prefer using the DelegateCommandAsync
        /// </summary>
        public DelegateCommandLight(Func<Task> executeAsync, Func<bool> canExecute = null, Action<Exception> onErrorOnAsync = null) : base(canExecute)
        {
            _executeAsync = executeAsync;
            _onErrorOnAsync = onErrorOnAsync;
        }

        /// <inheritdoc />
        public override void Execute()
        {
            if (_execute != null)
            {
                _execute();
            }
            else
            {
                // execute with a fire and forget BUT a try catch finally
                _executeAsync.WrapAsync(e => e.ManageErrorOnCommand(_onErrorOnAsync)).ConfigureAwait(false);
            }
        }
    }

    /// <inheritdoc />
    public sealed class DelegateCommandLight<T> : DelegateCommandLightWithParameterBase<T>
    {
        private readonly Action<T> _execute;
        private readonly Func<T, Task> _executeAsync;
        private readonly Action<Exception> _onErrorOnAsync;

        /// <inheritdoc />
        public DelegateCommandLight(Action<T> execute, Func<T, bool> canExecute = null) : base(canExecute)
        {
            _execute = execute;
        }

        /// <summary>
        /// constructor of an async command. Watch out, it is kind of a trick as it avoid that replacing a
        /// ctor DelegateCommandLight(() => ...) by an async one : DelegateCommandLight(async() => ....) leads to a major bug
        /// (cast of the second one to Action and no way to grab again the await part)
        /// Please prefer using the DelegateCommandAsync
        /// </summary>
        public DelegateCommandLight(Func<T, Task> executeAsync, Func<T, bool> canExecute = null, Action<Exception> onErrorOnAsync = null) : base(canExecute)
        {
            _executeAsync = executeAsync;
            _onErrorOnAsync = onErrorOnAsync;
        }

        /// <inheritdoc />
        public override void Execute(T parameter)
        {
            if (_execute != null)
            {
                _execute(parameter);
            }
            else
            {
                // execute with a fire and forget BUT a try catch finally
                WrapperCore.WrapAsync(async () => await _executeAsync(parameter).ConfigureAwait(false), e => e.ManageErrorOnCommand(_onErrorOnAsync));
            }
        }
    }

    /// <summary>
    /// An async command with parameter. When used as a ICommand by the framework, it will be a fire and forget call but with a try catch 
    /// </summary>
    public sealed class DelegateCommandLightAsync<T> : DelegateCommandLightWithParameterBase<T>, IDelegateCommandLightAsync<T>
    {
        private readonly Func<T, Task> _executeAsync;
        private readonly Action<Exception> _onErrorOnAsync;

        /// <summary>
        /// constructor of an async command with parameter
        /// </summary>
        /// <param name="executeAsync">the async execute method</param>
        /// <param name="canExecute">the can execute</param>
        /// <param name="onErrorOnAsync">When used as a ICommand by the framework, it will be a fire 
        /// and forget call but with a try catch. this action allow user to do error handleing. by default, a messagebox is displayed</param>
        public DelegateCommandLightAsync(Func<T, Task> executeAsync, Func<T, bool> canExecute = null, Action<Exception> onErrorOnAsync = null) : base(canExecute)
        {
            _executeAsync = executeAsync;
            _onErrorOnAsync = onErrorOnAsync;
        }

        /// <inheritdoc />
        public override void Execute(T parameter)
        {
            //Request an Execute async in a fire and forget mode
            ExecuteAsync(parameter).ConfigureAwait(false);
        }
        
        /// <inheritdoc />
        public async Task ExecuteAsync(T parameter)
        { 
            // execute with a fire and forget BUT a try catch finally
            await WrapperCore.WrapAsync(
                async () => await _executeAsync(parameter).ConfigureAwait(false), 
                e => e.ManageErrorOnCommand(_onErrorOnAsync))
                .ConfigureAwait(false);
        }
    }

    /// <summary>
    /// An async command without parameters. When used as a ICommand by the framework, it will be a fire and forget call but with a try catch 
    /// </summary>
    public sealed class DelegateCommandLightAsync : DelegateCommandLightWithoutParameterBase, IDelegateCommandLightAsync
    {
        private readonly Func<Task> _executeAsync;
        private readonly Action<Exception> _onErrorOnAsync;

        /// <summary>
        /// constructor of an async command with parameter
        /// </summary>
        /// <param name="executeAsync">the async execute method</param>
        /// <param name="canExecute">the can execute</param>
        /// <param name="onErrorOnAsync">When used as a ICommand by the framework, it will be a fire 
        /// and forget call but with a try catch. this action allow user to do error handleing. by default, a messagebox is displayed</param>
        public DelegateCommandLightAsync(Func<Task> executeAsync, Func<bool> canExecute = null, Action<Exception> onErrorOnAsync = null) : base(canExecute)
        {
            _executeAsync = executeAsync;
            _onErrorOnAsync = onErrorOnAsync;
        }

        /// <inheritdoc />
        public override void Execute()
        {
            //Request an Execute async in a fire and forget mode
            ExecuteAsync().ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task ExecuteAsync()
        {
            // execute with a fire and forget BUT a try catch finally
            await WrapperCore.WrapAsync(
                async () => await _executeAsync().ConfigureAwait(false),
                e => e.ManageErrorOnCommand(_onErrorOnAsync))
                .ConfigureAwait(false);
        }
    }

}
