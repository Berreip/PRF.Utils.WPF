using System;
using System.Threading;
using System.Threading.Tasks;
using NUnit.Framework;
using PRF.WPFCore.UiWorkerThread;

namespace PRF.WPFCore.UnitTests.UiWorkerThread
{
    [TestFixture]
    [Apartment(ApartmentState.STA)]
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

        [Test]
        public void ExecuteOnUI_forward_exception()
        {
            //Configuration

            //Test
            Assert.ThrowsAsync<ArithmeticException>(async () =>
            {
                await UiThreadDispatcher.ExecuteOnUI(async () =>
                {
                    await Task.Delay(50).ConfigureAwait(false);
                    throw new ArithmeticException("misc");
                }).ConfigureAwait(false);
            });

            //Verify
        }
    }
}