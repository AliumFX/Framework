// Copyright (c) Alium FX. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Alium.Membership
{
    using System;

    using Alium.Administration;
    using Alium.Data;

    /// <summary>
    /// Provides a base implementation of a user
    /// </summary>
    /// <typeparam name="TPrimaryKey">The primary key type</typeparam>
    public abstract class User<TPrimaryKey> : User<TPrimaryKey, AdministrationUserId>
        where TPrimaryKey : struct
    {
        /// <summary>
        /// Initialises a new instance of <see cref="User{TPrimaryKey}"/>
        /// </summary>
        /// <param name="emailAddress">The email address</param>
        /// <param name="userName">The username</param>
        protected User(string emailAddress, string userName)
            : base(emailAddress, userName) { }
    }

    /// <summary>
    /// Provides a base implementation of a user
    /// </summary>
    /// <typeparam name="TPrimaryKey">The primary key type</typeparam>
    /// <typeparam name="TUserId">The user ID type</typeparam>
    public abstract class User<TPrimaryKey, TUserId> : Entity<TPrimaryKey, TUserId>
        where TPrimaryKey : struct
        where TUserId : struct
    {
        /// <summary>
        /// Initialises a new instance of <see cref="User{TPrimaryKey, TUserId}"/>
        /// </summary>
        /// <param name="emailAddress">The email address</param>
        /// <param name="userName">The username</param>
        protected User(string emailAddress, string userName)
        {
            EmailAddress = Ensure.IsNotNullOrEmpty(emailAddress, nameof(emailAddress));
            UserName = Ensure.IsNotNullOrEmpty(userName, nameof(userName));
        }

        /// <summary>
        /// Gets or sets the email address
        /// </summary>
        public string EmailAddress { get; set; } = default!;

        /// <summary>
        /// Gets or sets the first name
        /// </summary>
        public string? FirstName { get; set; }

        /// <summary>
        /// Gets or sets the last name
        /// </summary>
        public string? LastName { get; set; }

        /// <summary>
        /// Gets or sets the username
        /// </summary>
        public string UserName { get; set; } = default!;
    }
}
