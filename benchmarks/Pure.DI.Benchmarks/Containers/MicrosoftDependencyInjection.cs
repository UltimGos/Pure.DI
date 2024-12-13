﻿// ReSharper disable UnusedType.Global
namespace Pure.DI.Benchmarks.Containers;

using Microsoft.Extensions.DependencyInjection;

// ReSharper disable once ClassNeverInstantiated.Global
internal sealed class MicrosoftDependencyInjection : BaseAbstractContainer<ServiceProvider>
{
    private readonly IServiceCollection _serviceCollection = new ServiceCollection();
    private readonly Lazy<ServiceProvider> _container;

    public MicrosoftDependencyInjection() =>
        _container = new Lazy<ServiceProvider>(() => _serviceCollection.BuildServiceProvider());

    public override ServiceProvider CreateContainer() => _container.Value;

    public override IAbstractContainer<ServiceProvider> Bind(
        Type contractType,
        Type implementationType,
        AbstractLifetime lifetime = AbstractLifetime.Transient,
        string? name = null)
    {
        switch (lifetime)
        {
            case AbstractLifetime.Transient:
                _serviceCollection.AddKeyedTransient(contractType, name, implementationType);
                break;

            case AbstractLifetime.Singleton:
                _serviceCollection.AddKeyedSingleton(contractType, name, implementationType);
                break;

            default:
                throw new ArgumentOutOfRangeException(nameof(lifetime), lifetime, null);
        }

        return this;
    }

    public override T Resolve<T>() where T : class => _container.Value.GetService<T>()!;

    public override void Dispose() { }
}