// Copyright (c) Alium Fx. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Alium.Tenancy
{
    using System;
    using Microsoft.Extensions.DependencyInjection;
    using Xunit;

    /// <summary>
    /// Provides tests for the <see cref="TenantScopedServiceDescriptor"/> type
    /// </summary>
    public class TenantScopedServiceDescriptorTests
    {
        [Fact]
        public void Constructor_ValidatesArguments()
        {
            // Arrange
            var personType = typeof(Person);

            // Act

            // Assert
            Assert.Throws<ArgumentNullException>("serviceType", () => new TenantScopedServiceDescriptor(
                null! /* serviceType */,
                personType));

            Assert.Throws<ArgumentNullException>("implementationType", () => new TenantScopedServiceDescriptor(
                personType,
                (Type)null! /* implementationType */));

            Assert.Throws<ArgumentNullException>("factory", () => new TenantScopedServiceDescriptor(
                personType,
                (Func<IServiceProvider, object>)null! /* factory */));

        }


        [Fact]
        public void Constructor_SetsProperties()
        {
            // Arrange
            Func<IServiceProvider, object> factory = sp => null!;

            // Act
            var descriptor1 = new TenantScopedServiceDescriptor(typeof(Person), typeof(Person));
            var descriptor2 = new TenantScopedServiceDescriptor(typeof(Person), factory);

            // Assert
            Assert.Equal(typeof(Person), descriptor1.ServiceType);
            Assert.Equal(typeof(Person), descriptor1.ImplementationType);
            Assert.Equal(factory, descriptor2.ImplementationFactory);
        }

        [Fact]
        public void Services_AreSingleton()
        {
            // Arrange

            // Act
            var descriptor = new TenantScopedServiceDescriptor(typeof(Person), typeof(Person));

            // Assert
            Assert.Equal(ServiceLifetime.Singleton, descriptor.Lifetime);
        }

        public class Person { }
    }
}
