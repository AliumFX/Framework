// Copyright (c) Alium Fx. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Alium.Modules
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using Moq;
    using Xunit;
    using Microsoft.AspNetCore.Builder;

    /// <summary>
    /// Provides tests for the <see cref="ModuleInitialiserStartupFilter"/> type.
    /// </summary>
    public class ModuleInitialiserStartupFilterTests
    {
        [Fact]
        public void Constructor_ValidatesArguments()
        {
            // Arrange

            // Act

            // Assert
            Assert.Throws<ArgumentNullException>(() =>
                new ModuleInitialiserStartupFilter(
                    null /* serviceProvider */,
                    null /* moduleProvider */));
            Assert.Throws<ArgumentNullException>(() =>
                new ModuleInitialiserStartupFilter(
                    Mock.Of<IServiceProvider>(),
                    null /* moduleProvider */));
        }

        [Fact]
        public void Configure_ValidatesArguments()
        {
            // Arrange
            var filter = new ModuleInitialiserStartupFilter(
                Mock.Of<IServiceProvider>(),
                Mock.Of<IModuleProvider>());

            // Act

            // Assert
            Assert.Throws<ArgumentNullException>(() => filter.Configure(null /* next */));
        }

        [Fact]
        public void Configure_ExecutesModuleInitialisation()
        {
            // Arrange
            ModuleInitialisationContext capturedContext = null;

            var serviceProvider = Mock.Of<IServiceProvider>();
            var filter = new ModuleInitialiserStartupFilter(
                serviceProvider,
                CreateModuleProvider(
                    CreateModule(mic => capturedContext = mic)));

            Action<IApplicationBuilder> configure = _ => { };

            // Act
            filter.Configure(configure);

            // Assert
            Assert.NotNull(capturedContext);
            Assert.Same(serviceProvider, capturedContext.ApplicationServices);
        }

        private IModuleProvider CreateModuleProvider(IModule module)
        {
            var mock = new Mock<IModuleProvider>();

            mock.Setup(mp => mp.Modules)
                .Returns(new ReadOnlyCollection<IModule>(
                    new List<IModule>() { module }));

            return mock.Object;
        }

        private IModule CreateModule(Action<ModuleInitialisationContext> onInit)
        {
            var mock = new Mock<IModule>();

            mock.Setup(m => m.Initialise(It.IsAny<ModuleInitialisationContext>()))
                .Callback<ModuleInitialisationContext>(onInit);

            return mock.Object;
        }
    }
}
