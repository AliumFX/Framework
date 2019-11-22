// Copyright (c) Alium FX. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Alium.Modules
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using Moq;
    using Xunit;

    /// <summary>
    /// Provides tests for the <see cref="ModuleBase"/> type.
    /// </summary>
    public class ModuleBaseTests
    {
        [Fact]
        public void Constructor_ValidatesParameters()
        {
            // Arrange
            var mock = new Mock<ModuleBase>(MockBehavior.Loose, ModuleId.Empty, (string?) null, (string?) null, (IEnumerable<ModuleId>?) null);

            // Act

            // Assert
            var tie = Assert.Throws<TargetInvocationException>(() => mock.Object); // MA - Catch TIE because of Moq.
            Assert.IsType<ArgumentException>(tie.InnerException);

            var aex = (ArgumentException)tie.InnerException!;
            Assert.Equal("id", aex.ParamName);
        }

        [Fact]
        public void Constructor_SetsDependencies_WhenProvidedSetIsNull()
        {
            // Arrange
            var mock = new Mock<ModuleBase>(MockBehavior.Loose, new ModuleId("Test"), (string?) null, (string?) null, (IEnumerable<ModuleId>?) null);
            var module = mock.Object;

            // Act

            // Assert
            Assert.NotNull(module.Dependencies);
        }
    }
}
