using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using PRF.WPFCore.CustomCollections;
using Xunit;

namespace PRF.WPFCore.UnitTests.Collections
{
    public sealed class ObservableCollectionRangedTests
    {
        private readonly ObservableCollectionRanged<object> _sut;
        private int _count;

        public ObservableCollectionRangedTests()
        {
            // software under test:
            _sut = new ObservableCollectionRanged<object>();
            _sut.CollectionChanged += (_, _) => Interlocked.Increment(ref _count);
        }


        [Fact]
        public void AddRange_Nominal()
        {
            //Arrange

            //Act
            _sut.AddRange(Enumerable.Range(0, 10).Select(o => new object()));

            //Assert
            Assert.Equal(1, _count);
        }

        [Fact]
        public void AddRange_Nominal_Twice()
        {
            //Arrange

            //Act
            _sut.AddRange(Enumerable.Range(0, 10).Select(o => new object()));
            _sut.AddRange(Enumerable.Range(0, 10).Select(o => new object()));

            //Assert
            Assert.Equal(2, _count);
        }


        [Fact]
        public void Add_MultiThreaded()
        {
            //Arrange

            //Act
            Parallel.For(0, 10_000, i => { _sut.Add(new object()); });

            //Assert
            Assert.Equal(10_000, _count);
        }


        [Fact]
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
            Assert.Equal(2, _count);
            Assert.Equal(3, _sut.Count);
            Assert.Same(sourceObject[1], _sut[0]); // the old reference has been kept unchanged
            Assert.Same(newSource[1], _sut[1]);
            Assert.Same(newSource[2], _sut[2]);
        }
        
        [Fact]
        public void AddRangeDifferential_Nominal_Content()
        {
            //Arrange
            var sut = new ObservableCollectionRanged<int>(new[] {1, 2, 3});

            //Act
            sut.AddRangeDifferential(new[] {3, 4, 5, 6});

            //Assert
            Assert.Equal(4, sut.Count);
            Assert.Equal(3, sut[0]);
            Assert.Equal(4, sut[1]);
            Assert.Equal(5, sut[2]);
            Assert.Equal(6, sut[3]);
            
        }
    }
}