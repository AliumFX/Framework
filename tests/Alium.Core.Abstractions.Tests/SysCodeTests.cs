// Copyright (c) Alium FX. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Alium.Core.Abstractions.Tests
{
    using System;
    using Xunit;

    /// <summary>
    /// Provides tests for the <see cref="SysCode"/> type.
    /// </summary>
    public class SysCodeTests
    {
        [Fact]
        public void Constructor_ValidatesArguments()
        {
            // Arrange

            // Act

            // Assert
            Assert.Throws<ArgumentException>(() => new SysCode(null));
            Assert.Throws<ArgumentException>(() => new SysCode(string.Empty));
        }
    }
}
