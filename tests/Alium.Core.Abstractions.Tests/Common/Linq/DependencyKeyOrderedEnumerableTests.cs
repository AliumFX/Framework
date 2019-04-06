// Copyright (c) Alium FX. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Alium
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Xunit;

    /// <summary>
    /// Provides tests for the <see cref="DependencyKeyOrderedEnumerable{TElement, TDependencyKey}"/> type.
    /// </summary>
    public class DependencyKeyOrderedEnumerableTests
    {
        [Fact]
        public void Constructor_ValidatesArguments()
        {
            // Arrange
            var source = Enumerable.Empty<string>();
            string keySelector(string s) => s;

            // Act

            // Assert
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference or unconstrained type parameter.
            Assert.Throws<ArgumentNullException>(() => new DependencyKeyOrderedEnumerable<string, string>(null /* source */, null /* keySelector */, null /* dependentKeySelector */));
            Assert.Throws<ArgumentNullException>(() => new DependencyKeyOrderedEnumerable<string, string>(source, null /* keySelector */, null /* dependentKeySelector */));
            Assert.Throws<ArgumentNullException>(() => new DependencyKeyOrderedEnumerable<string, string>(source, keySelector, null /* dependentKeySelector */));
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference or unconstrained type parameter.
        }

        [Fact]
        public void Enumerator_CreatesDependencySortedSet()
        {
            // Arrange
            var source = new[]
            {
                new Item { Key = "One", Dependencies = null },
                new Item { Key = "Two", Dependencies = new[] { "Three", "Four" } },
                new Item { Key = "Three", Dependencies = new[] { "One" } },
                new Item { Key = "Four", Dependencies = null }
            };
            var enumerable = new DependencyKeyOrderedEnumerable<Item, string>(source, i => i.Key, (i, k) => i.Dependencies);

            // Act
            var sorted = enumerable.ToList();

            // Assert
            Assert.Equal(4, sorted.Count);
            Assert.Same(source[0], sorted[0]);
            Assert.Same(source[2], sorted[1]);
            Assert.Same(source[3], sorted[2]);
            Assert.Same(source[1], sorted[3]);
        }

        [Fact]
        public void Enumerator_ThrowsExcetion_ForCyclicDependencies()
        {
            // Arrange
            var source = new[]
            {
                new Item { Key = "One", Dependencies = null },
                new Item { Key = "Two", Dependencies = new[] { "Three", "Four" } },
                new Item { Key = "Three", Dependencies = new[] { "One" } },
                new Item { Key = "Four", Dependencies = new[] { "Two" } }
            };
            var enumerable = new DependencyKeyOrderedEnumerable<Item, string>(source, i => i.Key, (i, k) => i.Dependencies);

            // Act

            // Assert
            var ioe = Assert.Throws<InvalidOperationException>(() => enumerable.ToList());
            Assert.Equal("Cyclic dependency: 'Two => Four => Two!'", ioe.Message);
        }

        [Fact]
        public void Enumerator_ThrowsExcetion_ForMissingDependencies()
        {
            // Arrange
            var source = new[]
            {
                new Item { Key = "One", Dependencies = null },
                new Item { Key = "Two", Dependencies = new[] { "Three", "Four" } },
                new Item { Key = "Three", Dependencies = new[] { "One" } },
                new Item { Key = "Four", Dependencies = new[] { "Five" } }
            };
            var enumerable = new DependencyKeyOrderedEnumerable<Item, string>(source, i => i.Key, (i, k) => i.Dependencies);

            // Act

            // Assert
            var ioe = Assert.Throws<InvalidOperationException>(() => enumerable.ToList());
            Assert.Equal("Missing dependency: 'Two => Four => Five?'", ioe.Message);
        }

        private class Item
        {
            public Item()
            {
                Key = string.Empty;
            }

            public string Key { get; set; }
            public string[]? Dependencies { get; set; }
        }
    }
}
