// Copyright (c) Alium FX. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

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
        public void Constructor_SetsProperties()
        {
            // Arrange
            var comparer = new DefaultObjectComparer<Person>();

            // Act

            // Assert
            Assert.NotEqual(0, comparer.HighValue);
            Assert.NotEqual(0, comparer.LowValue);
        }

        [Fact]
        public void Equals_ReturnsTrue_ForSameInstance()
        {
            // Arrange
            var person = new Person("Matthew", "Abbott");
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

            bool equals = comparer.Equals(null!, null!);


            // Assert
            Assert.True(equals);
        }

        [Fact]
        public void Equals_ReturnsFalse_ForNullInstance()
        {
            // Arrange
            var person = new Person("Matthew", "Abbott");
            var comparer = new DefaultObjectComparer<Person>();

            // Act

            bool equals0 = comparer.Equals(person, null!);
            bool equals1 = comparer.Equals(null!, person);


            // Assert
            Assert.False(equals0);
            Assert.False(equals1);
        }

        [Fact]
        public void Equals_ReturnsTrue_WhenNoPropertySelectorsProvided()
        {
            // Arrange
            var person0 = new Person("Matthew", "Abbott");
            var person1 = new Person("Matt", "Abbott");
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
            var person0 = new Person("Matt", "Abbott")
            {
                DateOfBirth = new DateTime(1984, 3, 11)
            };
            var person1 = new Person("Matt", "Abbott")
            {
                DateOfBirth = new DateTime(1984, 3, 11)
            };
            var person2 = new Person("Matt", "Abbott2")
            {
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
            var person0 = new Person("Matthew", "Abbott");
            var person1 = new Person("Matt", "Abbott");
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

            var result = comparer.Compare(null!, null!);


            // Assert
            Assert.Equal(0, result);
        }

        [Fact]
        public void Compare_ReturnsZero_ForSameInstance()
        {
            // Arrange
            var person = new Person("Matthew", "Abbott");
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
            var person = new Person("Matthew", "Abbott");
            // MA - Provide at least 1 property selector.
            var comparer = new DefaultObjectComparer<Person>(p => p.Forename);

            // Act

            var result0 = comparer.Compare(null!, person);
            var result1 = comparer.Compare(person, null!);


            // Assert
            Assert.Equal(-1, result0);
            Assert.Equal(1, result1);
        }

        [Fact]
        public void Compare_OrdersUsingPropertySelectors()
        {
            // Arrange
            var person0 = new Person("Matt", "Abbott")
            {
                DateOfBirth = new DateTime(1984, 3, 11)
            };
            var person1 = new Person("Abbott", "Matt")
            {
                DateOfBirth = new DateTime(1984, 11, 3)
            };
            var person2 = new Person("Matthew", "Abbott")
            {
                DateOfBirth = new DateTime(1984, 3, 11)
            };
            var comparer0 = new DefaultObjectComparer<Person>(
                p => p.Forename,
                p => p.Surname);
            var comparer1 = new DefaultObjectComparer<Person>(
                p => p.Surname,
                p => p.Forename);

            // Act
            // MA - Matt > Abbott
            int result0 = comparer0.Compare(person0, person1);
            // MA - Matt < Matthew
            int result1 = comparer0.Compare(person0, person2);
            // MA - Abbott < Matt
            int result2 = comparer1.Compare(person0, person1);
            // MA - Abbott = Abbott, Matt < Matthew 
            int result3 = comparer0.Compare(person0, person2);

            // Assert
            Assert.True(result0 > 0);
            Assert.True(result1 < 0);
            Assert.True(result2 < 0);
            Assert.True(result3 < 0);
        }

        private class Person
        {
            public Person(string forename, string surname)
            {
                Forename = forename;
                Surname = surname;
            }

            public string Forename { get; }
            public string Surname { get; }
            public DateTime DateOfBirth { get; set; }
        }
    }
}
