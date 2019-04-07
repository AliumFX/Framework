// Copyright (c) Alium Fx. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Alium.Modules
{
    using System;
    using System.Linq;
    using Xunit;

    /// <summary>
    /// Provides tests for the <see cref="ModuleProvider"/> type.
    /// </summary>
    public class ModuleProviderTests
    {
        [Fact]
        public void Constructor_ValidatesArguments()
        {
            // Arrange

            // Act

            // Assert
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference or unconstrained type parameter.
            Assert.Throws<ArgumentNullException>("modules", () => new ModuleProvider(null /* modules */));
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference or unconstrained type parameter.
        }

        [Fact]
        public void Modules_ReturnsProvidedModules_InDependencyOrder()
        {
            // Arrange
            var source = new IModule[]
            {
                new TestModuleOne(),
                new TestModuleTwo(),
                new TestModuleThree(),
                new TestModuleFour()
            };

            // Act
            var provider = new ModuleProvider(source);
            var sorted = provider.Modules.ToList();

            // Assert
            Assert.Equal(4, sorted.Count);
            Assert.Same(source[0], sorted[0]);
            Assert.Same(source[2], sorted[1]);
            Assert.Same(source[3], sorted[2]);
            Assert.Same(source[1], sorted[3]);
        }

        private class TestModuleOne : ModuleBase
        {
            public TestModuleOne()
                : base(new ModuleId("One")) { }
        }

        private class TestModuleTwo : ModuleBase
        {
            public TestModuleTwo()
                : base(new ModuleId("Two"), dependencies: new[] { new ModuleId("Three"), new ModuleId("Four") }) { }
        }

        private class TestModuleThree : ModuleBase
        {
            public TestModuleThree()
                : base(new ModuleId("Three"), dependencies: new[] { new ModuleId("One") }) { }
        }

        private class TestModuleFour : ModuleBase
        {
            public TestModuleFour()
                : base(new ModuleId("Four")) { }
        }
    }
}
