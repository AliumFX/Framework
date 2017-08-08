// Copyright (c) Alium Fx. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Alium.Infrastructure
{
    using System;
    using System.Collections.Generic;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.DependencyModel;
    using Xunit;

    using Alium.Configuration;
    using Alium.DependencyInjection;
    using Alium.Features;
    using Alium.Modules;
    using Alium.Parts;

    /// <summary>
    /// Provides tests for the <see cref="FrameworkInitialiser"/> type.
    /// </summary>
    public class FrameworkInitialiserTests
    {
        [Fact]
        public void FromModules_ValidatesArguments()
        {
            // Arrange

            // Act

            // Assert
            Assert.Throws<ArgumentNullException>(() => FrameworkInitialiser.FromModules(null /* modules */, null /* configuration */));
            Assert.Throws<ArgumentNullException>(() => FrameworkInitialiser.FromModules(Array.Empty<IModule>(), null /* configuration */));
        }

        [Fact]
        public void FromModules_CreatesPartManager()
        {
            // Arrange
            var module = new TestModule();
            var configuration = new ConfigurationBuilder().Build();

            // Act
            var init = FrameworkInitialiser.FromModules(new IModule[] { module }, configuration);

            // Assert
            Assert.NotNull(init);
            Assert.NotNull(init.PartManager);
            Assert.Contains(init.PartManager.PartFeatureProviders, pfp => pfp is ModulePartFeatureProvider);
            Assert.Contains(init.PartManager.Parts, p => p is AssemblyPart && ((AssemblyPart)p).Assembly == typeof(TestModule).Assembly);
        }

        [Fact]
        public void FromModules_CreatesModuleProvider()
        {
            // Arrange
            var module = new TestModule();
            var configuration = new ConfigurationBuilder().Build();

            // Act
            var init = FrameworkInitialiser.FromModules(new IModule[] { module }, configuration);

            // Assert
            Assert.NotNull(init);
            Assert.NotNull(init.ModuleProvider);
            Assert.Contains(init.ModuleProvider.Modules, m => m == module);
        }

        [Fact]
        public void FromModules_CreatesFeatureProvider()
        {
            // Arrange
            var module = new TestModule();
            var configuration = new ConfigurationBuilder().Build();

            // Act
            var init = FrameworkInitialiser.FromModules(new IModule[] { module }, configuration);

            // Assert
            Assert.NotNull(init);
            Assert.NotNull(init.FeatureProvider);
            Assert.Contains(init.FeatureProvider.Features, f => f is TestFeature);
        }

        [Fact]
        public void FromModules_CreatesFeatureStateProvider()
        {
            // Arrange
            var module = new TestModule();
            var configuration = new ConfigurationBuilder().Build();

            // Act
            var init = FrameworkInitialiser.FromModules(new IModule[] { module }, configuration);

            // Assert
            Assert.NotNull(init);
            Assert.NotNull(init.FeatureStateProvider);
        }

        [Fact]
        public void FromDependencyContext_ValidatesArguments()
        {
            // Arrange

            // Act

            // Assert
            Assert.Throws<ArgumentNullException>(() => FrameworkInitialiser.FromModules(null /* modules */, null /* configuration */));
            Assert.Throws<ArgumentNullException>(() => FrameworkInitialiser.FromModules(Array.Empty<IModule>(), null /* configuration */));
        }

        [Fact]
        public void FromDependencyContext_CreatesPartManager()
        {
            // Arrange
            var dependencyContext = DependencyContext.Default;
            var configuration = new ConfigurationBuilder().Build();

            // Act
            var init = FrameworkInitialiser.FromDependencyContext(dependencyContext, configuration);

            // Assert
            Assert.NotNull(init);
            Assert.NotNull(init.PartManager);
            Assert.Contains(init.PartManager.PartFeatureProviders, pfp => pfp is ModulePartFeatureProvider);
            Assert.Contains(init.PartManager.Parts, p => p is AssemblyPart && ((AssemblyPart)p).Assembly == typeof(TestModule).Assembly);
        }

        [Fact]
        public void FromDependencyContext_CreatesModuleProvider()
        {
            // Arrange
            var dependencyContext = DependencyContext.Default;
            var configuration = new ConfigurationBuilder().Build();

            // Act
            var init = FrameworkInitialiser.FromDependencyContext(dependencyContext, configuration);

            // Assert
            Assert.NotNull(init);
            Assert.NotNull(init.ModuleProvider);
        }

        [Fact]
        public void FromDependencyContext_CreatesFeatureProvider()
        {
            // Arrange
            var dependencyContext = DependencyContext.Default;
            var configuration = new ConfigurationBuilder().Build();

            // Act
            var init = FrameworkInitialiser.FromDependencyContext(dependencyContext, configuration);

            // Assert
            Assert.NotNull(init);
            Assert.NotNull(init.FeatureProvider);
        }

        [Fact]
        public void FromDependencyContext_CreatesFeatureStateProvider()
        {
            // Arrange
            var dependencyContext = DependencyContext.Default;
            var configuration = new ConfigurationBuilder().Build();

            // Act
            var init = FrameworkInitialiser.FromDependencyContext(dependencyContext, configuration);

            // Assert
            Assert.NotNull(init);
            Assert.NotNull(init.FeatureStateProvider);
        }

        [Fact]
        public void AddServices_AddsPartManager()
        {
            // Arrange
            var module = new TestModule();
            var configuration = new ConfigurationBuilder().Build();
            var init = FrameworkInitialiser.FromModules(new IModule[] { module }, configuration);
            var services = new ServiceCollection();

            // Act
            init.AddServices(services);

            // Assert
            Assert.Contains(services, sd => sd.ServiceType == typeof(IPartManager) && sd.Lifetime == ServiceLifetime.Singleton);
        }

        [Fact]
        public void AddServices_AddsModuleProvider()
        {
            // Arrange
            var module = new TestModule();
            var configuration = new ConfigurationBuilder().Build();
            var init = FrameworkInitialiser.FromModules(new IModule[] { module }, configuration);
            var services = new ServiceCollection();

            // Act
            init.AddServices(services);

            // Assert
            Assert.Contains(services, sd => sd.ServiceType == typeof(IModuleProvider) && sd.Lifetime == ServiceLifetime.Singleton);
        }

        [Fact]
        public void AddServices_AddsServicesFromModules()
        {
            // Arrange
            var module = new TestModule();
            var configuration = new ConfigurationBuilder().Build();
            var init = FrameworkInitialiser.FromModules(new IModule[] { module }, configuration);
            var services = new ServiceCollection();

            // Act
            init.AddServices(services);

            // Assert
            Assert.Contains(services, sd => sd.ServiceType == typeof(ModuleService));
        }

        [Fact]
        public void AddServices_AddsFeatureProvider()
        {
            // Arrange
            var module = new TestModule();
            var configuration = new ConfigurationBuilder().Build();
            var init = FrameworkInitialiser.FromModules(new IModule[] { module }, configuration);
            var services = new ServiceCollection();

            // Act
            init.AddServices(services);

            // Assert
            Assert.Contains(services, sd => sd.ServiceType == typeof(IFeatureProvider) && sd.Lifetime == ServiceLifetime.Singleton);
        }

        [Fact]
        public void AddServices_AddsServicesFromFeatures()
        {
            // Arrange
            var module = new TestModule();
            var configuration = new ConfigurationBuilder().Build();
            var init = FrameworkInitialiser.FromModules(new IModule[] { module }, configuration);
            var services = new ServiceCollection();

            // Act
            init.AddServices(services);

            // Assert
            Assert.Contains(services, sd => sd.ServiceType == typeof(FeatureService));
        }

        [Fact]
        public void AddServices_AddsFeatureStateProvider()
        {
            // Arrange
            var module = new TestModule();
            var configuration = new ConfigurationBuilder().Build();
            var init = FrameworkInitialiser.FromModules(new IModule[] { module }, configuration);
            var services = new ServiceCollection();

            // Act
            init.AddServices(services);

            // Assert
            Assert.Contains(services, sd => sd.ServiceType == typeof(IFeatureStateProvider) && sd.Lifetime == ServiceLifetime.Singleton);
        }

        [Fact]
        public void ExtendConfiguration_AddsModuleConfiguration()
        {
            // Arrange
            var module = new TestModule();
            var configuration = new ConfigurationBuilder().Build();
            var init = FrameworkInitialiser.FromModules(new IModule[] { module }, configuration);
            var builder = new ConfigurationBuilder();
            var context = new WebHostBuilderContext();

            // Act
            init.ExtendConfiguration(context, builder);

            // Assert
            Assert.Contains(builder.Sources, cs => cs is ModuleConfigurationSource);
        }

        [Fact]
        public void ExtendConfiguration_AddsFeatureConfiguration()
        {
            // Arrange
            var module = new TestModule();
            var configuration = new ConfigurationBuilder().Build();
            var init = FrameworkInitialiser.FromModules(new IModule[] { module }, configuration);
            var builder = new ConfigurationBuilder();
            var context = new WebHostBuilderContext();

            // Act
            init.ExtendConfiguration(context, builder);

            // Assert
            Assert.Contains(builder.Sources, cs => cs is FeatureConfigurationSource);
        }

        private class TestModule : ModuleBase, IFeaturesBuilder, IServicesBuilder, IAppConfigurationExtender
        {
            public TestModule()
                : base(new ModuleId("Test")) { }

            public void BuildConfiguration(WebHostBuilderContext context, IConfigurationBuilder builder)
            {
                builder.Add(new ModuleConfigurationSource());
            }

            public void BuildFeatures(ICollection<IFeature> features)
            {
                features.Add(new TestFeature());
            }

            public void BuildServices(IServiceCollection services)
            {
                services.AddTransient<ModuleService>();
            }
        }

        private class TestFeature : FeatureBase, IServicesBuilder, IAppConfigurationExtender
        {
            public TestFeature()
                : base(new FeatureId(new ModuleId("Test"), "Test"), enabledByDefault: true) { }

            public void BuildConfiguration(WebHostBuilderContext context, IConfigurationBuilder builder)
            {
                builder.Add(new FeatureConfigurationSource());
            }

            public void BuildServices(IServiceCollection services)
            {
                services.AddScoped<FeatureService>();
            }
        }

        private class ModuleService
        {

        }

        private class FeatureService
        {

        }

        private class ModuleConfigurationSource : IConfigurationSource
        {
            public IConfigurationProvider Build(IConfigurationBuilder builder)
            {
                throw new NotImplementedException();
            }
        }

        private class FeatureConfigurationSource : IConfigurationSource
        {
            public IConfigurationProvider Build(IConfigurationBuilder builder)
            {
                throw new NotImplementedException();
            }
        }
    }
}
