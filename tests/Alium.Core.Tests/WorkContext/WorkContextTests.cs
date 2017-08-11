// Copyright (c) Alium Fx. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Alium
{
    using Moq;
    using System.Globalization;
    using Xunit;

    /// <summary>
    /// Provides tests for the <see cref="WorkContext"/> type.
    /// </summary>
    public class WorkContextTests
    {
        [Fact]
        public void Constructor_CreatesExtensionsCollection_IfNotProvided()
        {
            // Arrange

            // Act
            var context = new WorkContext();

            // Assert
            Assert.NotNull(context.Extensions);
        }

        [Fact]
        public void Constructor_FlowsCollection_ToProperties()
        {
            // Arrange
            var collection = new WorkContextExtensionCollection();

            // Act
            var context = new WorkContext(collection);

            // Assert
            Assert.NotNull(context.Extensions);
            Assert.Same(collection, context.Extensions);
        }

        [Fact]
        public void Culture_ProvidesCultureFromExtension()
        {
            // Arrange
            var culture = CultureInfo.GetCultureInfo("en-US");
            var extension = CreateCultureWorkContextExtension(culture);
            var collection = new WorkContextExtensionCollection();
            collection.SetExtension(extension);

            // Act
            var context = new WorkContext(collection);

            // Assert
            Assert.NotNull(context.Culure);
            Assert.Same(culture, context.Culure);
        }

        private ICultureWorkContextExtension CreateCultureWorkContextExtension(CultureInfo culture)
        {
            var mock = new Mock<ICultureWorkContextExtension>();

            mock.Setup(cwce => cwce.Culture)
                .Returns(culture);

            return mock.Object;
        }
    }
}
