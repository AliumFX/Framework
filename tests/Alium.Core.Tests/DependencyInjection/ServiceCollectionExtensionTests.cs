// Copyright (c) Alium FX. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Alium.DependencyInjection
{
    using System;
    using System.Linq;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Xunit;

    using Alium.Features;
    using Alium.Modules;

    /// <summary>
    /// Provides tests for the <see cref="ServiceCollectionExtensions"/> type.
    /// </summary>
    public class ServiceCollectionExtensionTests
    {
        [Fact]
        public void AddModuleServices_ValidatesArguments()
        {
            // Arrange
            var services = new ServiceCollection();

            // Act

            // Assert
            Assert.Throws<ArgumentNullException>(() => ServiceCollectionExtensions.AddModuleServices(null /* services */, null /* moduleProvider */));
            Assert.Throws<ArgumentNullException>(() => ServiceCollectionExtensions.AddModuleServices(services, null /* moduleProvider */));
        }

        [Fact]
        public void AddModuleServices_AddsModuleProvider()
        {
            // Arrange
            var module = new TestModule();
            var moduleProvider = new ModuleProvider(new[] { module });
            var services = new ServiceCollection();

            // Act
            ServiceCollectionExtensions.AddModuleServices(services, moduleProvider);

            // Assert
            var descriptor = services.FirstOrDefault(sd => sd.ServiceType == typeof(IModuleProvider));
            Assert.NotNull(descriptor);
            Assert.Equal(ServiceLifetime.Singleton, descriptor.Lifetime);
        }

        [Fact]
        public void AddModuleServices_AddsServicesFromModule()
        {
            // Arrange
            var module = new TestModule();
            var moduleProvider = new ModuleProvider(new[] { module });
            var services = new ServiceCollection();

            // Act
            ServiceCollectionExtensions.AddModuleServices(services, moduleProvider);

            // Assert
            var descriptor = services.FirstOrDefault(sd => sd.ServiceType == typeof(IServiceOne));
            Assert.NotNull(descriptor);
        }

        [Fact]
        public void AddFeatureServices_ValidatesArguments()
        {
            // Arrange
            var services = new ServiceCollection();
            var featureProvider = new FeatureProvider(new[] { new TestFeature() });

            // Act

            // Assert
            Assert.Throws<ArgumentNullException>(() => ServiceCollectionExtensions.AddFeatureServices(null /* services */, null /* featureProvider */, null /* featureStateProvider */));
            Assert.Throws<ArgumentNullException>(() => ServiceCollectionExtensions.AddFeatureServices(services, null /* featureProvider */, null /* featureStateProvider */));
            Assert.Throws<ArgumentNullException>(() => ServiceCollectionExtensions.AddFeatureServices(services, featureProvider, null /* featureStateProvider */));
        }

        [Fact]
        public void AddFeatureServices_AddsFeatureProvider()
        {
            // Arrange
            var services = new ServiceCollection();
            var featureProvider = new FeatureProvider(new[] { new TestFeature() });
            var configuration = new ConfigurationBuilder().Build();
            var featureStateProvider = new FeatureStateProvider(featureProvider, configuration);

            // Act
            ServiceCollectionExtensions.AddFeatureServices(services, featureProvider, featureStateProvider);

            // Assert
            var descriptor = services.FirstOrDefault(sd => sd.ServiceType == typeof(IFeatureProvider));
            Assert.NotNull(descriptor);
            Assert.Equal(ServiceLifetime.Singleton, descriptor.Lifetime);
        }

        [Fact]
        public void AddFeatureServices_AddsFeatureStateProvider()
        {
            // Arrange
            var services = new ServiceCollection();
            var featureProvider = new FeatureProvider(new[] { new TestFeature() });
            var configuration = new ConfigurationBuilder().Build();
            var featureStateProvider = new FeatureStateProvider(featureProvider, configuration);

            // Act
            ServiceCollectionExtensions.AddFeatureServices(services, featureProvider, featureStateProvider);

            // Assert
            var descriptor = services.FirstOrDefault(sd => sd.ServiceType == typeof(IFeatureStateProvider));
            Assert.NotNull(descriptor);
            Assert.Equal(ServiceLifetime.Singleton, descriptor.Lifetime);
        }

        [Fact]
        public void AddFeatureServices_AddsServicesFromFeature_WhenFeatureEnabled()
        {
            // Arrange
            var services = new ServiceCollection();
            var featureProvider = new FeatureProvider(new[] { new TestFeature() });
            var configuration = new ConfigurationBuilder().Build();
            var featureStateProvider = new FeatureStateProvider(featureProvider, configuration);

            // Act
            ServiceCollectionExtensions.AddFeatureServices(services, featureProvider, featureStateProvider);

            // Assert
            var descriptor = services.FirstOrDefault(sd => sd.ServiceType == typeof(IServiceOne));
            Assert.NotNull(descriptor);
            Assert.Equal(ServiceLifetime.Transient, descriptor.Lifetime);
            var featureDescriptor = services.FirstOrDefault(sd => sd.ServiceType == typeof(IFeature<IServiceOne>));
            Assert.NotNull(featureDescriptor);
            Assert.Equal(ServiceLifetime.Transient, featureDescriptor.Lifetime);
        }

        [Fact]
        public void AddFeatureServices_DoesNotAddServicesFromFeature_WhenFeatureDisabled()
        {
            // Arrange
            var services = new ServiceCollection();
            var featureProvider = new FeatureProvider(new[] { new TestFeature(enabledByDefault: false) });
            var configuration = new ConfigurationBuilder().Build();
            var featureStateProvider = new FeatureStateProvider(featureProvider, configuration);

            // Act
            ServiceCollectionExtensions.AddFeatureServices(services, featureProvider, featureStateProvider);

            // Assert
            var descriptor = services.FirstOrDefault(sd => sd.ServiceType == typeof(IServiceOne));
            Assert.Null(descriptor);
            var featureDescriptor = services.FirstOrDefault(sd => sd.ServiceType == typeof(IFeature<IServiceOne>));
            Assert.NotNull(featureDescriptor);
            Assert.Equal(ServiceLifetime.Transient, featureDescriptor.Lifetime);
        }

        private class TestModule : ModuleBase, IServicesBuilder
        {
            public TestModule()
                : base(new ModuleId("Test")) { }

            public void BuildServices(IServiceCollection services)
            {
                services.AddTransient<IServiceOne, ServiceOne>();
            }
        }

        private class TestFeature : FeatureBase, IServicesBuilder
        {
            public TestFeature(bool enabledByDefault = true)
                : base(new FeatureId(new ModuleId("Test"), "Test"), enabledByDefault: enabledByDefault) { }
            
            public void BuildServices(IServiceCollection services)
            {
                services.AddTransient<IServiceOne, ServiceOne>();
            }
        }


        private interface IServiceOne
        {

        }

        public class ServiceOne : IServiceOne
        {

        }
    }
}
