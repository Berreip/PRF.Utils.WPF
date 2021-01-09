using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using NUnit.Framework;
using PRF.Utils.WPF.UiWorkerThread;

namespace PRF.Utils.WPF.UnitTests.UiWorkerThread
{
    [TestFixture]
    internal sealed class UiThreadDispatcherTests
    { 
        [Test]
        public async Task ExecuteOnUI_With_Task()
        {
            //Configuration
            var counter = 0;

            //Test
            await UiThreadDispatcher.ExecuteOnUI(async () =>
            {
                await Task.Delay(50);
                counter = 1;
            });

            //Verify
            Assert.AreEqual(1, counter);
        }

        [Test]
        public void ExecuteOnUI_Basic()
        {
            //Configuration
            var counter = 0;

            //Test
            UiThreadDispatcher.ExecuteOnUI(() =>
            {
                Thread.Sleep(50);
                counter = 1;
            });

            //Verify
            Assert.AreEqual(1, counter);
        }

        [Test]
        public async Task ExecuteOnUI_Task_Return()
        {
            //Configuration
            var counter = 0;

            //Test
            var res = await UiThreadDispatcher.ExecuteOnUI(async () =>
            {
                await Task.Delay(50);
                counter = 1;
                return 4;
            });

            //Verify
            Assert.AreEqual(4, res);
            Assert.AreEqual(1, counter);
        }

        [Test]
        public void ExecuteOnUI_Return()
        {
            //Configuration
            var counter = 0;

            //Test
            var res = UiThreadDispatcher.ExecuteOnUI(() =>
            {
                counter = 1;
                return 4;
            });

            //Verify
            Assert.AreEqual(4, res);
            Assert.AreEqual(1, counter);
        }

        [Test]
        public async Task ExecuteOnUIAsync()
        {
            //Configuration
            var counter = 0;

            //Test
            await UiThreadDispatcher.ExecuteOnUIAsync(async () =>
            {
                await Task.Delay(50);
                counter = 1;
            });

            //Verify
            Assert.AreEqual(1, counter);
        }

        [Test]
        public async Task ExecuteOnUIAsync_Basic_Return()
        {
            //Configuration
            var counter = 0;

            //Test
            var res = await UiThreadDispatcher.ExecuteOnUIAsync(() =>
            {
                Thread.Sleep(50);
                counter = 1;
                return 4;
            });

            //Verify
            Assert.AreEqual(4, res);
            Assert.AreEqual(1, counter);
        }


        [Test]
        public async Task ExecuteOnUIAsync_Return_Task()
        {
            //Configuration
            var counter = 0;

            //Test
            var res = await UiThreadDispatcher.ExecuteOnUIAsync(async () =>
            {
                await Task.Delay(50);
                counter = 1;
                return 4;
            });

            //Verify
            Assert.AreEqual(4, res);
            Assert.AreEqual(1, counter);
        }

        [Test]
        public async Task ExecuteOnUIAsync_But_Sync_Action()
        {
            //Configuration
            var counter = 0;

            //Test
            await UiThreadDispatcher.ExecuteOnUIAsync(() =>
            {
                Thread.Sleep(50);
                counter = 1;
            });

            //Verify
            Assert.AreEqual(1, counter);

        }
    }
}
