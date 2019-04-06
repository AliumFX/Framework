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

            // Act

            // Assert
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference or unconstrained type parameter.
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
            Assert.Throws<ArgumentNullException>(() => new TenantScopedServiceDescriptor(
                null /* serviceType */,
                (Type)null /* implemenationType */));

            Assert.Throws<ArgumentNullException>(() => new TenantScopedServiceDescriptor(
                typeof(Person),
                (Type)null /* implementationType */));

            Assert.Throws<ArgumentNullException>(() => new TenantScopedServiceDescriptor(
                typeof(Person),
                (Func<IServiceProvider, object>)null /* factory */));
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference or unconstrained type parameter.
        }


        [Fact]
        public void Constructor_SetsProperties()
        {
            // Arrange
#pragma warning disable CS8603 // Possible null reference return.
            Func<IServiceProvider, object> factory = sp => null;
#pragma warning restore CS8603 // Possible null reference return.

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
