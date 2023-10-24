// <auto-generated/>
#pragma warning disable

namespace Pure.DI.MS;

using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

/// <summary>
/// A base class for a composition that can be used as a service provider factory <see cref="Microsoft.Extensions.DependencyInjection.IServiceProviderFactory{TContainerBuilder}"/>.
/// <example>
/// For example:
/// <code>
/// internal partial class Composition: ServiceProviderFactory&lt;Composition&gt;
/// {
///     private static void Setup() =&gt;
///         DI.Setup(nameof(Composition))
///             .DependsOn(Base)
///             .Root&lt;HomeController&gt;();
/// }
/// </code>
/// </example> 
/// </summary>
/// <typeparam name="TComposition"></typeparam>
[global::System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
internal class ServiceProviderFactory<TComposition>: IServiceProviderFactory<IServiceCollection>
    where TComposition: ServiceProviderFactory<TComposition>
{
    /// <summary>
    /// The name of the Pure.DI setup to use as a dependency in other setups.
    /// <example>
    /// For example:
    /// <code>
    /// private static void Setup() =&amp;gt;
    ///     DI.Setup(nameof(Composition)).DependsOn(Base);
    /// </code>
    /// </example>
    /// </summary>
    protected const string Base = "Pure.DI.MS.ServiceProviderFactory";
    
    /// <summary>
    /// An instance of <see cref="Pure.DI.MS.ServiceCollectionFactory"/>.
    /// </summary>
    private static readonly ServiceCollectionFactory<TComposition> ServiceCollectionFactory = new();
    
    /// <summary>
    /// <see cref="System.IServiceProvider"/> instance for resolving external dependencies.
    /// </summary>
    private volatile IServiceProvider? _serviceProvider;
    
    /// <summary>
    /// DI setup hints.
    /// </summary>
    [global::System.Diagnostics.Conditional("A2768DE22DE3E430C9653990D516CC9B")]
    private static void HintsSetup() =>
        DI.Setup(Base, CompositionKind.Internal)
            .Hint(Hint.OnCannotResolve, "On")
            .Hint(Hint.OnCannotResolvePartial, "Off")
            .Hint(Hint.OnNewRoot, "On")
            .Hint(Hint.OnNewRootPartial, "Off");

    /// <summary>
    /// Creates a service collection <see cref="Microsoft.Extensions.DependencyInjection.ServiceCollection"/> based on the registered composition.
    /// </summary>
    /// <param name="composition">An instance of composition.</param>
    /// <returns>An instance of <see cref="Microsoft.Extensions.DependencyInjection.ServiceCollection"/>.</returns>
#if NETSTANDARD2_0_OR_GREATER || NETCOREAPP || NET40_OR_GREATER
    [global::System.Diagnostics.Contracts.Pure]
#endif
    [global::System.Runtime.CompilerServices.MethodImpl((global::System.Runtime.CompilerServices.MethodImplOptions)0x300)]
    protected static IServiceCollection CreateServiceCollection(TComposition composition) =>
        ServiceCollectionFactory.CreateServiceCollection(composition);

    /// <inheritdoc />
    [global::System.Runtime.CompilerServices.MethodImpl((global::System.Runtime.CompilerServices.MethodImplOptions)0x300)]
    public IServiceCollection CreateBuilder(IServiceCollection services) =>
        // Registers composition roots as services in the service collection.
        services.Add(CreateServiceCollection((TComposition)this));

    /// <inheritdoc />
    [global::System.Runtime.CompilerServices.MethodImpl((global::System.Runtime.CompilerServices.MethodImplOptions)0x300)]
    public IServiceProvider CreateServiceProvider(IServiceCollection services)
    {
        var serviceProvider = services.BuildServiceProvider();
        // Saves the service provider to use it to resolve dependencies external to this composition from the service provider.
        _serviceProvider ??= services.BuildServiceProvider();
        return serviceProvider;
    }

    /// <summary>
    /// Used to resolve external dependencies using the service provider <see cref="System.IServiceProvider"/>.
    /// </summary>
    /// <param name="tag">Dependency resolution tag.</param>
    /// <param name="lifetime">Dependency resolution lifetime.</param>
    /// <typeparam name="T">Dependency resolution type.</typeparam>
    /// <returns>Resolved dependency instance.</returns>
    [global::System.Runtime.CompilerServices.MethodImpl((global::System.Runtime.CompilerServices.MethodImplOptions)0x300)]
    protected T OnCannotResolve<T>(object? tag, Lifetime lifetime) =>
        _serviceProvider.GetRequiredService<T>();
    
    /// <summary>
    /// Registers a composition resolver for use in a service collection <see cref="Microsoft.Extensions.DependencyInjection.ServiceCollection"/>.
    /// </summary>
    /// <param name="resolver">Instance resolver.</param>
    /// <param name="name">The name of the composition root.</param>
    /// <param name="tag">The tag of the composition root.</param>
    /// <param name="lifetime">The lifetime of the composition root.</param>
    /// <typeparam name="TContract">The contract type of the composition root.</typeparam>
    /// <typeparam name="T">The implementation type of the composition root.</typeparam>
    [global::System.Runtime.CompilerServices.MethodImpl((global::System.Runtime.CompilerServices.MethodImplOptions)0x300)]
    protected static void OnNewRoot<TContract, T>(
        IResolver<TComposition, TContract> resolver,
        string name, object? tag, Lifetime lifetime) => 
        ServiceCollectionFactory.AddResolver(resolver);
}

#pragma warning restore