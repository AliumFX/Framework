// Copyright (c) Alium Fx. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Alium.Features
{
    using System;
    using Xunit;

    using Alium.Modules;

    /// <summary>
    /// Provides tests for the <see cref="FeatureProvider"/> type.
    /// </summary>
    public class FeatureProviderTests
    {
        [Fact]
        public void Constructor_ValidatesArguments()
        {
            // Arrange

            // Act

            // Assert
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference or unconstrained type parameter.
            Assert.Throws<ArgumentNullException>("features", () => new FeatureProvider(null /* features */));
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference or unconstrained type parameter.
        }

        [Fact]
        public void Features_ReturnsFeatures()
        {
            // Arrange
            var feature = new TestFeature();
            var provider = new FeatureProvider(new IFeature[] { feature });

            // Act
            var features = provider.Features;

            // Assert
            Assert.NotNull(features);
            Assert.Contains(features, f => f == feature);
        }

        [Fact]
        public void GetFeature_ReturnsFeature()
        {
            // Arrange
            var feature = new TestFeature();
            var provider = new FeatureProvider(new IFeature[] { feature });

            // Act
            var result = provider.GetFeature(TestFeature.TestFeatureId);

            // Assert
            Assert.NotNull(result);
            Assert.Same(feature, result);
        }

        private class TestFeature : FeatureBase
        {
            public static FeatureId TestFeatureId = new FeatureId(new ModuleId("Test"), "Test");

            public TestFeature()
                : base(TestFeatureId) { }
        }
    }
}
