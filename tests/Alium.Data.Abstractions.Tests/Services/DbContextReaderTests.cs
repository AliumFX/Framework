// Copyright (c) Alium FX. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Alium.Data
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
    using Xunit;

    using Alium.Administration;

    /// <summary>
    /// Provides tests for the <see cref="DbContextReader{TContext, TEntity, TPrimaryKey}"/> type
    /// </summary>
    public class DbContextReaderTests
    {
        [Fact]
        public void Constructor_ValidatesArguments()
        {
            // Arrange

            // Act

            // Assert
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference or unconstrained type parameter.
            Assert.Throws<ArgumentNullException>("context", () => new UserReader(null /* context */));
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference or unconstrained type parameter.
        }

        [Fact]
        public async Task GetByIdAsync_ReturnsMatchingEntity()
        {
            // Arrange
            var context = CreateDbContext(
                new User(name: "Matthew Abbott") { Id = new AdministrationUserId(1) });

            // Act
            var reader = new UserReader(context);
            var id = new AdministrationUserId(1);
            var user = await reader.GetByIdAsync(id);

            // Assert
            Assert.NotNull(user);
            Assert.Equal(id, user.Id);
        }

        [Fact]
        public async Task GetByIdAsync_SupportsFiltering_ThroughQuerySettings()
        {
            // Arrange
            var context = CreateDbContext(
                new User(name: "Matthew Abbott") { Id = new AdministrationUserId(1) });

            // Act
            var reader = new UserReader(context);
            var id = new AdministrationUserId(1);
            var settings = new QuerySettings<User>(
                // MA - Pre-filter the users dataset to only include enabled users
                source: users => users.Where(u => u.Enabled)
            );
            var user = await reader.GetByIdAsync(id, settings);

            // Assert
            Assert.Null(user);
        }

        private UserDbContext CreateDbContext(params User[] users)
        {
            var context = new UserDbContext();
            if (users != null && users.Length > 0)
            {
                context.Users?.AddRange(users);
                context.SaveChanges();
            }

            return context;
        }

        public class User : Entity<AdministrationUserId>
        {
            public User(string name)
            {
                Name = name;
            }

            public string Name { get; set; }// = default!;
        }

        public class UserDbContext : DbContext
        {
            public DbSet<User>? Users { get; set; }

            protected override void OnConfiguring(DbContextOptionsBuilder builder)
            {
                builder.UseInMemoryDatabase($"{Guid.NewGuid()}");
            }

            protected override void OnModelCreating(ModelBuilder builder)
            {
                builder.Entity<User>(etp =>
                {
                    etp.Property(u => u.Id).HasConversion(new AdministrationUserIdValueConverter()).IsRequired();
                });
            }
        }

        public class UserReader : DbContextReader<UserDbContext, User, AdministrationUserId>
        {
            public UserReader(UserDbContext context)
                : base(context)
            {

            }
        }

        public class AdministrationUserIdValueConverter : ValueConverter<AdministrationUserId, int>
        {
            public AdministrationUserIdValueConverter()
                : base(id => id.Value, id => new AdministrationUserId(id))
            {

            }
        }
    }
}
