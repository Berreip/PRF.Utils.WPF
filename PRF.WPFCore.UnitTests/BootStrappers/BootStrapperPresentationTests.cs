using System;
using System.Threading;
using System.Windows;
using NUnit.Framework;
using PRF.Utils.Injection.Containers;
using PRF.Utils.Injection.Utils;

namespace PRF.WPFCore.UnitTests.BootStrappers
{
    [TestFixture]
    [Apartment(ApartmentState.STA)]
    internal sealed class BootStrapperPresentationTests
    {
        [Test]
        public void RegisterTests()
        {
            //Configuration
            var sut = new BootForUnitTests();

            //Test
            var res = sut.Run();

            //Verify
            Assert.IsNotNull(res);
        }

        [Test]
        public void VerifyNominal()
        {
            //Configuration
            var sut = new BootForUnitTests();

            //Test
            var res = sut.Verify();

            //Verify
            Assert.IsNull(res);
        }

        [Test]
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
            public MissingRegisterType(IOtherUnknowType _) { }
        }

        private interface IOtherUnknowType { }


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
