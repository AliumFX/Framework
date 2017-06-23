// Copyright (c) Alium FX. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Alium
{
    using System.Collections.Generic;

    /// <summary>
    /// Defines the required contract for implementing an object comparer.
    /// </summary>
    /// <typeparam name="TObject"></typeparam>
    public interface IObjectComparer<in TObject> : IComparer<TObject>, IEqualityComparer<TObject>
        where TObject : class
    {
    }
}
