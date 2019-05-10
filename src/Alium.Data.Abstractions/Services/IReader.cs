// Copyright (c) Alium FX. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Alium.Data
{
    using System.Linq;
    using System.Threading.Tasks;

    /// <summary>
    /// Defines the required contract for implementing a reader
    /// </summary>
    /// <typeparam name="TRecord">The record type</typeparam>
    public interface IReader<TRecord> : IReader<TRecord, int>
    {
        
    }

    /// <summary>
    /// Defines the required contract for implementing a reader
    /// </summary>
    /// <typeparam name="TRecord">The record type</typeparam>
    /// <typeparam name="TPrimaryKey">The primary key type</typeparam>
    public interface IReader<TRecord, TPrimaryKey>
    {
        /// <summary>
        /// Gets the record with the given ID
        /// </summary>
        /// <param name="id">The ID value</param>
        /// <param name="settings">The query settings</param>
        /// <returns>The record value as a task-based operation</returns>
        Task<TRecord> GetByIdAsync(TPrimaryKey id, QuerySettings<TRecord> settings = default);
    }
}
