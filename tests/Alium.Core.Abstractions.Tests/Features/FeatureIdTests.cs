// Copyright (c) Alium FX. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Xunit.Sdk;

namespace Alium.Features
{
    using System;
    using Xunit;

    using Alium.Modules;

    /// <summary>
    /// Provides tests for the <see cref="FeatureId"/> type.
    /// </summary>
    public class FeatureIdTests
    {
        [Fact]
        public void Constructor_ValidatesArguments()
        {
            // Arrange
            var moduleId = new ModuleId("module");

            // Act

            // Assert
            Assert.Throws<ArgumentException>(() => new FeatureId(ModuleId.Empty, null));
            Assert.Throws<ArgumentException>(() => new FeatureId(ModuleId.Empty, string.Empty));
            Assert.Throws<ArgumentException>(() => new FeatureId(moduleId, null));
            Assert.Throws<ArgumentException>(() => new FeatureId(moduleId, string.Empty));
            Assert.Throws<ArgumentException>(() => new FeatureId(FeatureId.Empty, null));
            Assert.Throws<ArgumentException>(() => new FeatureId(FeatureId.Empty, string.Empty));
            Assert.Throws<ArgumentException>(() => new FeatureId(new FeatureId(moduleId, "feature"), null));
            Assert.Throws<ArgumentException>(() => new FeatureId(new FeatureId(moduleId, "feature"), string.Empty));
        }

        [Fact]
        public void InitialisedInstance_HasValue_WhenProvidingModuleId()
        {
            // Arrange
            var moduleId = new ModuleId("module");

            // Act
            var value = new FeatureId(moduleId, "feature");

            // Asset
            Assert.Equal(true, value.HasValue);
            Assert.Equal(true, value.ModuleId.HasValue);
            Assert.True(value.ModuleId.Equals(moduleId));
            Assert.Equal("feature", value.LocalValue);
            Assert.Equal("module.feature", value.Value);
        }

        [Fact]
        public void InitialisedInstance_HasValue_WhenProvidingFeatureId()
        {
            // Arrange
            var moduleId = new ModuleId("module");
            var featureId = new FeatureId(moduleId, "parentFeature");

            // Act
            var value = new FeatureId(featureId, "feature");

            // Asset
            Assert.Equal(true, value.HasValue);
            Assert.Equal(true, value.ModuleId.HasValue);
            Assert.True(value.ModuleId.Equals(moduleId));
            Assert.Equal("feature", value.LocalValue);
            Assert.Equal("module.parentFeature.feature", value.Value);
        }

        [Fact]
        public void InitialisingFeatureId_CreatesNestedValue()
        {
            // Arrange
            var moduleId = new ModuleId("module");

            // Act
            var featureAId = new FeatureId(moduleId, "featureA");
            var featureBId = new FeatureId(featureAId, "featureB");

            // Assert
            Assert.Equal("module.featureA", featureAId.Value);
            Assert.Equal("module.featureA.featureB", featureBId.Value);
        }

        [Fact]
        public void CanCompare_AgainstFeatureId()
        {
            // Arrange
            var moduleId = new ModuleId("module");
            var id = new FeatureId(moduleId, "feature");

            // Act
            int compare0 = id.CompareTo(FeatureId.Empty);
            int compare1 = id.CompareTo(new FeatureId(moduleId, "feature"));
            int compare2 = id.CompareTo(new FeatureId(moduleId, "zzzz"));
            int compare3 = id.CompareTo(new FeatureId(moduleId, "aaaa"));

            // Assert
            Assert.True(1 == compare0);
            Assert.True(0 == compare1);
            Assert.True(-1 == compare2);
            Assert.True(1 == compare3);
        }

        [Fact]
        public void CanCompare_AgainstString()
        {
            // Arrange
            var moduleId = new ModuleId("module");
            var id = new FeatureId(moduleId, "feature");

            // Act
            int compare0 = id.CompareTo((string) null);
            int compare1 = id.CompareTo("module.feature");
            int compare2 = id.CompareTo("module.zzzz");
            int compare3 = id.CompareTo("module.aaaa");

            // Assert
            Assert.True(1 == compare0);
            Assert.True(0 == compare1);
            Assert.True(-1 == compare2);
            Assert.True(1 == compare3);
        }

        [Fact]
        public void WhenComparing_UsesCaseInsensitiveCompare()
        {
            // Arrange
            var moduleId = new ModuleId("module");
            string value = "feature";
            var id = new FeatureId(moduleId, value);

            // Act
            int compare = id.CompareTo("MODULE.FEATURE");

            // Assert
            Assert.Equal(0, compare);
        }

        [Fact]
        public void CanEquate_AgainstFeatureId()
        {
            // Arrange
            var moduleId = new ModuleId("module");
            var id = new FeatureId(moduleId, "feature");

            // Act
            bool equate0 = id.Equals(FeatureId.Empty);
            bool equate1 = id.Equals(new FeatureId(moduleId, "feature"));
            bool equate2 = id.Equals(new FeatureId(moduleId, "aaaa"));

            // Assert
            Assert.Equal(false, equate0);
            Assert.Equal(true, equate1);
            Assert.Equal(false, equate2);
        }

        [Fact]
        public void CanEquate_AgainstString()
        {
            // Arrange
            var moduleId = new ModuleId("module");
            var id = new FeatureId(moduleId, "feature");

            // Act
            bool equate0 = id.Equals((string)null);
            bool equate1 = id.Equals("module.feature");
            bool equate2 = id.Equals("module.aaaa");

            // Assert
            Assert.Equal(false, equate0);
            Assert.Equal(true, equate1);
            Assert.Equal(false, equate2);
        }

        [Fact]
        public void WhenEquating_UsewCaseInsensitiveCompare()
        {
            // Arrange
            var moduleId = new ModuleId("module");
            var id = new FeatureId(moduleId, "feature");

            // Act
            bool equals = id.Equals(new FeatureId(moduleId, "FEATURE"));

            // Assert
            Assert.Equal(true, equals);
        }

        [Fact]
        public void CanExplicitlyCast_FromFeatureId_ToString()
        {
            // Arrange
            var moduleId = new ModuleId("module");
            var id = new FeatureId(moduleId, "feature");

            // Act
            string value = (string) id;

            // Assert
            Assert.Equal("module.feature", value);
        }
    }
}
