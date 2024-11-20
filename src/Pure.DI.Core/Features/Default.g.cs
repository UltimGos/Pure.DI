// <auto-generated/>
#if !PUREDI_API_SUPPRESSION || PUREDI_API_V2
#pragma warning disable

namespace Pure.DI
{
    internal static class Default
    {
        [global::System.Diagnostics.Conditional("A2768DE22DE3E430C9653990D516CC9B")]
        private static void Setup()
        {
            DI.Setup("", CompositionKind.Global)
                .GenericTypeArgumentAttribute<GenericTypeArgumentAttribute>()
                .TypeAttribute<TypeAttribute>()
                .TagAttribute<TagAttribute>()
                .OrdinalAttribute<OrdinalAttribute>()
                .Accumulate<global::System.IDisposable, Owned>(
                    Lifetime.Transient,
                    Lifetime.PerResolve,
                    Lifetime.PerBlock)
#if NETCOREAPP3_0_OR_GREATER || NETSTANDARD2_1_OR_GREATER
                .Accumulate<global::System.IAsyncDisposable, Owned>(
                    Lifetime.Transient,
                    Lifetime.PerResolve,
                    Lifetime.PerBlock)
#endif
                .Bind<IOwned>().To((Owned owned) => owned)
                .Bind<Owned<TT>>()
                    .As(Lifetime.PerBlock)
                    .To(ctx => {
                        // Creates the owner of an instance
                        ctx.Inject<Owned>(out var owned);
                        ctx.Inject<TT>(ctx.Tag, out var value);
                        return new Owned<TT>(value, owned);
                    })
                .Bind<global::System.Func<TT>>()
                    .As(Lifetime.PerBlock)
                    .To(ctx => new global::System.Func<TT>(() =>
                    {
                        ctx.Inject<TT>(ctx.Tag, out var value);
                        return value;
                    }))
                .Bind<global::System.Collections.Generic.IComparer<TT>>()
                .Bind<global::System.Collections.Generic.Comparer<TT>>()
                    .To(_ => global::System.Collections.Generic.Comparer<TT>.Default)
                .Bind<global::System.Collections.Generic.IEqualityComparer<TT>>()
                .Bind<global::System.Collections.Generic.EqualityComparer<TT>>()
                    .To(_ => global::System.Collections.Generic.EqualityComparer<TT>.Default)
#if NETSTANDARD || NET || NETCOREAPP || NET40_OR_GREATER
                .Bind<global::System.Lazy<TT>>()
                    .To(ctx =>
                    {
                        // Injects an instance factory
                        ctx.Inject<global::System.Func<TT>>(ctx.Tag, out var factory);
                        // Creates an instance that supports lazy initialization
                        return new global::System.Lazy<TT>(factory, true);
                    })
                .Bind<global::System.Threading.CancellationToken>().To(_ => global::System.Threading.CancellationToken.None)
                .Bind<global::System.Threading.Tasks.TaskScheduler>()
                    .To(_ => global::System.Threading.Tasks.TaskScheduler.Default)
                .Bind<global::System.Threading.Tasks.TaskCreationOptions>()
                    .To(_ => global::System.Threading.Tasks.TaskCreationOptions.None)
                .Bind<global::System.Threading.Tasks.TaskContinuationOptions>()
                    .To(_ => global::System.Threading.Tasks.TaskContinuationOptions.None)
                .Bind<global::System.Threading.Tasks.TaskFactory>().As(Lifetime.PerBlock)
                    .To((global::System.Threading.CancellationToken cancellationToken, global::System.Threading.Tasks.TaskCreationOptions taskCreationOptions, global::System.Threading.Tasks.TaskContinuationOptions taskContinuationOptions, global::System.Threading.Tasks.TaskScheduler taskScheduler) =>
                    new global::System.Threading.Tasks.TaskFactory(cancellationToken, taskCreationOptions, taskContinuationOptions, taskScheduler))
                .Bind<global::System.Threading.Tasks.TaskFactory<TT>>().As(Lifetime.PerBlock)
                    .To((global::System.Threading.CancellationToken cancellationToken, global::System.Threading.Tasks.TaskCreationOptions taskCreationOptions, global::System.Threading.Tasks.TaskContinuationOptions taskContinuationOptions, global::System.Threading.Tasks.TaskScheduler taskScheduler) =>
                    new global::System.Threading.Tasks.TaskFactory<TT>(cancellationToken, taskCreationOptions, taskContinuationOptions, taskScheduler))
                .Bind<global::System.Threading.Tasks.Task<TT>>()
                    .To(ctx =>
                    {
                        // Injects an instance factory
                        ctx.Inject(ctx.Tag, out global::System.Func<TT> factory);
                        // Injects a task factory creating and scheduling task objects
                        ctx.Inject(out global::System.Threading.Tasks.TaskFactory<TT> taskFactory);
                        // Creates and starts a task using the instance factory
                        return taskFactory.StartNew(factory);
                    })
#endif                
#if NETSTANDARD2_1_OR_GREATER || NET || NETCOREAPP
                .Bind<global::System.Threading.Tasks.ValueTask<TT>>()
                    .To(ctx =>
                    {
                        ctx.Inject(ctx.Tag, out TT value);
                        // Initializes a new instance of the ValueTask class using the supplied instance
                        return new global::System.Threading.Tasks.ValueTask<TT>(value);
                    })
#endif                
#if NETSTANDARD || NET || NETCOREAPP
                .Bind<global::System.Lazy<TT, TT1>>()
                    .To(ctx =>
                    {
                        // Injects an instance factory
                        ctx.Inject<global::System.Func<TT>>(ctx.Tag, out var factory);
                        // Injects a metadata
                        ctx.Inject<TT1>(ctx.Tag, out var metadata);
                        return new global::System.Lazy<TT, TT1>(factory, metadata, true);
                    })
#endif
                // Collections
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP2_1_OR_GREATER
                .Bind<global::System.Memory<TT>>()
                    .To((TT[] arr) => new global::System.Memory<TT>(arr))
                .Bind<global::System.ReadOnlyMemory<TT>>()
                    .To((TT[] arr) => new global::System.ReadOnlyMemory<TT>(arr))
                .Bind<global::System.Buffers.MemoryPool<TT>>()
                    .To(_ => global::System.Buffers.MemoryPool<TT>.Shared)
                .Bind<global::System.Buffers.ArrayPool<TT>>()
                    .To(_ => global::System.Buffers.ArrayPool<TT>.Shared)
#endif                
                .Bind<global::System.Collections.Generic.ICollection<TT>>()
                .Bind<global::System.Collections.Generic.IList<TT>>()
                .Bind<global::System.Collections.Generic.List<TT>>()
                    .To((TT[] arr) => new global::System.Collections.Generic.List<TT>(arr))
#if NETSTANDARD || NET || NETCOREAPP || NET45_OR_GREATER
                .Bind<global::System.Collections.Generic.IReadOnlyCollection<TT>>()
                .Bind<global::System.Collections.Generic.IReadOnlyList<TT>>()
                    .To((TT[] arr) => arr)
#endif
#if NETSTANDARD1_1_OR_GREATER || NET || NETCOREAPP || NET40_OR_GREATER
                .Bind<global::System.Collections.Concurrent.IProducerConsumerCollection<TT>>()
                .Bind<global::System.Collections.Concurrent.ConcurrentBag<TT>>()
                    .To((TT[] arr) => new global::System.Collections.Concurrent.ConcurrentBag<TT>(arr))
                .Bind<global::System.Collections.Concurrent.ConcurrentQueue<TT>>()
                    .To((TT[] arr) => new global::System.Collections.Concurrent.ConcurrentQueue<TT>(arr))
                .Bind<global::System.Collections.Concurrent.ConcurrentStack<TT>>()
                    .To((TT[] arr) => new global::System.Collections.Concurrent.ConcurrentStack<TT>(arr))
                .Bind<global::System.Collections.Concurrent.BlockingCollection<TT>>()
                    .To((global::System.Collections.Concurrent.ConcurrentBag<TT> concurrentBag) =>
                    new global::System.Collections.Concurrent.BlockingCollection<TT>(concurrentBag))
#endif
#if NETSTANDARD || NET || NETCOREAPP || NET40_OR_GREATER
                .Bind<global::System.Collections.Generic.ISet<TT>>()
#endif
#if NETSTANDARD || NET || NETCOREAPP || NET35_OR_GREATER
                .Bind<global::System.Collections.Generic.HashSet<TT>>()
                    .To((TT[] arr) =>new global::System.Collections.Generic.HashSet<TT>(arr))
#endif
#if NETSTANDARD || NET || NETCOREAPP || NET45_OR_GREATER
                .Bind<global::System.Collections.Generic.SortedSet<TT>>()
                    .To((TT[] arr) => new global::System.Collections.Generic.SortedSet<TT>(arr))
#endif                
                .Bind<global::System.Collections.Generic.Queue<TT>>()
                    .To((TT[] arr) => new global::System.Collections.Generic.Queue<TT>(arr))
                .Bind<global::System.Collections.Generic.Stack<TT>>()
                    .To((TT[] arr) => new global::System.Collections.Generic.Stack<TT>(arr))
#if NETCOREAPP || NET
#if NETCOREAPP3_0_OR_GREATER
                .Bind<global::System.Collections.Immutable.ImmutableArray<TT>>()
                    .To((TT[] arr) => global::System.Runtime.CompilerServices.Unsafe.As<TT[], global::System.Collections.Immutable.ImmutableArray<TT>>(ref arr))
                .Bind<global::System.Collections.Immutable.IImmutableList<TT>>()
                .Bind<global::System.Collections.Immutable.ImmutableList<TT>>()
                    .To((TT[] arr) => global::System.Runtime.CompilerServices.Unsafe.As<TT[], global::System.Collections.Immutable.ImmutableList<TT>>(ref arr))
                .Bind<global::System.Collections.Immutable.IImmutableSet<TT>>()
                .Bind<global::System.Collections.Immutable.ImmutableHashSet<TT>>()
                    .To((TT[] arr) => global::System.Runtime.CompilerServices.Unsafe.As<TT[], global::System.Collections.Immutable.ImmutableHashSet<TT>>(ref arr))
                .Bind<global::System.Collections.Immutable.ImmutableSortedSet<TT>>()
                    .To((TT[] arr) => global::System.Runtime.CompilerServices.Unsafe.As<TT[], global::System.Collections.Immutable.ImmutableSortedSet<TT>>(ref arr))
                .Bind<global::System.Collections.Immutable.IImmutableQueue<TT>>()
                .Bind<global::System.Collections.Immutable.ImmutableQueue<TT>>()
                    .To((TT[] arr) => global::System.Runtime.CompilerServices.Unsafe.As<TT[], global::System.Collections.Immutable.ImmutableQueue<TT>>(ref arr))
                .Bind<global::System.Collections.Immutable.IImmutableStack<TT>>()
                .Bind<global::System.Collections.Immutable.ImmutableStack<TT>>()
                    .To((TT[] arr) => global::System.Runtime.CompilerServices.Unsafe.As<TT[], global::System.Collections.Immutable.ImmutableStack<TT>>(ref arr))
#else                
                .Bind<global::System.Collections.Immutable.ImmutableArray<TT>>()
                    .To((TT[] arr) => global::System.Collections.Immutable.ImmutableArray.Create<TT>(arr))
                .Bind<global::System.Collections.Immutable.IImmutableList<TT>>()
                .Bind<global::System.Collections.Immutable.ImmutableList<TT>>()
                    .To((TT[] arr) => global::System.Collections.Immutable.ImmutableList.Create<TT>(arr))
                .Bind<global::System.Collections.Immutable.IImmutableSet<TT>>()
                .Bind<global::System.Collections.Immutable.ImmutableHashSet<TT>>()
                    .To((TT[] arr) => global::System.Collections.Immutable.ImmutableHashSet.Create<TT>(arr))
                .Bind<global::System.Collections.Immutable.ImmutableSortedSet<TT>>()
                    .To((TT[] arr) => global::System.Collections.Immutable.ImmutableSortedSet.Create<TT>(arr))
                .Bind<global::System.Collections.Immutable.IImmutableQueue<TT>>()
                .Bind<global::System.Collections.Immutable.ImmutableQueue<TT>>()
                    .To((TT[] arr) => global::System.Collections.Immutable.ImmutableQueue.Create<TT>(arr))
                .Bind<global::System.Collections.Immutable.IImmutableStack<TT>>()
                .Bind<global::System.Collections.Immutable.ImmutableStack<TT>>()
                    .To((TT[] arr) => global::System.Collections.Immutable.ImmutableStack.Create<TT>(arr))
#endif
#endif
#if NET6_0_OR_GREATER
                .Bind<global::System.Random>().To(_ =>
                {
                    // Provides a thread-safe Random instance that may be used concurrently from any thread
                    return global::System.Random.Shared;
                })
#endif
#if NETCOREAPP2_0 || NET || NETSTANDARD2_0_OR_GREATER
                .Bind<global::System.Text.Encoding>().To(_ =>
                {
                    // Gets an encoding for the operating system's current ANSI code page
                    return global::System.Text.Encoding.Default;
                })
#endif
                .Bind<global::System.Text.Decoder>().As(Lifetime.PerBlock).To((global::System.Text.Encoding encoding) =>
                {
                    // Gets a decoder that converts an encoded sequence of bytes into a sequence of characters
                    return encoding.GetDecoder();
                })
                .Bind<global::System.Text.Encoder>().As(Lifetime.PerBlock).To((global::System.Text.Encoding encoding) =>
                {
                    // Gets an encoder that converts a sequence of Unicode characters into an encoded sequence of bytes
                    return encoding.GetEncoder();
                })
;
        }
    }
}
#pragma warning restore
#endif
