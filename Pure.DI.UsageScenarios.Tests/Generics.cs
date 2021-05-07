﻿namespace Pure.DI.UsageScenarios.Tests
{
    using Shouldly;
    using Xunit;

    public class Generics
    {
        [Fact]
        public void Run()
        {
            // $visible=true
            // $tag=1 Basics
            // $priority=01
            // $description=Generics
            // $header=Autowring of generic types via binding of open generic types or generic type markers are working the same way.
            // {
            DI.Setup()
                .Bind<IDependency>().To<Dependency>()
                // Bind open generic interface to open generic implementation
                .Bind<IService<TT>>().To<Service<TT>>()
                .Bind<CompositionRoot<IService<int>>>().To<CompositionRoot<IService<int>>>();

            // Resolve a generic instance
            var instance = GenericsDI.Resolve<CompositionRoot<IService<int>>>().Root;
            // }
            // Check each instance
            instance.ShouldBeOfType<Service<int>>();
        }
    }
}
