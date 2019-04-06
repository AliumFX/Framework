// Copyright (c) Alium Fx. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Alium.Tenancy
{
    using System;
    using Microsoft.Extensions.DependencyInjection;
    using Xunit;

    /// <summary>
    /// Provides tests for the <see cref="TenantServiceProviderResolver"/>
    /// </summary>
    public class TenantServiceProviderResolverTests
    {
        [Fact]
        public void Constructor_ValidatesArguments()
        {
            // Arrange

            // Act 

            // Assert
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference or unconstrained type parameter.
            Assert.Throws<ArgumentNullException>(() => new TenantServiceProviderResolver(null /* serviceCollectionFactory */));
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference or unconstrained type parameter.
        }

        [Fact]
        public void GetTenantServiceProvider_ReturnsServiceProvider_ForTenant()
        {
            // Arrange
            var resolver = CreateResolver();
            var tenant = new TenantId("Test");

            // Act
            var tenantProvider = resolver.GetTenantServiceProvider(tenant);

            // Assert
            Assert.NotNull(tenantProvider);
        }

        [Fact]
        public void GetTenantServiceProvider_ReturnsSameServiceProvider_ForSameTenantId()
        {
            // Arrange
            var resolver = CreateResolver();
            var tenant = new TenantId("Test");

            // Act
            var tenantProvider1 = resolver.GetTenantServiceProvider(tenant);
            var tenantProvider2 = resolver.GetTenantServiceProvider(tenant);

            // Assert
            Assert.NotNull(tenantProvider1);
            Assert.NotNull(tenantProvider2);
            Assert.Same(tenantProvider1, tenantProvider2);
        }

        [Fact]
        public void GetTenantServiceProvider_ReturnsDifferentServiceProvider_ForDifferentTenantId()
        {
            // Arrange
            var resolver = CreateResolver();
            var tenant1 = new TenantId("Test");
            var tenant2 = new TenantId("Other");

            // Act
            var tenantProvider1 = resolver.GetTenantServiceProvider(tenant1);
            var tenantProvider2 = resolver.GetTenantServiceProvider(tenant2);

            // Assert
            Assert.NotNull(tenantProvider1);
            Assert.NotNull(tenantProvider2);
            Assert.NotSame(tenantProvider1, tenantProvider2);
        }

        private TenantServiceProviderResolver CreateResolver()
        {
            var services = new ServiceCollection();
            var provider = services.BuildServiceProvider();
            var factory = new TenantServiceCollectionFactory(provider, services);
            var resolver = new TenantServiceProviderResolver(factory);

            return resolver;
        }
    }
}
