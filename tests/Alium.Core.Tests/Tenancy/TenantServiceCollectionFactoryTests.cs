// Copyright (c) Alium Fx. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Alium.Tenancy
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.Extensions.DependencyInjection;
    using Xunit;

    /// <summary>
    /// Provides tests for the <see cref="TenantServiceCollectionFactory"/>
    /// </summary>
    public class TenantServiceCollectionFactoryTests
    {
        [Fact]
        public void Constructor_ValidatesArguments()
        {
            // Arrange
            var services = new ServiceCollection();
            var provider = services.BuildServiceProvider();

            // Act

            // Assert
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference or unconstrained type parameter.
            Assert.Throws<ArgumentNullException>(() => new TenantServiceCollectionFactory(null /* provider */, services));
            Assert.Throws<ArgumentNullException>(() => new TenantServiceCollectionFactory(provider, null /* services */));
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference or unconstrained type parameter.
        }

        [Fact]
        public void CreateServiceCollection_CreatesNewServiceCollection()
        {
            // Arrange
            var services = new ServiceCollection();
            var provider = services.BuildServiceProvider();
            var factory = new TenantServiceCollectionFactory(provider, services);

            // Act
            var tenantServices = factory.CreateServiceCollection();

            // Assert
            Assert.NotNull(tenantServices);
            Assert.NotSame(services, tenantServices);
        }

        [Fact]
        public void CreateServiceCollection_ReplacesSelfReferencingServiceCollection()
        {
            // Arrange
            var services = new ServiceCollection();
            services.AddSingleton<IServiceCollection>(services);
            var provider = services.BuildServiceProvider();
            var factory = new TenantServiceCollectionFactory(provider, services);

            // Act
            var tenantServices = factory.CreateServiceCollection();

            // Assert
            var descriptor = tenantServices.SingleOrDefault(sd => sd.ServiceType == typeof(IServiceCollection));
            Assert.NotNull(descriptor);
            Assert.Same(tenantServices, descriptor.ImplementationInstance);
            Assert.NotSame(services, descriptor.ImplementationInstance);
        }

        [Fact]
        public void CreateServiceCollection_ConsumesTransientServiceDescriptors()
        {
            // Arrange
            var services = new ServiceCollection();
            services.AddTransient<IService<string>, Service<string>>();
            var provider = services.BuildServiceProvider();
            var factory = new TenantServiceCollectionFactory(provider, services);

            // Act
            var tenantServices = factory.CreateServiceCollection();

            // Assert
            var descriptor = tenantServices.SingleOrDefault(sd => sd.ServiceType == typeof(IService<string>));
            Assert.NotNull(descriptor);
            Assert.Equal(typeof(Service<string>), descriptor.ImplementationType);
            Assert.Equal(ServiceLifetime.Transient, descriptor.Lifetime);
        }

        [Fact]
        public void CreateServiceCollection_ConsumesScopedServiceDescriptors()
        {
            // Arrange
            var services = new ServiceCollection();
            services.AddScoped<IService<string>, Service<string>>();
            var provider = services.BuildServiceProvider();
            var factory = new TenantServiceCollectionFactory(provider, services);

            // Act
            var tenantServices = factory.CreateServiceCollection();

            // Assert
            var descriptor = tenantServices.SingleOrDefault(sd => sd.ServiceType == typeof(IService<string>));
            Assert.NotNull(descriptor);
            Assert.Equal(typeof(Service<string>), descriptor.ImplementationType);
            Assert.Equal(ServiceLifetime.Scoped, descriptor.Lifetime);
        }

        [Fact]
        public void CreateServiceCollection_ConsumesTenantScopedServiceDescriptors()
        {
            // Arrange
            var services = new ServiceCollection();
            services.AddTenantScoped<IService<string>, Service<string>>();
            var provider = services.BuildServiceProvider();
            var factory = new TenantServiceCollectionFactory(provider, services);

            // Act
            var tenantServices = factory.CreateServiceCollection();

            // Assert
            var descriptor = tenantServices.SingleOrDefault(sd => sd.ServiceType == typeof(IService<string>));
            Assert.NotNull(descriptor);
            Assert.Equal(typeof(Service<string>), descriptor.ImplementationType);
            Assert.Equal(ServiceLifetime.Singleton, descriptor.Lifetime);
        }

        [Fact]
        public void CreateServiceCollection_ConsumesSingletonServiceDescriptors_ForOpenGenerics()
        {
            // Arrange
            var services = new ServiceCollection();
            services.AddSingleton(typeof(IService<>), typeof(Service<>));
            var provider = services.BuildServiceProvider();
            var factory = new TenantServiceCollectionFactory(provider, services);

            // Act
            var tenantServices = factory.CreateServiceCollection();

            // Assert
            var descriptor = tenantServices.SingleOrDefault(sd => sd.ServiceType == typeof(IService<>));
            Assert.NotNull(descriptor);
            Assert.Equal(typeof(Service<>), descriptor.ImplementationType);
            Assert.Equal(ServiceLifetime.Singleton, descriptor.Lifetime);
        }

        [Fact]
        public void CreateServiceCollection_ResolvesSingletons_FromApplicationServices_ForClosedTypes()
        {
            // Arrange
            var services = new ServiceCollection();
            services.AddSingleton<IService<string>, Service<string>>();
            var provider = services.BuildServiceProvider();
            var factory = new TenantServiceCollectionFactory(provider, services);

            // Act
            var tenantServices = factory.CreateServiceCollection();
            var applicationSingleton = provider.GetService<IService<string>>();

            // Assert
            var descriptor = tenantServices.SingleOrDefault(sd => sd.ServiceType == typeof(IService<string>));
            Assert.NotNull(descriptor);
            Assert.NotNull(descriptor.ImplementationInstance);
            Assert.Equal(ServiceLifetime.Singleton, descriptor.Lifetime);
            Assert.Same(applicationSingleton, descriptor.ImplementationInstance);
        }

        [Fact]
        public void CreateServiceCollection_ResolvesSingletons_FromApplicationServices_ForClosedTypes_AsEnumerable()
        {
            // Arrange
            var service1 = new Service<string>();
            var service2 = new Service<string>();

            var services = new ServiceCollection();
            services.AddSingleton<IService<string>>(service1);
            services.AddSingleton<IService<string>>(service2);
            var provider = services.BuildServiceProvider();
            var factory = new TenantServiceCollectionFactory(provider, services);

            // Act
            var tenantServices = factory.CreateServiceCollection();
            var applicationSingleton = provider.GetService<IService<string>>();

            // Assert
            var descriptor = tenantServices.SingleOrDefault(sd => sd.ServiceType == typeof(IEnumerable<IService<string>>));
            Assert.NotNull(descriptor);
            Assert.NotNull(descriptor.ImplementationInstance);
            Assert.Equal(ServiceLifetime.Singleton, descriptor.Lifetime);

            var enumerable = (IEnumerable<IService<string>>)descriptor.ImplementationInstance;
            Assert.Equal(2, enumerable.Count());
        }

        [Fact]
        public void CreateServiceCollection_TenantScopedServices_ResolvedPerTenantServiceProvider()
        {
            // Arrange
            var services = new ServiceCollection();
            services.AddTenantScoped<IService<string>, Service<string>>();
            var provider = services.BuildServiceProvider();
            var factory = new TenantServiceCollectionFactory(provider, services);

            // Act
            var tenantServiceProvider1 = factory.CreateServiceCollection().BuildServiceProvider();
            var tenantServiceProvider2 = factory.CreateServiceCollection().BuildServiceProvider();

            // Assert
            var service1 = tenantServiceProvider1.GetService<IService<string>>();
            var service2 = tenantServiceProvider2.GetService<IService<string>>();

            Assert.NotSame(service1, service2);
        }

        public interface IService<T>
        {

        }

        public class Service<T> : IService<T>
        {

        }
    }
}
