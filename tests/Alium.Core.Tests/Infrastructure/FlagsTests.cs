// Copyright (c) Alium Fx. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Alium.Infrastructure
{
    using System;
    using System.Collections.Generic;
    using Microsoft.Extensions.Configuration;
    using Xunit;

    /// <summary>
    /// Provides tests for the <see cref="Flags"/> type.
    /// </summary>
    public class FlagsTests
    {
        [Fact]
        public void Constructor_ValidatesArguments()
        {
            // Arrange

            // Act

            // Assert
            Assert.Throws<ArgumentNullException>("configuration", () => new Flags(null! /* configuration */));
        }

        [Fact]
        public void Value_ReturnsStringValue()
        {
            // Arrange
            var configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(new Dictionary<string, string?>
                    {
                        ["Flags:FlagKey"] = "Hello World"
                    }
                )
                .Build();
            var flags = new Flags(configuration);

            // Act
            string? value = flags.Value("FlagKey");

            // Assert
            Assert.Equal("Hello World", value);
        }

        [Fact]
        public void Value_ReturnsDefaultValue_ForUnknownFlag()
        {
            // Arrange
            var configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(new Dictionary<string, string?>
                    {
                        ["Flags:FlagKey"] = null
                    }
                )
                .Build();
            var flags = new Flags(configuration);

            // Act
            string? value = flags.Value("OtherFlagKey", "Hello World");

            // Assert
            Assert.Equal("Hello World", value);
        }

        [Fact]
        public void ValueOfT_ReturnsConvertedValue()
        {
            // Arrange
            var configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(new Dictionary<string, string?>
                    {
                        ["Flags:FlagKey"] = "123"
                    }
                )
                .Build();
            var flags = new Flags(configuration);

            // Act
            int value = flags.Value<int>("FlagKey");

            // Assert
            Assert.Equal(123, value);
        }

        [Fact]
        public void ValueOfT_ReturnsDefaultValue_ForUnknownFlag()
        {
            // Arrange
            var configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(new Dictionary<string, string?>
                    {
                        ["Flags:FlagKey"] = null
                    }
                )
                .Build();
            var flags = new Flags(configuration);

            // Act
            int value = flags.Value<int>("OtherFlagKey", 123);

            // Assert
            Assert.Equal(123, value);
        }
    }
}
