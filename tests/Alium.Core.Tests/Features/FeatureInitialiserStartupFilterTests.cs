// Copyright (c) Alium Fx. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Alium.Features
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using Microsoft.AspNetCore.Builder;
    using Moq;
    using Xunit;

    using Alium.Modules;

    /// <summary>
    /// Provides tests for the <see cref="FeatureInitialiserStartupFilter"/> type.
    /// </summary>
    public class FeatureInitialiserStartupFilterTests
    {
        [Fact]
        public void Constructor_ValidatesArguments()
        {
            // Arrange

            // Act

            // Assert
            Assert.Throws<ArgumentNullException>(() =>
                new FeatureInitialiserStartupFilter(
                    null /* serviceProvider */,
                    null /* featureProvider */,
                    null /* featureStateProvider */));
            Assert.Throws<ArgumentNullException>(() =>
                new FeatureInitialiserStartupFilter(
                    Mock.Of<IServiceProvider>(),
                    null /* featureProvider */,
                    null /* featureStateProvider */));
            Assert.Throws<ArgumentNullException>(() =>
                new FeatureInitialiserStartupFilter(
                    Mock.Of<IServiceProvider>(),
                    Mock.Of<IFeatureProvider>(),
                    null /* featureStateProvider */));
        }

        [Fact]
        public void Configure_ValidatesArguments()
        {
            // Arrange
            var filter = new FeatureInitialiserStartupFilter(
                Mock.Of<IServiceProvider>(),
                Mock.Of<IFeatureProvider>(),
                Mock.Of<IFeatureStateProvider>());

            // Act

            // Assert
            Assert.Throws<ArgumentNullException>(() => filter.Configure(null /* next */));
        }

        [Fact]
        public void Configure_ExecutesFeatureInitialisation_WhenFeatureEnabled()
        {
            // Arrange
            FeatureInitialisationContext capturedContext = null;

            var featureId = new FeatureId(new ModuleId("Test"), "Test");
            var serviceProvider = Mock.Of<IServiceProvider>();
            var filter = new FeatureInitialiserStartupFilter(
                serviceProvider,
                CreateFeatureProvider(
                    CreateFeature(featureId, fic => capturedContext = fic)),
                CreateFeatureStateProvider(featureId, true));

            Action<IApplicationBuilder> configure = _ => { };

            // Act
            filter.Configure(configure);

            // Assert
            Assert.NotNull(capturedContext);
            Assert.Same(serviceProvider, capturedContext.ApplicationServices);
        }

        [Fact]
        public void Configure_SkipsFeatureInitialisation_WhenFeatureDisabled()
        {
            // Arrange
            FeatureInitialisationContext capturedContext = null;

            var featureId = new FeatureId(new ModuleId("Test"), "Test");
            var serviceProvider = Mock.Of<IServiceProvider>();
            var filter = new FeatureInitialiserStartupFilter(
                serviceProvider,
                CreateFeatureProvider(
                    CreateFeature(featureId, fic => capturedContext = fic)),
                CreateFeatureStateProvider(featureId, false));

            Action<IApplicationBuilder> configure = _ => { };

            // Act
            filter.Configure(configure);

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
