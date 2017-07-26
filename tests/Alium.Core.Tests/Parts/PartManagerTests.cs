// Copyright (c) Alium FX. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Alium.Parts
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Xunit;

    /// <summary>
    /// Provides tests for the <see cref="PartManager"/>.
    /// </summary>
    public class PartManagerTests
    {
        [Fact]
        public void PopulateFeature_InvokesAllProvidersSequentially_ForAGivenFeature()
        {
            // Arrange
            var manager = new PartManager();
            manager.Parts.Add(new TestPart("TestPartA"));
            manager.Parts.Add(new OtherPart("OtherPartB"));
            manager.Parts.Add(new TestPart("TestPartC"));
            manager.PartFeatureProviders.Add(
                new TestPartFeatureProvider((f, n) => f.Values.Add($"TestPartFeatureProvider1{n}")));
            manager.PartFeatureProviders.Add(
                new TestPartFeatureProvider((f, n) => f.Values.Add($"TestPartFeatureProvider2{n}")));

            var feature = new TestFeature();
            var expectedResults = new[]
            {
                "TestPartFeatureProvider1TestPartA",
                "TestPartFeatureProvider1TestPartC",
                "TestPartFeatureProvider2TestPartA",
                "TestPartFeatureProvider2TestPartC"
            };

            // Act
            manager.PopulateFeature(feature);

            // Assert
            Assert.Equal(expectedResults, feature.Values.ToArray());
        }

        [Fact]
        public void PopulateFeature_InvokesOnlyProviders_ForAGivenFeature()
        {
            // Arrange
            var manager = new PartManager();
            manager.Parts.Add(new TestPart("TestPart"));
            manager.PartFeatureProviders.Add(
                new TestPartFeatureProvider((f, n) => f.Values.Add($"TestPartFeatureProvider{n}")));
            manager.PartFeatureProviders.Add(
                new OtherPartFeatureProvider((f, n) => f.Values.Add($"OtherPartFeatureProvider{n}")));

            var feature = new TestFeature();
            var expectedResults = new[] { "TestPartFeatureProviderTestPart" };

            // Act
            manager.PopulateFeature(feature);

            // Assert
            Assert.Equal(expectedResults, feature.Values.ToArray());
        }

        [Fact]
        public void PopulateFeature_SkipProviders_ForOtherFeatures()
        {
            // Arrange
            var manager = new PartManager();
            manager.Parts.Add(new OtherPart("OtherPart"));
            manager.PartFeatureProviders.Add(
                new TestPartFeatureProvider((f, n) => f.Values.Add($"TestPartFeatureProvider{n}")));

            var feature = new TestFeature();

            // Act
            manager.PopulateFeature(feature);

            // Assert
            Assert.Empty(feature.Values.ToArray());
        }

        private class TestPart : IPart
        {
            public TestPart(string name)
            {
                Name = name;
            }

            public string Name { get; }
        }

        private class OtherPart : IPart
        {
            public OtherPart(string name)
            {
                Name = name;
            }

            public string Name { get; }
        }

        private class TestFeature
        {
            public IList<string> Values { get; } = new List<string>();
        }

        private class OtherFeature
        {
            public IList<string> Values { get; } = new List<string>();
        }

        private class TestPartFeatureProvider : IPartFeatureProvider<TestFeature>
        {
            private Action<TestFeature, string> _operation;

            public TestPartFeatureProvider(Action<TestFeature, string> operation)
            {
                _operation = operation;
            }

            public void PopulateFeature(IEnumerable<IPart> parts, TestFeature feature)
            {
                foreach (var part in parts.OfType<TestPart>())
                {
                    _operation(feature, part.Name);
                }
            }
        }

        private class OtherPartFeatureProvider : IPartFeatureProvider<OtherFeature>
        {
            private Action<OtherFeature, string> _operation;

            public OtherPartFeatureProvider(Action<OtherFeature, string> operation)
            {
                _operation = operation;
            }

            public void PopulateFeature(IEnumerable<IPart> parts, OtherFeature feature)
            {
                foreach (var part in parts.OfType<OtherPart>())
                {
                    _operation(feature, part.Name);
                }
            }
        }
    }
}
