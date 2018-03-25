// Copyright (c) Alium Fx. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Alium.Tenancy
{
    using System;
    using System.Collections.Generic;
    using Microsoft.Extensions.DependencyInjection;

    /// <summary>
    /// Creates <see cref="IServiceCollection"/> instances for tenants
    /// </summary>
    public class TenantServiceCollectionFactory
    {
        private readonly IServiceProvider _provider;
        private readonly IServiceCollection _services;

        /// <summary>
        /// Initialises a new instance of <see cref="TenantServiceCollectionFactory"/>
        /// </summary>
        /// <param name="provider">The service provider</param>
        /// <param name="services">The set of services</param>
        public TenantServiceCollectionFactory(IServiceProvider provider, IServiceCollection services)
        {
            _provider = Ensure.IsNotNull(provider, nameof(provider));
            _services = Ensure.IsNotNull(services, nameof(services));
        }

        /// <summary>
        /// Creates a tenant-scoped service collection
        /// </summary>
        /// <returns>The service collection</returns>
        public IServiceCollection CreateServiceCollection()
        {
            /**
             * MA - So two problems here really - open generics that are singletons can only be defined using
             *      an open generic implementation type, instead of a delegate factory. Also, our singleton 
             *      instances need to be eagerly pulled from the root provider because if we didn't, then it could
             *      lead to scenarios where services are unexpectedly disposed by the tenant-scoped provider. We have
             *      to eagerly resolve the instances, and then also map across enumerable definitions of each to handle
             *      calls for GetServices(x). Let's do what OrchardCore does until future DI changes allow us to better
             *      support this.
             * 
             * See: https://github.com/aspnet/Home/issues/2964
             * See: https://github.com/OrchardCMS/OrchardCore/blob/b87afc47b2620a47799575de4360399782aca6e6/src/OrchardCore/OrchardCore.Environment.Shell.Abstractions/Builders/Extensions/ServiceProviderExtensions.cs
             */

            // MA - Collect the types we will need to find enumerables for
            var types = new HashSet<Type>();
            var serviceCollectionType = typeof(IServiceCollection);

            IServiceCollection services = new ServiceCollection();
            foreach (var descriptor in _services)
            {
                if (descriptor.Lifetime != ServiceLifetime.Singleton)
                {
                    // MA - Scoped and transient service descriptors can be added directly.
                    services.Add(descriptor);
                }
                else
                {
                    if (descriptor is TenantScopedServiceDescriptor tssd)
                    {
                        // MA - Tenant scoped services become singletons in tenant-scoped service provider.
                        services.Add(descriptor);
                    }
                    else if (descriptor.ServiceType != serviceCollectionType)
                    {
                        if (descriptor.ServiceType.IsGenericType && descriptor.ServiceType.GenericTypeArguments.Length == 0)
                        {
                            // MA - Register open generics directly with the tenant-scoped provider. Not ideal as it means they are not true singletons.
                            services.Add(descriptor);
                        }
                        else
                        {
                            // MA - Eagerly resolve the service
                            services.Add(new ServiceDescriptor(
                                descriptor.ServiceType,
                                _provider.GetService(descriptor.ServiceType)));

                            if (!types.Contains(descriptor.ServiceType))
                            {
                                types.Add(descriptor.ServiceType);

                                // MA - Create an enumerable type for resolving all instances
                                var enumerableType = typeof(IEnumerable<>).MakeGenericType(descriptor.ServiceType);
                                services.Add(new ServiceDescriptor(
                                    enumerableType,
                                    _provider.GetServices(descriptor.ServiceType)));
                            }
                        }
                    }
                }
            }

            // MA - Redefine this registration
            services.AddSingleton<IServiceCollection>(services);

            return services;
        }
    }
}
