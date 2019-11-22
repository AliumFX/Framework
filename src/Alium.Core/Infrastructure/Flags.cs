// Copyright (c) Alium FX. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Alium.Infrastructure
{
    using System;
    using System.Collections.Concurrent;
    using Microsoft.Extensions.Configuration;

    /// <summary>
    /// Provides access to flag values.
    /// </summary>
    public class Flags : IFlags
    {
        private readonly ConcurrentDictionary<string, string?> _stringFlags = new ConcurrentDictionary<string, string?>(StringComparer.OrdinalIgnoreCase);
        private readonly ConcurrentDictionary<string, object?> _convertedFlags = new ConcurrentDictionary<string, object?>(StringComparer.OrdinalIgnoreCase);
        private readonly IConfigurationSection _root;

        /// <summary>
        /// Initialises a new instance of <see cref="Flags"/>.
        /// </summary>
        /// <param name="configuration">The configuration instance.</param>
        public Flags(IConfiguration configuration)
        {
            _root = Ensure.IsNotNull(configuration, nameof(configuration)).GetSection("Flags");
        }

        /// <inheritdoc />
        public TValue Value<TValue>(string flag, TValue @default = default)
        {
            Ensure.IsNotNullOrEmpty(flag, nameof(flag));

            // MA - The value operation returns any type, which means we can't express the nullability of the return types here
            return (TValue)_convertedFlags.GetOrAdd(flag, f =>
            {
                var section = _root.GetSection(flag);
                if (section == null || string.IsNullOrEmpty(section.Value))
                {
                    return @default;
                }

                return (TValue)Convert.ChangeType(section.Value, typeof(TValue));
            })!;
        }

        /// <inheritdoc />
        public string? Value(string flag, string? @default = null)
        {
            Ensure.IsNotNullOrEmpty(flag, nameof(flag));

            return _stringFlags.GetOrAdd(flag, f =>
            {
                var section = _root.GetSection(flag);
                if (section == null || string.IsNullOrEmpty(section.Value))
                {
                    return @default;
                }

                return section.Value;
            });
        }
    }
}
