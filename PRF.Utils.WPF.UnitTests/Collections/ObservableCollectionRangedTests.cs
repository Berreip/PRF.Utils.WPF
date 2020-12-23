using NUnit.Framework;
using PRF.Utils.WPF.CustomCollections;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PRF.Utils.WPF.UnitTest.Collections
{
    [TestFixture]
    internal sealed class ObservableCollectionRangedTests
    {
        [Test]
        public void AddRange_Nominal()
        {
            var count = 0;
            //Arrange
            var sut = new ObservableCollectionRanged<object>();
            sut.CollectionChanged += (s, e) =>
            {
                Interlocked.Increment(ref count);
            };

            //Act
            sut.AddRange(Enumerable.Range(0, 10).Select(o => new object()));

            //Assert
            Assert.AreEqual(1, count);
        }

        [Test]
        public void AddRange_Nominal_Twice()
        {
            var count = 0;
            //Arrange
            var sut = new ObservableCollectionRanged<object>();
            sut.CollectionChanged += (s, e) =>
            {
                Interlocked.Increment(ref count);
            };

            //Act
            sut.AddRange(Enumerable.Range(0, 10).Select(o => new object()));
            sut.AddRange(Enumerable.Range(0, 10).Select(o => new object()));

            //Assert
            Assert.AreEqual(2, count);
        }
    }
}
