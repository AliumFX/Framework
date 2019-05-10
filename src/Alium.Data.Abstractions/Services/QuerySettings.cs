// Copyright (c) Alium FX. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Alium.Data
{
    using System;
    using System.Linq;

    /// <summary>
    /// Represents settings for query operations
    /// </summary>
    /// <typeparam name="TRecord">The record type</typeparam>
    public readonly struct QuerySettings<TRecord>
    {
        /// <summary>
        /// Initialises a new instance of <see cref="QuerySettings{TRecord}"/>
        /// </summary>
        /// <param name="async">The async settings</param>
        /// <param name="source">The query source</param>
        public QuerySettings(
            AsyncSettings @async = default,
            bool noTracking = false,
            Func<IQueryable<TRecord>, IQueryable<TRecord>>? source = default)
        {
            Async = @async;
            NoTracking = noTracking;
            Source = source;
        }

        /// <summary>
        /// Gets the async settings
        /// </summary>
        public AsyncSettings Async { get; }

        /// <summary>
        /// Gets whether the OR/M framework should not track entities
        /// </summary>
        public bool NoTracking { get; }

        /// <summary>
        /// Gets the query source
        /// </summary>
        public Func<IQueryable<TRecord>, IQueryable<TRecord>>? Source { get; }
    }
}
