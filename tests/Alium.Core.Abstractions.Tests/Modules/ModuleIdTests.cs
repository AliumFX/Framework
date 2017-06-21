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
        public void CanCompare_AgainstModuleId()
        {
            // Arrange
            var id = new ModuleId("test");

            // Act
            int compare0 = id.CompareTo(ModuleId.Empty);
            int compare1 = id.CompareTo(new ModuleId("test"));
            int compare2 = id.CompareTo(new ModuleId("zzzz"));
            int compare3 = id.CompareTo(new ModuleId("aaaa"));

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
            var id = new ModuleId("test");

            // Act
            int compare0 = id.CompareTo((string) null);
            int compare1 = id.CompareTo("test");
            int compare2 = id.CompareTo("zzzz");
            int compare3 = id.CompareTo("aaaa");

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
            string value = "test";
            var id = new ModuleId(value);

            // Act
            int compare = id.CompareTo(value.ToUpper());

            // Assert
            Assert.Equal(0, compare);
        }

        [Fact]
        public void CanEquate_AgainstModuleId()
        {
            // Arrange
            var id = new ModuleId("test");

            // Act
            bool equate0 = id.Equals(ModuleId.Empty);
            bool equate1 = id.Equals(new ModuleId("test"));
            bool equate2 = id.Equals(new ModuleId("aaaa"));

            // Assert
            Assert.Equal(false, equate0);
            Assert.Equal(true, equate1);
            Assert.Equal(false, equate2);
        }

        [Fact]
        public void CanEquate_AgainstString()
        {
            // Arrange
            var id = new ModuleId("test");

            // Act
            bool equate0 = id.Equals((string)null);
            bool equate1 = id.Equals("test");
            bool equate2 = id.Equals("aaaa");

            // Assert
            Assert.Equal(false, equate0);
            Assert.Equal(true, equate1);
            Assert.Equal(false, equate2);
        }

        [Fact]
        public void WhenEquating_UsewCaseInsensitiveCompare()
        {
            // Arrange
            var id = new ModuleId("test");

            // Act
            bool equals = id.Equals(new ModuleId("TEST"));

            // Assert
            Assert.Equal(true, equals);
        }

        [Fact]
        public void CanExplicitlyCast_FromString_ToModuleId()
        {
            // Arrange
            string value = "test";

            // Act
            var id = (ModuleId) value;

            // Assert
            Assert.Equal(new ModuleId("test"), id);
        }

        [Fact]
        public void CanExplicitlyCast_FromModuleId_ToString()
        {
            // Arrange
            var id = new ModuleId("test");

            // Act
            string value = (string) id;

            // Assert
            Assert.Equal("test", value);
        }
    }
}

