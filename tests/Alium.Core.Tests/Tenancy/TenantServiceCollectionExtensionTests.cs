// Copyright (c) Alium Fx. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Alium.Tenancy
{
    using System;
    using System.Linq;
    using Microsoft.Extensions.DependencyInjection;
    using Xunit;

    /// <summary>
    /// Provides tests for the <see cref="TenantServiceCollectionExtensions"/> type
    /// </summary>
    public class TenantServiceCollectionExtensionTests
    {
        [Fact]
        public void AddTenantScoped_AddServiceAsImplementation()
        {
            RunTests(
                config: services => services.AddTenantScoped<Service>(),
                predicate: descriptor => descriptor.ServiceType == typeof(Service)
                                         && descriptor.ImplementationType == typeof(Service));

            RunTests(
                config: services => services.AddTenantScoped(typeof(Service)),
                predicate: descriptor => descriptor.ServiceType == typeof(Service)
                                         && descriptor.ImplementationType == typeof(Service));
        }
        

        [Fact]
        public void AddTenantScoped_AddServiceWithImplementation()
        {
            RunTests(
                config: services => services.AddTenantScoped<IService, Service>(),
                predicate: descriptor => descriptor.ServiceType == typeof(IService)
                                         && descriptor.ImplementationType == typeof(Service));

            RunTests(
                config: services => services.AddTenantScoped(typeof(IService), typeof(Service)),
                predicate: descriptor => descriptor.ServiceType == typeof(IService)
                                         && descriptor.ImplementationType == typeof(Service));
        }

        [Fact]
        public void AddTenantScoped_AddServiceWithFactory()
        {
            // Arrange
            Func<IServiceProvider, object> objectFactory = sp => new Service();
            Func<IServiceProvider, Service> typedFactory = sp => new Service();

            RunTests(
                config: services => services.AddTenantScoped<IService>(typedFactory),
                predicate: descriptor => descriptor.ServiceType == typeof(IService)
                                         && descriptor.ImplementationFactory == typedFactory);

            // MA - Internally we have to wrap the delegate because we can cast, therefore check here is different
            RunTests(
                config: services => services.AddTenantScoped<IService, Service>(typedFactory),
                predicate: descriptor => descriptor.ServiceType == typeof(IService)
                                         && descriptor.ImplementationFactory != null);
            
            RunTests(
                config: services => services.AddTenantScoped(typeof(IService), objectFactory),
                predicate: descriptor => descriptor.ServiceType == typeof(IService)
                                         && descriptor.ImplementationFactory == objectFactory);
        }

        [Fact]
        public void AddTenantScoped_ValidatedParameters()
        {
            // Arrange
            var services = new ServiceCollection();

            // Act

            // Assert
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference or unconstrained type parameter.
            Assert.Throws<ArgumentNullException>("services", () => TenantServiceCollectionExtensions.AddTenantScoped(null /* services */, typeof(IService)));
            Assert.Throws<ArgumentNullException>("serviceType", () => TenantServiceCollectionExtensions.AddTenantScoped(services, (Type?)null /* serviceType */));

            Assert.Throws<ArgumentNullException>("services", () => TenantServiceCollectionExtensions.AddTenantScoped(null /* services */, typeof(IService), typeof(Service)));
            Assert.Throws<ArgumentNullException>("serviceType", () => TenantServiceCollectionExtensions.AddTenantScoped(services, (Type?)null /* serviceType */, typeof(Service)));
            Assert.Throws<ArgumentNullException>("implementationType", () => TenantServiceCollectionExtensions.AddTenantScoped(services, typeof(IService), (Type?)null /* implementationType */));

            Assert.Throws<ArgumentNullException>("services", () => TenantServiceCollectionExtensions.AddTenantScoped(null /* services */, typeof(IService), sp => new Service()));
            Assert.Throws<ArgumentNullException>("serviceType", () => TenantServiceCollectionExtensions.AddTenantScoped(services, (Type?)null /* serviceType */, sp => new Service()));
            Assert.Throws<ArgumentNullException>("factory", () => TenantServiceCollectionExtensions.AddTenantScoped(services, typeof(IService), (Func<IServiceProvider, object>?)null /* implementationFactory */));

            Assert.Throws<ArgumentNullException>("services", () => TenantServiceCollectionExtensions.AddTenantScoped<Service>(null /* services */));

            Assert.Throws<ArgumentNullException>("services", () => TenantServiceCollectionExtensions.AddTenantScoped<Service>(null /* services */, sp => new Service()));
            Assert.Throws<ArgumentNullException>("factory", () => TenantServiceCollectionExtensions.AddTenantScoped<Service>(services, null /* implementationFactory */));

            Assert.Throws<ArgumentNullException>("services", () => TenantServiceCollectionExtensions.AddTenantScoped<IService, Service>(null /* services */));

            Assert.Throws<ArgumentNullException>("services", () => TenantServiceCollectionExtensions.AddTenantScoped<IService, Service>(null /* services */, sp => new Service()));
            Assert.Throws<ArgumentNullException>("factory", () => TenantServiceCollectionExtensions.AddTenantScoped<IService, Service>(services, null /* implementationFactory */));
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference or unconstrained type parameter.
        }

        private void RunTests(
            Action<IServiceCollection> config,
            Func<TenantScopedServiceDescriptor, bool> predicate,
            Action<IServiceCollection, TenantScopedServiceDescriptor>? asserts = null)
        {
            // Arrange
            var services = new ServiceCollection();
            config(services);

            // Act

            // Assert
            var descriptor = services.OfType<TenantScopedServiceDescriptor>().FirstOrDefault(predicate);
            Assert.NotNull(descriptor);

            asserts?.DynamicInvoke(services, descriptor);
        }

        public interface IService
        {

        }

        public class Service : IService
        {

        }
    }
}
