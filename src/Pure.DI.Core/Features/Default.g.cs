// <auto-generated/>
namespace Pure.DI.Features
{
    internal static class Default
    {
        private static void Setup()
        {
            DI.Setup("", CompositionKind.Global)
                .TypeAttribute<TypeAttribute>()
                .TagAttribute<TagAttribute>()
                .OrdinalAttribute<OrdinalAttribute>()
                .Bind<System.Func<TT>>()
                    .To(ctx => new System.Func<TT>(() =>
                    {
                        TT value;
                        ctx.Inject<TT>(ctx.Tag, out value);
                        return value;
                    }))
                .Bind<System.Lazy<TT>>()
                    .To(ctx =>
                    {
                        System.Func<TT> func;
                        ctx.Inject<System.Func<TT>>(ctx.Tag, out func);
                        return new System.Lazy<TT>(func, true);
                    })
                .Bind<System.Lazy<TT, TT1>>()
                    .To(ctx =>
                    {
                        System.Func<TT> func;
                        ctx.Inject<System.Func<TT>>(ctx.Tag, out func);
                        TT1 metadata;
                        ctx.Inject<TT1>(ctx.Tag, out metadata);
                        return new System.Lazy<TT, TT1>(func, metadata, true);
                    })
                .Bind<System.Threading.Tasks.Task<TT>>()
                    .To(ctx =>
                    {
                        System.Func<TT> func;
                        ctx.Inject<System.Func<TT>>(ctx.Tag, out func);
                        return new System.Threading.Tasks.Task<TT>(func);
                    });
        }
    }
}