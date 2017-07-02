// Copyright (c) Alium FX. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Newtonsoft.Json.Bson;

namespace Alium
{
    using System;
    using Xunit;

    /// <summary>
    /// Provides tests for the <see cref="DefaultObjectComparer{TObject}"/> type.
    /// </summary>
    public class DefaultObjectComparerTests
    {
        [Fact]
        public void Equals_ReturnsTrue_ForSameInstance()
        {
            // Arrange
            var person = new Person();
            var comparer = new DefaultObjectComparer<Person>();

            // Act
            bool equals = comparer.Equals(person, person);

            // Assert
            Assert.True(equals);
        }

        [Fact]
        public void Equals_ReturnsTrue_ForNullInstances()
        {
            // Arrange
            var comparer = new DefaultObjectComparer<Person>();

            // Act
            bool equals = comparer.Equals(null, null);

            // Assert
            Assert.True(equals);
        }

        [Fact]
        public void Equals_ReturnsFalse_ForNullInstance()
        {
            // Arrange
            var person = new Person();
            var comparer = new DefaultObjectComparer<Person>();

            // Act
            bool equals0 = comparer.Equals(person, null);
            bool equals1 = comparer.Equals(null, person);

            // Assert
            Assert.False(equals0);
            Assert.False(equals1);
        }

        [Fact]
        public void Equals_ReturnsTrue_WhenNoPropertySelectorsProvided()
        {
            // Arrange
            var person0 = new Person();
            var person1 = new Person();
            var comparer = new DefaultObjectComparer<Person>();

            // Act
            bool equals = comparer.Equals(person0, person1);

            // Assert
            Assert.True(equals);
        }

        [Fact]
        public void Equals_UsesPropertySelectors()
        {
            // Arrange
            var person0 = new Person()
            {
                Forename = "Matt",
                Surname = "Abbott",
                DateOfBirth = new DateTime(1984, 3, 11)
            };
            var person1 = new Person()
            {
                Forename = "Matt",
                Surname = "Abbott",
                DateOfBirth = new DateTime(1984, 3, 11)
            };
            var person2 = new Person()
            {
                Forename = "Matt",
                Surname = "Abbott2",
                DateOfBirth = new DateTime(1984, 3, 11)
            };
            var comparer0 = new DefaultObjectComparer<Person>
            (
                p => p.Forename,
                p => p.Surname,
                p => p.DateOfBirth
            );
            var comparer1 = new DefaultObjectComparer<Person>
            (
                p => p.Surname
            );

            // Act
            bool equals0 = comparer0.Equals(person0, person1);
            bool equals1 = comparer1.Equals(person0, person1);
            bool equals2 = comparer1.Equals(person0, person2);

            // Assert
            Assert.True(equals0);
            Assert.True(equals1);
            Assert.False(equals2);
        }

        [Fact]
        public void Compare_ReturnsZero_WhenNoPropertySelectorsProvided()
        {
            // Arrange
            var person0 = new Person();
            var person1 = new Person();
            var comparer = new DefaultObjectComparer<Person>();

            // Act
            var result = comparer.Compare(person0, person1);

            // Assert
            Assert.Equal(0, result);
        }

        [Fact]
        public void Compare_ReturnsZero_NullInstances()
        {
            // Arrange
            var comparer = new DefaultObjectComparer<Person>();

            // Act
            var result = comparer.Compare(null, null);

            // Assert
            Assert.Equal(0, result);
        }

        [Fact]
        public void Compare_ReturnsZero_ForSameInstance()
        {
            // Arrange
            var person = new Person();
            var comparer = new DefaultObjectComparer<Person>();

            // Act
            var result = comparer.Compare(person, person);
            
            // Assert
            Assert.Equal(0, result);
        }

        [Fact]
        public void Compare_OrdersNullInstancesFirst()
        {
            // Arrange
            var person = new Person();
            // MA - Provide at least 1 property selector.
            var comparer = new DefaultObjectComparer<Person>(p => p.Forename);

            // Act
            var result0 = comparer.Compare(null, person);
            var result1 = comparer.Compare(person, null);

            // Assert
            Assert.Equal(-1, result0);
            Assert.Equal(1, result1);
        }

        private class Person
        {
            public string Forename { get; set; }
            public string Surname { get; set; }
            public DateTime DateOfBirth { get; set; }
        }
    }
}
