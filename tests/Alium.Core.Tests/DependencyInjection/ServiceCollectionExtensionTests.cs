// Copyright (c) Alium FX. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Alium.DependencyInjection
{
    using System;
    using System.Linq;
    using Microsoft.Extensions.DependencyInjection;
    using Xunit;

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

        private class TestModule : ModuleBase, IServicesBuilder
        {
            public TestModule()
                : base(new ModuleId("Test")) { }

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
