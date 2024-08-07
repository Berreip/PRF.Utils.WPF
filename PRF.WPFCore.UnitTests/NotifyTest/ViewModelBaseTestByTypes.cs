﻿using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace PRF.WPFCore.UnitTests.NotifyTest
{
    internal sealed class TestClass : ViewModelBase
    {
        private int _intValue;
        private int? _intNullable;
        private double _doubleValue;
        private double? _doubleNullable;
        private float _floatValue;
        private float? _floatNullable;
        private string _stringValue;
        private object _reference;

        public int Int
        {
            get => _intValue;
            set => SetProperty(ref _intValue, value);
        }

        public int? IntNullable
        {
            get => _intNullable;
            set => SetProperty(ref _intNullable, value);
        }

        public double Double
        {
            get => _doubleValue;
            set => SetProperty(ref _doubleValue, value);
        }

        public double? DoubleNullable
        {
            get => _doubleNullable;
            set => SetProperty(ref _doubleNullable, value);
        }

        public float Float
        {
            get => _floatValue;
            set => SetProperty(ref _floatValue, value);
        }

        public float? FloatNullable
        {
            get => _floatNullable;
            set => SetProperty(ref _floatNullable, value);
        }

        public string String
        {
            get => _stringValue;
            set => SetProperty(ref _stringValue, value);
        }

        public object Reference
        {
            get => _reference;
            set => SetProperty(ref _reference, value);
        }

        public void UpdateEpsilonExternal(double d)
        {
            UpdateEpsilon(d);
        }
    }

    public sealed class ViewModelBaseTestByTypes
    {
        private readonly TestClass _sut;
        private readonly List<string> _raises = new List<string>();

        public ViewModelBaseTestByTypes()
        {
            // software under test:
            _sut = new TestClass();
            _sut.PropertyChanged += (_, args) => _raises.Add(args.PropertyName);
        }

        [Fact]
        public void Int_raise_when_Changed()
        {
            //Configuration

            //Test
            _sut.Int++;

            //Verify
            Assert.Equal(nameof(TestClass.Int), _raises.Single());
        }

        [Fact]
        public void int_do_not_raise_when_not_Changed()
        {
            //Configuration

            //Test
            _sut.Int = _sut.Int;

            //Verify
            Assert.Empty(_raises);
        }
        
        [Fact]
        public void Reference_raise_when_Changed()
        {
            //Configuration
            var obj = new object();

            //Test
            _sut.Reference = obj;

            //Verify
            Assert.Equal(nameof(TestClass.Reference), _raises.Single());
        }

        [Fact]
        public void Reference_do_not_raise_when_not_Changed()
        {
            //Configuration
            var obj = new object();
            _sut.Reference = obj;
            _raises.Clear(); // reset calls count

            //Test
            _sut.Reference = obj;

            //Verify
            Assert.Empty(_raises);
        }  
        
        [Fact]
        public void Reference_raise_when_Changed_to_null()
        {
            //Configuration
            var obj = new object();
            _sut.Reference = obj;
            _raises.Clear(); // reset calls count

            //Test
            _sut.Reference = null;

            //Verify
            Assert.Equal(nameof(TestClass.Reference), _raises.Single());
        }

        [Fact]
        public void Reference_with_EqualsOverride_raise_when_Changed()
        {
            //Configuration
            var obj = new ObjectWithEqualsOverride(1);

            //Test
            _sut.Reference = obj;

            //Verify
            Assert.Equal(nameof(TestClass.Reference), _raises.Single());
        }

        [Fact]
        public void Reference_with_EqualsOverride_do_not_raise_when_not_Changed()
        {
            //Configuration
            _sut.Reference = new ObjectWithEqualsOverride(1);
            _raises.Clear(); // reset calls count

            //Test
            _sut.Reference = new ObjectWithEqualsOverride(1);

            //Verify
            Assert.Empty(_raises);
        }  
        
        [Fact]
        public void Reference_with_EqualsOverride_raise_when_Changed_to_null()
        {
            //Configuration
            _sut.Reference = new ObjectWithEqualsOverride(2);
            _raises.Clear(); // reset calls count

            //Test
            _sut.Reference = null;

            //Verify
            Assert.Equal(nameof(TestClass.Reference), _raises.Single());
        }

        [Fact]
        public void double_do_not_raise_when_not_Changed()
        {
            //Configuration

            //Test
            _sut.Double = _sut.Double;

            //Verify
            Assert.Empty(_raises);
        }

        [Fact]
        public void Double_raise_when_Changed()
        {
            //Configuration

            //Test
            _sut.Double++;

            //Verify
            Assert.Equal(nameof(TestClass.Double), _raises.Single());
        }
        
        [Fact]
        public void Double_do_not_raise_when_not_Changed_from_less_that_epsilon()
        {
            //Configuration

            //Test
            _sut.Double += 0.000000001f;

            //Verify
            Assert.Empty(_raises);
        }

        [Fact]
        public void Double_raise_when_Changed_when_updated_Epsilon()
        {
            //Configuration
            _sut.UpdateEpsilonExternal(0.00000000001d);

            //Test
            _sut.Double += 0.000000001f;

            //Verify
            Assert.Equal(nameof(TestClass.Double), _raises.Single());
        }


        [Fact]
        public void Float_do_not_raise_when_not_Changed()
        {
            //Configuration

            //Test
            _sut.Float = _sut.Float;

            //Verify
            Assert.Empty(_raises);
        }

        [Fact]
        public void Float_raise_when_Changed()
        {
            //Configuration

            //Test
            _sut.Float += 0.01f;

            //Verify
            Assert.Equal(nameof(TestClass.Float), _raises.Single());
        }

        [Fact]
        public void Float_do_not_raise_when_not_Changed_from_less_that_epsilon()
        {
            //Configuration

            //Test
            _sut.Float += 0.000000001f;

            //Verify
            Assert.Empty(_raises);
        }

        [Fact]
        public void Float_raise_when_Changed_when_updated_Epsilon()
        {
            //Configuration
            _sut.UpdateEpsilonExternal(0.00000000001d);

            //Test
            _sut.Float += 0.000000001f;

            //Verify
            Assert.Equal(nameof(TestClass.Float), _raises.Single());
        }

        [Fact]
        public void Int_do_not_raise_when_not_Changed()
        {
            //Configuration

            //Test
            _sut.Int = _sut.Int;

            //Verify
            Assert.Empty(_raises);
        }

        [Fact]
        public void IntNullable_raise_when_Changed_from_null()
        {
            //Configuration

            //Test
            _sut.IntNullable = 45;

            //Verify
            Assert.Equal(nameof(TestClass.IntNullable), _raises.Single());
        }

        [Fact]
        public void IntNullable_raise_when_Changed_from_Value_not_null()
        {
            //Configuration
            _sut.IntNullable = 45;
            _raises.Clear(); // reset calls count

            //Test
            _sut.IntNullable = 46;

            //Verify
            Assert.Equal(nameof(TestClass.IntNullable), _raises.Single());
        }

        [Fact]
        public void IntNullable_raise_when_Changed_from_Value_not_null_To_null()
        {
            //Configuration
            _sut.IntNullable = 45;
            _raises.Clear(); // reset calls count

            //Test
            _sut.IntNullable = null;

            //Verify
            Assert.Equal(nameof(TestClass.IntNullable), _raises.Single());
        }

        [Fact]
        public void IntNullable_do_not_raise_when_not_Changed()
        {
            //Configuration

            //Test
            _sut.IntNullable = _sut.IntNullable;

            //Verify
            Assert.Empty(_raises);
        }
    }

    internal sealed class ObjectWithEqualsOverride
    {
        private readonly int _id;

        public ObjectWithEqualsOverride(int id)
        {
            _id = id;
        }

        private bool Equals(ObjectWithEqualsOverride other)
        {
            return _id == other._id;
        }

        public override bool Equals(object obj)
        {
            return ReferenceEquals(this, obj) || obj is ObjectWithEqualsOverride other && Equals(other);
        }

        public override int GetHashCode()
        {
            return _id;
        }
    }
}