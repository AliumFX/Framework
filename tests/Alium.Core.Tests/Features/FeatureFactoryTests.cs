// Copyright (c) Alium Fx. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Alium.Features
{
    using System;
    using System.Collections.Generic;
    using Microsoft.Extensions.Configuration;
    using Moq;
    using Xunit;

    using Alium.Modules;

    /// <summary>
    /// Provides tests of the <see cref="FeatureFactory{TService}"/> type.
    /// </summary>
    public class FeatureFactoryTests
    {
        [Fact]
        public void Constructor_ValidatesArguments()
        {
            // Arrange
            var moduleId = new ModuleId("Test");
            var featureId = new FeatureId(moduleId, "Test");
            var config = new ConfigurationBuilder().Build();
            var featureStateProvider = CreateFeatureStateProvider(
                new FeatureState(featureId, config.GetSection("Features:Test.Test"), true));
            var workContext = new WorkContext();

            // Act

            // Assert
            Assert.Throws<ArgumentNullException>("featureStateProvider", () => new FeatureFactory<string>(null! /* featureStateProvider */, workContext));
            Assert.Throws<ArgumentNullException>("workContext", () => new FeatureFactory<string>(featureStateProvider, null! /* workContext */));
        }

        [Fact]
        public void CreateFeature_ReturnsFeature_ForValidFeatureService()
        {
            // Arrange
            var moduleId = new ModuleId("Test");
            var featureId = new FeatureId(moduleId, "Test");
            var config = new ConfigurationBuilder().Build();
            var featureStateProvider = CreateFeatureStateProvider(
                new FeatureState(featureId, config.GetSection("Features:Test.Test"), true));
            var workContext = new WorkContext();

            var factory = new FeatureFactory<string>(featureStateProvider, workContext);

            // Act
            var feature = factory.CreateFeature(
                CreateServiceProvider("StringService"),
                featureId);

            // Assert
            Assert.NotNull(feature);
            Assert.Equal("StringService", feature.Service);
            Assert.True(feature.Enabled);
            Assert.False(feature.Missing);
        }

        [Fact]
        public void CreateFeature_ReturnsFeature_ForDisabledFeatureService()
        {
            // Arrange
            var moduleId = new ModuleId("Test");
            var featureId = new FeatureId(moduleId, "Test");
            var config = new ConfigurationBuilder()
                .AddInMemoryCollection(new Dictionary<string, string>
                {
                    ["Features:Test.Test:Enabled"] = "false"
                })
                .Build();
            var featureStateProvider = CreateFeatureStateProvider(
                new FeatureState(featureId, config.GetSection("Features:Test.Test"), false));
            var workContext = new WorkContext();

            var factory = new FeatureFactory<string>(featureStateProvider, workContext);

            // Act
            var feature = factory.CreateFeature(
                CreateServiceProvider((string?)null),
                featureId);

            // Assert
            Assert.NotNull(feature);
            Assert.Null(feature.Service);
            Assert.False(feature.Enabled);
            Assert.True(feature.Missing);
        }

        private IFeatureStateProvider CreateFeatureStateProvider(FeatureState mockState)
        {
            var mock = new Mock<IFeatureStateProvider>();

            mock.Setup(fsp => fsp.GetFeatureState(It.IsAny<FeatureId>()))
                .Returns(mockState);

            return mock.Object;
        }

        private IServiceProvider CreateServiceProvider(object? service)
        {
            var mock = new Mock<IServiceProvider>();

            mock.Setup(sp => sp.GetService(It.IsAny<Type>()))
                .Returns(service!);

            return mock.Object;
        } 
    }
}
