using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using NUnit.Framework;
using PRF.WPFCore.CustomCollections;

namespace PRF.WPFCore.UnitTests.Collections
{
    [TestFixture]
    internal sealed class ObservableCollectionRangedTests
    {
        private ObservableCollectionRanged<object> _sut;
        private int _count;

        [SetUp]
        public void TestInitialize()
        {
            // mock:
            _count = 0;

            // software under test:
            _sut = new ObservableCollectionRanged<object>();
            _sut.CollectionChanged += (_, _) => Interlocked.Increment(ref _count);
        }


        [Test]
        public void AddRange_Nominal()
        {
            //Arrange

            //Act
            _sut.AddRange(Enumerable.Range(0, 10).Select(o => new object()));

            //Assert
            Assert.AreEqual(1, _count);
        }

        [Test]
        public void AddRange_Nominal_Twice()
        {
            //Arrange

            //Act
            _sut.AddRange(Enumerable.Range(0, 10).Select(o => new object()));
            _sut.AddRange(Enumerable.Range(0, 10).Select(o => new object()));

            //Assert
            Assert.AreEqual(2, _count);
        }


        [Test]
        public void Add_MultiThreaded()
        {
            //Arrange

            //Act
            Parallel.For(0, 10_000, i => { _sut.Add(new object()); });

            //Assert
            Assert.AreEqual(10_000, _count);
        }


        [Test]
        public void AddRangeDifferential_Nominal_Object_Reference()
        {
            //Arrange
            var sourceObject = Enumerable.Range(0, 5).Select(_ => new object()).ToArray();
            _sut.AddRange(sourceObject);

            //Act
            var newSource = new[]
            {
                sourceObject[1], // An old reference
                new object(), // and new ones
                new object()
            };
            
            _sut.AddRangeDifferential(newSource);

            //Assert
            Assert.AreEqual(2, _count);
            Assert.AreEqual(3, _sut.Count);
            Assert.AreSame(sourceObject[1], _sut[0]); // the old reference has been kept inchanged
            Assert.AreSame(newSource[1], _sut[1]);
            Assert.AreSame(newSource[2], _sut[2]);
        }
        
        [Test]
        public void AddRangeDifferential_Nominal_Content()
        {
            //Arrange
            var sut = new ObservableCollectionRanged<int>(new[] {1, 2, 3});

            //Act
            sut.AddRangeDifferential(new[] {3, 4, 5, 6});

            //Assert
            Assert.AreEqual(4, sut.Count);
            Assert.AreEqual(3, sut[0]);
            Assert.AreEqual(4, sut[1]);
            Assert.AreEqual(5, sut[2]);
            Assert.AreEqual(6, sut[3]);
            
        }
    }
}