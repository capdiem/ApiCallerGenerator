using MasaApiCallerGenerator.Models;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MasaApiCallerGenerator;

[Generator]
public class CallerGenerator : IIncrementalGenerator
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

        var source = context.AnalyzerConfigOptionsProvider.Combine(provider.Collect());

        context.RegisterSourceOutput(source, (ctx, z) =>
        {
            var (config, models) = z;

            if (!config.GlobalOptions.TryGetValue("build_property.masaapicaller_baseaddress", out var baseAdress))
            {
                //TODO: ReportDiagnostic
                //ctx.ReportDiagnostic(Diagnostic.Create())
            }

            if (!config.GlobalOptions.TryGetValue("build_property.masaapicaller_name", out string? name))
            {
                name = "Default";
            }

            var caller = new CallerModel
            {
                Name = name,
                BaseAddress = baseAdress,
                Services = models.ToList()!
            };

            foreach (var (hintName, sourceText) in CallerHelper.GenerateSources(caller))
            {
                ctx.AddSource(hintName, sourceText);
            }
        });
    }

    static bool IsSyntaxTargetForGeneration(SyntaxNode node, CancellationToken token)
    {
        return node is ClassDeclarationSyntax classDeclarationSyntax
             && classDeclarationSyntax.BaseList is not null
             && classDeclarationSyntax.BaseList.ToString().EndsWith("ServiceBase");
    }

    static ServiceModel? GetTargetDataModelForGeneration(GeneratorSyntaxContext context, CancellationToken token)
    {
        var masaServiceBase = context.SemanticModel.Compilation.GetTypeByMetadataName("MASA.Contrib.Service.MinimalAPIs.ServiceBase");
        var masaServiceBase2 = context.SemanticModel.Compilation.GetTypeByMetadataName("Masa.Contrib.Service.MinimalAPIs.ServiceBase");
        
        var fromServiceAttribute = context.SemanticModel.Compilation.GetTypeByMetadataName("Microsoft.AspNetCore.Mvc.FromServicesAttribute");

        var task = context.SemanticModel.Compilation.GetTypeByMetadataName(typeof(Task).FullName);

        if (masaServiceBase is null && masaServiceBase2 is null)
        {
            return null;
        }

        var classDeclaration = (ClassDeclarationSyntax)context.Node;

        var semanticModel = context.SemanticModel.Compilation.GetSemanticModel(classDeclaration.SyntaxTree);

        if (semanticModel.GetDeclaredSymbol(classDeclaration) is not INamedTypeSymbol classNamedTypeSymbol)
        {
            return null;
        }

        if (!IsInheritsFrom(classNamedTypeSymbol, masaServiceBase) && !IsInheritsFrom(classNamedTypeSymbol, masaServiceBase2))
        {
            return null;
        }

        var serviceName = classNamedTypeSymbol.Name;

        var service = new ServiceModel
        {
            Name = serviceName
        };

        foreach (var member in classNamedTypeSymbol.GetMembers())
        {
            if (member is not IMethodSymbol methodSymbol)
            {
                continue;
            }

            if (methodSymbol.MethodKind == MethodKind.Constructor)
            {
                var syntaxRefs = methodSymbol.DeclaringSyntaxReferences;
                var firstRef = syntaxRefs.FirstOrDefault();
                var firstSyntaxNode = firstRef?.GetSyntax();

                if (firstSyntaxNode is ConstructorDeclarationSyntax constructorDeclaration)
                {
                    if (constructorDeclaration.Initializer is not null)
                    {
                        var baseUriArgument = constructorDeclaration.Initializer.ArgumentList.Arguments.FirstOrDefault(arg => arg.Expression.IsKind(SyntaxKind.StringLiteralExpression));

                        var baseUri = baseUriArgument.GetLastToken().Value?.ToString();

                        service.BaseAdress = baseUri;
                    }

                    service.Methods = GetMethods(constructorDeclaration);
                }
            }
            else
            {
                var method = service.Methods.FirstOrDefault(m => m.Name == methodSymbol.Name);
                if (method is null)
                {
                    continue;
                }

                Dictionary<string, string>? query = null;
                foreach (var parameter in methodSymbol.Parameters)
                {
                    // ignore [FormService]
                    if (parameter.GetAttributes().Any(attr => attr.AttributeClass!.Equals(fromServiceAttribute, SymbolEqualityComparer.Default)))
                    {
                        continue;
                    }

                    var name = parameter.Name;
                    var type = parameter.Type.ToDisplayString();

                    if (query == null)
                    {
                        query = new Dictionary<string, string>();
                    }

                    query.Add(name, type);
                }

                method.Query = query;

                if (methodSymbol.ReturnType is INamedTypeSymbol returnType)
                {
                    if (returnType.BaseType is not null && returnType.BaseType.Equals(task, SymbolEqualityComparer.Default))
                    {
                        method.ReturnType = returnType.TypeArguments.First().ToDisplayString();
                    }
                    else if (returnType.Equals(task, SymbolEqualityComparer.Default))
                    {
                        method.ReturnType = "void";
                    }
                    else
                    {
                        method.ReturnType = returnType.ToDisplayString();
                    }
                }

                //if (methodSymbol.ReturnType is INamedTypeSymbol actionResultType
                //    && actionResultType.OriginalDefinition.ToString() == "Microsoft.AspNetCore.Mvc.ActionResult<TValue>")
                //{
                //    method.ReturnType = actionResultType.TypeArguments.First().ToDisplayString();
                //}

                //if (methodSymbol.ReturnType.OriginalDefinition.ToString() == "Microsoft.AspNetCore.Mvc.IActionResult")
                //{
                //    method.ReturnType = null;
                //}
            }
        }

        return service;
    }

    static bool IsInheritsFrom(INamedTypeSymbol classDeclaration, INamedTypeSymbol baseDeclaration)
    {
        var currentClassDeclaration = classDeclaration;
        while (currentClassDeclaration.BaseType is not null)
        {
            var currentBaseType = currentClassDeclaration.BaseType;
            if (currentBaseType.Equals(baseDeclaration, SymbolEqualityComparer.Default))
            {
                return true;
            }

            currentClassDeclaration = currentClassDeclaration.BaseType;
        }

        return false;
    }

    static List<MethodModel> GetMethods(ConstructorDeclarationSyntax constructorDeclaration)
    {
        List<MethodModel> methods = new();

        if (constructorDeclaration.Body is null)
        {
            return methods;
        }

        const string handlerConst = "handler";
        const string customUriConst = "customUri";
        const string trimEndAsyncConst = "trimEndAsync";

        foreach (var statement in constructorDeclaration.Body.Statements)
        {
            Dictionary<string, object> argumentsOfMap = new();
            string methodInvoked = null!;
            bool isOriginAppMap = false;

            if (statement is ExpressionStatementSyntax expressionStatementSyntax
                && expressionStatementSyntax.Expression is InvocationExpressionSyntax invocationExpressionSyntax)
            {
                var token = invocationExpressionSyntax.GetFirstToken().ValueText;
                if (token.StartsWith("App"))
                {
                    isOriginAppMap = true;
                    token = invocationExpressionSyntax.Expression.GetLastToken().ValueText;

                    for (int i = 0; i < invocationExpressionSyntax.ArgumentList.Arguments.Count; i++)
                    {
                        var argument = invocationExpressionSyntax.ArgumentList.Arguments[i];

                        if (i == 0)
                        {
                            argumentsOfMap[customUriConst] = argument.GetLastToken().Value ?? "";
                        }
                        else
                        {
                            argumentsOfMap[handlerConst] = argument.GetLastToken().Value!;
                        }
                    }
                }
                else
                {
                    for (int i = 0; i < invocationExpressionSyntax.ArgumentList.Arguments.Count; i++)
                    {
                        var argument = invocationExpressionSyntax.ArgumentList.Arguments[i];

                        if (i == 0)
                        {
                            argumentsOfMap[handlerConst] = argument.GetLastToken().Value!;
                        }
                        else if (argument.Expression.IsKind(SyntaxKind.StringLiteralExpression))
                        {
                            argumentsOfMap[customUriConst] = argument.GetLastToken().Value ?? "";
                        }
                        else
                        {
                            argumentsOfMap[trimEndAsyncConst] = argument.GetLastToken().Value!;
                        }
                    }
                }

                try
                {
                    methodInvoked = GetHttpMethod(token);
                }
                catch (Exception)
                {
                    // todo: ReportDiagnostic
                    throw;
                }
            }

            var handler = (string)argumentsOfMap[handlerConst];
            string relativeUri;

            if (argumentsOfMap.ContainsKey(customUriConst))
            {
                relativeUri = (string)argumentsOfMap[customUriConst];
            }
            else
            {
                bool trimEndAsync = true;
                if (argumentsOfMap.ContainsKey(trimEndAsyncConst))
                {
                    trimEndAsync = (bool)argumentsOfMap[trimEndAsyncConst];
                }

                relativeUri = trimEndAsync && handler.EndsWith("async", StringComparison.OrdinalIgnoreCase)
                    ? handler.Substring(0, handler.Length - 5)
                    : handler;
            }

            methods.Add(new MethodModel
            {
                Name = handler,
                MethodInvoked = methodInvoked,
                IsOriginAppMap = isOriginAppMap,
                RelativeUri = relativeUri,
            });
        }

        return methods;

        static string GetHttpMethod(string mapMethod) => mapMethod switch
        {
            "MapGet" => "Get",
            "MapPost" => "Post",
            "MapPut" => "Put",
            "MapDelete" => "Delete",
            _ => throw new NotImplementedException(),
        };
    }
}
