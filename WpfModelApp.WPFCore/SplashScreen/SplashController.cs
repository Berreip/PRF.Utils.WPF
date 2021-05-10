using System;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using PRF.WPFCore.BootStrappers;
using PRF.WPFCore.UiWorkerThread;

namespace WpfModelApp.WPFCore.SplashScreen
{
        public sealed class SplashController : ISplashController
        {
            /// <inheritdoc />
            public event Action<string> OnUpdateMessage;

            /// <inheritdoc />
            public event Action OnLoadingDoneSuccessfully;

            /// <inheritdoc />
            public event Action<Exception> OnLoadingFailed;

            private void RaiseOnUpdateMessage(string obj)
            {
                OnUpdateMessage?.Invoke(obj);
            }

            private void RaiseOnLoadingDoneSuccessfully()
            {
                OnLoadingDoneSuccessfully?.Invoke();
            }

            public async void OnStart(object sender, StartupEventArgs e)
            {
                try
                {
                    await Task.Run(async () =>
                    {
                        foreach (var i in Enumerable.Range(0, 50))
                        {
                            await Task.Delay(100).ConfigureAwait(true);
                            RaiseOnUpdateMessage($"iteration: {i}");
                            await Task.Delay(100).ConfigureAwait(true);
                            await WrapperCoreUI.DispatchAsync(() =>
                            {
                                RaiseOnUpdateMessage($"iteration inside UI: {i}");
                            });
                        }
                        // 
                        await WrapperCoreUI.DispatchAsync(RaiseOnLoadingDoneSuccessfully);
                    }).ConfigureAwait(false);
                }
                catch (Exception ex)
                {
                    await WrapperCoreUI.DispatchAsync(() => RaiseOnLoadingFailed(ex));
                }
            }

            private void RaiseOnLoadingFailed(Exception obj)
            {
                OnLoadingFailed?.Invoke(obj);
            }
        }
}