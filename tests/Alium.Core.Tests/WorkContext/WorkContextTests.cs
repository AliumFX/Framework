﻿// Copyright (c) Alium Fx. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Alium
{
    using Moq;
    using System.Globalization;
    using Xunit;

    using Alium.Tenancy;

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
        public void Cultures_ProvidedByExtension()
        {
            // Arrange
            var formatting = CultureInfo.GetCultureInfo("en-US");
            var resource = CultureInfo.GetCultureInfo("es-ES");
            var extension = CreateCultureWorkContextExtension(formatting, resource);
            var collection = new WorkContextExtensionCollection();
            collection.SetExtension(extension);

            // Act
            var context = new WorkContext(collection);

            // Assert
            Assert.NotNull(context.FormattingCulture);
            Assert.Same(formatting, context.FormattingCulture);

            Assert.NotNull(context.ResourceCulture);
            Assert.Same(resource, context.ResourceCulture);
        }

        [Fact]
        public void Cultures_ProvideDefault_IfNoExtensionSpecified()
        {
            // Arrange
            var formatting = CultureInfo.CurrentCulture;
            var resource = CultureInfo.CurrentUICulture;

            // Act
            var context = new WorkContext();

            // Assert
            Assert.NotNull(context.FormattingCulture);
            Assert.Equal(formatting, context.FormattingCulture);

            Assert.NotNull(context.ResourceCulture);
            Assert.Equal(resource, context.ResourceCulture);
        }

        [Fact]
        public void FormattingCulture_CanBeChanged_WhenSettingExtension()
        {
            // Arrange
            var culture1 = new CultureInfo("en-GB");
            var culture2 = new CultureInfo("en-US");

            var extension1 = CreateCultureWorkContextExtension(culture1, culture1);
            var extension2 = CreateCultureWorkContextExtension(culture2, culture2);

            var collection = new WorkContextExtensionCollection();
            collection.SetExtension(extension1);

            var context = new WorkContext(collection);

            // Act
            context.Extensions.SetExtension(extension2);

            // Assert
            Assert.Equal(culture2, context.FormattingCulture);
        }
        
        [Fact]
        public void TenantId_ProvidesDefaultTenantId_IfNotSpecified()
        {
            // Arrange
            var tenantId = TenantId.Default;

            // Act
            var context = new WorkContext();

            // Assert
            Assert.Equal(tenantId, context.TenantId);
        }

        [Fact]
        public void TenantId_ProvidesTenantId_FromExtension()
        {
            // Arrange
            var tenantId = new TenantId("custom");
            var extension = CreateTenantWorkContextExtension(tenantId);
            var collection = new WorkContextExtensionCollection();
            collection.SetExtension(extension);

            // Act
            var context = new WorkContext(collection);

            // Assert
            Assert.Equal(tenantId, context.TenantId);
        }

        [Fact]
        public void Tenant_CanBeChanged_WhenSettingExtension()
        {
            // Arrange
            var tenant1 = new TenantId("one");
            var tenant2 = new TenantId("two");

            var extension1 = CreateTenantWorkContextExtension(tenant1);
            var extension2 = CreateTenantWorkContextExtension(tenant2);

            var collection = new WorkContextExtensionCollection();
            collection.SetExtension(extension1);

            var context = new WorkContext(collection);

            // Act
            context.Extensions.SetExtension(extension2);

            // Assert
            Assert.Equal(tenant2, context.TenantId);
        }

        private ICultureWorkContextExtension CreateCultureWorkContextExtension(CultureInfo formattingCulture, CultureInfo resourceCulture)
        {
            var mock = new Mock<ICultureWorkContextExtension>();

            mock.Setup(cwce => cwce.FormattingCulture)
                .Returns(formattingCulture);

            mock.Setup(cwce => cwce.ResourceCulture)
                .Returns(resourceCulture);

            return mock.Object;
        }

        private ITenantWorkContextExtension CreateTenantWorkContextExtension(TenantId tenantId)
        {
            var mock = new Mock<ITenantWorkContextExtension>();

            mock.Setup(twce => twce.TenantId)
                .Returns(tenantId);

            return mock.Object;
        }
    }
}
