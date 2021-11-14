﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows;
using NUnit.Framework;
using PRF.WPFCore.Converters;
using PRF.WPFCore.CustomCollections;

namespace PRF.WPFCore.UnitTests.Converters
{
    [TestFixture]
    public class EmptyCollectionToVisibleConverterTests
    {
        private EmptyCollectionToVisibleConverter _sut;

        [SetUp]
        public void TestInitialize()
        {
            _sut = new EmptyCollectionToVisibleConverter();
        }

        [Test]
        public void Convert_returns_Visible_when_array_is_empty()
        {
            //Configuration
            var collection = new string[] { };

            //Test
            // ReSharper disable once PossibleNullReferenceException
            var res = (Visibility) _sut.Convert(collection, typeof(object), null, CultureInfo.CurrentCulture);

            //Verify
            Assert.AreEqual(Visibility.Visible, res);
        }


        [Test]
        public void Convert_returns_Visible_when_list_is_empty()
        {
            //Configuration
            var collection = new List<string>();

            //Test
            // ReSharper disable once PossibleNullReferenceException
            var res = (Visibility) _sut.Convert(collection, typeof(object), null, CultureInfo.CurrentCulture);

            //Verify
            Assert.AreEqual(Visibility.Visible, res);
        }

        [Test]
        public void Convert_returns_Visible_when_ListCollectionView_is_empty()
        {
            //Configuration
            var collection = ObservableCollectionSource.GetDefaultView(new string[] { });

            //Test
            // ReSharper disable once PossibleNullReferenceException
            var res = (Visibility) _sut.Convert(collection, typeof(object), null, CultureInfo.CurrentCulture);

            //Verify
            Assert.AreEqual(Visibility.Visible, res);
        }

        [Test]
        public void Convert_returns_Collapsed_when_array_is_not_empty()
        {
            //Configuration
            var collection = new[] {"foo"};

            //Test
            // ReSharper disable once PossibleNullReferenceException
            var res = (Visibility) _sut.Convert(collection, typeof(object), null, CultureInfo.CurrentCulture);

            //Verify
            Assert.AreEqual(Visibility.Collapsed, res);
        }


        [Test]
        public void Convert_returns_Collapsed_when_list_is_not_empty()
        {
            //Configuration
            var collection = new List<string> {"foo"};

            //Test
            // ReSharper disable once PossibleNullReferenceException
            var res = (Visibility) _sut.Convert(collection, typeof(object), null, CultureInfo.CurrentCulture);

            //Verify
            Assert.AreEqual(Visibility.Collapsed, res);
        }

        [Test]
        public void Convert_returns_Collapsed_when_ListCollectionView_is_not_empty()
        {
            //Configuration
            var collection = ObservableCollectionSource.GetDefaultView(new[] {"foo"});

            //Test
            // ReSharper disable once PossibleNullReferenceException
            var res = (Visibility) _sut.Convert(collection, typeof(object), null, CultureInfo.CurrentCulture);

            //Verify
            Assert.AreEqual(Visibility.Collapsed, res);
        }
        
        
        [Test]
        public void Convert_returns_Collapsed_when_list_is_null()
        {
            //Configuration

            //Test
            // ReSharper disable once PossibleNullReferenceException
            var res = (Visibility) _sut.Convert(null, typeof(object), null, CultureInfo.CurrentCulture);

            //Verify
            Assert.AreEqual(Visibility.Collapsed, res);
        }
        
        [Test]
        public void Convert_returns_Visible_if_not_a_list()
        {
            //Configuration
            var collection = 67;

            //Test
            // ReSharper disable once PossibleNullReferenceException
            var res = (Visibility) _sut.Convert(collection, typeof(object), null, CultureInfo.CurrentCulture);

            //Verify
            Assert.AreEqual(Visibility.Visible, res);
        }
        
        [Test]
        public void ConvertBack_throw_NotSupportedException()
        {
            //Configuration

            //Test
            Assert.Throws<NotSupportedException>(() => _sut.ConvertBack(new object(), typeof(object), null, CultureInfo.CurrentCulture));

            //Verify
        }

    }
}