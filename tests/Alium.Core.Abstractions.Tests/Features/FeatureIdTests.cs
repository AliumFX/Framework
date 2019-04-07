// Copyright (c) Alium FX. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

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
            string value = "submodule";
            var moduleId = new ModuleId("module");
            var featureId = new FeatureId(moduleId, "feature");

            // Act

            // Assert
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference or unconstrained type parameter.
            Assert.Throws<ArgumentException>("sourceModuleId", () => new FeatureId(ModuleId.Empty, value));
            Assert.Throws<ArgumentException>("sourceModuleId", () => new FeatureId(ModuleId.Empty, value));
            Assert.Throws<ArgumentException>("value", () => new FeatureId(moduleId, null /* value */));
            Assert.Throws<ArgumentException>("value", () => new FeatureId(moduleId, string.Empty /* value */));
            Assert.Throws<ArgumentException>("sourceModuleId", () => new FeatureId(ModuleId.Empty, FeatureId.Empty, value));
            Assert.Throws<ArgumentException>("sourceModuleId", () => new FeatureId(ModuleId.Empty, FeatureId.Empty, value));
            Assert.Throws<ArgumentException>("parentFeatureId", () => new FeatureId(moduleId, FeatureId.Empty, value));
            Assert.Throws<ArgumentException>("parentFeatureId", () => new FeatureId(moduleId, FeatureId.Empty, value));
            Assert.Throws<ArgumentException>("value", () => new FeatureId(moduleId, featureId, null /* value */));
            Assert.Throws<ArgumentException>("value", () => new FeatureId(moduleId, featureId, string.Empty /* value */));
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference or unconstrained type parameter.
        }

        [Fact]
        public void InitialisedInstance_HasValue_WhenProvidingModuleId()
        {
            // Arrange
            var moduleId = new ModuleId("module");

            // Act
            var value = new FeatureId(moduleId, "feature");

            // Asset
            Assert.True(value.HasValue);
            Assert.True(value.ParentModuleId.HasValue);
            Assert.True(value.ParentModuleId.Equals(moduleId));
            Assert.True(value.SourceModuleId.HasValue);
            Assert.True(value.SourceModuleId.Equals(moduleId));
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
            var value = new FeatureId(moduleId, featureId, "feature");

            // Asset
            Assert.True(value.HasValue);
            Assert.True(value.ParentModuleId.HasValue);
            Assert.True(value.ParentModuleId.Equals(moduleId));
            Assert.True(value.ParentModuleId.HasValue);
            Assert.True(value.ParentModuleId.Equals(moduleId));
            Assert.True(value.SourceModuleId.HasValue);
            Assert.True(value.SourceModuleId.Equals(moduleId));
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
            var featureBId = new FeatureId(moduleId, featureAId, "featureB");

            // Assert
            Assert.Equal("module.featureA", featureAId.Value);
            Assert.Equal("module.featureA.featureB", featureBId.Value);
            Assert.Equal(featureAId, featureBId.ParentFeatureId);
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
            int compare0 = id.CompareTo((string?) null);
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
            Assert.False(equate0);
            Assert.True(equate1);
            Assert.False(equate2);
        }

        [Fact]
        public void CanEquate_AgainstString()
        {
            // Arrange
            var moduleId = new ModuleId("module");
            var id = new FeatureId(moduleId, "feature");

            // Act
            bool equate0 = id.Equals((string?)null);
            bool equate1 = id.Equals("module.feature");
            bool equate2 = id.Equals("module.aaaa");

            // Assert
            Assert.False(equate0);
            Assert.True(equate1);
            Assert.False(equate2);
        }

        [Fact]
        public void WhenEquating_UsesCaseInsensitiveCompare()
        {
            // Arrange
            var moduleId = new ModuleId("module");
            var id = new FeatureId(moduleId, "feature");

            // Act
            bool equals = id.Equals(new FeatureId(moduleId, "FEATURE"));

            // Assert
            Assert.True(equals);
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

        [Fact]
        public void ParentMdoule_InheritedFromParentFeature()
        {
            // Arrange
            var moduleId = new ModuleId("module");
            var otherModuleId = new ModuleId("otherModule");
            var featureId = new FeatureId(moduleId, "feature");
            var otherFeatureId = new FeatureId(otherModuleId, featureId, "feature");

            // Act

            // Assert
            Assert.Equal(moduleId, otherFeatureId.ParentModuleId);
            Assert.Equal(otherModuleId, otherFeatureId.SourceModuleId);
        }
    }
}

