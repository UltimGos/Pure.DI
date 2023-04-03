#### Composition root

[![CSharp](https://img.shields.io/badge/C%23-code-blue.svg)](tests\Pure.DI.UsageTests\Basics\CompositionRootScenario.cs)

This example demonstrates the most efficient way to get the root object of a composition without impacting memory consumption or performance.

``` CSharp
internal interface IDependency { }

internal class Dependency : IDependency { }

internal interface IService { }

internal class Service : IService
{
    public Service(IDependency dependency)
    {
    }
}

DI.Setup("Composition")
    .Bind<IDependency>().To<Dependency>()
    .Bind<IService>().To<Service>()
    .Root<IService>("Root");

var composition = new Composition();
var service = composition.Root;
```

```mermaid
classDiagram
class Composition {
+IService Root
+T ResolveᐸTᐳ()
+T ResolveᐸTᐳ(object? tag)
+object ResolveᐸTᐳ(Type type)
+object ResolveᐸTᐳ(Type type, object? tag)
}
Dependency --|> IDependency : 
class Dependency {
+Dependency()
}
Service --|> IService : 
class Service {
+Service(IDependency dependency)
}
Composition ..> Service : IService Root
Service *-- Dependency : IDependency dependency
```

Actually, the property _Root_ looks like:
```csharp
public IService Root
{
  get
  {
    return new Service(new Dependency());
  }
}
```

#### Resolve methods

[![CSharp](https://img.shields.io/badge/C%23-code-blue.svg)](tests\Pure.DI.UsageTests\Basics\ResolveScenario.cs)

This example shows how to resolve the composition roots using the _Resolve_ methods.

``` CSharp
internal interface IDependency { }

internal class Dependency : IDependency { }

internal interface IService { }

internal class Service : IService
{
    public Service(IDependency dependency)
    {
    }
}

DI.Setup("Composition")
    .Bind<IDependency>().To<Dependency>()
    .Bind<IService>().To<Service>();

var composition = new Composition();
var service1 = composition.Resolve<IService>();
var service2 = composition.Resolve(typeof(IService));
```

```mermaid
classDiagram
class Composition {
+T ResolveᐸTᐳ()
+T ResolveᐸTᐳ(object? tag)
+object ResolveᐸTᐳ(Type type)
+object ResolveᐸTᐳ(Type type, object? tag)
}
Dependency --|> IDependency : 
class Dependency {
+Dependency()
}
Service --|> IService : 
class Service {
+Service(IDependency dependency)
}
Service *-- Dependency : IDependency dependency
```

#### Factory

[![CSharp](https://img.shields.io/badge/C%23-code-blue.svg)](tests\Pure.DI.UsageTests\Basics\FactoryScenario.cs)

This example demonstrates how to create and initialize an instance manually.

``` CSharp
internal interface IDependency
{
    DateTimeOffset Time { get; }

    bool IsInitialized { get; }
}

internal class Dependency : IDependency
{
    public Dependency(DateTimeOffset time)
    {
        Time = time;
    }

    public DateTimeOffset Time { get; }

    public bool IsInitialized { get; private set; }

    public void Initialize()
    {
        IsInitialized = true;
    }
}

internal interface IService
{
    IDependency Dependency { get; }
}

internal class Service : IService
{
    public Service(IDependency dependency)
    {
        Dependency = dependency;
    }

    public IDependency Dependency { get; }
}

DI.Setup("Composition")
    .Bind<IDependency>().To(_ =>
    {
        var dependency = new Dependency(DateTimeOffset.Now);
        dependency.Initialize();
        return dependency;
    })
    .Bind<IService>().To<Service>()
    .Root<IService>("Root");

var composition = new Composition();
var service = composition.Root;
service.Dependency.IsInitialized.ShouldBeTrue();
```

```mermaid
classDiagram
class Composition {
+IService Root
+T ResolveᐸTᐳ()
+T ResolveᐸTᐳ(object? tag)
+object ResolveᐸTᐳ(Type type)
+object ResolveᐸTᐳ(Type type, object? tag)
}
Dependency --|> IDependency : 
class Dependency
Service --|> IService : 
class Service {
+Service(IDependency dependency)
}
Composition ..> Service : IService Root
Service *-- Dependency : IDependency dependency
```

#### Injection

[![CSharp](https://img.shields.io/badge/C%23-code-blue.svg)](tests\Pure.DI.UsageTests\Basics\InjectScenario.cs)

This example shows how to create and initialize an instance manually injecting required dependencies.

``` CSharp
internal interface IDependency { }

internal class Dependency : IDependency
{
}

internal interface IService
{
    IDependency Dependency { get; }
}

internal class Service : IService
{
    public Service(IDependency dependency)
    {
        Dependency = dependency;
    }

    public IDependency Dependency { get; }
}

DI.Setup("Composition")
    .Bind<IDependency>().To<Dependency>()
    .Bind<IService>().To(ctx =>
    {
        ctx.Inject<IDependency>(out var dependency);
        return new Service(dependency);
    })
    .Root<IService>("Root");

var composition = new Composition();
var service = composition.Root;
```

```mermaid
classDiagram
class Composition {
+IService Root
+T ResolveᐸTᐳ()
+T ResolveᐸTᐳ(object? tag)
+object ResolveᐸTᐳ(Type type)
+object ResolveᐸTᐳ(Type type, object? tag)
}
Dependency --|> IDependency : 
class Dependency {
+Dependency()
}
Service --|> IService : 
class Service
Composition ..> Service : IService Root
Service *-- Dependency : IDependency
```

#### Generics

[![CSharp](https://img.shields.io/badge/C%23-code-blue.svg)](tests\Pure.DI.UsageTests\Basics\GenericsScenario.cs)

Generic types are also supported, this is easy to do by binding generic types and specifying generic markers like `TT`, `TT1` etc. as generic type parameters:

``` CSharp
internal interface IDependency<T> { }

internal class Dependency<T> : IDependency<T> { }

internal interface IService
{
    IDependency<int> IntDependency { get; }

    IDependency<string> StringDependency { get; }
}

internal class Service : IService
{
    public Service(IDependency<int> intDependency, IDependency<string> stringDependency)
    {
        IntDependency = intDependency;
        StringDependency = stringDependency;
    }

    public IDependency<int> IntDependency { get; }

    public IDependency<string> StringDependency { get; }
}

DI.Setup("Composition")
    .Bind<IDependency<TT>>().To<Dependency<TT>>()
    .Bind<IService>().To<Service>()
    .Root<IService>("Root");

var composition = new Composition();
var service = composition.Root;
service.IntDependency.ShouldBeOfType<Dependency<int>>();
service.StringDependency.ShouldBeOfType<Dependency<string>>();
```

```mermaid
classDiagram
class Composition {
+IService Root
+T ResolveᐸTᐳ()
+T ResolveᐸTᐳ(object? tag)
+object ResolveᐸTᐳ(Type type)
+object ResolveᐸTᐳ(Type type, object? tag)
}
Service --|> IService : 
class Service {
+Service(IDependencyᐸInt32ᐳ intDependency, IDependencyᐸStringᐳ stringDependency)
}
DependencyᐸInt32ᐳ --|> IDependencyᐸInt32ᐳ : 
class DependencyᐸInt32ᐳ {
+Dependency()
}
DependencyᐸStringᐳ --|> IDependencyᐸStringᐳ : 
class DependencyᐸStringᐳ {
+Dependency()
}
Composition ..> Service : IService Root
Service *-- DependencyᐸInt32ᐳ : IDependencyᐸInt32ᐳ intDependency
Service *-- DependencyᐸStringᐳ : IDependencyᐸStringᐳ stringDependency
```

Actually, the property _Root_ looks like:
```csharp
public IService Root
{
  get
  {
    return new Service(new Dependency<int>(), new Dependency<string>());
  }
}
```

#### Arguments

[![CSharp](https://img.shields.io/badge/C%23-code-blue.svg)](tests\Pure.DI.UsageTests\Basics\ArgumentsScenario.cs)

``` CSharp
internal interface IDependency { }

internal class Dependency : IDependency { }

internal interface IService
{
    string Name { get; }
}

internal class Service : IService
{
    public Service(string name, IDependency dependency)
    {
        Name = name;
    }

    public string Name { get; }
}

DI.Setup("Composition")
    .Bind<IDependency>().To<Dependency>()
    .Bind<IService>().To<Service>()
    .Arg<string>("serviceName")
    .Root<IService>("Root");

var composition = new Composition("Abc");
var service = composition.Root;
service.Name.ShouldBe("Abc");
```

```mermaid
classDiagram
class Composition {
+IService Root
+T ResolveᐸTᐳ()
+T ResolveᐸTᐳ(object? tag)
+object ResolveᐸTᐳ(Type type)
+object ResolveᐸTᐳ(Type type, object? tag)
}
Dependency --|> IDependency : 
class Dependency {
+Dependency()
}
Service --|> IService : 
class Service {
+Service(String name, IDependency dependency)
}
class String
Composition ..> Service : IService Root
Service o-- String : Argument "serviceName"
Service *-- Dependency : IDependency dependency
```

#### Tags

[![CSharp](https://img.shields.io/badge/C%23-code-blue.svg)](tests\Pure.DI.UsageTests\Basics\TagsScenario.cs)

This example demonstrates the most efficient way to get the root object of a composition without impacting memory consumption or performance.

``` CSharp
internal interface IDependency { }

internal class AbcDependency : IDependency { }

internal class XyzDependency : IDependency { }

internal class Dependency : IDependency { }

internal interface IService
{
    IDependency Dependency1 { get; }

    IDependency Dependency2 { get; }
}

internal class Service : IService
{
    public Service(
        [Tag("Abc")] IDependency dependency1,
        [Tag("Xyz")] IDependency dependency2)
    {
        Dependency1 = dependency1;
        Dependency2 = dependency2;
    }

    public IDependency Dependency1 { get; }

    public IDependency Dependency2 { get; }
}

DI.Setup("Composition")
    .Bind<IDependency>("Abc").To<AbcDependency>()
    .Bind<IDependency>("Xyz").To<XyzDependency>()
    .Bind<IService>().To<Service>()
    .Root<IService>("Root");

var composition = new Composition();
var service = composition.Root;
service.Dependency1.ShouldBeOfType<AbcDependency>();
service.Dependency2.ShouldBeOfType<XyzDependency>();
```

```mermaid
classDiagram
class Composition {
+IService Root
+T ResolveᐸTᐳ()
+T ResolveᐸTᐳ(object? tag)
+object ResolveᐸTᐳ(Type type)
+object ResolveᐸTᐳ(Type type, object? tag)
}
AbcDependency --|> IDependency : "Abc" 
class AbcDependency {
+AbcDependency()
}
XyzDependency --|> IDependency : "Xyz" 
class XyzDependency {
+XyzDependency()
}
Service --|> IService : 
class Service {
+Service(IDependency dependency1, IDependency dependency2)
}
Composition ..> Service : IService Root
Service *-- AbcDependency : "Abc"  IDependency dependency1
Service *-- XyzDependency : "Xyz"  IDependency dependency2
```

Sometimes it's important to take control of building a dependency graph. In this case, _tags_ help:

#### Auto-bindings

[![CSharp](https://img.shields.io/badge/C%23-code-blue.svg)](tests\Pure.DI.UsageTests\Basics\AutoBindingScenario.cs)

This approach works great even if DI doesn't have the appropriate bindings. :warning: But it can't be recommended if you follow the dependency inversion principle and want to make sure your types only depend on abstractions.

``` CSharp
internal class Dependency
{
}

internal class Service
{
    public Service(Dependency dependency)
    {
    }
}

DI.Setup("Composition")
    .Root<Service>("Root");

var composition = new Composition();
var service1 = composition.Root;
var service2 = composition.Resolve<Service>();
var service3 = composition.Resolve(typeof(Service));
```

```mermaid
classDiagram
class Composition {
+Service Root
+T ResolveᐸTᐳ()
+T ResolveᐸTᐳ(object? tag)
+object ResolveᐸTᐳ(Type type)
+object ResolveᐸTᐳ(Type type, object? tag)
}
class Dependency {
+Dependency()
}
Composition ..> Service : Service Root
Service *-- Dependency : Dependency dependency
```

#### Child composition

[![CSharp](https://img.shields.io/badge/C%23-code-blue.svg)](tests\Pure.DI.UsageTests\Basics\ChildCompositionScenario.cs)

``` CSharp
internal interface IDependency
{
    bool IsDisposed { get; }
}

internal class Dependency : IDependency, IDisposable
{
    public bool IsDisposed { get; private set; }

    public void Dispose()
    {
        IsDisposed = true;
    }
}

internal interface IService
{
    IDependency Dependency { get; }
}

internal class Service : IService
{
    public Service(IDependency dependency)
    {
        Dependency = dependency;
    }

    public IDependency Dependency { get; }
}

DI.Setup("Composition")
    .Bind<IDependency>().As(Lifetime.Singleton).To<Dependency>()
    .Bind<IService>().To<Service>()
    .Root<IService>("Root");

var composition = new Composition();
IService serviceFromChild;
using (var childComposition = new Composition(composition))
{
    serviceFromChild = childComposition.Root;
}
        
serviceFromChild.Dependency.IsDisposed.ShouldBeTrue();
        
var service = composition.Root;
using (var childComposition = new Composition(composition))
{
    childComposition.Root.Dependency.ShouldBe(service.Dependency);
}
```

```mermaid
classDiagram
class Composition {
+IService Root
+T ResolveᐸTᐳ()
+T ResolveᐸTᐳ(object? tag)
+object ResolveᐸTᐳ(Type type)
+object ResolveᐸTᐳ(Type type, object? tag)
}
Composition --|> IDisposable
Dependency --|> IDependency : 
class Dependency {
+Dependency()
}
Service --|> IService : 
class Service {
+Service(IDependency dependency)
}
Composition ..> Service : IService Root
Service o-- "Singleton" Dependency : IDependency dependency
```

#### Singleton

[![CSharp](https://img.shields.io/badge/C%23-code-blue.svg)](tests\Pure.DI.UsageTests\Lifetimes\SingletonScenario.cs)

``` CSharp
internal interface IDependency { }

internal class Dependency : IDependency { }

internal interface IService
{
    public IDependency Dependency1 { get; }

    public IDependency Dependency2 { get; }
}

internal class Service : IService
{
    public Service(IDependency dependency1, IDependency dependency2)
    {
        Dependency1 = dependency1;
        Dependency2 = dependency2;
    }

    public IDependency Dependency1 { get; }

    public IDependency Dependency2 { get; }
}

DI.Setup("Composition")
    .Bind<IDependency>().As(Lifetime.Singleton).To<Dependency>()
    .Bind<IService>().To<Service>()
    .Root<IService>("Root");

var composition = new Composition();
var service1 = composition.Root;
var service2 = composition.Root;
service1.Dependency1.ShouldBe(service1.Dependency2);
service2.Dependency1.ShouldBe(service1.Dependency1);
```

```mermaid
classDiagram
class Composition {
+IService Root
+T ResolveᐸTᐳ()
+T ResolveᐸTᐳ(object? tag)
+object ResolveᐸTᐳ(Type type)
+object ResolveᐸTᐳ(Type type, object? tag)
}
Dependency --|> IDependency : 
class Dependency {
+Dependency()
}
Service --|> IService : 
class Service {
+Service(IDependency dependency1, IDependency dependency2)
}
Composition ..> Service : IService Root
Service o-- "Singleton" Dependency : IDependency dependency1
Service o-- "Singleton" Dependency : IDependency dependency2
```

#### PerResolve

[![CSharp](https://img.shields.io/badge/C%23-code-blue.svg)](tests\Pure.DI.UsageTests\Lifetimes\PerResolveScenario.cs)

``` CSharp
internal interface IDependency { }

internal class Dependency : IDependency { }

internal interface IService
{
    public IDependency Dependency1 { get; }

    public IDependency Dependency2 { get; }
}

internal class Service : IService
{
    public Service(IDependency dependency1, IDependency dependency2)
    {
        Dependency1 = dependency1;
        Dependency2 = dependency2;
    }

    public IDependency Dependency1 { get; }

    public IDependency Dependency2 { get; }
}

DI.Setup("Composition")
    .Bind<IDependency>().As(Lifetime.PerResolve).To<Dependency>()
    .Bind<IService>().To<Service>()
    .Root<IService>("Root");

var composition = new Composition();
var service1 = composition.Root;
var service2 = composition.Root;
service1.Dependency1.ShouldBe(service1.Dependency2);
service2.Dependency1.ShouldNotBe(service1.Dependency1);
```

```mermaid
classDiagram
class Composition {
+IService Root
+T ResolveᐸTᐳ()
+T ResolveᐸTᐳ(object? tag)
+object ResolveᐸTᐳ(Type type)
+object ResolveᐸTᐳ(Type type, object? tag)
}
Dependency --|> IDependency : 
class Dependency {
+Dependency()
}
Service --|> IService : 
class Service {
+Service(IDependency dependency1, IDependency dependency2)
}
Composition ..> Service : IService Root
Service o-- "PerResolve" Dependency : IDependency dependency1
Service o-- "PerResolve" Dependency : IDependency dependency2
```

#### Transient

[![CSharp](https://img.shields.io/badge/C%23-code-blue.svg)](tests\Pure.DI.UsageTests\Lifetimes\TransientScenario.cs)

``` CSharp
internal interface IDependency { }

internal class Dependency : IDependency { }

internal interface IService
{
    public IDependency Dependency1 { get; }

    public IDependency Dependency2 { get; }
}

internal class Service : IService
{
    public Service(IDependency dependency1, IDependency dependency2)
    {
        Dependency1 = dependency1;
        Dependency2 = dependency2;
    }

    public IDependency Dependency1 { get; }

    public IDependency Dependency2 { get; }
}

DI.Setup("Composition")
    .Bind<IDependency>().As(Lifetime.Transient).To<Dependency>()
    .Bind<IService>().To<Service>()
    .Root<IService>("Root");

var composition = new Composition();
var service1 = composition.Root;
var service2 = composition.Root;
service1.Dependency1.ShouldNotBe(service1.Dependency2);
service2.Dependency1.ShouldNotBe(service1.Dependency1);
```

```mermaid
classDiagram
class Composition {
+IService Root
+T ResolveᐸTᐳ()
+T ResolveᐸTᐳ(object? tag)
+object ResolveᐸTᐳ(Type type)
+object ResolveᐸTᐳ(Type type, object? tag)
}
Dependency --|> IDependency : 
class Dependency {
+Dependency()
}
Service --|> IService : 
class Service {
+Service(IDependency dependency1, IDependency dependency2)
}
Composition ..> Service : IService Root
Service *-- Dependency : IDependency dependency1
Service *-- Dependency : IDependency dependency2
```

#### Disposable Singleton

[![CSharp](https://img.shields.io/badge/C%23-code-blue.svg)](tests\Pure.DI.UsageTests\Lifetimes\DisposableSingletonScenario.cs)

To dispose all created singleton instances, simply dispose the composition instance:

``` CSharp
internal interface IDependency
{
    bool IsDisposed { get; }
}

internal class Dependency : IDependency, IDisposable
{
    public bool IsDisposed { get; private set; }

    public void Dispose()
    {
        IsDisposed = true;
    }
}

internal interface IService
{
    public IDependency Dependency { get; }
}

internal class Service : IService
{
    public Service(IDependency dependency)
    {
        Dependency = dependency;
    }

    public IDependency Dependency { get; }
}

DI.Setup("Composition")
    .Bind<IDependency>().As(Lifetime.Singleton).To<Dependency>()
    .Bind<IService>().To<Service>()
    .Root<IService>("Root");

IDependency dependency;
using (var composition = new Composition())
{
    var service = composition.Root;
    dependency = service.Dependency;
}

dependency.IsDisposed.ShouldBeTrue();
```

```mermaid
classDiagram
class Composition {
+IService Root
+T ResolveᐸTᐳ()
+T ResolveᐸTᐳ(object? tag)
+object ResolveᐸTᐳ(Type type)
+object ResolveᐸTᐳ(Type type, object? tag)
}
Composition --|> IDisposable
Dependency --|> IDependency : 
class Dependency {
+Dependency()
}
Service --|> IService : 
class Service {
+Service(IDependency dependency)
}
Composition ..> Service : IService Root
Service o-- "Singleton" Dependency : IDependency dependency
```

#### Default lifetime

[![CSharp](https://img.shields.io/badge/C%23-code-blue.svg)](tests\Pure.DI.UsageTests\Lifetimes\DefaultLifetimeScenario.cs)

``` CSharp
internal interface IDependency { }

internal class Dependency : IDependency { }

internal interface IService
{
    public IDependency Dependency1 { get; }

    public IDependency Dependency2 { get; }
}

internal class Service : IService
{
    public Service(IDependency dependency1, IDependency dependency2)
    {
        Dependency1 = dependency1;
        Dependency2 = dependency2;
    }

    public IDependency Dependency1 { get; }

    public IDependency Dependency2 { get; }
}

DI.Setup("Composition")
    .DefaultLifetime(Lifetime.Singleton)
    .Bind<IDependency>().To<Dependency>()
    .Bind<IService>().To<Service>()
    .Root<IService>("Root");

var composition = new Composition();
var service1 = composition.Root;
var service2 = composition.Root;
service1.ShouldBe(service2);
service1.Dependency1.ShouldBe(service1.Dependency2);
```

```mermaid
classDiagram
class Composition {
+IService Root
+T ResolveᐸTᐳ()
+T ResolveᐸTᐳ(object? tag)
+object ResolveᐸTᐳ(Type type)
+object ResolveᐸTᐳ(Type type, object? tag)
}
Dependency --|> IDependency : 
class Dependency {
+Dependency()
}
Service --|> IService : 
class Service {
+Service(IDependency dependency1, IDependency dependency2)
}
Composition ..> Service : IService Root
Service "Singleton" o-- "Singleton" Dependency : IDependency dependency1
Service "Singleton" o-- "Singleton" Dependency : IDependency dependency2
```

#### Func

[![CSharp](https://img.shields.io/badge/C%23-code-blue.svg)](tests\Pure.DI.UsageTests\BaseClassLibrary\FuncScenario.cs)

_Func<T>_ helps when logic needs to inject instances of some type on demand and multiple times.

``` CSharp
internal interface IDependency { }

internal class Dependency : IDependency { }

internal interface IService
{
    ImmutableArray<IDependency> Dependencies { get; }
}

internal class Service : IService
{
    public Service(Func<IDependency> dependencyFactory)
    {
        Dependencies = Enumerable
            .Range(0, 10)
            .Select(_ => dependencyFactory())
            .ToImmutableArray();
    }

    public ImmutableArray<IDependency> Dependencies { get; }
}

DI.Setup("Composition")
    .Bind<IDependency>().To<Dependency>()
    .Bind<IService>().To<Service>()
    .Root<IService>("Root");

var composition = new Composition();
var service = composition.Root;
service.Dependencies.Length.ShouldBe(10);
```

```mermaid
classDiagram
class Composition {
+IService Root
+T ResolveᐸTᐳ()
+T ResolveᐸTᐳ(object? tag)
+object ResolveᐸTᐳ(Type type)
+object ResolveᐸTᐳ(Type type, object? tag)
}
Dependency --|> IDependency : 
class Dependency {
+Dependency()
}
Service --|> IService : 
class Service {
+Service(FuncᐸIDependencyᐳ dependencyFactory)
}
class FuncᐸIDependencyᐳ
Composition ..> Service : IService Root
Service *-- FuncᐸIDependencyᐳ : FuncᐸIDependencyᐳ dependencyFactory
FuncᐸIDependencyᐳ *-- Dependency :   IDependency
```

#### IEnumerable

[![CSharp](https://img.shields.io/badge/C%23-code-blue.svg)](tests\Pure.DI.UsageTests\BaseClassLibrary\EnumerableScenario.cs)

Specifying `IEnumerable<T>` as the injection type allows instances of all bindings that implement type `T` to be injected in a lazy manner - the instances will be provided one by one.

``` CSharp
internal interface IDependency { }

internal class AbcDependency : IDependency { }

internal class XyzDependency : IDependency { }

internal interface IService
{
    ImmutableArray<IDependency> Dependencies { get; }
}

internal class Service : IService
{
    public Service(IEnumerable<IDependency> dependencies)
    {
        Dependencies = dependencies.ToImmutableArray();
    }

    public ImmutableArray<IDependency> Dependencies { get; }
}

DI.Setup("Composition")
    .Bind<IDependency>().To<AbcDependency>()
    .Bind<IDependency>(2).To<XyzDependency>()
    .Bind<IService>().To<Service>()
    .Root<IService>("Root");

var composition = new Composition();
var service = composition.Root;
service.Dependencies.Length.ShouldBe(2);
service.Dependencies[0].ShouldBeOfType<AbcDependency>();
service.Dependencies[1].ShouldBeOfType<XyzDependency>();
```

```mermaid
classDiagram
class Composition {
+IService Root
+T ResolveᐸTᐳ()
+T ResolveᐸTᐳ(object? tag)
+object ResolveᐸTᐳ(Type type)
+object ResolveᐸTᐳ(Type type, object? tag)
}
AbcDependency --|> IDependency : 
class AbcDependency {
+AbcDependency()
}
XyzDependency --|> IDependency : 2 
class XyzDependency {
+XyzDependency()
}
Service --|> IService : 
class Service {
+Service(IEnumerableᐸIDependencyᐳ dependencies)
}
class IEnumerableᐸIDependencyᐳ
Composition ..> Service : IService Root
Service *-- IEnumerableᐸIDependencyᐳ : IEnumerableᐸIDependencyᐳ dependencies
IEnumerableᐸIDependencyᐳ *-- AbcDependency : 
IEnumerableᐸIDependencyᐳ *-- XyzDependency : 2  
```

#### Array

[![CSharp](https://img.shields.io/badge/C%23-code-blue.svg)](tests\Pure.DI.UsageTests\BaseClassLibrary\ArrayScenario.cs)

Specifying `T[]` as the injection type allows instances from all bindings that implement the `T` type to be injected.

``` CSharp
internal interface IDependency { }

internal class AbcDependency : IDependency { }

internal class XyzDependency : IDependency { }

internal interface IService
{
    IDependency[] Dependencies { get; }
}

internal class Service : IService
{
    public Service(IDependency[] dependencies)
    {
        Dependencies = dependencies;
    }

    public IDependency[] Dependencies { get; }
}

DI.Setup("Composition")
    .Bind<IDependency>().To<AbcDependency>()
    .Bind<IDependency>(2).To<XyzDependency>()
    .Bind<IService>().To<Service>()
    .Root<IService>("Root");

var composition = new Composition();
var service = composition.Root;
service.Dependencies.Length.ShouldBe(2);
service.Dependencies[0].ShouldBeOfType<AbcDependency>();
service.Dependencies[1].ShouldBeOfType<XyzDependency>();
```

```mermaid
classDiagram
class Composition {
+IService Root
+T ResolveᐸTᐳ()
+T ResolveᐸTᐳ(object? tag)
+object ResolveᐸTᐳ(Type type)
+object ResolveᐸTᐳ(Type type, object? tag)
}
AbcDependency --|> IDependency : 
class AbcDependency {
+AbcDependency()
}
XyzDependency --|> IDependency : 2 
class XyzDependency {
+XyzDependency()
}
Service --|> IService : 
class Service {
+Service(ArrayᐸIDependencyᐳ dependencies)
}
class ArrayᐸIDependencyᐳ
Composition ..> Service : IService Root
Service *-- ArrayᐸIDependencyᐳ : ArrayᐸIDependencyᐳ dependencies
ArrayᐸIDependencyᐳ *-- AbcDependency : 
ArrayᐸIDependencyᐳ *-- XyzDependency : 2  
```

In addition to arrays, other collection types are also supported, such as:
- System.Memory<T>
- System.ReadOnlyMemory<T>
- System.Span<T>
- System.ReadOnlySpan<T>
- System.Collections.Generic.ICollection<T>
- System.Collections.Generic.IList<T>
- System.Collections.Generic.List<T>
- System.Collections.Generic.IReadOnlyCollection<T>
- System.Collections.Generic.IReadOnlyList<T>
- System.Collections.Generic.ISet<T>
- System.Collections.Generic.HashSet<T>
- System.Collections.Generic.SortedSet<T>
- System.Collections.Generic.Queue<T>
- System.Collections.Generic.Stack<T>
- System.Collections.Immutable.ImmutableArray<T>
- System.Collections.Immutable.IImmutableList<T>
- System.Collections.Immutable.ImmutableList<T>
- System.Collections.Immutable.IImmutableSet<T>
- System.Collections.Immutable.ImmutableHashSet<T>
- System.Collections.Immutable.ImmutableSortedSet<T>
- System.Collections.Immutable.IImmutableQueue<T>
- System.Collections.Immutable.ImmutableQueue<T>
- System.Collections.Immutable.IImmutableStack<T>

#### Lazy

[![CSharp](https://img.shields.io/badge/C%23-code-blue.svg)](tests\Pure.DI.UsageTests\BaseClassLibrary\LazyScenario.cs)

``` CSharp
internal interface IDependency { }

internal class Dependency : IDependency { }

internal interface IService
{
    IDependency Dependency { get; }
}

internal class Service : IService
{
    private readonly Lazy<IDependency> _dependency;

    public Service(Lazy<IDependency> dependency)
    {
        _dependency = dependency;
    }

    public IDependency Dependency => _dependency.Value;
}

DI.Setup("Composition")
    .Bind<IDependency>().To<Dependency>()
    .Bind<IService>().To<Service>()
    .Root<IService>("Root");

var composition = new Composition();
var service = composition.Root;
service.Dependency.ShouldBe(service.Dependency);
```

```mermaid
classDiagram
class Composition {
+IService Root
+T ResolveᐸTᐳ()
+T ResolveᐸTᐳ(object? tag)
+object ResolveᐸTᐳ(Type type)
+object ResolveᐸTᐳ(Type type, object? tag)
}
Dependency --|> IDependency : 
class Dependency {
+Dependency()
}
Service --|> IService : 
class Service {
+Service(LazyᐸIDependencyᐳ dependency)
}
class LazyᐸIDependencyᐳ
class FuncᐸIDependencyᐳ
Composition ..> Service : IService Root
Service *-- LazyᐸIDependencyᐳ : LazyᐸIDependencyᐳ dependency
LazyᐸIDependencyᐳ *-- FuncᐸIDependencyᐳ :   FuncᐸIDependencyᐳ
FuncᐸIDependencyᐳ *-- Dependency :   IDependency
```

#### Span and ReadOnlySpan

[![CSharp](https://img.shields.io/badge/C%23-code-blue.svg)](tests\Pure.DI.UsageTests\BaseClassLibrary\SpanScenario.cs)

Specifying `Span<T>` and `ReadOnlySpan<T>` work the same as with the array `T[]`.

``` CSharp
internal class Dependency
{
}

internal interface IService
{
    int Count { get; }
}

internal class Service : IService
{
    public Service(ReadOnlySpan<Dependency> dependencies)
    {
        Count = dependencies.Length;
    }

    public int Count { get; }
}

DI.Setup("Composition")
    .Bind<Dependency>('a').To<Dependency>()
    .Bind<Dependency>('b').To<Dependency>()
    .Bind<Dependency>('c').To<Dependency>()
    .Bind<IService>().To<Service>()
    .Root<IService>("Root");

var composition = new Composition();
var service = composition.Root;
service.Count.ShouldBe(3);
```

```mermaid
classDiagram
class Composition {
+IService Root
+T ResolveᐸTᐳ()
+T ResolveᐸTᐳ(object? tag)
+object ResolveᐸTᐳ(Type type)
+object ResolveᐸTᐳ(Type type, object? tag)
}
class Dependency {
+Dependency()
}
Service --|> IService : 
class Service {
+Service(ReadOnlySpanᐸDependencyᐳ dependencies)
}
class ReadOnlySpanᐸDependencyᐳ
Composition ..> Service : IService Root
Service *-- ReadOnlySpanᐸDependencyᐳ : ReadOnlySpanᐸDependencyᐳ dependencies
ReadOnlySpanᐸDependencyᐳ *-- Dependency : 'a'  
ReadOnlySpanᐸDependencyᐳ *-- Dependency : 'b'  
ReadOnlySpanᐸDependencyᐳ *-- Dependency : 'c'  
```

This scenario is even more efficient when the `Span[]` or `ReadOnlySpan[]` element has a value type. In this case, there are no heap allocations, and the composition root `IService` looks like this:
```csharp
public IService Root
{
  get
  {
    ReadOnlySpan<Dependency> dependencies = stackalloc Dependency[3] { new Dependency(), new Dependency(), new Dependency() };
    return new Service(dependencies);
  }
}
```

#### Decorator

[![CSharp](https://img.shields.io/badge/C%23-code-blue.svg)](tests\Pure.DI.UsageTests\Interception\DecoratorScenario.cs)

``` CSharp
internal interface IDependency { }

internal class Dependency : IDependency { }

internal interface IService
{
    string GetMessage();
}

internal class Service : IService
{
    public Service(IDependency dependency)
    {
    }

    public string GetMessage() => "Hello World";
}

internal class DecoratorService : IService
{
    private readonly IService _baseService;

    public DecoratorService([Tag("base")] IService baseService) => _baseService = baseService;

    public string GetMessage() => $"{_baseService.GetMessage()} !!!";
}


DI.Setup("Composition")
    .Bind<IDependency>().To<Dependency>()
    .Bind<IService>("base").To<Service>()
    .Bind<IService>().To<DecoratorService>()
    .Root<IService>("Root");

var composition = new Composition();
var service = composition.Root;
service.GetMessage().ShouldBe("Hello World !!!");
```

```mermaid
classDiagram
class Composition {
+IService Root
+T ResolveᐸTᐳ()
+T ResolveᐸTᐳ(object? tag)
+object ResolveᐸTᐳ(Type type)
+object ResolveᐸTᐳ(Type type, object? tag)
}
Dependency --|> IDependency : 
class Dependency {
+Dependency()
}
Service --|> IService : "base" 
class Service {
+Service(IDependency dependency)
}
DecoratorService --|> IService : 
class DecoratorService {
+DecoratorService(IService baseService)
}
Composition ..> DecoratorService : IService Root
Service *-- Dependency : IDependency dependency
DecoratorService *-- Service : "base"  IService baseService
```

#### Interception

[![CSharp](https://img.shields.io/badge/C%23-code-blue.svg)](tests\Pure.DI.UsageTests\Interception\InterceptionScenario.cs)

``` CSharp
public interface IDependency
{
    void DependencyCall();
}

public class Dependency : IDependency
{
    public void DependencyCall()
    {
    }
}

public interface IService
{
    IDependency Dependency { get; }

    void ServiceCall();
}

public class Service : IService
{
    public Service(IDependency dependency)
    {
        Dependency = dependency;
    }

    public IDependency Dependency { get; }

    public void ServiceCall()
    {
    }
}

internal partial class Composition: IInterceptor
{
    private readonly List<string> _log;
    private static readonly ProxyGenerator ProxyGenerator = new();

    public Composition(List<string> log)
        : this()
    {
        _log = log;
    }

    private partial T OnDependencyInjection<T>(in T value, object? tag, object? lifetime) =>
        typeof(T).IsValueType
            ? value
            : (T)ProxyGenerator.CreateInterfaceProxyWithTargetInterface(typeof(T), value, this);

    public void Intercept(IInvocation invocation)
    {
        _log.Add(invocation.Method.Name);
        invocation.Proceed();
    }
}

// OnDependencyInjection = On
DI.Setup("Composition")
    .Bind<IDependency>().To<Dependency>()
    .Bind<IService>().Tags().To<Service>()
    .Root<IService>("Root");

var log = new List<string>();
var composition = new Composition(log);
var service = composition.Root;
service.ServiceCall();
service.Dependency.DependencyCall();
log.ShouldBe(ImmutableArray.Create("ServiceCall", "get_Dependency", "DependencyCall"));
```

```mermaid
classDiagram
class Composition {
+IService Root
+T ResolveᐸTᐳ()
+T ResolveᐸTᐳ(object? tag)
+object ResolveᐸTᐳ(Type type)
+object ResolveᐸTᐳ(Type type, object? tag)
}
Dependency --|> IDependency : 
class Dependency {
+Dependency()
}
Service --|> IService : 
class Service {
+Service(IDependency dependency)
}
Composition ..> Service : IService Root
Service *-- Dependency : IDependency dependency
```

#### Advanced interception

[![CSharp](https://img.shields.io/badge/C%23-code-blue.svg)](tests\Pure.DI.UsageTests\Interception\AdvancedInterceptionScenario.cs)

``` CSharp
public interface IDependency
{
    void DependencyCall();
}

public class Dependency : IDependency
{
    public void DependencyCall()
    {
    }
}

public interface IService
{
    IDependency Dependency { get; }

    void ServiceCall();
}

public class Service : IService
{
    public Service(IDependency dependency)
    {
        Dependency = dependency;
    }

    public IDependency Dependency { get; }

    public void ServiceCall()
    {
    }
}

internal partial class Composition: IInterceptor
{
    private readonly List<string> _log;
    private static readonly IProxyBuilder ProxyBuilder = new DefaultProxyBuilder();
    private readonly IInterceptor[] _interceptors;

    public Composition(List<string> log)
        : this()
    {
        _log = log;
        _interceptors = new IInterceptor[]{ this };
    }

    private partial T OnDependencyInjection<T>(in T value, object? tag, object? lifetime) =>
        typeof(T).IsValueType
            ? value :
            ProxyFactory<T>.GetFactory(ProxyBuilder)(value, _interceptors);

    public void Intercept(IInvocation invocation)
    {
        _log.Add(invocation.Method.Name);
        invocation.Proceed();
    }

    private static class ProxyFactory<T>
    {
        private static Func<T, IInterceptor[], T>? _factory;

        public static Func<T, IInterceptor[], T> GetFactory(IProxyBuilder proxyBuilder) =>
            _factory ?? CreateFactory(proxyBuilder);

        private static Func<T, IInterceptor[], T> CreateFactory(IProxyBuilder proxyBuilder)
        {
            // Compiles a delegate to create a proxy for the performance boost
            var proxyType = proxyBuilder.CreateInterfaceProxyTypeWithTargetInterface(typeof(T), Type.EmptyTypes, ProxyGenerationOptions.Default);
            var ctor = proxyType.GetConstructors().Single(i => i.GetParameters().Length == 2);
            var instance = Expression.Parameter(typeof(T));
            var interceptors = Expression.Parameter(typeof(IInterceptor[]));
            var newProxyExpression = Expression.New(ctor, interceptors, instance);
            return _factory = Expression.Lambda<Func<T, IInterceptor[], T>>(newProxyExpression, instance, interceptors).Compile();
        }
    }
}

// OnDependencyInjection = On
DI.Setup("Composition")
    .Bind<IDependency>().To<Dependency>()
    .Bind<IService>().Tags().To<Service>()
    .Root<IService>("Root");

var log = new List<string>();
var composition = new Composition(log);
var service = composition.Root;
service.ServiceCall();
service.Dependency.DependencyCall();
log.ShouldBe(ImmutableArray.Create("ServiceCall", "get_Dependency", "DependencyCall"));
```

```mermaid
classDiagram
class Composition {
+IService Root
+T ResolveᐸTᐳ()
+T ResolveᐸTᐳ(object? tag)
+object ResolveᐸTᐳ(Type type)
+object ResolveᐸTᐳ(Type type, object? tag)
}
Dependency --|> IDependency : 
class Dependency {
+Dependency()
}
Service --|> IService : 
class Service {
+Service(IDependency dependency)
}
Composition ..> Service : IService Root
Service *-- Dependency : IDependency dependency
```
