using System;
using System.Globalization;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using PRF.Utils.WPF.Converters;

namespace PRF.Utils.WPF.UnitTest.Converters
{
    [TestClass]
    public class InvertBoolConverterTest
    {
        private InvertBoolConverter _instance;

        [TestInitialize]
        public void TestInitialize()
        {
            // mock:
            
            // instance de test:
            _instance = new InvertBoolConverter();
        }
        
        [TestMethod]
        public void ConvertV1()
        {
            //Configuration

            //Test
            var res = _instance.Convert(true, typeof(bool), null, It.IsAny<CultureInfo>());

            //Verify
            Assert.IsNotNull(res);
            Assert.IsFalse((bool)res);
        }

        [TestMethod]
        public void ConvertV2()
        {
            //Configuration

            //Test
            var res = _instance.Convert(false, typeof(bool), null, It.IsAny<CultureInfo>());

            //Verify
            Assert.IsNotNull(res);
            Assert.IsTrue((bool)res);
        }

        [TestMethod]
        public void ConvertBackV1()
        {
            //Configuration

            //Test
            var res = _instance.ConvertBack(true, typeof(bool), null, It.IsAny<CultureInfo>());

            //Verify
            Assert.IsNotNull(res);
            Assert.IsFalse((bool)res);
        }

        [TestMethod]
        public void ConvertBackV2()
        {
            //Configuration

            //Test
            var res = _instance.ConvertBack(false, typeof(bool), null, It.IsAny<CultureInfo>());

            //Verify
            Assert.IsNotNull(res);
            Assert.IsTrue((bool)res);
        }

        [ExpectedException(typeof(InvalidCastException))]
        [TestMethod]
        public void ConvertNotABool()
        {
            //Configuration

            //Test
            var _ = _instance.Convert(45, typeof(bool), null, It.IsAny<CultureInfo>());

            //Verify
            Assert.Fail("should have raised an InvalidCastException");
        }

        [ExpectedException(typeof(InvalidCastException))]
        [TestMethod]
        public void ConvertBackNotABool()
        {
            //Configuration

            //Test
            var _ = _instance.ConvertBack(45, typeof(bool), null, It.IsAny<CultureInfo>());

            //Verify
            Assert.Fail("should have raised an InvalidCastException");
        }

    }
}
