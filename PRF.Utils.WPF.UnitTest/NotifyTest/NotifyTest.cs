using System;
using System.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;

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
    }

    [TestClass]
    public class NotifyTest
    {
        private TestNotifierBaseClass _instance;

        [TestInitialize]
        public void TestInitialize()
        {
            // mock:

            // instance de test:
            _instance = new TestNotifierBaseClass();
        }

        /// <summary>
        /// Cas 1: test performance
        /// </summary>
        [TestMethod]
        public void NotifyV1()
        {
            //Configuration
            const int upper = 5_000_000;

            //Test
            var watch = Stopwatch.StartNew();
            for (int i = 0; i < upper; i++)
            {
                _instance.Property++;
            }
            watch.Stop();

            //Verify
            Assert.IsTrue(watch.Elapsed < TimeSpan.FromMilliseconds(500), $"trop long: {watch.ElapsedMilliseconds} ms");
            
        }


    }
}
