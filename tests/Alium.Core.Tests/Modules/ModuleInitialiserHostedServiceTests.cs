// Copyright (c) Alium Fx. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Alium.Modules
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Threading;
    using System.Threading.Tasks;
    using Moq;
    using Xunit;

    /// <summary>
    /// Provides tests for the <see cref="ModuleInitialiserHostedService"/> type.
    /// </summary>
    public class ModuleInitialiserHostedServiceTests
    {
        [Fact]
        public void Constructor_ValidatesArguments()
        {
            // Arrange

            // Act

            // Assert
            Assert.Throws<ArgumentNullException>("serviceProvider", () =>
                new ModuleInitialiserHostedService(
                    null! /* serviceProvider */,
                    Mock.Of<IModuleProvider>()));
            Assert.Throws<ArgumentNullException>("moduleProvider", () =>
                new ModuleInitialiserHostedService(
                    Mock.Of<IServiceProvider>(),
                    null! /* moduleProvider */));
        }

        [Fact]
        public async Task StartAsync_ExecutesModuleInitialisation()
        {
            // Arrange
            ModuleInitialisationContext? capturedContext = null;

            var serviceProvider = Mock.Of<IServiceProvider>();
            var service = new ModuleInitialiserHostedService(
                serviceProvider,
                CreateModuleProvider(
                    CreateModule(mic => capturedContext = mic)));

            // Act
            await service.StartAsync(CancellationToken.None);

            // Assert
            Assert.NotNull(capturedContext);
            Assert.Same(serviceProvider, capturedContext?.ApplicationServices);
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
