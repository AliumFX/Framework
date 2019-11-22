// Copyright (c) Alium FX. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Alium
{
    using System;
    using Moq;
    using Xunit;

    /// <summary>
    /// Provides tests for the <see cref="WorkContextExtensionCollectionExtensions"/> type.
    /// </summary>
    public class WorkContextExtensionCollectionExtensionsTests
    {
        [Fact]
        public void GetExtension_ValidatesArguments()
        {
            // Arrange

            // Act

            // Assert
            Assert.Throws<ArgumentNullException>(() => WorkContextExtensionCollectionExtensions.GetExtension<object>(null! /* collection */));
        }

        [Fact]
        public void GetExtension_GetsExtensionFromCollection()
        {
            // Arrange
            var extension = new object();
            var collection = CreateCollection<object>(extension);

            // Act
            var result = WorkContextExtensionCollectionExtensions.GetExtension<object>(collection);

            // Assert
            Assert.NotNull(result);
            Assert.Same(extension, result);
        }

        [Fact]
        public void SetExtension_ValidatesArguments()
        {
            // Arrange
            var collection = CreateCollection<object>();

            // Act

            // Assert
            Assert.Throws<ArgumentNullException>(() => WorkContextExtensionCollectionExtensions.SetExtension<object>(null! /* collection */, null! /* extension */));
        }

        [Fact]
        public void SetExtension_SetsExtensionInCollection()
        {
            // Arrange
            var extension = new object();
            object? capturedExtension = null;
            var collection = CreateCollection<object>(onSet: f => capturedExtension = f);

            // Act
            WorkContextExtensionCollectionExtensions.SetExtension<object>(collection, extension);

            // Assert
            Assert.NotNull(capturedExtension);
            Assert.Same(extension, capturedExtension);
        }

        private IWorkContextExtensionCollection CreateCollection<TExtension>(
            TExtension? extension = null,
            Action<TExtension>? onSet = null)
            where TExtension : class
        {
            var mock = new Mock<IWorkContextExtensionCollection>();

            if (extension is object)
            {
                mock.SetupGet(wcfc => wcfc[It.IsAny<Type>()])
                    .Returns(extension);
            }
            if (onSet is object)
            {
                mock.SetupSet(wcfc => wcfc[typeof(TExtension)] = It.IsAny<TExtension>())
                    .Callback<Type, TExtension>((t, f) => onSet?.DynamicInvoke(f));
            }

            return mock.Object;
        }
    }
}
