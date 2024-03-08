﻿using Build;

DI.Setup(nameof(Composition))
    .Root<RootTarget>("RootTarget")
    
    .DefaultLifetime(Lifetime.PerBlock)
    
    .Bind().To<RootCommand>()
    .Bind().To<Settings>()
    .Bind<ITeamCityArtifactsWriter>().To(_ => GetService<ITeamCityWriter>())
    .Bind().To(_ => GetService<INuGet>())
    
    // Targets
    .Bind(Tag.Type).To<GeneratorTarget>()
    .Bind(Tag.Type).To<LibrariesTarget>()
    .Bind(Tag.Type).To<CompatibilityCheckTarget>()
    .Bind(Tag.Type).To<PackTarget>()
    .Bind(Tag.Type).To<ReadmeTarget>()
    .Bind(Tag.Type).To<BenchmarksTarget>()
    .Bind(Tag.Type).To<DeployTarget>()
    .Bind(Tag.Type).To<TemplateTarget>()
    .Bind(Tag.Type).To<UpdateTarget>()
    .Bind(Tag.Type).To<PublishBlazorTarget>();

return await new Composition().RootTarget.RunAsync(CancellationToken.None);