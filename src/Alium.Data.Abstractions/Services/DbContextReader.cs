// Copyright (c) Alium FX. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Alium.Data
{
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;

    /// <summary>
    /// Provides an implementation of a reader, backed by a <see cref="DbContext"/>
    /// </summary>
    /// <typeparam name="TContext">The context type</typeparam>
    /// <typeparam name="TEntity">The record type</typeparam>
    public abstract class DbContextReader<TContext, TEntity> : DbContextReader<TContext, TEntity, int>, IReader<TEntity>
        where TContext : DbContext
        where TEntity : class, IEntity<int>
    {
        /// <summary>
        /// Initialises a new instance of <see cref="DbContextReader{TContext, TEntity, TPrimaryKey}"/>
        /// </summary>
        /// <param name="context">The context instance</param>
        protected DbContextReader(TContext context)
            : base(context) { }
    }

    /// <summary>
    /// Provides an implementation of a reader, backed by a <see cref="DbContext"/>
    /// </summary>
    /// <typeparam name="TContext">The context type</typeparam>
    /// <typeparam name="TEntity">The record type</typeparam>
    /// <typeparam name="TPrimaryKey">The primary key type</typeparam>
    public abstract class DbContextReader<TContext, TEntity, TPrimaryKey> : IReader<TEntity, TPrimaryKey>
        where TContext : DbContext
        where TEntity : class, IEntity<TPrimaryKey>
        where TPrimaryKey : struct
    {
        /// <summary>
        /// Initialises a new instance of <see cref="DbContextReader{TContext, TEntity, TPrimaryKey}"/>
        /// </summary>
        /// <param name="context">The context instance</param>
        protected DbContextReader(TContext context)
        {
            Context = Ensure.IsNotNull(context, nameof(context));
        }

        /// <summary>
        /// Gets the context
        /// </summary>
        protected TContext Context { get; }

        /// <summary>
        /// Gets the query
        /// </summary>
        protected IQueryable<TEntity> Query => Context.Set<TEntity>();

        /// <inheritdoc />
        public async Task<TEntity> GetByIdAsync(TPrimaryKey id, QuerySettings<TEntity> settings = default)
        {
            // MA - Resolve the queryable source based on the available settings
            var query = GetQueryableSource(settings);

            // MA - Expect a single item
            return await query.SingleOrDefaultAsync(e => e.Id.Equals(id), settings.Async.CancellationToken).WithSettings(settings.Async);
        }

        private IQueryable<TEntity> GetQueryableSource(QuerySettings<TEntity> settings)
        {
            var query = Query;

            if (settings.Source != null)
            {
                // MA - Customise the query per-caller (for instance, applying includes, etc.)
                query = settings.Source(query);
            }

            if (settings.NoTracking)
            {
                // MA - Disable tracking as requested
                query = query.AsNoTracking();
            }

            return query;
        }
    }
}
