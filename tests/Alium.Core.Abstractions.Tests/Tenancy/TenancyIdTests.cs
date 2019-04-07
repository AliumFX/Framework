// Copyright (c) Alium FX. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Alium.Tenancy
{
    using System;
    using Xunit;

    /// <summary>
    /// Provides tests for the <see cref="TenantId"/> type.
    /// </summary>
    public class TenantIdTests
    {
        [Fact]
        public void Constructor_ValidatesArguments()
        {
            // Arrange

            // Act

            // Assert
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference or unconstrained type parameter.
            Assert.Throws<ArgumentException>("value", () => new TenantId(null /* value */));
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference or unconstrained type parameter.
            Assert.Throws<ArgumentException>("value", () => new TenantId(string.Empty /* value */));
        }

        [Fact]
        public void InitialisedInstance_HasValue()
        {
            // Arrange

            // Act
            var value = new TenantId("test");

            // Asset
            Assert.True(value.HasValue);
            Assert.Equal("test", value.Value);
        }

        [Fact]
        public void CanCompare_AgainstTenantId()
        {
            // Arrange
            var id = new TenantId("test");

            // Act
            int compare0 = id.CompareTo(TenantId.Empty);
            int compare1 = id.CompareTo(new TenantId("test"));
            int compare2 = id.CompareTo(new TenantId("zzzz"));
            int compare3 = id.CompareTo(new TenantId("aaaa"));

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
            var id = new TenantId("test");

            // Act
            int compare0 = id.CompareTo((string?) null);
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
            var id = new TenantId(value);

            // Act
            int compare = id.CompareTo(value.ToUpper());

            // Assert
            Assert.Equal(0, compare);
        }

        [Fact]
        public void CanEquate_AgainstTenantId()
        {
            // Arrange
            var id = new TenantId("test");

            // Act
            bool equate0 = id.Equals(TenantId.Empty);
            bool equate1 = id.Equals(new TenantId("test"));
            bool equate2 = id.Equals(new TenantId("aaaa"));

            // Assert
            Assert.False(equate0);
            Assert.True(equate1);
            Assert.False(equate2);
        }

        [Fact]
        public void CanEquate_AgainstString()
        {
            // Arrange
            var id = new TenantId("test");

            // Act
            bool equate0 = id.Equals((string?) null);
            bool equate1 = id.Equals("test");
            bool equate2 = id.Equals("aaaa");

            // Assert
            Assert.False(equate0);
            Assert.True(equate1);
            Assert.False(equate2);
        }

        [Fact]
        public void WhenEquating_UsewCaseInsensitiveCompare()
        {
            // Arrange
            var id = new TenantId("test");

            // Act
            bool equals = id.Equals(new TenantId("TEST"));

            // Assert
            Assert.True(equals);
        }

        [Fact]
        public void CanExplicitlyCast_FromString_ToTenantId()
        {
            // Arrange
            string value = "test";

            // Act
            var id = (TenantId) value;

            // Assert
            Assert.Equal(new TenantId("test"), id);
        }

        [Fact]
        public void CanExplicitlyCast_FromTenantId_ToString()
        {
            // Arrange
            var id = new TenantId("test");

            // Act
            string value = (string) id;

            // Assert
            Assert.Equal("test", value);
        }
    }
}

