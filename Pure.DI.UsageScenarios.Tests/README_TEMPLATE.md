﻿
## Usage Scenarios

- Basics
  - [Autowiring](#autowiring-)
  - [Bindings](#bindings-)
  - [Constants](#constants-)
  - [Generics](#generics-)
  - [Tags](#tags-)
  - [Aspect-oriented DI](#aspect-oriented-di-)
  - [Several contracts](#several-contracts-)
  - [Autowiring with initialization](#autowiring-with-initialization-)
  - [Dependency tag](#dependency-tag-)
  - [Generic autowiring](#generic-autowiring-)
  - [Injection of default parameters](#injection-of-default-parameters-)
- Lifetimes
  - [Singleton lifetime](#singleton-lifetime-)
  - [Custom lifetime: thread Singleton](#custom-lifetime:-thread-singleton-)
- BCL types
  - [Arrays](#arrays-)
  - [Collections](#collections-)
  - [Enumerables](#enumerables-)
  - [Funcs](#funcs-)
  - [Lazy](#lazy-)
  - [Sets](#sets-)
  - [ThreadLocal](#threadlocal-)
  - [Tuples](#tuples-)
- Advanced
  - [Constructor choice](#constructor-choice-)

### Autowiring [![CSharp](https://img.shields.io/badge/C%23-code-blue.svg)](https://raw.githubusercontent.com/DevTeam/IoCContainer/master/IoC.Tests/UsageScenarios/Autowiring.cs)

Autowring is the most natural way to use containers. In the first step, we should create a container. At the second step, we bind interfaces to their implementations. After that, the container is ready to resolve dependencies.

``` CSharp
// Create the container and configure it, using full autowiring
DI.Setup()
    .Bind<IDependency>().To<Dependency>()
    .Bind<IService>().To<Service>();

// Resolve an instance of interface `IService`
var instance = AutowiringDI.Resolve<IService>();
```



### Bindings [![CSharp](https://img.shields.io/badge/C%23-code-blue.svg)](https://raw.githubusercontent.com/DevTeam/IoCContainer/master/IoC.Tests/UsageScenarios/Bindings.cs)

It is possible to bind any number of types.

``` CSharp
DI.Setup()
    .Bind<IDependency>().To<Dependency>()
    // Bind using few types
    .Bind<IService>().Bind<IAnotherService>().Tag("abc").To<Service>();

// Resolve instances using different types
var instance1 = BindingsDI.Resolve<IService>("abc");
var instance2 = BindingsDI.Resolve<IAnotherService>("abc");
```



### Constants [![CSharp](https://img.shields.io/badge/C%23-code-blue.svg)](https://raw.githubusercontent.com/DevTeam/IoCContainer/master/IoC.Tests/UsageScenarios/Constants.cs)

It's obvious here.

``` CSharp
DI.Setup()
    .Bind<int>().To(_ => 10);

// Resolve an integer
var val = ConstantsDI.Resolve<int>();
// Check the value
val.ShouldBe(10);
```



### Generics [![CSharp](https://img.shields.io/badge/C%23-code-blue.svg)](https://raw.githubusercontent.com/DevTeam/IoCContainer/master/IoC.Tests/UsageScenarios/Generics.cs)

Autowring of generic types via binding of open generic types or generic type markers are working the same way.

``` CSharp
DI.Setup()
    .Bind<IDependency>().To<Dependency>()
    // Bind open generic interface to open generic implementation
    .Bind<IService<TT>>().To<Service<TT>>()
    .Bind<CompositionRoot<IService<int>>>().To<CompositionRoot<IService<int>>>();

// Resolve a generic instance
var instance = GenericsDI.Resolve<CompositionRoot<IService<int>>>().Root;
```



### Tags [![CSharp](https://img.shields.io/badge/C%23-code-blue.svg)](https://raw.githubusercontent.com/DevTeam/IoCContainer/master/IoC.Tests/UsageScenarios/Tags.cs)

Tags are useful while binding to several implementations of the same abstract types.

``` CSharp
DI.Setup()
    .Bind<IDependency>().To<Dependency>()
    // Bind using several tags
    .Bind<IService>().Tag(10).Tag("abc").To<Service>()
    .Bind<IService>().To<Service>();

// Resolve instances using tags
var instance1 = TagsDI.Resolve<IService>("abc");
var instance2 = TagsDI.Resolve<IService>(10);

// Resolve the instance using the empty tag
var instance3 = TagsDI.Resolve<IService>();
```



### Aspect-oriented DI [![CSharp](https://img.shields.io/badge/C%23-code-blue.svg)](https://raw.githubusercontent.com/DevTeam/IoCContainer/master/IoC.Tests/UsageScenarios/AspectOriented.cs)

This framework has no special predefined attributes to support aspect-oriented auto wiring because a non-infrastructure code should not have references to this framework. But this code may contain these attributes by itself. And it is quite easy to use these attributes for aspect-oriented auto wiring, see the sample below.

``` CSharp
public void Run()
{
    DI.Setup()
        // Define attributes for aspect-oriented autowiring
        .TypeAttribute<TypeAttribute>()
        .OrderAttribute<OrderAttribute>()
        .TagAttribute<TagAttribute>()
        // Configure the container to use DI aspects
        .Bind<IConsole>().Tag("MyConsole").To(_ => AspectOriented.Console.Object)
        .Bind<string>().Tag("Prefix").To(_ => "info")
        .Bind<ILogger>().To<Logger>();

    // Create a logger
    var logger = AspectOrientedDI.Resolve<ILogger>();

    // Log the message
    logger.Log("Hello");

    // Check the output has the appropriate format
    Console.Verify(i => i.WriteLine(It.IsRegex(".+ - info: Hello")));
}

// Represents the dependency aspect attribute to specify a type for injection.
[AttributeUsage(AttributeTargets.Parameter | AttributeTargets.Method)]
public class TypeAttribute : Attribute
{
    // A type, which will be used during an injection
    public readonly Type Type;

    public TypeAttribute(Type type) => Type = type;
}

// Represents the dependency aspect attribute to specify a tag for injection.
[AttributeUsage(AttributeTargets.Parameter | AttributeTargets.Method | AttributeTargets.Property)]
public class TagAttribute : Attribute
{
    // A tag, which will be used during an injection
    public readonly object Tag;

    public TagAttribute(object tag) => Tag = tag;
}

// Represents the dependency aspect attribute to specify an order for injection.
[AttributeUsage(AttributeTargets.Method | AttributeTargets.Property)]
public class OrderAttribute : Attribute
{
    // An order to be used to invoke a method
    public readonly int Order;

    public OrderAttribute(int order) => Order = order;
}

public interface IConsole { void WriteLine(string text); }

public interface IClock { DateTimeOffset Now { get; } }

public interface ILogger { void Log(string message); }

public class Logger : ILogger
{
    private readonly IConsole _console;
    private IClock? _clock;

    // Constructor injection using the tag "MyConsole"
    public Logger([Tag("MyConsole")] IConsole console) => _console = console;

    // Method injection after constructor using specified type _Clock_
    [Order(1)] public void Initialize([Type(typeof(Clock))] IClock clock) => _clock = clock;

    // Setter injection after the method injection above using the tag "Prefix"
    [Tag("Prefix"), Order(2)]
    public string Prefix { get; set; } = string.Empty;

    // Adds current time and prefix before a message and writes it to console
    public void Log(string message) => _console?.WriteLine($"{_clock?.Now} - {Prefix}: {message}");
}

public class Clock : IClock
{
    // "clockName" dependency is not resolved here but has default value
    public Clock([Type(typeof(string)), Tag("ClockName")] string clockName = "SPb") { }

    public DateTimeOffset Now => DateTimeOffset.Now;
}
```

You can also specify your own aspect-oriented autowiring by implementing the interface [_IAutowiringStrategy_](IoCContainer/blob/master/IoC/IAutowiringStrategy.cs).

### Several contracts [![CSharp](https://img.shields.io/badge/C%23-code-blue.svg)](https://raw.githubusercontent.com/DevTeam/IoCContainer/master/IoC.Tests/UsageScenarios/SeveralContracts.cs)

It is possible to bind several types to a single implementation.

``` CSharp
DI.Setup()
    .Bind<IDependency>().To<Dependency>()
    .Bind<Service>().Bind<IService>().Bind<IAnotherService>().To<Service>();

// Resolve instances
var instance1 = SeveralContractsDI.Resolve<IService>();
var instance2 = SeveralContractsDI.Resolve<IAnotherService>();
```



### Autowiring with initialization [![CSharp](https://img.shields.io/badge/C%23-code-blue.svg)](https://raw.githubusercontent.com/DevTeam/IoCContainer/master/IoC.Tests/UsageScenarios/AutowiringWithInitialization.cs)

Sometimes instances required some actions before you give them to use - some methods of initialization or fields which should be defined. You can solve these things easily.

``` CSharp
// Create a container and configure it using full autowiring
DI.Setup()
    .Bind<IDependency>().To<Dependency>()
    .Bind<INamedService>().To<InitializingNamedService>(
        ctx =>
        {
            var service = new InitializingNamedService(ctx.Resolve<IDependency>());
            // Configure the container to invoke method "Initialize" for every created instance of this type
            service.Initialize("Initialized!", ctx.Resolve<IDependency>());
            return service;
        });

// Resolve an instance of interface `IService`
var instance = AutowiringWithInitializationDI.Resolve<INamedService>();

// Check the instance
instance.ShouldBeOfType<InitializingNamedService>();

// Check that the initialization has took place
instance.Name.ShouldBe("Initialized!");
```

:warning: It is not recommended because it is a cause of hidden dependencies.

### Dependency tag [![CSharp](https://img.shields.io/badge/C%23-code-blue.svg)](https://raw.githubusercontent.com/DevTeam/IoCContainer/master/IoC.Tests/UsageScenarios/DependencyTag.cs)

Use a _tag_ to bind few dependencies for the same types.

``` CSharp
DI.Setup()
    .Bind<IDependency>().Tag("MyDep").To<Dependency>()
    // Configure autowiring and inject dependency tagged by "MyDep"
    .Bind<IService>().To(ctx => new Service(ctx.Resolve<IDependency>("MyDep")));

// Resolve an instance
var instance = DependencyTagDI.Resolve<IService>();
```



### Injection of default parameters [![CSharp](https://img.shields.io/badge/C%23-code-blue.svg)](https://raw.githubusercontent.com/DevTeam/IoCContainer/master/IoC.Tests/UsageScenarios/DefaultParamsInjection.cs)



``` CSharp
public void Run()
{
    DI.Setup()
        .Bind<IDependency>().To<Dependency>()
        .Bind<IService>().To<SomeService>();

    // Resolve an instance
    var instance = DefaultParamsInjectionDI.Resolve<IService>();

    // Check the optional dependency
    instance.State.ShouldBe("empty");
}

public class SomeService: IService
{
    // "state" dependency is not resolved here but it has the default value "empty"
    public SomeService(IDependency dependency, string state = "empty")
    {
        Dependency = dependency;
        State = state;
    }

    public IDependency Dependency { get; }

    public string State { get; }
}
```



### Generic autowiring [![CSharp](https://img.shields.io/badge/C%23-code-blue.svg)](https://raw.githubusercontent.com/DevTeam/IoCContainer/master/IoC.Tests/UsageScenarios/GenericAutowiring.cs)

Autowiring of generic types as simple as autowiring of other simple types. Just use a generic parameters markers like _TT_, _TT1_, _TT2_ and etc. or TTI, TTI1, TTI2 ... for interfaces or TTS, TTS1, TTS2 ... for value types or other special markers like TTDisposable, TTDisposable1 and etc. TTList<>, TTDictionary<> ... or create your own generic parameters markers or bind open generic types.

``` CSharp
public void Run()
{
    // Create and configure the container using autowiring
    DI.Setup()
        .Bind<IDependency>().To<Dependency>()
        // Bind using the predefined generic parameters marker TT (or TT1, TT2, TT3 ...)
        .Bind<IService<TT>>().To<Service<TT>>()
        // Bind using the predefined generic parameters marker TTList (or TTList1, TTList2 ...)
        // For other cases there are TTComparable, TTComparable<in T>, TTEquatable<T>, TTEnumerable<out T>, TTDictionary<TKey, TValue> and etc.
        .Bind<IListService<TTList<int>>>().To<ListService<TTList<int>>>()
        // Bind using the custom generic parameters marker TCustom
        .Bind<IService<TTMy>>().Tag("custom marker").To<Service<TTMy>>()
        .Bind<CompositionRoot<IListService<IList<int>>>>().To<CompositionRoot<IListService<IList<int>>>>()
        .Bind<CompositionRoot<ICollection<IService<int>>>>().To<CompositionRoot<ICollection<IService<int>>>>();

    // Resolve a generic instance
    var listService = GenericAutowiringDI.Resolve<CompositionRoot<IListService<IList<int>>>>().Root;
    var instances = GenericAutowiringDI.Resolve<CompositionRoot<ICollection<IService<int>>>>().Root;

    instances.Count.ShouldBe(2);
    // Check the instance's type
    foreach (var instance in instances)
    {
        instance.ShouldBeOfType<Service<int>>();
    }

    listService.ShouldBeOfType<ListService<IList<int>>>();
}

// Custom generic type marker using predefined attribute `GenericTypeArgument`
[GenericTypeArgument]
class TTMy { }
```



### Singleton lifetime [![CSharp](https://img.shields.io/badge/C%23-code-blue.svg)](https://raw.githubusercontent.com/DevTeam/IoCContainer/master/IoC.Tests/UsageScenarios/SingletonLifetime.cs)

[Singleton](https://en.wikipedia.org/wiki/Singleton_pattern) is a design pattern that supposes for having only one instance of some class during the whole application lifetime. The main complaint about Singleton is that it contradicts the Dependency Injection principle and thus hinders testability. It essentially acts as a global constant, and it is hard to substitute it with a test when needed. The _Singleton lifetime_ is indispensable in this case.

``` CSharp
DI.Setup()
    .Bind<IDependency>().To<Dependency>()
    // Use the Singleton lifetime
    .Bind<IService>().As(Singleton).To<Service>();

// Resolve the singleton twice
var instance1 = SingletonLifetimeDI.Resolve<IService>();
var instance2 = SingletonLifetimeDI.Resolve<IService>();

// Check that instances are equal
instance1.ShouldBe(instance2);
```



### Custom lifetime: thread Singleton [![CSharp](https://img.shields.io/badge/C%23-code-blue.svg)](https://raw.githubusercontent.com/DevTeam/IoCContainer/master/IoC.Tests/UsageScenarios/ThreadSingletonLifetime.cs)

Sometimes it is useful to have a [singleton](https://en.wikipedia.org/wiki/Singleton_pattern) instance per a thread (or more generally a singleton per something else). There is no special "lifetime" type in this framework to achieve this requirement. Still, it is quite easy to create your own "lifetime" type for that using base type [_KeyBasedLifetime<>_](IoC/Lifetimes/KeyBasedLifetime.cs).

``` CSharp
public void Run()
{
    DI.Setup()
        .Bind<IDependency>().To<Dependency>()
        // Bind an interface to an implementation using the singleton per a thread lifetime
        .Bind<IService>().As(Lifetime.PerThread).To<Service>();

    // Resolve the singleton twice
    var instance1 = ThreadSingletonLifetimeDI.Resolve<IService>();
    var instance2 = ThreadSingletonLifetimeDI.Resolve<IService>();
    IService? instance3 = null;
    IService? instance4 = null;

    var finish = new ManualResetEvent(false);
    var newThread = new Thread(() =>
    {
        instance3 = ThreadSingletonLifetimeDI.Resolve<IService>();
        instance4 = ThreadSingletonLifetimeDI.Resolve<IService>();
        finish.Set();
    });

    newThread.Start();
    finish.WaitOne();

    // Check that instances resolved in a main thread are equal
    instance1.ShouldBe(instance2);

    // Check that instance resolved in a new thread is not null
    instance3!.ShouldNotBeNull();

    // Check that instances resolved in different threads are not equal
    instance1.ShouldNotBe(instance3!);

    // Check that instances resolved in a new thread are equal
    instance4!.ShouldBe(instance3!);
}
```



### Arrays [![CSharp](https://img.shields.io/badge/C%23-code-blue.svg)](https://raw.githubusercontent.com/DevTeam/IoCContainer/master/IoC.Tests/UsageScenarios/Arrays.cs)

To resolve all possible instances of any tags of the specific type as an _array_ just use the injection _T[]_

``` CSharp
DI.Setup()
    .Bind<IDependency>().To<Dependency>()
    // Bind to the implementation #1
    .Bind<IService>().Tag(1).To<Service>()
    // Bind to the implementation #2
    .Bind<IService>().Tag(2).Tag("abc").To<Service>()
    // Bind to the implementation #3
    .Bind<IService>().Tag(3).To<Service>()
    .Bind<CompositionRoot<CompositionRoot<IService[]>>>().To<CompositionRoot<CompositionRoot<IService[]>>>();

// Resolve all appropriate instances
var composition = ArraysDI.Resolve<CompositionRoot<IService[]>>();
```



### Collections [![CSharp](https://img.shields.io/badge/C%23-code-blue.svg)](https://raw.githubusercontent.com/DevTeam/IoCContainer/master/IoC.Tests/UsageScenarios/Collections.cs)

To resolve all possible instances of any tags of the specific type as a _collection_ just use the injection _ICollection<T>_

``` CSharp
DI.Setup()
    .Bind<IDependency>().To<Dependency>()
    // Bind to the implementation #1
    .Bind<IService>().Tag(1).To<Service>()
    // Bind to the implementation #2
    .Bind<IService>().Tag(2).Tag("abc").To<Service>()
    // Bind to the implementation #3
    .Bind<IService>().Tag(3).To<Service>()
    .Bind<CompositionRoot<ICollection<IService>>>().To<CompositionRoot<ICollection<IService>>>();

// Resolve all appropriate instances
var composition = CollectionsDI.Resolve<CompositionRoot<ICollection<IService>>>();

// Check the number of resolved instances
composition.Root.Count.ShouldBe(3);
```



### Enumerables [![CSharp](https://img.shields.io/badge/C%23-code-blue.svg)](https://raw.githubusercontent.com/DevTeam/IoCContainer/master/IoC.Tests/UsageScenarios/Enumerables.cs)

To resolve all possible instances of any tags of the specific type as an _enumerable_ just use the injection _IEnumerable<T>_.

``` CSharp
DI.Setup()
    .Bind<IDependency>().To<Dependency>()
    // Bind to the implementation #1
    .Bind<IService>().Tag(1).To<Service>()
    // Bind to the implementation #2
    .Bind<IService>().Tag(2).Tag("abc").To<Service>()
    // Bind to the implementation #3
    .Bind<IService>().Tag(3).To<Service>()
    .Bind<CompositionRoot<IEnumerable<IService>>>().To<CompositionRoot<IEnumerable<IService>>>(); ;

// Resolve all appropriate instances
var instances = EnumerablesDI.Resolve<CompositionRoot<IEnumerable<IService>>>().Root.ToList();

// Check the number of resolved instances
instances.Count.ShouldBe(3);
```



### Funcs [![CSharp](https://img.shields.io/badge/C%23-code-blue.svg)](https://raw.githubusercontent.com/DevTeam/IoCContainer/master/IoC.Tests/UsageScenarios/Funcs.cs)

_Func<>_ helps when a logic needs to inject some type of instances on-demand or solve circular dependency issues.

``` CSharp
DI.Setup()
    .Bind<IDependency>().To<Dependency>()
    .Bind<IService>().To<Service>()
    .Bind<CompositionRoot<Func<IService>>>().To<CompositionRoot<Func<IService>>>();

// Resolve function to create instances
var factory = FuncsDI.Resolve<CompositionRoot<Func<IService>>>().Root;

// Resolve few instances
var instance1 = factory();
var instance2 = factory();
```



### Lazy [![CSharp](https://img.shields.io/badge/C%23-code-blue.svg)](https://raw.githubusercontent.com/DevTeam/IoCContainer/master/IoC.Tests/UsageScenarios/Lazy.cs)

_Lazy_ dependency helps when a logic needs to inject _Lazy<T>_ to get instance once on demand.

``` CSharp
DI.Setup()
    .Bind<IDependency>().To<Dependency>()
    .Bind<IService>().To<Service>()
    .Bind<CompositionRoot<Lazy<IService>>>().To<CompositionRoot<Lazy<IService>>>();

// Resolve the instance of Lazy<IService>
var lazy = LazyDI.Resolve<CompositionRoot<Lazy<IService>>>().Root;

// Get the instance via Lazy
var instance = lazy.Value;
```



### Sets [![CSharp](https://img.shields.io/badge/C%23-code-blue.svg)](https://raw.githubusercontent.com/DevTeam/IoCContainer/master/IoC.Tests/UsageScenarios/Sets.cs)

To resolve all possible instances of any tags of the specific type as a _ISet<>_ just use the injection _ISet<T>_.

``` CSharp
DI.Setup()
    .Bind<IDependency>().To<Dependency>()
    // Bind to the implementation #1
    .Bind<IService>().Tag(1).To<Service>()
    // Bind to the implementation #2
    .Bind<IService>().Tag(2).Tag("abc").To<Service>()
    // Bind to the implementation #3
    .Bind<IService>().Tag(3).To<Service>()
    .Bind<CompositionRoot<ISet<IService>>>().To<CompositionRoot<ISet<IService>>>();

// Resolve all appropriate instances
var instances = SetsDI.Resolve<CompositionRoot<ISet<IService>>>().Root;

// Check the number of resolved instances
instances.Count.ShouldBe(3);
```



### ThreadLocal [![CSharp](https://img.shields.io/badge/C%23-code-blue.svg)](https://raw.githubusercontent.com/DevTeam/IoCContainer/master/IoC.Tests/UsageScenarios/ThreadLocal.cs)



``` CSharp
DI.Setup()
    .Bind<IDependency>().To<Dependency>()
    .Bind<IService>().To<Service>()
    .Bind<CompositionRoot<ThreadLocal<IService>>>().To<CompositionRoot<ThreadLocal<IService>>>();

// Resolve the instance of ThreadLocal<IService>
var threadLocal = ThreadLocalDI.Resolve<CompositionRoot<ThreadLocal<IService>>>().Root;

// Get the instance via ThreadLocal
var instance = threadLocal.Value;
```



### Tuples [![CSharp](https://img.shields.io/badge/C%23-code-blue.svg)](https://raw.githubusercontent.com/DevTeam/IoCContainer/master/IoC.Tests/UsageScenarios/Tuples.cs)

[Tuple](https://docs.microsoft.com/en-us/dotnet/api/system.tuple) has a set of elements that should be resolved at the same time.

``` CSharp
DI.Setup()
    .Bind<IDependency>().To<Dependency>()
    .Bind<IService>().To<Service>()
    .Bind<INamedService>().To<NamedService>(ctx => new NamedService(ctx.Resolve<IDependency>(), "some name"))
    .Bind<CompositionRoot<Tuple<IService, INamedService>>>().To<CompositionRoot<Tuple<IService, INamedService>>>();

// Resolve an instance of type Tuple<IService, INamedService>
var tuple = TuplesDI.Resolve<CompositionRoot<Tuple<IService, INamedService>>>().Root;
```



### Constructor choice [![CSharp](https://img.shields.io/badge/C%23-code-blue.svg)](https://raw.githubusercontent.com/DevTeam/IoCContainer/master/IoC.Tests/UsageScenarios/ConstructorChoice.cs)

We can specify a constructor manually and all its arguments.

``` CSharp
DI.Setup()
    .Bind<IDependency>().To<Dependency>()
    .Bind<IService>().To(
        // Select the constructor and inject required dependencies
        ctx => new Service(ctx.Resolve<IDependency>(), "some state"));

var instance = ConstructorChoiceDI.Resolve<IService>();

// Check the injected constant
instance.State.ShouldBe("some state");
```


