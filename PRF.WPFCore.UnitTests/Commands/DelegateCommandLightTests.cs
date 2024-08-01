using System;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using PRF.WPFCore.Commands;
using Xunit;

namespace PRF.WPFCore.UnitTests.Commands
{
    public sealed class DelegateCommandLightTests
    {
        [Fact]
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
            Assert.Equal(1, count);
        }

        [Fact]
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
            Assert.Equal(1, count);
        }

        [Fact]
        public void DelegateCommand_Parameter_Execute_Nominal()
        {
            //Arrange
            var count = 0;
            var sut = new DelegateCommandLight<int>((i) =>
            {
                Interlocked.Increment(ref count);
                Assert.Equal(42, i);
            });

            //Act
            sut.Execute(42);

            //Assert
            Assert.Equal(1, count);
        }

        [Fact]
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

        [Fact]
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
            await sut.ExecuteAsync().ConfigureAwait(true);

            //Assert
            Assert.Equal(1, count);
        }

        [Fact]
        public async Task DelegateCommandAsync_With_Parameters()
        {
            //Arrange
            var count = 0;
            var sut = new DelegateCommandLightAsync<string>(async (s) =>
            {
                await Task.Delay(5);
                Assert.Equal("foo", s);
                Interlocked.Increment(ref count);
            });

            //Act
            await sut.ExecuteAsync("foo").ConfigureAwait(true);

            //Assert
            Assert.Equal(1, count);
        }

    }
}
