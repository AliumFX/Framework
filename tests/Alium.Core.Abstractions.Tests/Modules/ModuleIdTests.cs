// Copyright (c) Alium FX. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Alium.Modules
{
    using System;
    using Xunit;

    /// <summary>
    /// Provides tests for the <see cref="ModuleId"/> type.
    /// </summary>
    public class ModuleIdTests
    {
        [Fact]
        public void Constructor_ValidatesArguments()
        {
            // Arrange

            // Act

            // Assert
            Assert.Throws<ArgumentException>(() => new ModuleId(null));
            Assert.Throws<ArgumentException>(() => new ModuleId(string.Empty));
        }

        [Fact]
        public void InitialisedInstance_HasValue()
        {
            // Arrange

            // Act
            var value = new ModuleId("test");

            // Asset
            Assert.Equal(true, value.HasValue);
            Assert.Equal("test", value.Value);
        }

        [Fact]
        public void CanCompare_AgainstString()
        {
            // Arrange
            var code = new ModuleId("test");

            // Act
            int compare1 = code.CompareTo("test");
            int compare2 = code.CompareTo("zzzz");
            int compare3 = code.CompareTo("aaaa");

            // Assert
            Assert.Equal(0, compare1);
            Assert.Equal(-1, compare2);
            Assert.Equal(1, compare3);
        }

        [Fact]
        public void WhenComparing_ToString_UsesCaseInsensitiveCompare()
        {
            // Arrange
            string value = "test";
            var code = new ModuleId(value);

            // Act
            int compare = code.CompareTo(value.ToUpper());

            // Assert
            Assert.Equal(0, compare);
        }
    }
}

