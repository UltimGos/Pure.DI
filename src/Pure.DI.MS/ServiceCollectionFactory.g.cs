// <auto-generated/>
// #pragma warning disable

namespace Pure.DI.MS;

using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

/// <summary>
/// Creates a service collection <see cref="Microsoft.Extensions.DependencyInjection.ServiceCollection"/> based resolvers.
/// </summary>
/// <typeparam name="TComposition">The composition class itself.</typeparam>
[global::System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
internal class ServiceCollectionFactory<TComposition>
{
    /// <summary>
    /// A list of instance resolvers, specifying the type of object that the resolver returns.
    /// </summary>
    private readonly List<(Type ServiceType, IResolver<TComposition, object> Resolver)> _resolvers = new();
    
    /// <summary>
    /// Registers the resolver of a composition for use in a collection of services.
    /// </summary>
    /// <param name="resolver">Instance resolver.</param>
    /// <typeparam name="TContract">The type of object that the resolver returns.</typeparam>
    [global::System.Runtime.CompilerServices.MethodImpl((global::System.Runtime.CompilerServices.MethodImplOptions)0x300)]
    public void AddResolver<TContract>(IResolver<TComposition, TContract> resolver) => 
        _resolvers.Add((typeof(TContract), (IResolver<TComposition, object>)resolver));

    /// <summary>
    /// Creates a service collection <see cref="Microsoft.Extensions.DependencyInjection.ServiceCollection"/> based on all previously registered resolvers.
    /// </summary>
    /// <param name="composition">An instance of composition.</param>
    /// <returns>An instance of <see cref="Microsoft.Extensions.DependencyInjection.ServiceCollection"/>.</returns>
#if NETSTANDARD2_0_OR_GREATER || NETCOREAPP || NET40_OR_GREATER
    [global::System.Diagnostics.Contracts.Pure]
#endif
    [global::System.Runtime.CompilerServices.MethodImpl((global::System.Runtime.CompilerServices.MethodImplOptions)0x300)]
    public IServiceCollection CreateServiceCollection(TComposition composition) => 
        new ServiceCollection().Add(CreateDescriptors(composition));

    /// <summary>
    /// Creates an enumeration of service descriptors based on all previously registered resolvers.
    /// </summary>
    /// <param name="composition">An instance of composition.</param>
    /// <returns>A enumeration of <see cref="Microsoft.Extensions.DependencyInjection.ServiceDescriptor"/>.</returns>
    [global::System.Runtime.CompilerServices.MethodImpl((global::System.Runtime.CompilerServices.MethodImplOptions)0x300)]
    private IEnumerable<ServiceDescriptor> CreateDescriptors(TComposition composition) =>
        _resolvers.Select(resolver => CreateDescriptor(composition, resolver.ServiceType, resolver.Resolver));

    /// <summary>
    /// Creates a service descriptor for the passed resolver.
    /// </summary>
    /// <param name="composition">An instance of composition.</param>
    /// <param name="serviceType">The type of object that the resolver returns.</param>
    /// <param name="resolver">Instance resolver.</param>
    /// <returns>An instance of <see cref="Microsoft.Extensions.DependencyInjection.ServiceDescriptor"/>.</returns>
    [global::System.Runtime.CompilerServices.MethodImpl((global::System.Runtime.CompilerServices.MethodImplOptions)0x300)]
    private static ServiceDescriptor CreateDescriptor(TComposition composition, Type serviceType, IResolver<TComposition, object> resolver) =>
        new(serviceType, _ => resolver.Resolve(composition), ServiceLifetime.Transient);
}

// #pragma warning restore