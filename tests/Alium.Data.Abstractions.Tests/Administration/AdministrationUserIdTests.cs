// Copyright (c) Alium FX. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Alium.Administration
{
    using System;
    using System.IO;
    using Newtonsoft.Json;
    using Xunit;

    /// <summary>
    /// Provides tests for the <see cref="AdministrationUserId"/>
    /// </summary>
    public class AdministrationUserIdTests
    {
        [Fact]
        public void Constructor_SetsValues()
        {
            // Arrange
            var empty = new AdministrationUserId();
            var system = new AdministrationUserId(0);
            var user = new AdministrationUserId(1);

            // Act

            // Assert
            Assert.False(empty.HasValue);
            Assert.Equal(0, empty.Value);

            Assert.True(system.HasValue);
            Assert.Equal(0, system.Value);

            Assert.True(user.HasValue);
            Assert.Equal(1, user.Value);
        }

        [Fact]
        public void CompareTo_ComparesValue()
        {
            // Arrange
            var user1 = new AdministrationUserId(1);
            var user2 = new AdministrationUserId(2);
            var user3 = new AdministrationUserId(3);

            // Act
            int user1BeforeUser2 = user1.CompareTo(user2);
            int user2EqualsUser2 = user2.CompareTo(user2);
            int user3AfterUser2 = user3.CompareTo(user2);

            // Assert
            Assert.True(user1BeforeUser2 < 0);
            Assert.Equal(0, user2EqualsUser2);
            Assert.True(user3AfterUser2 > 0);
        }

        [Fact]
        public void Equals_ReturnsTrue_IfBothHaveSameValue()
        {
            // Arrange
            var user1 = new AdministrationUserId(1);
            var user1Other = new AdministrationUserId(1);

            // Act
            bool equals = user1.Equals(user1Other);

            // Assert
            Assert.True(equals);
        }

        [Fact]
        public void Equals_ReturnsTrue_IfSameInstance()
        {
            // Arrange
            var user1 = new AdministrationUserId(1);

            // Act
            bool equals = user1.Equals(user1);

            // Assert
            Assert.True(equals);
        }

        [Fact]
        public void Equals_ReturnsTrue_IfBothEmpty()
        {
            // Arrange

            // Act
            bool equals = AdministrationUserId.Empty.Equals(AdministrationUserId.Empty);

            // Assert
            Assert.True(equals);
        }

        [Theory]
        [InlineData(null, 1)]
        [InlineData(1, 2)]
        [InlineData(1, null)]
        public void Equals_ReturnFalse_ForDifferentValues(int? leftValue, int? rightValue)
        {
            // Arrange
            var left = leftValue.HasValue ? new AdministrationUserId(leftValue.Value) : AdministrationUserId.Empty;
            var right = rightValue.HasValue ? new AdministrationUserId(rightValue.Value) : AdministrationUserId.Empty;

            // Act
            bool equals = left.Equals(right);

            // Assert
            Assert.False(equals);
        }

        [Fact]
        public void GetHashCode_ReturnsHashCode_OfValue()
        {
            // Arrange
            int value = 10;
            var id = new AdministrationUserId(value);

            // Act
            int valueHashCode = value.GetHashCode();
            int idHashCode = id.GetHashCode();

            // Assert
            Assert.Equal(valueHashCode, idHashCode);
        }

        [Fact]
        public void ToString_ReturnsString_OfValue()
        {
            // Arrange
            var id = new AdministrationUserId(1);

            // Act
            string value = id.ToString();

            // Assert
            Assert.Equal("1", value);
        }

        [Fact]
        public void ToString_ReturnsEmptyString_ForEmpty()
        {
            // Arrange
            var empty = AdministrationUserId.Empty;

            // Act
            string value = empty.ToString();

            // Assert
            Assert.Equal(string.Empty, value);
        }

        [Fact]
        public void FromProviderValue_ReturnsValue_ForInt32()
        {
            // Arrange

            // Act
            var id = AdministrationUserId.FromProviderValue(10);

            // Assert
            Assert.True(id.HasValue);
            Assert.Equal(10, id.Value);
        }

        [Fact]
        public void FromProviderValue_ReturnsValue_ForNullableInt32_WithValue()
        {
            // Arrange
            int? value = 10;

            // Act
            var id = AdministrationUserId.FromProviderValue(value);

            // Assert
            Assert.True(id.HasValue);
            Assert.Equal(10, id.Value);
        }

        [Fact]
        public void FromProviderValue_ReturnsEmpty_ForNullableInt32_WithoutValue()
        {
            // Arrange
            int? value = null;

            // Act
            var id = AdministrationUserId.FromProviderValue(value);

            // Assert
            Assert.False(id.HasValue);
        }

        [Fact]
        public void FromProviderValue_ReturnsEmpty_ForDbNull()
        {
            // Arrange

            // Act
            var id = AdministrationUserId.FromProviderValue(Convert.DBNull);

            // Assert
            Assert.False(id.HasValue);
        }

        [Fact]
        public void FromProviderValue_ReturnsEmpty_ForNull()
        {
            // Arrange

            // Act
            var id = AdministrationUserId.FromProviderValue(null);

            // Assert
            Assert.False(id.HasValue);
        }

        [Theory]
        [InlineData("10")]
        [InlineData(" 10 ")]
        public void TryParse_ReturnsUserIdAsOutput_ForValidStringValue(string value)
        {
            // Arrange

            // Act
            bool success = AdministrationUserId.TryParse(value, out AdministrationUserId id);

            // Assert
            Assert.True(success);
            Assert.True(id.HasValue);
            Assert.Equal(10, id.Value);
        }

        [Theory]
        [InlineData("")]
        [InlineData("  ")]
        [InlineData("10x")]
        [InlineData("Matthew")]
        public void TryParse_ReturnsEmptyUserIdAsOutput_ForInalidStringValue(string value)
        {
            // Arrange

            // Act
            bool success = AdministrationUserId.TryParse(value, out AdministrationUserId id);

            // Assert
            Assert.False(success);
            Assert.False(id.HasValue);
        }

        [Theory]
        [InlineData("10")]
        [InlineData(" 10 ")]
        public void arse_ReturnsUserId_ForValidStringValue(string value)
        {
            // Arrange

            // Act
            var id = AdministrationUserId.Parse(value);

            // Assert
            Assert.True(id.HasValue);
            Assert.Equal(10, id.Value);
        }

        [Theory]
        [InlineData("")]
        [InlineData("  ")]
        [InlineData("10x")]
        [InlineData("Matthew")]
        public void Parse_ThrowsFormatException_ForInalidStringValue(string value)
        {
            // Arrange

            // Act

            // Assert
            Assert.Throws<FormatException>(() => AdministrationUserId.Parse(value));
        }

        [Fact]
        public void EqualsOperator_ReturnsTrue_IfBothHaveSameValue()
        {
            // Arrange
            var user1 = new AdministrationUserId(1);
            var user1Other = new AdministrationUserId(1);

            // Act
            bool equals = user1 == user1Other;

            // Assert
            Assert.True(equals);
        }

        [Fact]
        public void EqualsOperator_ReturnsTrue_IfSameInstance()
        {
            // Arrange
            var user1 = new AdministrationUserId(1);

            // Act
#pragma warning disable CS1718 // Comparison made to same variable
            bool equals = user1 == user1;
#pragma warning restore CS1718 // Comparison made to same variable

            // Assert
            Assert.True(equals);
        }

        [Fact]
        public void EqualsOperator_ReturnsTrue_IfBothEmpty()
        {
            // Arrange

            // Act
#pragma warning disable CS1718 // Comparison made to same variable
            bool equals = AdministrationUserId.Empty == AdministrationUserId.Empty;
#pragma warning restore CS1718 // Comparison made to same variable

            // Assert
            Assert.True(equals);
        }

        [Fact]
        public void EqualsOperator_ReturnsFalse_ForDifferentValue()
        {
            // Arrange
            var user1 = new AdministrationUserId(1);
            var user2 = new AdministrationUserId(2);


            // Act
            bool equals = user1 == user2;

            // Assert
            Assert.False(equals);
        }

        [Fact]
        public void NotEqualsOperator_ReturnsFalse_IfBothHaveSameValue()
        {
            // Arrange
            var user1 = new AdministrationUserId(1);
            var user1Other = new AdministrationUserId(1);

            // Act
            bool equals = user1 != user1Other;

            // Assert
            Assert.False(equals);
        }

        [Fact]
        public void NotEqualsOperator_ReturnsFalse_IfSameInstance()
        {
            // Arrange
            var user1 = new AdministrationUserId(1);

            // Act
#pragma warning disable CS1718 // Comparison made to same variable
            bool equals = user1 != user1;
#pragma warning restore CS1718 // Comparison made to same variable

            // Assert
            Assert.False(equals);
        }

        [Fact]
        public void NotEqualsOperator_ReturnsFalse_IfBothEmpty()
        {
            // Arrange

            // Act
#pragma warning disable CS1718 // Comparison made to same variable
            bool equals = AdministrationUserId.Empty != AdministrationUserId.Empty;
#pragma warning restore CS1718 // Comparison made to same variable

            // Assert
            Assert.False(equals);
        }

        [Fact]
        public void NotEqualsOperator_ReturnsTrue_ForDifferentValue()
        {
            // Arrange
            var user1 = new AdministrationUserId(1);
            var user2 = new AdministrationUserId(2);


            // Act
            bool equals = user1 != user2;

            // Assert
            Assert.True(equals);
        }

        [Fact]
        public void UserIdJsonConverter_CanConvertFromJsonNumber()
        {
            // Arrange
            var serialiser = new JsonSerializer();
            var reader = CreateJsonReader("10");
            var converter = new AdministrationUserId.AdministrationUserIdJsonConverter();

            // Act
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference or unconstrained type parameter.
            var result = converter.ReadJson(reader, typeof(AdministrationUserId), null, serialiser);
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference or unconstrained type parameter.

            // Assert
            Assert.NotNull(result);
            Assert.IsType<AdministrationUserId>(result);

            var id = (AdministrationUserId)result;
            Assert.True(id.HasValue);
            Assert.Equal(10, id.Value);
        }

        private JsonTextReader CreateJsonReader(string json)
            => new JsonTextReader(new StringReader(json));
    }
}
