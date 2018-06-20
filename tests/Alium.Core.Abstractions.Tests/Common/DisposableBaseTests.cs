// Copyright (c) Alium FX. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Alium
{
    using System;
    using Xunit;

    /// <summary>
    /// Provides tests for the <see cref="DisposableBase"/> type
    /// </summary>
    public class DisposableBaseTests
    {
        [Fact]
        public void Disposed_IsTrueIfObjectIsDisposed()
        {
            // Arrange
            var disposable = new Disposable();

            // Act
            Assert.False(disposable.Disposed);
            disposable.Dispose();

            // Assert
            Assert.True(disposable.Disposed);
        }

        [Fact]
        public void ExplicitDispose_IsCalledThroughDispose()
        {
            // Arrange
            bool @explicit = false;
            var disposable = new Disposable(onExplicitDispose: () => @explicit = true);

            // Act
            disposable.Dispose();

            // Assert
            Assert.True(@explicit);
        }

        [Fact]
        public void ImplicitDispose_IsCalledThroughDispose()
        {
            // Arrange
            bool @implicit = false;
            var disposable = new Disposable(onImplicitDispose: () => @implicit = true);

            // Act
            disposable.Dispose();

            // Assert
            Assert.True(@implicit);
        }

        [Fact]
        public void ExplicitDispose_IsNotCalledThroughFinaliser()
        {
            // Arrange
            bool @explicit = false;
            WeakReference<Disposable> weak = null;

            Action act = () =>
            {
                var disposable = new Disposable(onExplicitDispose: () => @explicit = true);
                weak = new WeakReference<Disposable>(disposable, true);
            };

            // Act
            act();
            GC.Collect(0, GCCollectionMode.Forced);
            GC.WaitForPendingFinalizers();

            // Assert
            Assert.False(@explicit);
        }

        [Fact]
        public void ImplicitDispose_IsCalledThroughFinaliser()
        {
            // Arrange
            bool @implicit = false;
            WeakReference<Disposable> weak = null;

            Action act = () =>
            {
                var disposable = new Disposable(onImplicitDispose: () => @implicit = true);
                weak = new WeakReference<Disposable>(disposable, true);
            };

            // Act
            act();
            GC.Collect(0, GCCollectionMode.Forced);
            GC.WaitForPendingFinalizers();

            // Assert
            Assert.True(@implicit);
        }

        [Fact]
        public void Dispose_WillOnlyProcessOnce()
        {
            // Arrange
            int calls = 0;
            var disposable = new Disposable(onImplicitDispose: () => calls++);

            // Act
            disposable.Dispose();
            disposable.Dispose();

            // Assert
            Assert.Equal(1, calls);
        }

        class Disposable : DisposableBase
        {
            private readonly Action _onExplicitDispose;
            private readonly Action _onImplicitDispose;

            public Disposable(Action onExplicitDispose = null, Action onImplicitDispose = null)
            {
                _onExplicitDispose = onExplicitDispose;
                _onImplicitDispose = onImplicitDispose;
            }

            protected override void DisposeExplicit()
                => _onExplicitDispose?.DynamicInvoke();

            protected override void DisposeImplicit()
                => _onImplicitDispose?.DynamicInvoke();
        }
    }
}
