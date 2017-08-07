// Copyright (c) Alium Fx. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Alium.Modules
{
    using System;
    using System.Linq;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.DependencyModel;
    using Xunit;

    using Alium.Parts;

    /// <summary>
    /// Provides tests for the <see cref="ModuleWebHostBuilderExtensions"/> type.
    /// </summary>
    public class ModuleWebHostBuilderExtensionsTests
    {
        [Fact]
        public void UseModules_ValidatesArguments()
        {
            // Arrange
            var builder = new TestWebHostBuilder();

            // Act

            // Assert
            Assert.Throws<ArgumentNullException>(() => ModuleWebHostBuilderExtensions.UseModules(null /* builder */, (IModule[])null /* modules */));
            Assert.Throws<ArgumentNullException>(() => ModuleWebHostBuilderExtensions.UseModules(builder, (IModule[])null /* modules */));
        }

        [Fact]
        public void UseModules_AddsModuleProvider_ToServiceCollection()
        {
            // Arrange
            var builder = new TestWebHostBuilder();

            // Act
            ModuleWebHostBuilderExtensions.UseModules(builder);

            // Assert
            var descriptor = builder.Services.FirstOrDefault(sd => sd.ServiceType == typeof(IModuleProvider));
            Assert.NotNull(descriptor);
        }

        [Fact]
        public void UseModules_AddsModuleProvider_ToServiceCollection_WithProvidedModules()
        {
            // Arrange
            var builder = new TestWebHostBuilder();
            var module = new TestModule();


            // Act
            ModuleWebHostBuilderExtensions.UseModules(builder, module);

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
            var builder = new TestWebHostBuilder();

            // Act
            ModuleWebHostBuilderExtensions.UseModules(builder);

            // Assert
            var descriptor = builder.Services.FirstOrDefault(sd => sd.ServiceType == typeof(IPartManager));
            Assert.NotNull(descriptor);
        }

        [Fact]
        public void UseModules_AddsPartManager_ToServiceCollection_WithProvidedModuleAssemblyParts()
        {
            // Arrange
            var builder = new TestWebHostBuilder();
            var module = new TestModule();

            // Act
            ModuleWebHostBuilderExtensions.UseModules(builder, module);

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
            var builder = new TestWebHostBuilder();

            // Act

            // Assert
            Assert.Throws<ArgumentNullException>(() => ModuleWebHostBuilderExtensions.UseDiscoveredModules(null /* builder */));
        }

        [Fact]
        public void UseDiscoveredModules_AddsModuleProvider_ToServiceCollection()
        {
            // Arrange
            var builder = new TestWebHostBuilder();

            // Act
            ModuleWebHostBuilderExtensions.UseDiscoveredModules(builder);

            // Assert
            var descriptor = builder.Services.FirstOrDefault(sd => sd.ServiceType == typeof(IModuleProvider));
            Assert.NotNull(descriptor);
        }

        [Fact]
        public void UseDiscoveredModules_AddsModuleProvider_ToServiceCollection_WithDiscoveredModules()
        {
            // Arrange
            var builder = new TestWebHostBuilder();

            // Act
            ModuleWebHostBuilderExtensions.UseDiscoveredModules(builder);

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
            var builder = new TestWebHostBuilder();

            // Act
            ModuleWebHostBuilderExtensions.UseDiscoveredModules(builder);

            // Assert
            var descriptor = builder.Services.FirstOrDefault(sd => sd.ServiceType == typeof(IPartManager));
            Assert.NotNull(descriptor);
        }

        [Fact]
        public void UseDiscoveredModules_AddsPartManager_ToServiceCollection_WithDiscoveredModuleAssemblyParts()
        {
            // Arrange
            var builder = new TestWebHostBuilder();

            // Act
            ModuleWebHostBuilderExtensions.UseDiscoveredModules(builder);

            // Assert
            var descriptor = builder.Services.FirstOrDefault(sd => sd.ServiceType == typeof(IPartManager));
            Assert.NotNull(descriptor);

            var manager = (IPartManager)descriptor.ImplementationInstance;
            
            Assert.Contains(manager.Parts, p => p.Name == "Alium.Core");
        }

        private class TestWebHostBuilder : IWebHostBuilder
        {
            public IServiceCollection Services = new ServiceCollection();

            public IWebHost Build()
            {
                throw new NotImplementedException();
            }

            public IWebHostBuilder ConfigureAppConfiguration(Action<WebHostBuilderContext, IConfigurationBuilder> configureDelegate)
            {
                return this;
            }

            public IWebHostBuilder ConfigureServices(Action<IServiceCollection> configureServices)
            {
                configureServices(Services);
                return this;
            }

            public IWebHostBuilder ConfigureServices(Action<WebHostBuilderContext, IServiceCollection> configureServices)
            {
                return this;
            }

            public string GetSetting(string key)
            {
                throw new NotImplementedException();
            }

            public IWebHostBuilder UseSetting(string key, string value)
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
