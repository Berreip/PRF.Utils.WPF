using System;

// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable InheritdocConsiderUsage
// ReSharper disable AutoPropertyCanBeMadeGetOnly.Local

namespace PRF.Utils.WPF.Commands
{
    /// <inheritdoc />
    public class DelegateCommandLight : DelegateCommandLightBase
    {
        private readonly Action _execute;
        private readonly Func<bool> _canExecute;

        /// <inheritdoc />
        public DelegateCommandLight(Action execute, Func<bool> canExecute = null)
        {
            _execute = execute;
            _canExecute = canExecute;
        }

        /// <inheritdoc />
        public override bool CanExecute(object parameter)
        {
            return _canExecute == null || _canExecute();
        }

        /// <inheritdoc />
        public override void Execute(object parameter)
        {
            _execute();
        }
    }

    /// <inheritdoc />
    public class DelegateCommandLight<T> : DelegateCommandLightBase
    {
        private readonly Action<T> _execute;
        private readonly Func<T, bool> _canExecute;

        /// <inheritdoc />
        public DelegateCommandLight(Action<T> execute, Func<T, bool> canExecute = null)
        {
            _execute = execute;
            _canExecute = canExecute;
        }

        /// <inheritdoc />
        public override bool CanExecute(object parameter)
        {
            return _canExecute == null || _canExecute(CheckCastParameter(parameter));
        }

        /// <inheritdoc />
        public override void Execute(object parameter)
        {
            _execute(CheckCastParameter(parameter));
        }

        private static T CheckCastParameter(object parameter)
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
    }
}
