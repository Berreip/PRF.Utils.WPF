using System;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using PRF.WPFCore.Commands;

namespace PRF.WPFCore.UnitTests.Commands
{
    [TestFixture]
    internal sealed class DelegateCommandLightTests
    {
        [Test]
        public void DelegateCommand_Execute_Nominal()
        {
            //Arrange
            var count = 0;
            var sut = new DelegateCommandLight(() =>
            {
                Interlocked.Increment(ref count);
            });

            //Act
            sut.Execute();

            //Assert
            Assert.AreEqual(1, count);
        }

        [Test]
        public void DelegateCommand_Execute_Nominal_Ignore_Parameter()
        {
            //Arrange
            var count = 0;
            var sut = new DelegateCommandLight(() =>
            {
                Interlocked.Increment(ref count);
            });

            //Act
            sut.Execute(It.IsAny<object>());

            //Assert
            Assert.AreEqual(1, count);
        }

        [Test]
        public void DelegateCommand_Parameter_Execute_Nominal()
        {
            //Arrange
            var count = 0;
            var sut = new DelegateCommandLight<int>((i) =>
            {
                Interlocked.Increment(ref count);
                Assert.AreEqual(42, i);
            });

            //Act
            sut.Execute(42);

            //Assert
            Assert.AreEqual(1, count);
        }

        [Test]
        public void DelegateCommand_Parameter_Execute_With_Async_Send_exception_in_ctor()
        {
            //Arrange

            //Act
#pragma warning disable CS0618
            Assert.Throws<ArgumentException>(() =>
            {
                var _ = new DelegateCommandLight(async () =>
                {
                    await Task.Delay(5);
                    throw new Exception();
                });
            });
#pragma warning restore CS0618
            
            //Assert
        }

        [Test]
        public async Task DelegateCommandAsync_Without_Parameters()
        {
            //Arrange
            var count = 0;
            var sut = new DelegateCommandLightAsync(async () =>
            {
                await Task.Delay(5);
                Interlocked.Increment(ref count);
            });

            //Act
            await sut.ExecuteAsync().ConfigureAwait(false);

            //Assert
            Assert.AreEqual(1, count);
        }

        [Test]
        public async Task DelegateCommandAsync_With_Parameters()
        {
            //Arrange
            var count = 0;
            var sut = new DelegateCommandLightAsync<string>(async (s) =>
            {
                await Task.Delay(5);
                Assert.AreEqual("foo", s);
                Interlocked.Increment(ref count);
            });

            //Act
            await sut.ExecuteAsync("foo").ConfigureAwait(false);

            //Assert
            Assert.AreEqual(1, count);
        }

    }
}
