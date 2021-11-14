using System.Threading;
using NUnit.Framework;

namespace PRF.WPFCore.UnitTests.NotifyTest
{
    public class TestViewModelUncheckedBaseClass : ViewModelBaseUnchecked
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

    [TestFixture]
    internal sealed class ViewModelUncheckedBaseTests
    {
        private TestViewModelUncheckedBaseClass _sut;

        [SetUp]
        public void TestInitialize()
        {
            _sut = new TestViewModelUncheckedBaseClass();
        }

        [Test]
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
            Assert.AreEqual(5, count);
            Assert.AreEqual(4, _sut.Property);
        }

        [Test]
        public void SetProperty_Nominal()
        {
            //Configuration
            var count = 0;
            _sut.PropertyChanged += (_, _) => Interlocked.Increment(ref count);

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
            _sut.PropertyChanged += (_, _) => Interlocked.Increment(ref count);

            //Test
            _sut.Property2 = true;
            _sut.Property2 = true;

            //Verify
            Assert.AreEqual(1, count);
            Assert.IsTrue(_sut.Property2);
        }

        [Test]
        public void SetProperty_Nullable_Nominal()
        {
            //Configuration
            var count = 0;
            _sut.PropertyChanged += (_, _) => Interlocked.Increment(ref count);

            //Test
            _sut.PropertyNullable = "niak";

            //Verify
            Assert.AreEqual(1, count);
            Assert.AreEqual("niak", _sut.PropertyNullable);
        }

        [Test]
        public void SetProperty_Nullable_Both_Null()
        {
            //Configuration
            var count = 0;
            _sut.PropertyChanged += (_, _) => Interlocked.Increment(ref count);

            //Test
            _sut.PropertyNullable = null;

            //Verify
            Assert.AreEqual(0, count); // was null at first
            Assert.AreEqual(null, _sut.PropertyNullable);
        }

        [Test]
        public void SetProperty_Nullable_From_AndTo_Null()
        {
            //Configuration
            var count = 0;
            _sut.PropertyChanged += (_, _) => Interlocked.Increment(ref count);

            //Test
            _sut.PropertyNullable = @"niak";
            _sut.PropertyNullable = null;

            //Verify
            Assert.AreEqual(2, count);
            Assert.AreEqual(null, _sut.PropertyNullable);
        }
        
        [Test]
        public void RaisePropertyChanged_do_not_throw_when_property_name_is_not_a_valid_property_of_the_object()
        {
            //Configuration

            //Test
            _sut.RaiseExternal("stub_prop");

            //Verify
            Assert.Pass();
        }

    }
}
