// Copyright (c) Alium FX. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Alium.Tenancy
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.Configuration;
    using Moq;
    using Xunit;

    using Alium.Features;
        
    /// <summary>
    /// Provides tests for the <see cref="TenantResolver"/> type.
    /// </summary>
    public class TenantResolverTests
    {
        [Fact]
        public void Constructor_ValidatesArguments()
        {
            // Arrange

            // Act

            // Assert
            Assert.Throws<ArgumentNullException>(() => new TenantResolver(null /* featureStateProvider */));
        }

        [Fact]
        public async Task ResolveCurrentAsync_ValidatesArguments()
        {
            // Arrange
            var featureStateProvider = CreateFeatureStateProvider();
            var resolver = new TenantResolver(featureStateProvider);

            // Act

            // Assert
            await Assert.ThrowsAsync<ArgumentNullException>(async () => await resolver.ResolveCurrentAsync(null /* httpContext */));
        }

        [Fact]
        public async Task ResolveCurrentAsync_ReturnsDefault_IfTenancyNotEnabled()
        {
            // Arrange
            var configuration = CreateConfiguration(new Dictionary<string, string>
            {
                ["Features:Core.Tenancy:Enabled"] = "false"
            });
            var featureStateProvider = CreateFeatureStateProvider(
                state: new FeatureState(CoreInfo.TenancyFeatureId, configuration.GetSection("Features:Core.Tenancy"), false));
            var httpContext = CreateHttpContext(new HostString("localhost:5000"));
            var resolver = new TenantResolver(featureStateProvider);

            // Act
            var tenantId = await resolver.ResolveCurrentAsync(httpContext);

            // Assert
            Assert.Equal(TenantId.Default, tenantId);
        }

        [Fact]
        public async Task ResolveCurrentAsync_ReturnsTenantId_ForMatchingHostName()
        {
            // Arrange
            var expectedTenantId = new TenantId("Tenant01");
            var configuration = CreateConfiguration(new Dictionary<string, string>
            {
                ["Features:Core.Tenancy:Enabled"] = "true",
                ["Features:Core.Tenancy:Tenants:Tenant01:HostNames:0"] = "localhost:5000"
            });
            var featureStateProvider = CreateFeatureStateProvider(
                state: new FeatureState(CoreInfo.TenancyFeatureId, configuration.GetSection("Features:Core.Tenancy"), true));
            var httpContext = CreateHttpContext(new HostString("localhost:5000"));
            var resolver = new TenantResolver(featureStateProvider);

            // Act
            var tenantId = await resolver.ResolveCurrentAsync(httpContext);

            // Assert
            Assert.Equal(expectedTenantId, tenantId);
        }

        [Fact]
        public async Task ResolveCurrentAsync_BeginsFeatureStateTenantScope_ForMatchingHostName()
        {
            // Arrange
            var matchedTenantId = TenantId.Empty;
            var configuration = CreateConfiguration(new Dictionary<string, string>
            {
                ["Features:Core.Tenancy:Enabled"] = "true",
                ["Features:Core.Tenancy:Tenants:Tenant01:HostNames:0"] = "localhost:5000"
            });
            var featureStateProvider = CreateFeatureStateProvider(
                state: new FeatureState(CoreInfo.TenancyFeatureId, configuration.GetSection("Features:Core.Tenancy"), true),
                onBeginTenantScope: t => matchedTenantId = t);
            var httpContext = CreateHttpContext(new HostString("localhost:5000"));
            var resolver = new TenantResolver(featureStateProvider);

            // Act
            var tenantId = await resolver.ResolveCurrentAsync(httpContext);

            // Assert
            Assert.Equal(tenantId, matchedTenantId);
        }

        [Fact]
        public async Task ResolveCurrentAsync_ReturnsEmpty_ForMissingTenantId()
        {
            // Arrange
            var configuration = CreateConfiguration(new Dictionary<string, string>
            {
                ["Features:Core.Tenancy:Enabled"] = "true",
                ["Features:Core.Tenancy:Tenants:Tenant01:HostNames:0"] = "localhost:5000"
            });
            var featureStateProvider = CreateFeatureStateProvider(
                state: new FeatureState(CoreInfo.TenancyFeatureId, configuration.GetSection("Features:Core.Tenancy"), true));
            var httpContext = CreateHttpContext(new HostString("localhost:5001"));
            var resolver = new TenantResolver(featureStateProvider);

            // Act
            var tenantId = await resolver.ResolveCurrentAsync(httpContext);

            // Assert
            Assert.Equal(TenantId.Empty, tenantId);
        }

        private HttpContext CreateHttpContext(HostString host)
        {
            var context = new DefaultHttpContext();
            context.Request.Host = host;

            return context;
        }

        private IConfiguration CreateConfiguration(IDictionary<string, string> values)
        {
            var configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(values)
                .Build();

            return configuration;
        }

        private IFeatureStateProvider CreateFeatureStateProvider(FeatureState state = null, Action<TenantId> onBeginTenantScope = null)
        {
            var mock = new Mock<IFeatureStateProvider>();

            mock.Setup(fsp => fsp.GetFeatureState(It.IsAny<FeatureId>()))
                .Returns(state);

            mock.Setup(fsp => fsp.BeginTenantScope(It.IsAny<TenantId>()))
                .Callback(onBeginTenantScope ?? (t => { }));

            return mock.Object;
        }
    }
}
