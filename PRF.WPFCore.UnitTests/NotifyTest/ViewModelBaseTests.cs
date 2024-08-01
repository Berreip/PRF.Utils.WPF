using System;
using System.Threading;
using Xunit;

namespace PRF.WPFCore.UnitTests.NotifyTest
{
    public class TestViewModelBaseClass : ViewModelBase
    {
        private int _property;

        public int Property
        {
            get => _property;
            set
            {
                _property = value;
                RaisePropertyChanged();
            }
        }


        private bool _property2;

        public bool Property2
        {
            get => _property2;
            set => SetProperty(ref _property2, value);
        }


        private string _propertyNullable;

        public string PropertyNullable
        {
            get => _propertyNullable;
            set => SetProperty(ref _propertyNullable, value);
        }

        public void RaiseExternal(string strToraise)
        {
            RaisePropertyChanged(strToraise);
        }
    }

    public sealed class ViewModelBaseTests
    {
        private readonly TestViewModelBaseClass _sut = new TestViewModelBaseClass();

        [Fact]
        public void Notify_Nominal()
        {
            //Configuration
            var count = 0;
            _sut.PropertyChanged += (_, _) => Interlocked.Increment(ref count);

            //Test
            for (var i = 0; i < 5; i++)
            {
                _sut.Property = i;
            }

            //Verify
            Assert.Equal(5, count);
            Assert.Equal(4, _sut.Property);
        }

        [Fact]
        public void SetProperty_Nominal()
        {
            //Configuration
            var count = 0;
            _sut.PropertyChanged += (_, _) => Interlocked.Increment(ref count);

            //Test
            _sut.Property2 = true;

            //Verify
            Assert.Equal(1, count);
            Assert.True(_sut.Property2);
        }

        [Fact]
        public void SetProperty_Nominal_Multiple()
        {
            //Configuration
            var count = 0;
            _sut.PropertyChanged += (_, _) => Interlocked.Increment(ref count);

            //Test
            _sut.Property2 = true;
            _sut.Property2 = true;

            //Verify
            Assert.Equal(1, count);
            Assert.True(_sut.Property2);
        }

        [Fact]
        public void SetProperty_Nullable_Nominal()
        {
            //Configuration
            var count = 0;
            _sut.PropertyChanged += (_, _) => Interlocked.Increment(ref count);

            //Test
            _sut.PropertyNullable = "niak";

            //Verify
            Assert.Equal(1, count);
            Assert.Equal("niak", _sut.PropertyNullable);
        }

        [Fact]
        public void SetProperty_Nullable_Both_Null()
        {
            //Configuration
            var count = 0;
            _sut.PropertyChanged += (_, _) => Interlocked.Increment(ref count);

            //Test
            _sut.PropertyNullable = null;

            //Verify
            Assert.Equal(0, count); // was null at first
            Assert.Null(_sut.PropertyNullable);
        }

        [Fact]
        public void SetProperty_Nullable_From_AndTo_Null()
        {
            //Configuration
            var count = 0;
            _sut.PropertyChanged += (_, _) => Interlocked.Increment(ref count);

            //Test
            _sut.PropertyNullable = @"niak";
            _sut.PropertyNullable = null;

            //Verify
            Assert.Equal(2, count);
            Assert.Null(_sut.PropertyNullable);
        }

        [Fact]
        public void RaisePropertyChanged_throw_when_property_name_is_not_a_valid_property_of_the_object()
        {
            //Configuration

            //Test
            Assert.Throws<InvalidOperationException>(() => _sut.RaiseExternal("stub_prop"));

            //Verify
        }
    }
}