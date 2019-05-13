// Copyright (c) Alium FX. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Alium.Administration
{
    using Alium.Membership;

    /// <summary>
    /// Represents an administration user
    /// </summary>
    public class AdministrationUser : User<AdministrationUserId>
    {
        /// <summary>
        /// Initialises a new instance of <see cref="AdministrationUser"/>
        /// </summary>
        /// <param name="emailAddress">The email address</param>
        /// <param name="userName">The username</param>
        public AdministrationUser(string emailAddress, string userName)
            : base(emailAddress, userName) { }
    }
}
