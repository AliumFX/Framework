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
            Assert.Equal(true, equals);
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
            Assert.Equal(false, equals0);
            Assert.Equal(false, equals1);
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
            Assert.Equal(true, equals);
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
            Assert.Equal(true, equals0);
            Assert.Equal(true, equals1);
            Assert.Equal(false, equals2);
        }

        private class Person
        {
            public string Forename { get; set; }
            public string Surname { get; set; }
            public DateTime DateOfBirth { get; set; }
        }
    }
}
