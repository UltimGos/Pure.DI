// ReSharper disable HeapView.ObjectAllocation
// ReSharper disable HeapView.BoxingAllocation
// ReSharper disable HeapView.ObjectAllocation.Evident
namespace Pure.DI.Core.Models;

internal record MdSetup(
    SyntaxNode Source,
    in CompositionName Name,
    in ImmutableArray<MdUsingDirectives> UsingDirectives,
    CompositionKind Kind,
    IHints Hints,
    in ImmutableArray<MdBinding> Bindings,
    in ImmutableArray<MdRoot> Roots,
    in ImmutableArray<MdDependsOn> DependsOn,
    in ImmutableArray<MdTypeAttribute> TypeAttributes,
    in ImmutableArray<MdTagAttribute> TagAttributes,
    in ImmutableArray<MdOrdinalAttribute> OrdinalAttributes,
    ITypeConstructor? TypeConstructor = default)
{
    public IEnumerable<string> ToStrings(int indent)
    {
        var walker = new MetadataToLinesWalker(indent);
        walker.VisitSetup(this);
        return walker;
    }
    
    public override string ToString() => string.Join(Environment.NewLine, ToStrings(0));
}