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
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference or unconstrained type parameter.
            Assert.Throws<ArgumentException>(() => Ensure.IsNotNullOrEmpty((string) null, ""));
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference or unconstrained type parameter.
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.
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
