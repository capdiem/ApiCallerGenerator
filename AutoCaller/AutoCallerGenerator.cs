using Microsoft.CodeAnalysis;
using System.Diagnostics;
using System.Linq;
using System.Threading;

namespace AutoCaller;

[Generator]
internal class AutoCallerGenerator : IIncrementalGenerator
{
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
#if DEBUG
        if (!Debugger.IsAttached)
        {
            //Debugger.Launch();
        }
#endif

        var provider = context.SyntaxProvider
                              .CreateSyntaxProvider(IsSyntaxTargetForGeneration, GetTargetDataModelForGeneration)
                              .Where(e => e is not null);

        context.RegisterSourceOutput(provider.Collect(), (ctx, models) =>
        {

        });
    }

    static bool IsSyntaxTargetForGeneration(SyntaxNode node, CancellationToken token)
    {
        return false;
    }

    static object GetTargetDataModelForGeneration(GeneratorSyntaxContext context, CancellationToken token)
    {
        return null;
    }
}
