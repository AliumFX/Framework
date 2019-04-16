// Copyright (c) Alium FX. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Alium.Security
{
    using System;
    using System.ComponentModel;
    using System.Diagnostics;
    using Newtonsoft.Json;

    using Alium.Data;
    using System.Globalization;

    /// <summary>
    /// Represents the ID of a user
    /// </summary>
    [DebuggerDisplay("{DebuggerToString(),nq}"), JsonConverter(typeof(UserIdJsonConverter)), TypeConverter(typeof(UserIdTypeConverter))]
    public readonly struct UserId : IComparable<UserId>, IEquatable<UserId>, IValueStruct<int>
    {
        /// <summary>
        /// Represents an empty user ID
        /// </summary>
        public static readonly UserId Empty = new UserId();

        /// <summary>
        /// Represents the ID of the system
        /// </summary>
        public static readonly UserId System = new UserId(value: 0);

        private readonly int _value;
        private readonly bool _hasValue;

        /// <summary>
        /// Initialises a new instance of <see cref="UserId" />
        /// </summary>
        /// <param name="value">The ID value</param>
        public UserId(int value)
        {
            _value = value;
            _hasValue = true;
        }

        /// <summary>
        /// Gets whether the ID has value
        /// </summary>
        public bool HasValue => _hasValue;

        /// <summary>
        /// Gets the value
        /// </summary>
        public int Value => _value;

        /// <inheritdoc />
        public int CompareTo(UserId other) => Value.CompareTo(other.Value);

        /// <inheritdoc />
        public bool Equals(UserId other) => (HasValue && other.HasValue && Value.Equals(other.Value)) || (!HasValue && !other.HasValue);

        /// <inheritdoc />
        public override bool Equals(object obj)
        {
            if (obj is null)
            {
                return false;
            }

            return obj is UserId other && Equals(other);
        }

        /// <inheritdoc />
        public override int GetHashCode() => Value.GetHashCode();

        /// <inheritdoc />
        public override string ToString() => HasValue ? Value.ToString() : string.Empty;

        /// <summary>
        /// Returns the debug string representation of the current instance
        /// </summary>
        /// <returns>The debug string representation of the current instance</returns>
        internal string DebuggerToString() => HasValue ? Value.ToString() : "(empty)";

        /// <summary>
        /// Provides conversion from another value sourced from another provider, for instance, a database value, or a JSON value
        /// </summary>
        /// <remarks>
        ///     If value is an int, or an int? with a value, a UserId representing that value is returned.
        ///     All other values will return Empty
        /// </remarks>
        /// <param name="value">The value object</param>
        /// <returns>The <see cref="UserId"/> value</returns>
        public static UserId FromProviderValue(object? value)
        {
            if (value is int intValue)
            {
                return new UserId(intValue);
            }

            if (value is string stringValue && TryParse(stringValue, out UserId id))
            {
                return id;
            }

            return Empty;
        }

        /// <summary>
        /// Attempts to parse the given value to extract a <see cref="UserId"/>, throwing an exception of the input value
        /// was not in the correct format
        /// </summary>
        /// <param name="value">The input value</param>
        /// <param name="id">[Out] The parsed <see cref="UserId"/></param>
        /// <returns>True if the value could be parsed, otherwise false</returns>
        public static UserId Parse(string value)
        {
            if (TryParse(value, out UserId id))
            {
                return id;
            }

            throw new FormatException("Input string was not in a correct format.");
        }

        /// <summary>
        /// Attempts to parse the given value to extract a <see cref="UserId"/>
        /// </summary>
        /// <param name="value">The input value</param>
        /// <param name="id">[Out] The parsed <see cref="UserId"/></param>
        /// <returns>True if the value could be parsed, otherwise false</returns>
        public static bool TryParse(string value, out UserId id)
        {
            if (int.TryParse(value, out int intValue))
            {
                id = new UserId(intValue);
                return true;
            }

            id = Empty;
            return false;
        }

        /// <summary>
        /// Determines if the two given values are equal
        /// </summary>
        /// <param name="left">The left operand</param>
        /// <param name="right">The right operand</param>
        /// <returns>True if the values are equal, otherwise false</returns>
        public static bool operator ==(UserId left, UserId right) => left.Equals(right);

        /// <summary>
        /// Determines if the two given values are not equal
        /// </summary>
        /// <param name="left">The left operand</param>
        /// <param name="right">The right operand</param>
        /// <returns>True if the values are not equal, otherwise false</returns>
        public static bool operator !=(UserId left, UserId right) => !left.Equals(right);

        /// <summary>
        /// Handles custom conversion during JSON.NET serialisation
        /// </summary>
        public class UserIdJsonConverter : JsonConverter
        {
            /// <inheritdoc />
            public override bool CanConvert(Type objectType) => objectType == typeof(UserId);

            /// <inheritdoc />
            public override object? ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
            {
                int? id = serializer.Deserialize<int?>(reader);
                if (!id.HasValue)
                {
                    return Empty;
                }

                return new UserId(id.Value);
            }

            /// <inheritdoc />
            public override void WriteJson(JsonWriter writer, object source, JsonSerializer serializer)
            {
                if (source is UserId value)
                {
                    if (value.HasValue)
                    {
                        serializer.Serialize(writer, value.Value);
                        return;
                    }
                }

                serializer.Serialize(writer, null);
            }
        }

        /// <summary>
        /// Handles custom conversion from an <see cref="int"/> to a <see cref="UserId"/>
        /// </summary>
        public class UserIdTypeConverter : TypeConverter
        {
            /// <inheritdoc />
            public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
                => sourceType == typeof(int) || sourceType == typeof(int?);

            /// <inheritdoc />
            public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
            {
                if (value is int intValue)
                {
                    return new UserId(intValue);
                }

                return Empty;
            }
        }
    }
}
