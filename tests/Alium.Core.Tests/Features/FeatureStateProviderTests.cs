// Copyright (c) Alium Fx. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Alium.Features
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using Microsoft.Extensions.Configuration;
    using Moq;
    using Xunit;

    using Alium.Modules;

    /// <summary>
    /// Provides tests for the <see cref="FeatureStateProvider"/>.
    /// </summary>
    public class FeatureStateProviderTests
    {
        [Fact]
        public void Constructor_ValidatesArguments()
        {
            // Arrange
            var moduleId = new ModuleId("Test");
            var featureId = new FeatureId(moduleId, "FeatureA");
            var feature = new TestFeature(featureId);
            var featureProvider = CreateFeatureProvider(feature);
            var configuration = CreateConfiguration(new Dictionary<string, string>
            {
                ["Features:Test.FeatureA"] = "true"
            });

            // Act

            // Assert
            Assert.Throws<ArgumentNullException>("featureProvider", () => new FeatureStateProvider(
                null! /* featureProvider */,
                configuration));
            Assert.Throws<ArgumentNullException>("configuration", () => new FeatureStateProvider(
                Mock.Of<IFeatureProvider>(),
                null! /* configuration */));
        }

        [Fact]
        public void GetFeatureState_ReturnsFeatureState_ForKnownFeature()
        {
            // Arrange
            var moduleId = new ModuleId("Test");
            var featureId = new FeatureId(moduleId, "FeatureA");
            var feature = new TestFeature(featureId);
            var featureProvider = CreateFeatureProvider(feature);
            var configuration = CreateConfiguration(new Dictionary<string, string>
            {
                ["Features:Test.FeatureA"] = "true"
            });
            var featureStateProvider = new FeatureStateProvider(featureProvider, configuration);

            // Act
            var state = featureStateProvider.GetFeatureState(featureId);

            // Assert
            Assert.NotNull(state);
            Assert.Equal(featureId, state.FeatureId);
            Assert.True(state.Enabled);
            Assert.NotNull(state.ConfigurationSection);
        }

        [Fact]
        public void GetFeatureState_ReturnsDisabledFeatureState_ForDirectBooleanValue()
        {
            // Arrange
            var moduleId = new ModuleId("Test");
            var featureId = new FeatureId(moduleId, "FeatureA");
            var feature = new TestFeature(featureId);
            var featureProvider = CreateFeatureProvider(feature);
            var configuration = CreateConfiguration(new Dictionary<string, string>
            {
                ["Features:Test.FeatureA"] = "false"
            });
            var featureStateProvider = new FeatureStateProvider(featureProvider, configuration);

            // Act
            var state = featureStateProvider.GetFeatureState(featureId);

            // Assert
            Assert.NotNull(state);
            Assert.Equal(featureId, state.FeatureId);
            Assert.False(state.Enabled);
            Assert.NotNull(state.ConfigurationSection);
        }

        [Fact]
        public void GetFeatureState_ReturnsDisabledFeatureState_ForNestedBooleanValue()
        {
            // Arrange
            var moduleId = new ModuleId("Test");
            var featureId = new FeatureId(moduleId, "FeatureA");
            var feature = new TestFeature(featureId);
            var featureProvider = CreateFeatureProvider(feature);
            var configuration = CreateConfiguration(new Dictionary<string, string>
            {
                ["Features:Test.FeatureA:Enabled"] = "false"
            });
            var featureStateProvider = new FeatureStateProvider(featureProvider, configuration);

            // Act
            var state = featureStateProvider.GetFeatureState(featureId);

            // Assert
            Assert.NotNull(state);
            Assert.Equal(featureId, state.FeatureId);
            Assert.False(state.Enabled);
            Assert.NotNull(state.ConfigurationSection);
        }

        [Fact]
        public void GetFeatureState_ReturnsDisabledFeatureState_ForDisabledParentFeature()
        {
            // Arrange
            var moduleId = new ModuleId("Test");
            var parentFeatureId = new FeatureId(moduleId, "ParentFeature");
            var parentFeature = new TestFeature(parentFeatureId);
            var childFeatureId = new FeatureId(moduleId, parentFeatureId, "ChildFeature");
            var childFeature = new TestFeature(childFeatureId);
            var featureProvider = CreateFeatureProvider(parentFeature, childFeature);
            var configuration = CreateConfiguration(new Dictionary<string, string>
            {
                ["Features:Test.ParentFeature:Enabled"] = "false",
                ["Features:Test.ParentFeature.ChildFeature:Enabled"] = "true"
            });
            var featureStateProvider = new FeatureStateProvider(featureProvider, configuration);

            // Act
            var state = featureStateProvider.GetFeatureState(childFeatureId);

            // Assert
            Assert.NotNull(state);
            Assert.Equal(childFeatureId, state.FeatureId);
            Assert.False(state.Enabled);
            Assert.NotNull(state.ConfigurationSection);
        }

        [Fact]
        public void GetFeatureState_ReturnsFeatureState_ForUnknownFeature()
        {
            // Arrange
            var moduleId = new ModuleId("Test");
            var featureId = new FeatureId(moduleId, "FeatureA");
            var featureProvider = CreateFeatureProvider();
            var configuration = CreateConfiguration(new Dictionary<string, string>
            {

            });
            var featureStateProvider = new FeatureStateProvider(featureProvider, configuration);

            // Act
            var state = featureStateProvider.GetFeatureState(featureId);

            // Assert
            Assert.NotNull(state);
            Assert.Equal(featureId, state.FeatureId);
            Assert.False(state.Enabled);
            Assert.NotNull(state.ConfigurationSection);
        }

        [Fact]
        public void GetFeatureState_ReturnsEnabledFeatureState_WhenConfigurationMissing_AndFeatureEnabledByDefault()
        {
            // Arrange
            var moduleId = new ModuleId("Test");
            var featureId = new FeatureId(moduleId, "FeatureA");
            var feature = new TestFeature(featureId, enabledByDefault: true);
            var featureProvider = CreateFeatureProvider(feature);
            var configuration = CreateConfiguration(new Dictionary<string, string>
            {

            });
            var featureStateProvider = new FeatureStateProvider(featureProvider, configuration);

            // Act
            var state = featureStateProvider.GetFeatureState(featureId);

            // Assert
            Assert.NotNull(state);
            Assert.Equal(featureId, state.FeatureId);
            Assert.True(state.Enabled);
            Assert.NotNull(state.ConfigurationSection);
        }

        private IFeatureProvider CreateFeatureProvider(params IFeature[] features)
        {
            var mock = new Mock<IFeatureProvider>();

            mock.Setup(fp => fp.Features)
                .Returns(new ReadOnlyCollection<IFeature>(features.ToList()));

            mock.Setup(fp => fp.GetFeature(It.IsAny<FeatureId>()))
                .Returns<FeatureId>(featureId => features.FirstOrDefault(f => f.Id.Equals(featureId)));

            return mock.Object;
        }

        private IConfiguration CreateConfiguration(IDictionary<string, string> values)
        {
            var configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(values)
                .Build();

            return configuration;
        }

        private class TestFeature : FeatureBase
        {
            public TestFeature(FeatureId featureId, bool enabledByDefault = false) : base(
                featureId,
                enabledByDefault: enabledByDefault)
            {

            }
        }
    }
}
