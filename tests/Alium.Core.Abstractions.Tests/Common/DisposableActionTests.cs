// Copyright (c) Alium FX. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Alium
{
    using System;
    using Xunit;

    /// <summary>
    /// Provides tests for the <see cref="DisposableAction"/> type
    /// </summary>
    public class DisposableActionTests
    {
        [Fact]
        public void Constructor_ValidatesArguments()
        {
            // Arrange

            // Act

            // Assert

            Assert.Throws<ArgumentNullException>(() => new Disposable(null! /* action */));
            Assert.Throws<ArgumentNullException>(() => new GenericDisposable(null! /* action */));

        }

        [Fact]
        public void DisposeExplicit_CallsAction_IfNotAllowedOnImplicitDispose()
        {
            // Arrange
            bool @explicit = false;
            bool @implicit = false;
            var disposable = new Disposable(
                () =>
                {
                    Assert.True(@explicit);
                    Assert.False(@implicit);
                },
                onExplicitDispose: () => @explicit = true,
                onImplicitdispose: () => @implicit = true);

            // Act
            disposable.Dispose();

            // Assert
        }

        [Fact]
        public void DisposeImplicit_CallsAction_IfAllowedOnImplicitDispose()
        {
            // Arrange
            bool @explicit = false;
            bool @implicit = false;
            var disposable = new Disposable(
                () =>
                {
                    Assert.False(@explicit);
                    Assert.True(@implicit);
                },
                allowOnImplicitDispose: true,
                onImplicitdispose: () => @implicit = true);

            // Act
            disposable.Dispose();

            // Assert
        }

        [Fact]
        public void Generic_DisposeExplicit_CallsAction_IfNotAllowedOnImplicitDispose()
        {
            // Arrange
            bool @explicit = false;
            bool @implicit = false;
            var disposable = new GenericDisposable(
                d =>
                {
                    Assert.True(@explicit);
                    Assert.False(@implicit);
                },
                onExplicitDispose: () => @explicit = true,
                onImplicitdispose: () => @implicit = true);

            // Act
            disposable.Dispose();

            // Assert
        }

        [Fact]
        public void Generic_DisposeImplicit_CallsAction_IfAllowedOnImplicitDispose()
        {
            // Arrange
            bool @explicit = false;
            bool @implicit = false;
            var disposable = new GenericDisposable(
                d =>
                {
                    Assert.False(@explicit);
                    Assert.True(@implicit);
                },
                allowOnImplicitDispose: true,
                onImplicitdispose: () => @implicit = true);

            // Act
            disposable.Dispose();

            // Assert
        }

        [Fact]
        public void Generic_Dispose_FlowsCurrentInstanceToAction()
        {
            // Arrange
            GenericDisposable? captured = null;
            var disposable = new GenericDisposable(
                d =>
                {
                    captured = d;
                });

            // Act
            disposable.Dispose();

            // Assert
            Assert.Same(disposable, captured);
        }

        public class Disposable : DisposableAction
        {
            private readonly Action? _onExplicitDispose;
            private readonly Action? _onImplicitDispose;

            public Disposable(
                Action action,
                bool allowOnImplicitDispose = false,
                Action? onExplicitDispose = null,
                Action? onImplicitdispose = null)
                : base(action, allowOnImplicitDispose)
            {
                _onExplicitDispose = onExplicitDispose;
                _onImplicitDispose = onImplicitdispose;
            }

            protected override void DisposeExplicit()
            {
                _onExplicitDispose?.DynamicInvoke();

                base.DisposeExplicit();
            }

            protected override void DisposeImplicit()
            {
                _onImplicitDispose?.DynamicInvoke();

                base.DisposeImplicit();
            }
        }

        public class GenericDisposable : DisposableAction<GenericDisposable>
        {
            private readonly Action? _onExplicitDispose;
            private readonly Action? _onImplicitDispose;

            public GenericDisposable(
                Action<GenericDisposable> action, 
                bool allowOnImplicitDispose = false,
                Action? onExplicitDispose = null,
                Action? onImplicitdispose = null)
                : base(action, allowOnImplicitDispose)
            {
                _onExplicitDispose = onExplicitDispose;
                _onImplicitDispose = onImplicitdispose;
            }

            protected override void DisposeExplicit()
            {
                _onExplicitDispose?.DynamicInvoke();

                base.DisposeExplicit();
            }

            protected override void DisposeImplicit()
            {
                _onImplicitDispose?.DynamicInvoke();

                base.DisposeImplicit();
            }
        }
    }
}
