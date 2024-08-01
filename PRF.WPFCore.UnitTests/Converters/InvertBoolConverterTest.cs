using System;
using System.Globalization;
using Moq;
using PRF.WPFCore.Converters;
using Xunit;

namespace PRF.WPFCore.UnitTests.Converters
{
    public class InvertBoolConverterTest
    {
        private readonly InvertBoolConverter _instance = new InvertBoolConverter();

        [Fact]
        public void ConvertV1()
        {
            //Configuration

            //Test
            var res = _instance.Convert(true, typeof(bool), null, It.IsAny<CultureInfo>());

            //Verify
            Assert.NotNull(res);
            Assert.False((bool)res);
        }

        [Fact]
        public void ConvertV2()
        {
            //Configuration

            //Test
            var res = _instance.Convert(false, typeof(bool), null, It.IsAny<CultureInfo>());

            //Verify
            Assert.NotNull(res);
            Assert.True((bool)res);
        }

        [Fact]
        public void ConvertBackV1()
        {
            //Configuration

            //Test
            var res = _instance.ConvertBack(true, typeof(bool), null, It.IsAny<CultureInfo>());

            //Verify
            Assert.NotNull(res);
            Assert.False((bool)res);
        }

        [Fact]
        public void ConvertBackV2()
        {
            //Configuration

            //Test
            var res = _instance.ConvertBack(false, typeof(bool), null, It.IsAny<CultureInfo>());

            //Verify
            Assert.NotNull(res);
            Assert.True((bool)res);
        }

        [Fact]
        public void ConvertNotABool()
        {
            //Configuration

            //Test
            Assert.Throws<InvalidCastException>(() => _instance.Convert(45, typeof(bool), null, It.IsAny<CultureInfo>()));

            //Verify
        }

        [Fact]
        public void ConvertBackNotABool()
        {
            //Configuration

            //Test
            Assert.Throws<InvalidCastException>(() => _instance.ConvertBack(45, typeof(bool), null, It.IsAny<CultureInfo>()));

            //Verify
        }

    }
}
