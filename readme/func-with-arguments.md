#### Func with arguments

[![CSharp](https://img.shields.io/badge/C%23-code-blue.svg)](../tests/Pure.DI.UsageTests/BaseClassLibrary/FuncWithArgumentsScenario.cs)

At any time a BCL type binding can be added manually:

```c#
internal interface IClock
{
    DateTimeOffset Now { get; }
}

internal class Clock : IClock
{
    public DateTimeOffset Now => DateTimeOffset.Now;
}

internal interface IDependency
{
    int Id { get; }
}

internal class Dependency : IDependency
{
    public Dependency(IClock clock)
    {
    }

    public int Id { get; set; }
}

internal interface IService
{
    ImmutableArray<IDependency> Dependencies { get; }
}

internal class Service : IService
{
    public Service(Func<int, IDependency> dependencyFactory)
    {
        Dependencies = Enumerable
            .Range(0, 10)
            .Select((_, index) => dependencyFactory(index))
            .ToImmutableArray();
    }

    public ImmutableArray<IDependency> Dependencies { get; }
}

DI.Setup("Composition")
    .Bind<IClock>().As(Lifetime.Singleton).To<Clock>()
    .Bind<Func<int, IDependency>>().To(ctx => new Func<int, IDependency>(id =>
    {
        ctx.Inject<Dependency>(out var dependency);
        dependency.Id = id;
        return dependency;
    }))
    .Bind<IService>().To<Service>().Root<IService>("Root");

var composition = new Composition();
var service = composition.Root;
service.Dependencies.Length.ShouldBe(10);
service.Dependencies[3].Id.ShouldBe(3);
```

<details open>
<summary>Class Diagram</summary>

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
    +Dependency(IClock clock)
  }
  Service --|> IService : 
  class Service {
    +Service(FuncᐸInt32ˏIDependencyᐳ dependencyFactory)
  }
  Clock --|> IClock : 
  class Clock {
    +Clock()
  }
  class FuncᐸInt32ˏIDependencyᐳ
  class IService {
    <<abstract>>
  }
  class IClock {
    <<abstract>>
  }
  Dependency o--  "Singleton" Clock : IClock clock
  Service *--  FuncᐸInt32ˏIDependencyᐳ : FuncᐸInt32ˏIDependencyᐳ dependencyFactory
  Composition ..> Service : IService Root
  FuncᐸInt32ˏIDependencyᐳ *--  Dependency : Dependency
```

</details>

<details>
<summary>Generated Code</summary>

```c#
partial class Composition
{
  private readonly System.IDisposable[] _disposables12CAAA;
  private Pure.DI.UsageTests.BCL.FuncWithArgumentsScenario.Clock _f22Singleton12CAAA;
  
  public Composition()
  {
    _disposables12CAAA = new System.IDisposable[0];
  }
  
  internal Composition(Composition parent)
  {
    _disposables12CAAA = new System.IDisposable[0];
    lock (parent._disposables12CAAA)
    {
      _f22Singleton12CAAA = parent._f22Singleton12CAAA;
    }
  }
  
  #region Composition Roots
  public Pure.DI.UsageTests.BCL.FuncWithArgumentsScenario.IService Root
  {
    [global::System.Runtime.CompilerServices.MethodImpl((global::System.Runtime.CompilerServices.MethodImplOptions)0x300)]
    get
    {
      System.Func<int, Pure.DI.UsageTests.BCL.FuncWithArgumentsScenario.IDependency> v25Local12CAAA;
      v25Local12CAAA = new Func<int, IDependency>(id =>
      {
          if (global::System.Object.ReferenceEquals(_f22Singleton12CAAA, null))
          {
              lock (_disposables12CAAA)
              {
                  if (global::System.Object.ReferenceEquals(_f22Singleton12CAAA, null))
                  {
                      _f22Singleton12CAAA = new Pure.DI.UsageTests.BCL.FuncWithArgumentsScenario.Clock();
                  }
              }
          }
          Pure.DI.UsageTests.BCL.FuncWithArgumentsScenario.Dependency v28Local12CAAA = new Pure.DI.UsageTests.BCL.FuncWithArgumentsScenario.Dependency(_f22Singleton12CAAA);
          Pure.DI.UsageTests.BCL.FuncWithArgumentsScenario.Dependency dependency = v28Local12CAAA;
          dependency.Id = id;
          return dependency;
      });
      Pure.DI.UsageTests.BCL.FuncWithArgumentsScenario.Service v24Local12CAAA = new Pure.DI.UsageTests.BCL.FuncWithArgumentsScenario.Service(v25Local12CAAA);
      return v24Local12CAAA;
    }
  }
  #endregion
  
  #region API
  #if NETSTANDARD2_0_OR_GREATER || NETCOREAPP || NET40_OR_GREATER
  [global::System.Diagnostics.Contracts.Pure]
  #endif
  [global::System.Runtime.CompilerServices.MethodImpl((global::System.Runtime.CompilerServices.MethodImplOptions)0x300)]
  public T Resolve<T>()
  {
    return Resolver12CAAA<T>.Value.Resolve(this);
  }
  
  #if NETSTANDARD2_0_OR_GREATER || NETCOREAPP || NET40_OR_GREATER
  [global::System.Diagnostics.Contracts.Pure]
  #endif
  [global::System.Runtime.CompilerServices.MethodImpl((global::System.Runtime.CompilerServices.MethodImplOptions)0x300)]
  public T Resolve<T>(object? tag)
  {
    return Resolver12CAAA<T>.Value.ResolveByTag(this, tag);
  }
  
  #if NETSTANDARD2_0_OR_GREATER || NETCOREAPP || NET40_OR_GREATER
  [global::System.Diagnostics.Contracts.Pure]
  #endif
  [global::System.Runtime.CompilerServices.MethodImpl((global::System.Runtime.CompilerServices.MethodImplOptions)0x300)]
  public object Resolve(global::System.Type type)
  {
    int index = (int)(_bucketSize12CAAA * ((uint)global::System.Runtime.CompilerServices.RuntimeHelpers.GetHashCode(type) % 1));
    ref var pair = ref _buckets12CAAA[index];
    if (ReferenceEquals(pair.Key, type))
    {
      return pair.Value.Resolve(this);
    }
    
    int maxIndex = index + _bucketSize12CAAA;
    for (int i = index + 1; i < maxIndex; i++)
    {
      pair = ref _buckets12CAAA[i];
      if (ReferenceEquals(pair.Key, type))
      {
        return pair.Value.Resolve(this);
      }
    }
    
    throw new global::System.InvalidOperationException($"Cannot resolve composition root of type {type}.");
  }
  
  #if NETSTANDARD2_0_OR_GREATER || NETCOREAPP || NET40_OR_GREATER
  [global::System.Diagnostics.Contracts.Pure]
  #endif
  [global::System.Runtime.CompilerServices.MethodImpl((global::System.Runtime.CompilerServices.MethodImplOptions)0x300)]
  public object Resolve(global::System.Type type, object? tag)
  {
    int index = (int)(_bucketSize12CAAA * ((uint)global::System.Runtime.CompilerServices.RuntimeHelpers.GetHashCode(type) % 1));
    ref var pair = ref _buckets12CAAA[index];
    if (ReferenceEquals(pair.Key, type))
    {
      return pair.Value.ResolveByTag(this, tag);
    }
    
    int maxIndex = index + _bucketSize12CAAA;
    for (int i = index + 1; i < maxIndex; i++)
    {
      pair = ref _buckets12CAAA[i];
      if (ReferenceEquals(pair.Key, type))
      {
        return pair.Value.ResolveByTag(this, tag);
      }
    }
    
    throw new global::System.InvalidOperationException($"Cannot resolve composition root of type {type}.");
  }
  
  #endregion
  
  public override string ToString()
  {
    return
      "classDiagram\n" +
        "  class Composition {\n" +
          "    +IService Root\n" +
          "    +T ResolveᐸTᐳ()\n" +
          "    +T ResolveᐸTᐳ(object? tag)\n" +
          "    +object ResolveᐸTᐳ(Type type)\n" +
          "    +object ResolveᐸTᐳ(Type type, object? tag)\n" +
        "  }\n" +
        "  class Dependency {\n" +
          "    +Dependency(IClock clock)\n" +
        "  }\n" +
        "  Service --|> IService : \n" +
        "  class Service {\n" +
          "    +Service(FuncᐸInt32ˏIDependencyᐳ dependencyFactory)\n" +
        "  }\n" +
        "  Clock --|> IClock : \n" +
        "  class Clock {\n" +
          "    +Clock()\n" +
        "  }\n" +
        "  class FuncᐸInt32ˏIDependencyᐳ\n" +
        "  class IService {\n" +
          "    <<abstract>>\n" +
        "  }\n" +
        "  class IClock {\n" +
          "    <<abstract>>\n" +
        "  }\n" +
        "  Dependency o--  \"Singleton\" Clock : IClock clock\n" +
        "  Service *--  FuncᐸInt32ˏIDependencyᐳ : FuncᐸInt32ˏIDependencyᐳ dependencyFactory\n" +
        "  Composition ..> Service : IService Root\n" +
        "  FuncᐸInt32ˏIDependencyᐳ *--  Dependency : Dependency";
  }
  
  private readonly static int _bucketSize12CAAA;
  private readonly static global::Pure.DI.Pair<global::System.Type, global::Pure.DI.IResolver<Composition, object>>[] _buckets12CAAA;
  
  static Composition()
  {
    Resolver12CAAA0 valResolver12CAAA0 = new Resolver12CAAA0();
    Resolver12CAAA<Pure.DI.UsageTests.BCL.FuncWithArgumentsScenario.IService>.Value = valResolver12CAAA0;
    _buckets12CAAA = global::Pure.DI.Buckets<global::System.Type, global::Pure.DI.IResolver<Composition, object>>.Create(
      1,
      out _bucketSize12CAAA,
      new global::Pure.DI.Pair<global::System.Type, global::Pure.DI.IResolver<Composition, object>>[1]
      {
         new global::Pure.DI.Pair<global::System.Type, global::Pure.DI.IResolver<Composition, object>>(typeof(Pure.DI.UsageTests.BCL.FuncWithArgumentsScenario.IService), valResolver12CAAA0)
      });
  }
  
  #region Resolvers
  private class Resolver12CAAA<T>
  {
    public static global::Pure.DI.IResolver<Composition, T> Value;
  }
  
  private sealed class Resolver12CAAA0: global::Pure.DI.IResolver<Composition, Pure.DI.UsageTests.BCL.FuncWithArgumentsScenario.IService>
  {
    [global::System.Runtime.CompilerServices.MethodImpl((global::System.Runtime.CompilerServices.MethodImplOptions)0x300)]
    public Pure.DI.UsageTests.BCL.FuncWithArgumentsScenario.IService Resolve(Composition composition)
    {
      return composition.Root;
    }
    
    [global::System.Runtime.CompilerServices.MethodImpl((global::System.Runtime.CompilerServices.MethodImplOptions)0x300)]
    public Pure.DI.UsageTests.BCL.FuncWithArgumentsScenario.IService ResolveByTag(Composition composition, object tag)
    {
      if (Equals(tag, null)) return composition.Root;
      throw new global::System.InvalidOperationException($"Cannot resolve composition root \"{tag}\" of type Pure.DI.UsageTests.BCL.FuncWithArgumentsScenario.IService.");
    }
  }
  #endregion
}
```

</details>
