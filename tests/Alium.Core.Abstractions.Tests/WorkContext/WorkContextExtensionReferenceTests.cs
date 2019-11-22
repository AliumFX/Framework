// Copyright (c) Alium FX. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Alium
{
    using System;
    using Moq;
    using Xunit;

    /// <summary>
    /// Provides tests for the <see cref="WorkContextExtensionReference{TExtension}"/> type.
    /// </summary>
    public class WorkContextExtensionReferenceTests
    {
        [Fact]
        public void Update_SetsExtension_InCollection()
        {
            // Arrange
            object extension = new object();
            object? capturedExtension = null;
            var collection = CreateCollection<object>(
                revision: 1, 
                onSet: e => capturedExtension = e);
            var reference = new WorkContextExtensionReference<object>();

            // Act
            var result = reference.Update(collection, extension);

            // Assert
            Assert.NotNull(capturedExtension);
            Assert.Same(extension, capturedExtension);
            Assert.NotNull(result);
            Assert.Same(extension, result);
        }

        [Fact]
        public void Update_CapturesExtension_AndRevision()
        {
            // Arrange
            object extension = new object();
            object? capturedExtension = null;
            var collection = CreateCollection<object>(
                revision: 1, 
                onSet: e => capturedExtension = e);
            var reference = new WorkContextExtensionReference<object>();

            // Act
            var result = reference.Update(collection, extension);

            // Assert
            Assert.Equal(1, reference.Revision);
            Assert.Equal(extension, reference.Extension);
        }

        [Fact]
        public void Fetch_ReturnsCapturedExtension_IfCollectionNotChanged()
        {
            // Arrange
            object extension = new object();
            var collection = CreateCollection<object>(
                revision: 1,
                extension: extension);
            var reference = new WorkContextExtensionReference<object>();

            reference.Update(collection, extension);

            // Act
            var result = reference.Fetch(collection);

            // Assert
            Assert.NotNull(result);
            Assert.Same(extension, result);
            Assert.Same(extension, reference.Extension);
        }

        [Fact]
        public void Fetch_ReturnsExtensionFromCollecton_IfCollectionChanged()
        {
            // Arrange
            int revision = 1;
            object extension1 = new object();
            object extension2 = new object();

            object entry = extension1;
            var collection = CreateCollection<object>(
                onRevision: () => revision,
                onGet: () => entry);
            var reference = new WorkContextExtensionReference<object>();

            reference.Update(collection, extension1);

            // Act
            var result1 = reference.Fetch(collection);
            int revision1 = reference.Revision;

            revision++;
            entry = extension2;

            var result2 = reference.Fetch(collection);
            int revision2 = reference.Revision;

            // Assert
            Assert.NotNull(result1);
            Assert.Same(extension1, result1);
            Assert.Equal(1, revision1);

            Assert.NotNull(result2);
            Assert.Same(extension2, result2);
            Assert.Equal(2, revision2);
        }

        private IWorkContextExtensionCollection CreateCollection<TExtension>(
            int? revision = null,
            Func<int>? onRevision = null,
            TExtension? extension = null,
            Func<TExtension>? onGet = null,
            Action<TExtension>? onSet = null)
            where TExtension : class
        {
            var mock = new Mock<IWorkContextExtensionCollection>();

            if (onRevision is object)
            {
                mock.Setup(wcec => wcec.Revision)
                    .Returns(() =>
                    {
                        if (onRevision is object)
                        {
                            return onRevision();
                        }

                        return revision.GetValueOrDefault(1);
                    });
            }
            else if (revision is object)
            {
                mock.Setup(wcec => wcec.Revision)
                    .Returns(() => revision.GetValueOrDefault(1));
            }
            else
            {
                mock.Setup(wcec => wcec.Revision)
                    .Returns(() => 0);
            }

            if (extension is object)
            {
                mock.SetupGet(wcfc => wcfc[It.IsAny<Type>()])
                    .Returns(extension);
            }

            if (onGet is object)
            {
                mock.SetupGet(wcfc => wcfc[It.IsAny<Type>()])
                    .Returns(() =>
                    {
                        if (onGet is object)
                        {
                            return onGet();
                        }
                        return default;
                    });
            }

            if (onSet is object)
            {
                mock.SetupSet(wcfc => wcfc[typeof(TExtension)] = It.IsAny<TExtension>())
                    .Callback<Type, TExtension>((t, f) => onSet?.Invoke(f));
            }

            return mock.Object;
        }
    }
}
