using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Routya.ConfigKit.Generator.Emitters;
using Routya.ConfigKit.Generator.Models;
using Routya.ConfigKit.Generator.Parsers;
using System.Linq;

namespace Routya.ConfigKit.Generator.Generators
{
    [Generator]
    public sealed class ConfigGenerator : IIncrementalGenerator
    {
        public void Initialize(IncrementalGeneratorInitializationContext context)
        {
            var configClasses = context.SyntaxProvider
               .CreateSyntaxProvider(
                   predicate: static (node, _) => node is ClassDeclarationSyntax c && c.AttributeLists.Count > 0,
                   transform: static (context, _) => ProcessClass((ClassDeclarationSyntax)context.Node, context.SemanticModel))
               .Where(model => model != null);

            context.RegisterSourceOutput(configClasses, (ctx, model) =>
            {
                var source = ConfigBindingEmitter.Generate(model);
                ctx.AddSource($"{model.ClassName}_ConfigBinding.g.cs", source);
            });
        }

        private static ConfigClassModel? ProcessClass(ClassDeclarationSyntax classSyntax, SemanticModel semanticModel)
        {
            var model = ConfigClassParser.Parse(classSyntax, semanticModel);

            return string.IsNullOrWhiteSpace(model?.SectionName) ? null : model;
        }
    }
}