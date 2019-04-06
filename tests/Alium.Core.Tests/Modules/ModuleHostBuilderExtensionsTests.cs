// Copyright (c) Alium Fx. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Alium.Modules
{
    using System;
    using System.Linq;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.DependencyModel;
    using Microsoft.Extensions.Hosting;
    using Xunit;

    using Alium.Parts;
    using System.Collections.Generic;

    /// <summary>
    /// Provides tests for the <see cref="ModuleHostBuilderExtensions"/> type.
    /// </summary>
    public class ModuleHostBuilderExtensionsTests
    {
        [Fact]
        public void UseModules_ValidatesArguments()
        {
            // Arrange
            var builder = new TestHostBuilder();

            // Act

            // Assert
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference or unconstrained type parameter.
            Assert.Throws<ArgumentNullException>(() => ModuleHostBuilderExtensions.UseModules(null /* builder */, null /* modules */));
            Assert.Throws<ArgumentNullException>(() => ModuleHostBuilderExtensions.UseModules(builder, null /* modules */));
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference or unconstrained type parameter.
        }

        [Fact]
        public void UseModules_AddsModuleProvider_ToServiceCollection()
        {
            // Arrange
            var builder = new TestHostBuilder();

            // Act
            ModuleHostBuilderExtensions.UseModules(builder);

            // Assert
            var descriptor = builder.Services.FirstOrDefault(sd => sd.ServiceType == typeof(IModuleProvider));
            Assert.NotNull(descriptor);
        }

        [Fact]
        public void UseModules_AddsModuleProvider_ToServiceCollection_WithProvidedModules()
        {
            // Arrange
            var builder = new TestHostBuilder();
            var module = new TestModule();


            // Act
            ModuleHostBuilderExtensions.UseModules(builder, module);

            // Assert
            var descriptor = builder.Services.FirstOrDefault(sd => sd.ServiceType == typeof(IModuleProvider));
            Assert.NotNull(descriptor);

            var provider = (IModuleProvider)descriptor.ImplementationInstance;
            Assert.Equal(1, provider.Modules.Count);
            Assert.Contains(provider.Modules, m => m == module);
        }

        [Fact]
        public void UseModules_AddsPartManager_ToServiceCollection()
        {
            // Arrange
            var builder = new TestHostBuilder();

            // Act
            ModuleHostBuilderExtensions.UseModules(builder);

            // Assert
            var descriptor = builder.Services.FirstOrDefault(sd => sd.ServiceType == typeof(IPartManager));
            Assert.NotNull(descriptor);
        }

        [Fact]
        public void UseModules_AddsPartManager_ToServiceCollection_WithProvidedModuleAssemblyParts()
        {
            // Arrange
            var builder = new TestHostBuilder();
            var module = new TestModule();

            // Act
            ModuleHostBuilderExtensions.UseModules(builder, module);

            // Assert
            var descriptor = builder.Services.FirstOrDefault(sd => sd.ServiceType == typeof(IPartManager));
            Assert.NotNull(descriptor);

            var manager = (IPartManager)descriptor.ImplementationInstance;
            Assert.Equal(1, manager.Parts.Count);

            var assemblyPart = (AssemblyPart)manager.Parts[0];
            Assert.Equal(GetType().Assembly, assemblyPart.Assembly);
        }

        [Fact]
        public void UseDiscoveredModules_ValidatesArguments()
        {
            // Arrange
            var builder = new TestHostBuilder();

            // Act

            // Assert
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference or unconstrained type parameter.
            Assert.Throws<ArgumentNullException>(() => ModuleHostBuilderExtensions.UseDiscoveredModules(null /* builder */));
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference or unconstrained type parameter.
        }

        [Fact]
        public void UseDiscoveredModules_AddsModuleProvider_ToServiceCollection()
        {
            // Arrange
            var builder = new TestHostBuilder();

            // Act
            ModuleHostBuilderExtensions.UseDiscoveredModules(builder);

            // Assert
            var descriptor = builder.Services.FirstOrDefault(sd => sd.ServiceType == typeof(IModuleProvider));
            Assert.NotNull(descriptor);
        }

        [Fact]
        public void UseDiscoveredModules_AddsModuleProvider_ToServiceCollection_WithDiscoveredModules()
        {
            // Arrange
            var builder = new TestHostBuilder();

            // Act
            ModuleHostBuilderExtensions.UseDiscoveredModules(builder);

            // Assert
            var descriptor = builder.Services.FirstOrDefault(sd => sd.ServiceType == typeof(IModuleProvider));
            Assert.NotNull(descriptor);

            var provider = (IModuleProvider)descriptor.ImplementationInstance;
            Assert.Contains(provider.Modules, m => m is CoreModule);
        }

        [Fact]
        public void UseDiscoveredModules_AddsPartManager_ToServiceCollection()
        {
            // Arrange
            var builder = new TestHostBuilder();

            // Act
            ModuleHostBuilderExtensions.UseDiscoveredModules(builder);

            // Assert
            var descriptor = builder.Services.FirstOrDefault(sd => sd.ServiceType == typeof(IPartManager));
            Assert.NotNull(descriptor);
        }

        [Fact]
        public void UseDiscoveredModules_AddsPartManager_ToServiceCollection_WithDiscoveredModuleAssemblyParts()
        {
            // Arrange
            var builder = new TestHostBuilder();

            // Act
            ModuleHostBuilderExtensions.UseDiscoveredModules(builder);

            // Assert
            var descriptor = builder.Services.FirstOrDefault(sd => sd.ServiceType == typeof(IPartManager));
            Assert.NotNull(descriptor);

            var manager = (IPartManager)descriptor.ImplementationInstance;
            
            Assert.Contains(manager.Parts, p => p.Name == "Alium.Core");
        }

        private class TestHostBuilder : IHostBuilder
        {
            public IServiceCollection Services = new ServiceCollection();
            public HostBuilderContext Context = new HostBuilderContext(new Dictionary<object, object>());

            public IDictionary<object, object> Properties { get; } = new Dictionary<object, object>();

            public IHost Build()
            {
                throw new NotImplementedException();
            }

            public IHostBuilder ConfigureAppConfiguration(Action<HostBuilderContext, IConfigurationBuilder> configureDelegate)
            {
                return this;
            }

            public IHostBuilder ConfigureContainer<TContainerBuilder>(Action<HostBuilderContext, TContainerBuilder> configureDelegate)
            {
                return this;
            }

            public IHostBuilder ConfigureHostConfiguration(Action<IConfigurationBuilder> configureDelegate)
            {
                return this;
            }

            public IHostBuilder ConfigureServices(Action<IServiceCollection> configureServices)
            {
                configureServices(Services);
                return this;
            }

            public IHostBuilder ConfigureServices(Action<HostBuilderContext, IServiceCollection> configureServices)
            {
                configureServices(Context, Services);
                return this;
            }

            public string? GetSetting(string key) => default;

            public IHostBuilder UseServiceProviderFactory<TContainerBuilder>(IServiceProviderFactory<TContainerBuilder> factory) => this;

            public IHostBuilder UseServiceProviderFactory<TContainerBuilder>(Func<HostBuilderContext, IServiceProviderFactory<TContainerBuilder>> factory) => this;

            public IHostBuilder UseSetting(string key, string value)
            {
                return this;
            }
        }

        private class TestModule : ModuleBase
        {
            public TestModule()
                : base(new ModuleId("Test"), name: "Test") { }
        }
    }
}
