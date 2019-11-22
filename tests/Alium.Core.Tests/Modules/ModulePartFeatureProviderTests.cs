// Copyright (c) Alium Fx. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Alium.Modules
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using Xunit;

    using Alium.Parts;

    /// <summary>
    /// Provides tests for the <see cref="ModulePartFeatureProvider"/> type.
    /// </summary>
    public class ModulePartFeatureProviderTests
    {
        [Fact]
        public void PopulateFeature_ValidatesArguments()
        {
            // Arrange
            var provider = new ModulePartFeatureProvider();
            var feature = new ModulePartFeature();
            var parts = Enumerable.Empty<IPart>();

            // Act

            // Assert
            Assert.Throws<ArgumentNullException>("parts", () => provider.PopulateFeature(null! /* parts */, feature));
            Assert.Throws<ArgumentNullException>("feature", () => provider.PopulateFeature(parts, null! /* feature */));
        }

        [Fact]
        public void PopulateFeature_PopulatesModuleTypesIntoPartFeature()
        {
            // Arrange
            var provider = new ModulePartFeatureProvider();
            var feature = new ModulePartFeature();
            var parts = new IPart[]
            {
                new TestPart()
            };

            // Act
            provider.PopulateFeature(parts, feature);

            // Assert
            Assert.NotEmpty(feature.ModuleTypes);
            Assert.Contains(feature.ModuleTypes, t => t == typeof(CoreModule).GetTypeInfo());
        }

        private class TestPart : IPart, IPartTypeProvider
        {
            public IEnumerable<TypeInfo> Types
                => new[] { typeof(CoreModule).GetTypeInfo() };

            public string Name => "TestPart";
        }
    }
}
