using System;
using System.Windows;
using PRF.Utils.Injection.Containers;
using PRF.Utils.Injection.Utils;
using Xunit;
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable ClassNeverInstantiated.Local

namespace PRF.WPFCore.UnitTests.BootStrappers
{
    public sealed class BootStrapperPresentationTests
    {
        [WpfFact]
        public void RegisterTests()
        {
            //Configuration
            var sut = new BootForUnitTests();

            //Test
            var res = sut.Run();

            //Verify
            Assert.NotNull(res);
        }

        [WpfFact]
        public void VerifyNominal()
        {
            //Configuration
            var sut = new BootForUnitTests();

            //Test
            var res = sut.Verify();

            //Verify
            Assert.Null(res);
        }

        [WpfFact]
        public void Verify_Issues()
        {
            //Configuration
            var sut = new BootForUnitTests(c =>
            {
                c.RegisterType<MissingRegisterType>(LifeTime.Singleton);
            });

            //Test
            Assert.Throws<InvalidOperationException>(() => sut.Verify());

            //Verify
        }

        private class MissingRegisterType
        {
            public MissingRegisterType(IOtherUnknownType _) { }
        }

        private interface IOtherUnknownType { }


        private sealed class BootForUnitTests : WPFCore.BootStrappers.BootStrapperPresentation<ViewUnitTest, ViewModelUnitTest>
        {
            private readonly Action<IInjectionContainerRegister>[] _additionalRegister;

            public BootForUnitTests(params Action<IInjectionContainerRegister>[] additionalRegister)
            {
                _additionalRegister = additionalRegister;
            }

            public int UnRegisteredRaised { get; private set; }

            protected override void ResolveUnregisteredType(object sender, Type type)
            {
                UnRegisteredRaised += 1;
            }


            protected override void Register(IInjectionContainerRegister container)
            {
                container.Register<IUnitTestRegisterType1, UnitTestRegisterType1>(LifeTime.Singleton);
                foreach (var action in _additionalRegister)
                {
                    action.Invoke(container);
                }
            }
        }

        internal interface IUnitTestRegisterType1
        {
        }

        internal class UnitTestRegisterType1 : IUnitTestRegisterType1
        {
        }


        private class ViewModelUnitTest : ViewModelBase
        {
        }

        private class ViewUnitTest : Window
        {
        }
    }


}
