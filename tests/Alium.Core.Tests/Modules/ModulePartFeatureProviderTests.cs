// Copyright (c) Alium Project. All rights reserved.
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
            var parts = Enumerable.Empty<IPart>();

            // Act

            // Assert
            Assert.Throws<ArgumentNullException>(() => provider.PopulateFeature(null /* parts */, null /* feature */));
            Assert.Throws<ArgumentNullException>(() => provider.PopulateFeature(parts, null /* feature */));
        }

        [Fact]
        public void PopulateFeature_PopulatesModuleTypesIntoPartFeature()
        {
            // Arrange
            var provider = new ModulePartFeatureProvider();
            var feature = new ModulePartFeature();
            var parts = new IPart[]
            {
                new TestPart(), new TestPart2()
            };

            // Act
            provider.PopulateFeature(parts, feature);

            // Assert
            Assert.NotEmpty(feature.ModuleTypes);
            Assert.Contains(feature.ModuleTypes, t => t == typeof(TestModule).GetTypeInfo());
            Assert.Contains(feature.ModuleTypes, t => t == typeof(TestModule2).GetTypeInfo());
        }

        private class TestModule : ModuleBase
        {
            protected TestModule() 
                : base(new ModuleId("Test"), name: "Test")
            {
            }
        }

        private class TestPart : IPart, IPartTypeProvider
        {
            public IEnumerable<TypeInfo> Types
                => new[] { typeof(TestModule).GetTypeInfo() };

            public string Name => "TestPart";
        }

        private class TestModule2 : ModuleBase
        {
            protected TestModule2()
                : base(new ModuleId("Test2"), name: "Test2")
            {
            }
        }

        private class TestPart2 : IPart, IPartTypeProvider
        {
            public IEnumerable<TypeInfo> Types
                => new[] { typeof(TestModule2).GetTypeInfo() };

            public string Name => "TestPart2";
        }
    }
}
