// Copyright (c) Alium Fx. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Alium.Tenancy
{
    using System;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.DependencyInjection;
    using Moq;
    using Xunit;

    /// <summary>
    /// Provides tests for the <see cref="TenantMiddleware"/> type
    /// </summary>
    public class TenantMiddlewareTests
    {
        [Fact]
        public void Constructor_ValidatesArguments()
        {
            // Arrange
            var tenantResolver = Mock.Of<ITenantResolver<HttpContext>>();
            var tenantServiceProviderResolver = Mock.Of<ITenantServiceProviderResolver>();

            // Act

            // Assert
            Assert.Throws<ArgumentNullException>("tenantResolver", () => new TenantMiddleware(
                null! /* tenantResolver */,
                tenantServiceProviderResolver));
            Assert.Throws<ArgumentNullException>("tenantServiceProviderResolver", () => new TenantMiddleware(
                tenantResolver,
                null! /* tenantServiceProviderResolver */));
        }

        [Fact]
        public async Task InvokeAsync_ValidatesArguments()
        {
            // Arrange
            var middleware = CreateMiddleware();
            var httpContext = new DefaultHttpContext();
            Task next(HttpContext context) => Task.CompletedTask;

            // Act

            // Assert
            await Assert.ThrowsAsync<ArgumentNullException>("context", async () => await middleware.InvokeAsync(
                null! /* httpContext */,
                next));
            await Assert.ThrowsAsync<ArgumentNullException>("next", async () => await middleware.InvokeAsync(
                httpContext,
                null! /* next */));
        }

        [Fact]
        public async Task InvokeAsync_FlowsTenantId_ToWorkContext()
        {
            // Arrange
            var tenantId = new TenantId("Test");
            var workContext = new WorkContext();
            var middleware = CreateMiddleware(
                tenantId: tenantId,
                workContext: workContext);
            var httpContext = new DefaultHttpContext();
            Task next(HttpContext context)
            {
                return Task.CompletedTask;
            }


            // Act
            await middleware.InvokeAsync(httpContext, next);

            // Assert
            Assert.Equal(tenantId, workContext.TenantId);
        }

        [Fact]
        public async Task InvokeAsync_DoesNotFlowTenantId_IfEmpty()
        {
            // Arrange
            var tenantId = TenantId.Empty;
            var workContext = new WorkContext();
            var middleware = CreateMiddleware(
                tenantId: tenantId,
                workContext: workContext);
            var httpContext = new DefaultHttpContext();
            Task next(HttpContext context)
            {
                return Task.CompletedTask;
            }


            // Act
            await middleware.InvokeAsync(httpContext, next);

            // Assert
            Assert.Equal(TenantId.Default, workContext.TenantId);
        }

        [Fact]
        public async Task InvokeAsync_DoesNotFlowTenantId_IfDefault()
        {
            // Arrange
            var tenantId = TenantId.Default;
            var workContext = new WorkContext();
            var middleware = CreateMiddleware(
                tenantId: tenantId,
                workContext: workContext);
            var httpContext = new DefaultHttpContext();
            Task next(HttpContext context)
            {
                return Task.CompletedTask;
            }


            // Act
            await middleware.InvokeAsync(httpContext, next);

            // Assert
            Assert.Equal(TenantId.Default, workContext.TenantId);
        }

        [Fact]
        public async Task InvokeAsync_ReplacesAndRestoresRequestServices_InvokingNext()
        {
            // Arrange
            var tenantId = new TenantId("Test");
            var tenantServices = new ServiceCollection();
            tenantServices.AddScoped<IWorkContext>(sp => new WorkContext());
            var tenantServiceProvider = tenantServices.BuildServiceProvider();
            var middleware = CreateMiddleware(
                tenantId: tenantId,
                tenantServiceProvider: tenantServiceProvider);
            var httpContext = new DefaultHttpContext();
            var requestServices = new ServiceCollection().BuildServiceProvider();
            httpContext.RequestServices = requestServices;
            Task next(HttpContext context)
            {
                Assert.Same(httpContext, context);
                Assert.NotSame(requestServices, context.RequestServices);

                return Task.CompletedTask;
            }

            // Act
            await middleware.InvokeAsync(httpContext, next);

            // Assert
            Assert.Same(requestServices, httpContext.RequestServices);
        }

        [Fact]
        public async Task InvokeAsync_SetsAndRemovesTenantServices_InvokingNext()
        {
            // Arrange
            var tenantId = new TenantId("Test");
            var tenantServices = new ServiceCollection();
            tenantServices.AddScoped<IWorkContext>(sp => new WorkContext());
            var tenantServiceProvider = tenantServices.BuildServiceProvider();
            var middleware = CreateMiddleware(
                tenantId: tenantId,
                tenantServiceProvider: tenantServiceProvider);
            var httpContext = new DefaultHttpContext();

            var preMiddlewareServiceProvider = httpContext.GetTenantServices();

            Task next(HttpContext context)
            {
                var duringMiddlewareServiceProvider = context.GetTenantServices();

                Assert.Same(tenantServiceProvider, duringMiddlewareServiceProvider);

                return Task.CompletedTask;
            }

            // Act
            await middleware.InvokeAsync(httpContext, next);

            var postMiddlewareServiceProvider = httpContext.GetTenantServices();

            // Assert
            Assert.Null(preMiddlewareServiceProvider);
            Assert.Null(postMiddlewareServiceProvider);
        }

        private TenantMiddleware CreateMiddleware(
            TenantId? tenantId = null,
            IServiceProvider? tenantServiceProvider = null,
            IServiceCollection? tenantServices = null,
            IWorkContext? workContext = null)
        {
            if (tenantServiceProvider == null)
            {
                if (tenantServices == null)
                {
                    tenantServices = new ServiceCollection();
                }
                if (workContext != null)
                {
                    var wc = workContext;
                    tenantServices.AddScoped<IWorkContext>(sp => wc);
                }
                tenantServiceProvider = tenantServices.BuildServiceProvider();
            }

            return new TenantMiddleware(
                CreateTenantResolver(tenantId),
                CreateTenantServiceProviderResolver(tenantServiceProvider));
        }

        private ITenantResolver<HttpContext> CreateTenantResolver(TenantId? tenantId = null)
        {
            var mock = new Mock<ITenantResolver<HttpContext>>();
            mock.Setup(m => m.ResolveCurrentAsync(It.IsAny<HttpContext>()))
                .Returns(() => Task.FromResult(tenantId.GetValueOrDefault(TenantId.Default)));

            return mock.Object;
        }

        private ITenantServiceProviderResolver CreateTenantServiceProviderResolver(IServiceProvider? tenantServiceProvider = null)
        {
            var mock = new Mock<ITenantServiceProviderResolver>();
            mock.Setup(m => m.GetTenantServiceProvider(It.IsAny<TenantId>()))
                .Returns(() => tenantServiceProvider ?? new ServiceCollection().BuildServiceProvider());

            return mock.Object;
        }
    }
}
