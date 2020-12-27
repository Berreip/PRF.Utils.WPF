using System;
using System.Diagnostics;
using System.Threading;
using NUnit.Framework;

namespace PRF.Utils.WPF.UnitTest.NotifyTest
{
    public class TestNotifierBaseClass : NotifierBase
    {
        private int _property;

        public int Property
        {
            get => _property;
            set
            {
                _property = value;
                Notify();
            }
        }


        private bool _property2;

        public bool Property2
        {
            get => _property2;
            set
            {
                SetProperty(ref _property2, value);
            }
        }
    }

    [TestFixture]
    public class NotifyTest
    {
        private TestNotifierBaseClass _sut;

        [SetUp]
        public void TestInitialize()
        {
            // mock:

            // instance de test:
            _sut = new TestNotifierBaseClass();
        }

        [Test]
        public void Notify_Nominal()
        {
            //Configuration
            var count = 0;
            _sut.PropertyChanged += (s, p) => Interlocked.Increment(ref count);

            //Test
            for (int i = 0; i < 5; i++)
            {
                _sut.Property = i;
            }

            //Verify
            Assert.AreEqual(5, count);
            Assert.AreEqual(4, _sut.Property);
        }

        [Test]
        public void SetProperty_Nominal()
        {
            //Configuration
            var count = 0;
            _sut.PropertyChanged += (s, p) => Interlocked.Increment(ref count);

            //Test
            _sut.Property2 = true;

            //Verify
            Assert.AreEqual(1, count);
            Assert.IsTrue(_sut.Property2);
        }

        [Test]
        public void SetProperty_Nominal_Multiple()
        {
            //Configuration
            var count = 0;
            _sut.PropertyChanged += (s, p) => Interlocked.Increment(ref count);

            //Test
            _sut.Property2 = true;
            _sut.Property2 = true;

            //Verify
            Assert.AreEqual(1, count);
            Assert.IsTrue(_sut.Property2);
        }


    }
}
