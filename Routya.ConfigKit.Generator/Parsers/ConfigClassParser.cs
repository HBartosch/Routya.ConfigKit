using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Routya.ConfigKit.Generator.Constants;
using Routya.ConfigKit.Generator.Models;
using System;
using System.Collections.Generic;

namespace Routya.ConfigKit.Generator.Parsers
{
    internal static class ConfigClassParser
    {
        public static ConfigClassModel? Parse(ClassDeclarationSyntax classSyntax, SemanticModel semanticModel)
        {
            var metadata = GetSectionMetadata(classSyntax, semanticModel);
            if (metadata == null)
                return null;

            var model = new ConfigClassModel
            (
                GetNamespace(classSyntax),
                classSyntax.Identifier.Text,
                metadata.SectionName,
                metadata.BindingMode,
                new List<ConfigPropertyModel>()
            );

            foreach (var member in classSyntax.Members)
            {
                var property = member as PropertyDeclarationSyntax;
                if (property == null)
                    continue;

                model.Properties.Add(
                    new ConfigPropertyModel
                    {
                        Name = property.Identifier.Text,
                        TypeName = property.Type.ToString()
                    });
            }

            return model;
        }

        private static string? GetNamespace(ClassDeclarationSyntax classSyntax)
        {
            for (SyntaxNode? current = classSyntax.Parent; current != null; current = current.Parent)
            {
                switch (current)
                {
                    case NamespaceDeclarationSyntax namespaceDecl:
                        return namespaceDecl.Name.ToString();
                    case FileScopedNamespaceDeclarationSyntax fileScopedNamespaceDecl:
                        return fileScopedNamespaceDecl.Name.ToString();
                }
            }
            return null;
        }

        private static ConfigSectionMetadata? GetSectionMetadata(ClassDeclarationSyntax classSyntax, SemanticModel semanticModel)
        {
            foreach (var attributeList in classSyntax.AttributeLists)
            {
                foreach (var attribute in attributeList.Attributes)
                {
                    var symbol = semanticModel.GetSymbolInfo(attribute).Symbol as IMethodSymbol;
                    if (symbol?.ContainingType?.Name != ParserConstants.ConfigSectionAttribute)
                        continue;

                    string? sectionName = null;
                    ConfigBindingMode mode = ConfigBindingMode.Singleton;

                    // Map constructor args by parameter name
                    if (attribute.ArgumentList != null)
                    {
                        for (int i = 0; i < attribute.ArgumentList.Arguments.Count; i++)
                        {
                            var arg = attribute.ArgumentList.Arguments[i];
                            var paramSymbol = i < symbol.Parameters.Length ? symbol.Parameters[i] : null;
                            var paramName = paramSymbol?.Name;

                            // Positional or named — both work
                            if (paramName == ParserConstants.SectionParameterName)
                            {
                                var constVal = semanticModel.GetConstantValue(arg.Expression);
                                if (constVal.HasValue && constVal.Value is string str)
                                    sectionName = str;
                            }
                            else if (paramName == ParserConstants.ConfigBindingModeParameterName)
                            {
                                var fieldSymbol = semanticModel.GetSymbolInfo(arg.Expression).Symbol as IFieldSymbol;
                                if (fieldSymbol?.ContainingType?.Name == nameof(ConfigBindingMode))
                                {
                                    var constVal = semanticModel.GetConstantValue(arg.Expression);

                                    if (Enum.TryParse(constVal.Value?.ToString(), out ConfigBindingMode parsed))
                                    {
                                        mode = parsed;
                                    }
                                }
                            }
                        }
                    }

                    return !string.IsNullOrWhiteSpace(sectionName)
                        ? new ConfigSectionMetadata(sectionName, mode)
                        : null;
                }
            }

            return null;
        }

    }
}