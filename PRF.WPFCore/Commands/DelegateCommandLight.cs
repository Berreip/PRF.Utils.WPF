using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Input;
using PRF.Utils.CoreComponents.Async;
using PRF.WPFCore.UiWorkerThread;

// ReSharper disable UnusedMember.Global

namespace PRF.WPFCore.Commands
{
    /// <summary>
    /// define the most basic command that have a manual RaiseCanExecuteChanged
    /// </summary>
    public interface IDelegateCommandLightBase : ICommand
    {
        /// <summary>
        /// Request an evaluation of the CanExecute of the given command
        /// </summary>
        void RaiseCanExecuteChanged();

        /// <summary>
        /// Request an evaluation of the CanExecute of the given command in a async way
        /// </summary>
        Task RaiseCanExecuteChangedAsync();

        /// <summary>
        /// Request an evaluation of the CanExecute of the given command in fire and forget (avoid any deadlock from any given thread)
        /// <remarks>Please note that this method is NOT awaitable: It does swallow all exceptions to
        /// avoid crashing application as it is often the case if you don't await it.
        /// If your canExecute could send business exception, you have to manage them in your callback if you wants informations.
        /// If you wants more control; use the RaiseCanExecuteChanged async.</remarks>
        /// </summary>
        void RaiseCanExecuteChangedInFireAndForget();
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
    public interface IDelegateCommandLight<in T> : IDelegateCommandLightBase
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
        public void RaiseCanExecuteChanged()
        {
            var handler = CanExecuteChanged;
            if (handler != null)
            {
                try
                {
                    UiThreadDispatcher.ExecuteOnUI(() => handler.Invoke(this, EventArgs.Empty));
                }
                catch (Exception e)
                {
                    Trace.TraceError(e.ToString());
                }
            }
        }

        /// <inheritdoc />
        public async Task RaiseCanExecuteChangedAsync()
        {
            var handler = CanExecuteChanged;
            if (handler != null)
            {
                await UiThreadDispatcher.ExecuteOnUIAsync(() => handler.Invoke(this, EventArgs.Empty)).ConfigureAwait(false);
            }
        }

        /// <inheritdoc />
        public async void RaiseCanExecuteChangedInFireAndForget()
        {
            var handler = CanExecuteChanged;
            if (handler != null)
            {
                try
                {
                    await UiThreadDispatcher.ExecuteOnUIAsync(() => handler.Invoke(this, EventArgs.Empty)).ConfigureAwait(false);
                }
                catch (Exception e)
                {
                    Trace.TraceError(e.ToString());
                }
            }
        }

        /// <inheritdoc />
        public abstract bool CanExecute(object parameter);

        /// <inheritdoc />
        public abstract void Execute(object parameter);

        /// <inheritdoc />
        public event EventHandler CanExecuteChanged;
    }

    /// <inheritdoc cref="DelegateCommandLightBase" />
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

    /// <inheritdoc cref="DelegateCommandLightBase" />
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

        /// <inheritdoc />
        public DelegateCommandLight(Action execute, Func<bool> canExecute = null) : base(canExecute)
        {
            _execute = execute;
        }

        /// <summary>
        /// Watch out, you are trying to use an async delegate in a sync constructor. It leads to a major bug as the
        /// delegate will be downcasted to a sync Action and there will be no way to grab again the await part: everything
        /// after the first real awaitable call will be on its own. Please use the DelegateCommandAsync if you want an
        /// async delegate
        /// </summary>
        [Obsolete(@"WARNING: you are trying to use an async delegate in a sync constructor. It leads to a MAJOR BUG as the
delegate will be downcasted to a sync Action and there will be no way to grab again the await part: everything after the first real awaitable call will be on its own. Please use the DelegateCommandAsync if you want an async delegate")]
        // ReSharper disable once UnusedParameter.Local
        public DelegateCommandLight(Func<Task> executeAsync, Func<bool> canExecute = null) : base(canExecute)
        {
            throw new ArgumentException(
                @"WARNING: you are trying to use an async delegate in a sync constructor. It leads to a MAJOR BUG as the
delegate will be downcasted to a sync Action and there will be no way to grab again the await part: everything after the first real awaitable call will be on its own. Please use the DelegateCommandAsync if you want an async delegate");
        }

        /// <inheritdoc />
        public override void Execute() => _execute();
    }

    /// <inheritdoc />
    public sealed class DelegateCommandLight<T> : DelegateCommandLightWithParameterBase<T>
    {
        private readonly Action<T> _execute;

        /// <inheritdoc />
        public DelegateCommandLight(Action<T> execute, Func<T, bool> canExecute = null) : base(canExecute)
        {
            _execute = execute;
        }

        /// <summary>
        /// Watch out, you are trying to use an async delegate in a sync constructor. It leads to a major bug as the
        /// delegate will be downcasted to a sync Action and there will be no way to grab again the await part: everything
        /// after the first real awaitable call will be on its own. Please use the DelegateCommandAsync if you want an
        /// async delegate
        /// </summary>
        [Obsolete(@"WARNING: you are trying to use an async delegate in a sync constructor. It leads to a MAJOR BUG as the
delegate will be downcasted to a sync Action and there will be no way to grab again the await part: everything after the first real awaitable call will be on its own. Please use the DelegateCommandAsync if you want an async delegate")]
        // ReSharper disable once UnusedParameter.Local
        public DelegateCommandLight(Func<T, Task> executeAsync, Func<T, bool> canExecute = null) : base(canExecute)
        {
            throw new ArgumentException(
                @"WARNING: you are trying to use an async delegate in a sync constructor. It leads to a MAJOR BUG as the
delegate will be downcasted to a sync Action and there will be no way to grab again the await part: everything after the first real awaitable call will be on its own. Please use the DelegateCommandAsync if you want an async delegate");
        }

        /// <inheritdoc />
        public override void Execute(T parameter) => _execute(parameter);
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
            await AsyncWrapperBase.WrapAsync(
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
            await AsyncWrapperBase.WrapAsync(
                    async () => await _executeAsync().ConfigureAwait(false),
                    e => e.ManageErrorOnCommand(_onErrorOnAsync))
                .ConfigureAwait(false);
        }
    }
}