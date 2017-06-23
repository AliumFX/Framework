// Copyright (c) Alium FX. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Alium
{
    using System;
    using Xunit;

    /// <summary>
    /// Provides tests for the <see cref="Ensure"/> type.
    /// </summary>
    public class EnsureTests
    {
        [Fact]
        public void IsNotNullOrEmpty_ThrowsExceptionForNullOrEmptyValues()
        {
            // Arrange

            // Act

            // Assert
            Assert.Throws<ArgumentException>(() => Ensure.IsNotNullOrEmpty((string) null, ""));
            Assert.Throws<ArgumentException>(() => Ensure.IsNotNullOrEmpty(string.Empty, ""));
        }

        [Fact]
        public void IsNotNullOrEmpty_ReturnsValue()
        {
            // Arrange
            string value = "hello";

            // Act
            string value2 = Ensure.IsNotNullOrEmpty(value, "");

            // Assert
            Assert.Equal(value, value2);
        }
    }
}
