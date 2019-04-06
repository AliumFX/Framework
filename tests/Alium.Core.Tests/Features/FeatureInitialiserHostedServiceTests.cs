// Copyright (c) Alium Fx. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Alium.Features
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Builder;
    using Moq;
    using Xunit;

    using Alium.Modules;

    /// <summary>
    /// Provides tests for the <see cref="FeatureInitialiserHostedService"/> type.
    /// </summary>
    public class FeatureInitialiserHostedServiceTests
    {
        [Fact]
        public void Constructor_ValidatesArguments()
        {
            // Arrange

            // Act

            // Assert
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference or unconstrained type parameter.
            Assert.Throws<ArgumentNullException>(() =>
                new FeatureInitialiserHostedService(
                    null /* serviceProvider */,
                    null /* featureProvider */,
                    null /* featureStateProvider */));
            Assert.Throws<ArgumentNullException>(() =>
                new FeatureInitialiserHostedService(
                    Mock.Of<IServiceProvider>(),
                    null /* featureProvider */,
                    null /* featureStateProvider */));
            Assert.Throws<ArgumentNullException>(() =>
                new FeatureInitialiserHostedService(
                    Mock.Of<IServiceProvider>(),
                    Mock.Of<IFeatureProvider>(),
                    null /* featureStateProvider */));
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference or unconstrained type parameter.
        }

        [Fact]
        public async Task StartAsync_ExecutesFeatureInitialisation_WhenFeatureEnabled()
        {
            // Arrange
            FeatureInitialisationContext? capturedContext = null;

            var featureId = new FeatureId(new ModuleId("Test"), "Test");
            var serviceProvider = Mock.Of<IServiceProvider>();
            var service = new FeatureInitialiserHostedService(
                serviceProvider,
                CreateFeatureProvider(
                    CreateFeature(featureId, fic => capturedContext = fic)),
                CreateFeatureStateProvider(featureId, true));

            // Act
            await service.StartAsync(CancellationToken.None);

            // Assert
            Assert.NotNull(capturedContext);
            Assert.Same(serviceProvider, capturedContext?.ApplicationServices);
        }

        [Fact]
        public async Task StartAsync_SkipsFeatureInitialisation_WhenFeatureDisabled()
        {
            // Arrange
            FeatureInitialisationContext? capturedContext = null;

            var featureId = new FeatureId(new ModuleId("Test"), "Test");
            var serviceProvider = Mock.Of<IServiceProvider>();
            var service = new FeatureInitialiserHostedService(
                serviceProvider,
                CreateFeatureProvider(
                    CreateFeature(featureId, fic => capturedContext = fic)),
                CreateFeatureStateProvider(featureId, false));

            // Act
            await service.StartAsync(CancellationToken.None);

            // Assert
            Assert.Null(capturedContext);
        }

        private IFeatureProvider CreateFeatureProvider(IFeature feature)
        {
            var mock = new Mock<IFeatureProvider>();

            mock.Setup(fp => fp.Features)
                .Returns(new ReadOnlyCollection<IFeature>(
                    new List<IFeature>() { feature }));

            return mock.Object;
        }

        private IFeature CreateFeature(FeatureId featureId, Action<FeatureInitialisationContext> onInit)
        {
            var mock = new Mock<IFeature>();

            mock.Setup(f => f.Id)
                .Returns(featureId);

            mock.Setup(f => f.Initialise(It.IsAny<FeatureInitialisationContext>()))
                .Callback<FeatureInitialisationContext>(onInit);

            return mock.Object;
        }

        private IFeatureStateProvider CreateFeatureStateProvider(FeatureId featureId, bool enabled)
        {
            var mock = new Mock<IFeatureStateProvider>();

            mock.Setup(fsp => fsp.GetFeatureState(It.Is<FeatureId>(fid => fid.Equals(featureId))))
                .Returns<FeatureId>(fid => new FeatureState(fid, null, enabled));

            return mock.Object;
        }
    }
}
