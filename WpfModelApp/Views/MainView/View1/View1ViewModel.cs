using System;
using System.ComponentModel;
using System.Linq;
using PRF.Utils.WPF;
using PRF.Utils.WPF.Commands;
using PRF.Utils.WPF.CustomCollections;
using PRF.Utils.WPF.Helpers;

namespace WpfModelApp.Views.MainView.View1
{
    // ReSharper disable once ClassNeverInstantiated.Global
    internal class View1ViewModel : NotifierBase
    {
        private bool _isRunning;
        private readonly ObservableCollectionRanged<Guid> _backingCollection;

        public IDelegateCommandLight StartAddCommand { get; }
        public IDelegateCommandLight ResetCommand { get; }
        public IDelegateCommandLight StartAddRangeCommand { get; }

        public ICollectionView Collection { get; }

        public View1ViewModel()
        {
            StartAddCommand = new DelegateCommandLight(ExecuteStartAddCommand, CanExecuteStartAddCommand);
            ResetCommand = new DelegateCommandLight(ExecuteResetCommand, CanExecuteResetCommand);
            StartAddRangeCommand = new DelegateCommandLight(ExecuteStartAddRangeCommand, CanStartAddRangeCommand);
            Collection = ObservableCollectionSource.GetDefaultView(out _backingCollection);

            Collection.SortDescriptions.Add(new SortDescription());
        }

        private bool CanStartAddRangeCommand() => !IsRunning;
        private bool CanExecuteResetCommand() => !IsRunning;
        private bool CanExecuteStartAddCommand() => !IsRunning;

        private async void ExecuteStartAddRangeCommand()
        {
            IsRunning = true;
            var limit = Limit;
            await WrapperCoreMessageBox.DispatchAndWrapAsync(() => _backingCollection.AddRange(Enumerable.Range(0, limit).Select(o => Guid.NewGuid())), () => IsRunning = false);
        }

        private async void ExecuteResetCommand()
        {
            IsRunning = true;
            await WrapperCoreMessageBox.DispatchAndWrapAsync(() => _backingCollection.Clear(), () => IsRunning = false);
        }

        private async void ExecuteStartAddCommand()
        {
            IsRunning = true;
            var limit = Limit;
            await WrapperCoreMessageBox.DispatchAndWrapAsync(() =>
            {
                for (int i = 0; i < limit; i++)
                {
                    _backingCollection.Add(Guid.NewGuid());
                }
            }, () => IsRunning = false);
        }

        public bool IsRunning
        {
            get => _isRunning;
            set
            {
                if (_isRunning == value) return;
                _isRunning = value;
                Notify();
                ResetCommand.RaiseCanExecuteChanged();
                StartAddCommand.RaiseCanExecuteChanged();
                StartAddRangeCommand.RaiseCanExecuteChanged();
            }
        }

        public int Limit { get; set; } = 10000;
    }
}
