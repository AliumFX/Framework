// Copyright (c) Alium FX. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Alium
{
    using System;
    using System.Linq;
    using Xunit;

    /// <summary>
    /// Provides tests for the <see cref="WorkContextExtensionCollection"/> type.
    /// </summary>
    public class WorkContextExtensionCollectionTests
    {
        [Fact]
        public void Indexer_ValidatesArguments()
        {
            // Arrange
            var collection = new WorkContextExtensionCollection();
            object? extension = null;

            // Act

            // Assert

            Assert.Throws<ArgumentNullException>("key", () => extension = collection[null /* key */]);
            Assert.Throws<ArgumentNullException>("key", () => collection[null /* key */] = extension);

        }

        [Fact]
        public void Indexer_SetsExtension_InCollection()
        {
            // Arrange
            var extension = new object();
            var collection = new WorkContextExtensionCollection();

            // Act
            collection[typeof(object)] = extension;
            var result = collection[typeof(object)];

            // Assert
            Assert.NotNull(result);
            Assert.Equal(1, collection.Revision);
        }

        [Fact]
        public void Indexer_RemovesExtension_InCollection_WhenSettingNullValue()
        {
            // Arrange
            var extension = new object();
            var collection = new WorkContextExtensionCollection();
            collection[typeof(object)] = extension;

            // Act
            collection[typeof(object)] = null;
            var result = collection[typeof(object)];

            // Assert
            Assert.Null(result);
            Assert.Equal(2, collection.Revision);
        }

        [Fact]
        public void Indexer_DoesntStepRevision_IfSettingNullValue_WhenSettingDidntExist()
        {
            // Arrange
            var extension = new object();
            var collection = new WorkContextExtensionCollection();

            // Act
            collection[typeof(object)] = null;
            var result = collection[typeof(object)];

            // Assert
            Assert.Null(result);
            Assert.Equal(0, collection.Revision);
        }

        [Fact]
        public void Indexer_GetsExtension_FromDefaults_WhenNotInDirectCollection()
        {
            // Arrange
            var extension = new object();
            var defaults = new WorkContextExtensionCollection();
            defaults[typeof(object)] = extension;

            var collection = new WorkContextExtensionCollection(defaults);

            // Act
            var result = collection[typeof(object)];

            // Assert
            Assert.NotNull(result);
            Assert.Same(extension, result);
        }

        [Fact]
        public void Revision_InitiallyZero()
        {
            // Arrange

            // Act
            var collection = new WorkContextExtensionCollection();

            // Assert
            Assert.Equal(0, collection.Revision);
        }

        [Fact]
        public void Revision_IncludesDefaultCollectionRevision()
        {
            // Arrange
            var extension = new object();
            var defaults = new WorkContextExtensionCollection();
            defaults[typeof(object)] = extension;

            // Act
            var collection = new WorkContextExtensionCollection(defaults);

            // Assert
            Assert.Equal(1, collection.Revision);
        }

        [Fact]
        public void Enumerator_ReturnsElementsInCollection()
        {
            // Arrange
            var extension1 = new ExtensionOne();
            var extension2 = new ExtensionTwo();
            var collection = new WorkContextExtensionCollection();
            collection[typeof(ExtensionOne)] = extension1;
            collection[typeof(ExtensionTwo)] = extension2;

            // Act
            var extensions = collection.ToList();

            // Assert
            Assert.Equal(2, extensions.Count);
            Assert.Contains(extensions, p => p.Key == typeof(ExtensionOne) && p.Value == extension1);
            Assert.Contains(extensions, p => p.Key == typeof(ExtensionTwo) && p.Value == extension2);
        }

        [Fact]
        public void Enumerator_ReturnsElementsInDefault_ExceptThoseInDirectCollection()
        {
            // Arrange
            var extension1 = new ExtensionOne();
            var extension2 = new ExtensionTwo();
            var extension2InDefault = new ExtensionTwo();
            var extension3InDefault = new ExtensionThree();
            var defaults = new WorkContextExtensionCollection();
            defaults[typeof(ExtensionTwo)] = extension2InDefault;
            defaults[typeof(ExtensionThree)] = extension3InDefault;
            var collection = new WorkContextExtensionCollection(defaults);
            collection[typeof(ExtensionOne)] = extension1;
            collection[typeof(ExtensionTwo)] = extension2;

            // Act
            var extensions = collection.ToList();

            // Assert
            Assert.Equal(3, extensions.Count);
            Assert.Contains(extensions, p => p.Key == typeof(ExtensionOne) && p.Value == extension1);
            Assert.Contains(extensions, p => p.Key == typeof(ExtensionTwo) && p.Value == extension2);
            Assert.Contains(extensions, p => p.Key == typeof(ExtensionThree) && p.Value == extension3InDefault);
            Assert.DoesNotContain(extensions, p => p.Key == typeof(ExtensionTwo) && p.Value == extension2InDefault);
        }

        private class ExtensionOne
        {

        }

        private class ExtensionTwo
        {

        }

        private class ExtensionThree
        {

        }
    }
}
