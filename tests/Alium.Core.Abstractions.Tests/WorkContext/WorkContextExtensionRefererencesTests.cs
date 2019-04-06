// Copyright (c) Alium FX. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Alium
{
    using System;
    using Moq;
    using Xunit;

    /// <summary>
    /// Provides tests for the <see cref="WorkContextExtensionReferences{TCache}"/> type.
    /// </summary>
    public class WorkContextExtensionRefererencesTests
    {
        [Fact]
        public void Constructor_ValidatesArguments()
        {
            // Arrange
            var collection = CreateCollection<TestExtension>();

            // Act

            // Assert
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference or unconstrained type parameter.
            Assert.Throws<ArgumentNullException>(() => new WorkContextExtensionReferences<ExtensionCache>(null /* extensions */));
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference or unconstrained type parameter.
        }

        [Fact]
        public void Constructor_SetsProperties()
        {
            // Arrange
            var collection = CreateCollection<TestExtension>(revision: 1);

            // Act
            var refs = new WorkContextExtensionReferences<ExtensionCache>(collection);

            // Assert
            Assert.Equal(1, refs.Revision);
            Assert.Same(collection, refs.Extensions);
        }

        [Fact]
        public void Fetch_FetchesExtension_FromCollection_WhenNotCached_AndExtensionExists()
        {
            // Arrange
            var existingExtension = new TestExtension();
            TestExtension? capturedExtension = null;
            var collection = CreateCollection<TestExtension>(
                revision: 1,
                extension: existingExtension,
                onSet: e => capturedExtension = e);
            var refs = new WorkContextExtensionReferences<ExtensionCache>(collection);

            // Act
            var extension = refs.Fetch<TestExtension>(ref refs.Cache.Test, e => new TestExtension());

            // Assert
            Assert.NotNull(extension);
            Assert.Same(existingExtension, extension);
            Assert.Null(capturedExtension);
        }

        [Fact]
        public void Fetch_CreatesExtension_UsingFactory_WhenNotCached_AndExtensionDoesNotExist()
        {
            // Arrange
            int revision = 1;
            var existingExtension = new TestExtension();
            TestExtension? capturedExtension = null;
            var collection = CreateCollection<TestExtension>(
                onRevision: () => revision,
                extension: null,
                onSet: e => capturedExtension = e);
            var refs = new WorkContextExtensionReferences<ExtensionCache>(collection);

            // Act
            revision++;
            var extension = refs.Fetch<TestExtension>(ref refs.Cache.Test, e => new TestExtension());

            // Assert
            Assert.NotNull(extension);
            Assert.NotSame(existingExtension, extension);
            Assert.NotNull(capturedExtension);
            Assert.Equal(2, refs.Revision);
        }

        [Fact]
        public void Fetch_ReturnsCachedExtension_WhenCollectionUnchanged()
        {
            // Arrange
            int revision = 1;
            var createdExtension = new TestExtension();
            TestExtension? capturedExtension = null;
            var collection = CreateCollection<TestExtension>(
                onRevision: () => revision,
                onGet: () => createdExtension,
                onSet: e => capturedExtension = e);
            var refs = new WorkContextExtensionReferences<ExtensionCache>(collection);
            refs.Fetch<TestExtension>(ref refs.Cache.Test, e => createdExtension);

            // Act
            var extension = refs.Fetch<TestExtension>(ref refs.Cache.Test, e => new TestExtension());

            // Assert
            Assert.NotNull(extension);
            Assert.Same(createdExtension, extension);
            Assert.Equal(1, refs.Revision);
        }

        [Fact]
        public void Fetch_ClearsCachedExtension_AndFetchesNewExtension_WhenUnderlyingCollectionChanged()
        {
            // Arrange
            int revision = 1;
            var createdExtension = new TestExtension();
            var replacementExtension = new TestExtension();
            TestExtension? capturedExtension = null;
            var collection = CreateCollection<TestExtension>(
                onRevision: () => revision,
                extension: null,
                onSet: e => capturedExtension = e);
            var refs = new WorkContextExtensionReferences<ExtensionCache>(collection);
            refs.Fetch<TestExtension>(ref refs.Cache.Test, e => createdExtension);

            // Act
            revision++;
            var extension = refs.Fetch<TestExtension>(ref refs.Cache.Test, e => replacementExtension);

            // Assert
            Assert.NotNull(extension);
            Assert.Same(replacementExtension, extension);
            Assert.Equal(2, refs.Revision);
            Assert.NotNull(capturedExtension);
            Assert.Same(replacementExtension, capturedExtension);
        }

        private struct ExtensionCache
        {
            public TestExtension Test;
        }

        private class TestExtension
        {

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

            if (onRevision != null)
            {
                mock.Setup(wcec => wcec.Revision)
                    .Returns(() =>
                    {
                        if (onRevision != null)
                        {
                            return onRevision();
                        }
                        return revision.GetValueOrDefault(1);
                    });
            }
            else if (revision != null)
            {
                mock.Setup(wcec => wcec.Revision)
                    .Returns(() => revision.GetValueOrDefault(1));
            }
            else
            {
                mock.Setup(wcec => wcec.Revision)
                    .Returns(() => 0);
            }

            if (extension != null)
            {
                mock.SetupGet(wcfc => wcfc[It.IsAny<Type>()])
                    .Returns(extension);
            }

            if (onGet != null)
            {
                mock.SetupGet(wcfc => wcfc[It.IsAny<Type>()])
                    .Returns(() =>
                    {
                        if (onGet != null)
                        {
                            return onGet();
                        }
                        return default;
                    });
            }

            if (onSet != null)
            {
                mock.SetupSet(wcfc => wcfc[typeof(TExtension)] = It.IsAny<TExtension>())
                    .Callback<Type, object>((t, f) => onSet?.DynamicInvoke((TExtension)f));
            }

            return mock.Object;
        }
    }
}
