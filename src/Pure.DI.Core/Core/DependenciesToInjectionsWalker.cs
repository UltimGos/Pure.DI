namespace Pure.DI.Core;

internal sealed class DependenciesToInjectionsWalker: DependenciesWalker, IEnumerable<Injection>
{
    private readonly List<Injection> _injections = new();

    public override void VisitInjection(in Injection injection, in ImmutableArray<Location> locations)
    {
        _injections.Add(injection);
        base.VisitInjection(in injection, locations);
    }

    public IEnumerator<Injection> GetEnumerator() => _injections.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}