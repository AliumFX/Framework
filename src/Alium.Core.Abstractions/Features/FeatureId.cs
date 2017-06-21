﻿// Copyright (c) Alium FX. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Alium.Features
{
    using System;
    using System.Diagnostics;

    using Alium.Modules;

    /// <summary>
    /// Represents a feature id.
    /// </summary>
    [DebuggerDisplay("Feature Id: {nameof(Value)}")]
    public struct FeatureId : IComparable<string>, IComparable<FeatureId>, IEquatable<string>, IEquatable<FeatureId>
    {
        /// <summary>
        /// Represents an empty feature id.
        /// </summary>
        public static readonly FeatureId Empty = new FeatureId();

        /// <summary>
        /// Initialises a new instance of <see cref="FeatureId"/>.
        /// </summary>
        /// <param name="parentModuleId">The parent module id.</param>
        /// <param name="value">The feature id value.</param>
        public FeatureId(ModuleId parentModuleId, string value)
        {
            if (parentModuleId.Equals(ModuleId.Empty))
            {
                throw new ArgumentException("The parent module id must be provided and cannot be ModuleId.Empty",
                    nameof(parentModuleId));
            }

            HasValue = true;
            ModuleId = parentModuleId;

            LocalValue = Ensure.IsNotNullOrEmpty(value, nameof(value));
            Value = ResolveValue(ModuleId, Empty, value);
        }

        /// <summary>
        /// Initialises a new instance of <see cref="FeatureId"/>.
        /// </summary>
        /// <param name="parentFeatureId">The parent feature id.</param>
        /// <param name="value">The feature id value.</param>
        public FeatureId(FeatureId parentFeatureId, string value)
        {
            if (parentFeatureId.Equals(Empty))
            {
                throw new ArgumentException("The parent feature id must be provided and cannot be FeatureId.Empty",
                    nameof(parentFeatureId));
            }

            HasValue = true;
            ModuleId = parentFeatureId.ModuleId;

            LocalValue = Ensure.IsNotNullOrEmpty(value, nameof(value));
            Value = ResolveValue(ModuleId, parentFeatureId, value);
        }

        /// <summary>
        /// Gets whether the current instance has a value.
        /// </summary>
        public bool HasValue { get; }

        /// <summary>
        /// Gets the local value of the feature id.
        /// </summary>
        public string LocalValue { get; }

        /// <summary>
        /// Gets the parent module id.
        /// </summary>
        public ModuleId ModuleId { get; }

        /// <summary>
        /// Gets the feature id value.
        /// </summary>
        public string Value { get; }

        /// <inheritdoc />
        public int CompareTo(string other)
        {
            if (HasValue)
            {
                int stringCompare = StringComparer.OrdinalIgnoreCase.Compare(Value, other);
                return stringCompare < 0
                    ? -1
                    : stringCompare == 0
                        ? 0
                        : 1;
            }

            if (string.IsNullOrEmpty(other))
            {
                return 1;
            }

            return 0;
        }

        /// <inheritdoc />
        public int CompareTo(FeatureId other)
        {
            if (other.HasValue)
            {
                return CompareTo(other.Value);
            }

            if (HasValue)
            {
                return 1;
            }

            return 0;
        }

        /// <inheritdoc />
        public bool Equals(string other)
            => CompareTo(other) == 0;

        /// <inheritdoc />
        public bool Equals(FeatureId other)
            => CompareTo(other) == 0;

        /// <inheritdoc />
        public override bool Equals(object obj)
        {
            if (obj is ModuleId c)
            {
                return Equals(c);
            }
            if (obj is string s)
            {
                return Equals(s);
            }

            return false;
        }

        /// <inheritdoc />
        public override int GetHashCode()
            => HasValue ? Value.GetHashCode() : 0;

        /// <inheritdoc />
        public override string ToString()
            => Value;

        /// <summary>
        /// Converts from a <see cref="FeatureId"/> to a <see cref="string"/>
        /// </summary>
        /// <param name="featureId">The <see cref="FeatureId"/> value.</param>
        public static explicit operator string(FeatureId featureId)
            => featureId.Value;

        /// <summary>
        /// Resolves the feature id value.
        /// </summary>
        /// <param name="parentModuleId"></param>
        /// <param name="parentFeatureId"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        private static string ResolveValue(ModuleId parentModuleId, FeatureId parentFeatureId, string value)
        {
            if (parentFeatureId.Equals(FeatureId.Empty))
            {
                return $"{parentModuleId.Value}.{value}";
            }

            return $"{parentFeatureId.Value}.{value}";
        }
    }
}
