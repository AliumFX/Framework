// Copyright (c) Alium FX. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.


namespace Alium
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Sorts a source collection of elements using a topological sort.
    /// </summary>
    /// <typeparam name="TElement">The element type.</typeparam>
    /// <typeparam name="TDependencyKey">The dependency key.</typeparam>
    public class DependencyKeyOrderedEnumerable<TElement, TDependencyKey> : IEnumerable<TElement>
    {
        private readonly IEnumerable<TElement> _source;
        private readonly Func<TElement, TDependencyKey> _keySelector;
        private readonly Func<TElement, TDependencyKey, IEnumerable<TDependencyKey>?> _dependentKeySelector;

        /// <summary>
        /// Initialises a new instance of <see cref="DependencyKeyOrderedEnumerable{TElement, TDependencyKey}"/>.
        /// </summary>
        /// <param name="source">The source set of items.</param>
        /// <param name="keySelector">The key selector.</param>
        /// <param name="dependentKeySelector">The dependent key selector.</param>
        public DependencyKeyOrderedEnumerable(
            IEnumerable<TElement> source,
            Func<TElement, TDependencyKey> keySelector,
            Func<TElement, TDependencyKey, IEnumerable<TDependencyKey>?> dependentKeySelector)
        {
            _source = Ensure.IsNotNull(source, nameof(source));
            _keySelector = Ensure.IsNotNull(keySelector, nameof(keySelector));
            _dependentKeySelector = Ensure.IsNotNull(dependentKeySelector, nameof(dependentKeySelector));
        }

        /// <inheritdoc />
        public IEnumerator<TElement> GetEnumerator()
            => new DependencyKeyOrderedEnumerator(_source, _keySelector, _dependentKeySelector);

        /// <inheritdoc />
        IEnumerator IEnumerable.GetEnumerator()
            => GetEnumerator();

        private class DependencyKeyOrderedEnumerator : IEnumerator<TElement>
        {
            private readonly Dictionary<TDependencyKey, TElement> _source;
            private readonly Func<TElement, TDependencyKey, IEnumerable<TDependencyKey>?> _dependentKeySelector;

            private readonly IEnumerator<TElement> _output;

            /// <summary>
            /// Initialises a new instance of <see cref="DependencyKeyOrderedEnumerator"/>.
            /// </summary>
            /// <param name="source">The source set of items.</param>
            /// <param name="keySelector">The key selector.</param>
            /// <param name="dependentKeySelector">The dependent key selector.</param>
            public DependencyKeyOrderedEnumerator(
                IEnumerable<TElement> source,
                Func<TElement, TDependencyKey> keySelector,
                Func<TElement, TDependencyKey, IEnumerable<TDependencyKey>?> dependentKeySelector)
            {
                Ensure.IsNotNull(source, nameof(source));
                Ensure.IsNotNull(keySelector, nameof(keySelector));

                _source = source.ToDictionary(e => keySelector(e));
                _dependentKeySelector = Ensure.IsNotNull(dependentKeySelector, nameof(dependentKeySelector));

                _output = SortDependencies();
            }

            private IEnumerator<TElement> SortDependencies()
            {
                var sorted = new HashSet<TElement>();
                var visited = new Dictionary<TDependencyKey, bool>();
                var stack = new Stack<TDependencyKey>();

                foreach (var pair in _source)
                {
                    Visit(pair.Value, pair.Key, sorted, visited, stack);
                }

                return sorted.GetEnumerator();
            }

            private void Visit(TElement item, TDependencyKey key, HashSet<TElement> sorted, Dictionary<TDependencyKey, bool> visited, Stack<TDependencyKey> stack)
            {
                if (visited.TryGetValue(key, out bool visiting))
                {
                    if (visiting)
                    {
                        throw new InvalidOperationException($"Cyclic dependency: '{string.Join(" => ", stack.Reverse())} => {key}!'");
                    }
                }
                else
                {
                    visited[key] = true;
                    stack.Push(key);

                    var dependencies = _dependentKeySelector(item, key);
                    if (dependencies is object)
                    {
                        foreach (var dependency in dependencies)
                        {
                            if (_source.TryGetValue(dependency, out TElement dependentItem))
                            {
                                Visit(dependentItem, dependency, sorted, visited, stack);
                            }
                            else
                            {
                                throw new InvalidOperationException($"Missing dependency: '{string.Join(" => ", stack.Reverse())} => {dependency}?'");
                            }
                        }
                    }

                    stack.Pop();
                    visited[key] = false;
                    sorted.Add(item);
                }
            }

            /// <inheritdoc />
            public TElement Current 
                => _output.Current;

            /// <inheritdoc />
            object? IEnumerator.Current
                => Current;

            /// <inheritdoc />
            public void Dispose()
                => _output.Dispose();

            /// <inheritdoc />
            public bool MoveNext()
                => _output.MoveNext();

            /// <inheritdoc />
            public void Reset()
                => _output.Reset();
        }
    }
}
