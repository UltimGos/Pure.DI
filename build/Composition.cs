// ReSharper disable UnusedMember.Local
namespace Build;

using System.CommandLine;
using Pure.DI;

internal partial class Composition
{
    private static void Setup() =>
        // FormatCode = On
        // Resolve = Off
        DI.Setup("Composition")
            .Arg<Settings>("settings")
            .DefaultLifetime(Lifetime.PerResolve)
            .Bind<JetBrains.TeamCity.ServiceMessages.Write.Special.ITeamCityWriter>().As(Lifetime.PerResolve).To(_ => GetService<JetBrains.TeamCity.ServiceMessages.Write.Special.ITeamCityWriter>())
            .Bind<ITarget<int>>().Bind<ICommandProvider>().Tags(nameof(ReadmeTarget)).To<ReadmeTarget>()
            .Bind<ITarget<string>>().Bind<ICommandProvider>().Tags(nameof(PackTarget)).To<PackTarget>()
            .Bind<ITarget<int>>().Bind<ICommandProvider>().Tags(nameof(BenchmarksTarget)).To<BenchmarksTarget>()
            .Bind<ITarget<int>>().Bind<ICommandProvider>().Tags(nameof(DeployTarget)).To<DeployTarget>()
            .Bind<ITarget<string>>().Bind<ICommandProvider>().Tags(nameof(TemplateTarget)).To<TemplateTarget>()
            .Bind<IEnumerable<Command>>().To(ctx =>
            {
                ctx.Inject(out IEnumerable<ICommandProvider> commandProviders);
                return commandProviders.Select(i => i.Command);
            })
            .Root<Program>("Root");
}