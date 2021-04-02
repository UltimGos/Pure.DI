﻿namespace Sample
{
    using System;
    using Pure.DI;
    using static Pure.DI.Lifetime;

    // Let's create an abstraction

    interface IBox<out T> { T Content { get; } }

    interface ICat { State State { get; } }

    enum State { Alive, Dead }

    // Here is our implementation

    class CardboardBox<T> : IBox<T>
    {
        public CardboardBox(T content) => Content = content;

        public T Content { get; }

        public override string ToString() => $"[{Content}]";
    }

    class ShroedingersCat : ICat
    {
        // Represents the superposition of the states
        private readonly Lazy<State> _superposition;

        public ShroedingersCat(Lazy<State> superposition) => _superposition = superposition;

        // The decoherence of the superposition at the time of observation via an irreversible process
        public State State => _superposition.Value;

        public override string ToString() => $"{State} cat";
    }

    // Let's glue all together

    static partial class Composer
    {
        // Models a random subatomic event that may or may not occur
        private static readonly Random Indeterminacy = new();

        static Composer()
        {
            DI.Setup()
                // .NET BCL types
                .Bind<Func<TT>>().To(ctx => new Func<TT>(ctx.Resolve<TT>))
                .Bind<Lazy<TT>>().To<Lazy<TT>>()
                // Represents a quantum superposition of 2 states: Alive or Dead
                .Bind<State>().To(ctx => (State)Indeterminacy.Next(2))
                // Represents schrodinger's cat
                .Bind<ICat>().To<ShroedingersCat>()
                // Represents a cardboard box with any content
                .Bind<IBox<TT>>().To<CardboardBox<TT>>()
                // Composition Root
                .Bind<Program>().As(Singleton).To<Program>();
        }
    }

    // Time to open boxes!

    class Program
    {
        // Composition Root, a single place in an application where the composition of the object graphs for an application take place
        public static void Main() => Composer.Resolve<Program>().Run();

        private readonly IBox<ICat> _box;

        internal Program(IBox<ICat> box) => _box = box;

        private void Run() => Console.WriteLine(_box);
    }
}