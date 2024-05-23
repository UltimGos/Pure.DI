﻿/*
$v=true
$p=99
$d=Service provider
$h=The `// ObjectResolveMethodName = GetService` hint overrides the _object Resolve(Type type)_ method name in _GetService_, allowing the _IServiceProvider_ interface to be implemented in a partial class.
*/

// ReSharper disable ClassNeverInstantiated.Local
// ReSharper disable CheckNamespace
// ReSharper disable UnusedParameter.Local
// ReSharper disable ArrangeTypeModifiers
// ReSharper disable UnusedMember.Local
// ReSharper disable ArrangeTypeMemberModifiers
namespace Pure.DI.UsageTests.BCL.ServiceProviderScenario;

using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using Xunit;

// {
interface IDependency;

class Dependency : IDependency;

interface IService
{
    IDependency Dependency { get; }
}

class Service(IDependency dependency) : IService
{
    public IDependency Dependency { get; } = dependency;
}

partial class Composition: IServiceProvider
{
    void Setup() =>
        DI.Setup()
            // The following hint overrides the name of the
            // "object Resolve(Type type)" method in "GetService",
            // which implements the "IServiceProvider" interface
            .Hint(Hint.ObjectResolveMethodName, "GetService")
            .Bind<IDependency>().As(Lifetime.Singleton).To<Dependency>()
            .Bind<IService>().To<Service>()
            .Root<IDependency>()
            .Root<IService>();
}
// }

public class Scenario
{
    [Fact]
    public void Run()
    {
// {            
        var serviceProvider = new Composition();
        var service = serviceProvider.GetRequiredService<IService>();
        var dependency = serviceProvider.GetRequiredService<IDependency>();
        service.Dependency.ShouldBe(dependency);
// }            
        serviceProvider.SaveClassDiagram();
    }
}