﻿/*
$v=true
$p=9
$d=Global compositions
$h=When the `Setup(name, kind)` method is called, the second optional parameter specifies the composition kind. If you set it as `CompositionKind.Global`, no composition class will be created, but this setup will be the base setup for all others in the current project, and `DependsOn(...)` is not required. The setups will be applied in the sort order of their names.
*/

// ReSharper disable CheckNamespace
// ReSharper disable UnusedMember.Local
// ReSharper disable UnusedType.Global
// ReSharper disable ArrangeTypeModifiers
// ReSharper disable ArrangeTypeMemberModifiers
namespace Pure.DI.UsageTests.Advanced.GlobalCompositionsScenario;

// {
class MyGlobalComposition
{
    void Setup() =>
        DI.Setup(nameof(GlobalCompositionsScenario), CompositionKind.Global)
            .Hint(Hint.ToString, "On")
            .Hint(Hint.FormatCode, "Off");
}

class MyGlobalComposition2
{
    void Setup() =>
        DI.Setup(nameof(MyGlobalComposition2), CompositionKind.Global)
            .Hint(Hint.FormatCode, "On");
}
// }