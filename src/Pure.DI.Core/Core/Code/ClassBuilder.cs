// ReSharper disable ClassNeverInstantiated.Global
namespace Pure.DI.Core.Code;

using System.Buffers.Text;

internal sealed class ClassBuilder(
    [Tag(WellknownTag.UsingDeclarationsBuilder)] IBuilder<CompositionCode, CompositionCode> usingDeclarationsBuilder,
    [Tag(WellknownTag.FieldsBuilder)] IBuilder<CompositionCode, CompositionCode> fieldsBuilder,
    [Tag(WellknownTag.ArgFieldsBuilder)] IBuilder<CompositionCode, CompositionCode> argFieldsBuilder,
    [Tag(WellknownTag.ParameterizedConstructorBuilder)] IBuilder<CompositionCode, CompositionCode> parameterizedConstructorBuilder,
    [Tag(WellknownTag.DefaultConstructorBuilder)] IBuilder<CompositionCode, CompositionCode> defaultConstructorBuilder,
    [Tag(WellknownTag.ScopeConstructorBuilder)] IBuilder<CompositionCode, CompositionCode> scopeConstructorBuilder,
    [Tag(WellknownTag.RootMethodsBuilder)] IBuilder<CompositionCode, CompositionCode> rootPropertiesBuilder,
    [Tag(WellknownTag.ApiMembersBuilder)] IBuilder<CompositionCode, CompositionCode> apiMembersBuilder,
    [Tag(WellknownTag.DisposeMethodBuilder)] IBuilder<CompositionCode, CompositionCode> disposeMethodBuilder,
    [Tag(WellknownTag.ToStringMethodBuilder)] IBuilder<CompositionCode, CompositionCode> toStringBuilder,
    [Tag(WellknownTag.ResolversFieldsBuilder)] IBuilder<CompositionCode, CompositionCode> resolversFieldsBuilder,
    [Tag(WellknownTag.StaticConstructorBuilder)] IBuilder<CompositionCode, CompositionCode> staticConstructorBuilder,
    [Tag(WellknownTag.ResolverClassesBuilder)] IBuilder<CompositionCode, CompositionCode> resolversClassesBuilder,
    IInformation information,
    IComments comments,
    IBuilder<IEnumerable<string>, Uri> mermaidUrlBuilder,
    CancellationToken cancellationToken)
    : IBuilder<CompositionCode, CompositionCode>
{
    private readonly ImmutableArray<IBuilder<CompositionCode, CompositionCode>> _codeBuilders = ImmutableArray.Create(
        fieldsBuilder,
        argFieldsBuilder,
        parameterizedConstructorBuilder,
        defaultConstructorBuilder,
        scopeConstructorBuilder,
        rootPropertiesBuilder,
        apiMembersBuilder,
        disposeMethodBuilder,
        toStringBuilder,
        resolversFieldsBuilder,
        staticConstructorBuilder,
        resolversClassesBuilder);

    public CompositionCode Build(CompositionCode composition)
    {
        var code = composition.Code;
        code.AppendLine("// <auto-generated/>");
        code.AppendLine($"// by {information.Description}");
        code.AppendLine("#nullable enable");
        code.AppendLine("#pragma warning disable");
        code.AppendLine();

        composition = usingDeclarationsBuilder.Build(composition);
        
        var nsIndent = Disposables.Empty;
        var name = composition.Source.Source.Name;
        if (!string.IsNullOrWhiteSpace(name.Namespace))
        {
            code.AppendLine($"namespace {name.Namespace}");
            code.AppendLine("{");
            nsIndent = code.Indent();
        }

        var hints = composition.Source.Source.Hints;
        var isCommentsEnabled = hints.GetHint(Hint.Comments, SettingState.On) == SettingState.On;
        if (isCommentsEnabled)
        {
            var privateRootAdditionalComment = $"is a private composition root that can be resolved by methods like <see cref=\"{hints.GetValueOrDefault(Hint.ResolveMethodName, Names.ResolveMethodName)}{{T}}()\"/>.";
            var classComments = composition.Source.Source.Comments;
            if (classComments.Count > 0 || composition.Roots.Length > 0)
            {
                code.AppendLine("/// <summary>");
                if (classComments.Count > 0)
                {
                    code.AppendLine("/// <para>");
                    foreach (var comment in comments.Format(classComments))
                    {
                        code.AppendLine(comment);
                    }

                    code.AppendLine("/// </para>");
                }
                
                if (!composition.Diagram.IsEmpty)
                {
                    var diagramUrl = mermaidUrlBuilder.Build(composition.Diagram.Select(i => i.Text));
                    code.AppendLine($"/// <a href=\"{diagramUrl}\">Class diagram</a><br/>");
                }

                var orderedRoots = composition.Roots
                    .OrderByDescending(root => root.IsPublic)
                    .ThenBy(root => root.PropertyName)
                    .ThenBy(root => root.Node.Binding)
                    .ToArray();

                if (composition.Roots.Length > 0)
                {
                    var rootComments = comments.FormatList(
                        "Composition roots:",
                        orderedRoots.Select(root => (CreateTerms(root), CreateDescriptions(root))));

                    foreach (var rootComment in rootComments)
                    {
                        code.AppendLine(rootComment);
                    }

                    IReadOnlyCollection<string> CreateTerms(Root root) =>
                        root.IsPublic
                            ? [$"<see cref=\"{root.Node.Type}\"/> {root.PropertyName}"]
                            : [$"<see cref=\"{root.Node.Type}\"/> {privateRootAdditionalComment}"];

                    IReadOnlyCollection<string> CreateDescriptions(Root root) => root.Source.Comments;
                }

                var root = orderedRoots.FirstOrDefault(i => i.IsPublic);
                if (root is not null)
                {
                    var classArgsStr = "()";
                    if (!composition.Args.IsEmpty)
                    {
                        classArgsStr = $"({string.Join(", ", composition.Args.Select(arg => $"{arg.Node.Arg?.Source.ArgName ?? "..."}"))})";
                    }

                    var rootArgsStr = "";
                    if (!root.Args.IsEmpty || (root.Kind & RootKinds.Method) == RootKinds.Method)
                    {
                        rootArgsStr = $"({string.Join(", ", root.Args.Select(arg => $"{arg.Node.Arg?.Source.ArgName ?? "..."}"))})";
                    }

                    code.AppendLine("/// <example>");
                    code.AppendLine($"/// This shows how to get an instance of type <see cref=\"{root.Node.Type}\"/> using the composition root <see cref=\"{root.PropertyName}\"/>:");
                    code.AppendLine("/// <code>");
                    code.AppendLine($"/// {(composition.DisposablesCount == 0 ? "" : "using ")}var composition = new {name.ClassName}{classArgsStr};");
                    code.AppendLine($"/// var instance = composition.{root.PropertyName}{rootArgsStr};");
                    code.AppendLine("/// </code>");
                    code.AppendLine("/// </example>");
                }

                code.AppendLine("/// This class was created by <a href=\"https://github.com/DevTeam/Pure.DI\">Pure.DI</a> source code generator.");
                code.AppendLine("/// </summary>");
                code.AppendLine("/// <seealso cref=\"Pure.DI.DI.Setup\"/>");
            }
        }

        code.AppendLine($"[{Names.SystemNamespace}Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]");
        var implementingInterfaces = composition.DisposablesCount > 0 ? $": {Names.IDisposableInterfaceName}" : "";
        code.AppendLine($"partial class {name.ClassName}{implementingInterfaces}");
        code.AppendLine("{");

        using (code.Indent())
        {
            // Generate class members
            foreach (var builder in _codeBuilders)
            {
                cancellationToken.ThrowIfCancellationRequested();
                composition = builder.Build(composition);
            }
        }

        code.AppendLine("}");

        // ReSharper disable once InvertIf
        if (!string.IsNullOrWhiteSpace(name.Namespace))
        {
            // ReSharper disable once RedundantAssignment
            nsIndent.Dispose();
            code.AppendLine("}");
        }
        
        code.AppendLine("#pragma warning restore");
        return composition;
    }
}