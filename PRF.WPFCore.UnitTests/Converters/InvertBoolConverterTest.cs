using System;
using System.Globalization;
using Moq;
using NUnit.Framework;
using PRF.WPFCore.Converters;

namespace PRF.WPFCore.UnitTests.Converters
{
    [TestFixture]
    public class InvertBoolConverterTest
    {
        private InvertBoolConverter _instance;

        [SetUp]
        public void TestInitialize()
        {
            // mock:
            
            // instance de test:
            _instance = new InvertBoolConverter();
        }
        
        [Test]
        public void ConvertV1()
        {
            //Configuration

            //Test
            var res = _instance.Convert(true, typeof(bool), null, It.IsAny<CultureInfo>());

            //Verify
            Assert.IsNotNull(res);
            Assert.IsFalse((bool)res);
        }

        [Test]
        public void ConvertV2()
        {
            //Configuration

            //Test
            var res = _instance.Convert(false, typeof(bool), null, It.IsAny<CultureInfo>());

            //Verify
            Assert.IsNotNull(res);
            Assert.IsTrue((bool)res);
        }

        [Test]
        public void ConvertBackV1()
        {
            //Configuration

            //Test
            var res = _instance.ConvertBack(true, typeof(bool), null, It.IsAny<CultureInfo>());

            //Verify
            Assert.IsNotNull(res);
            Assert.IsFalse((bool)res);
        }

        [Test]
        public void ConvertBackV2()
        {
            //Configuration

            //Test
            var res = _instance.ConvertBack(false, typeof(bool), null, It.IsAny<CultureInfo>());

            //Verify
            Assert.IsNotNull(res);
            Assert.IsTrue((bool)res);
        }

        [Test]
        public void ConvertNotABool()
        {
            //Configuration

            //Test
            Assert.Throws<InvalidCastException>(() => _instance.Convert(45, typeof(bool), null, It.IsAny<CultureInfo>()));

            //Verify
        }

        [Test]
        public void ConvertBackNotABool()
        {
            //Configuration

            //Test
            Assert.Throws<InvalidCastException>(() => _instance.ConvertBack(45, typeof(bool), null, It.IsAny<CultureInfo>()));

            //Verify
        }

    }
}
