using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Routya.ConfigKit.Generator.Generators;
namespace Routya.ConfigKit.Test;

public class ConfigGeneratorTests
{
    [Fact]
    public void Should_Generate_ConfigExtension_Method()
    {
        // Arrange: Build a dummy input Compilation manually
        var sourceCode = @"
using Routya.ConfigKit.Attributes;
using System.ComponentModel.DataAnnotations;

[ConfigSection(""MyService"")]
public class MyServiceOptions
{
    [Required]
    public int RetryCount { get; init; }
    public bool UseCaching { get; init; }
}";

        var syntaxTree = CSharpSyntaxTree.ParseText(sourceCode);

        var references = AppDomain.CurrentDomain.GetAssemblies()
            .Where(a => !a.IsDynamic && !string.IsNullOrWhiteSpace(a.Location))
            .Select(a => MetadataReference.CreateFromFile(a.Location))
            .Cast<MetadataReference>()
            .ToList();

        var compilation = CSharpCompilation.Create(
            "TestProject",
            new[] { syntaxTree },
            references,
            new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary)
        );

        // Act: Set up and run the generator
        var generator = new ConfigGenerator();

        var sourceGenerator = generator.AsSourceGenerator();

        GeneratorDriver driver = CSharpGeneratorDriver.Create(
            generators: new ISourceGenerator[] { sourceGenerator },
            driverOptions: new GeneratorDriverOptions(default, trackIncrementalGeneratorSteps: true)
        );

        driver = driver.RunGenerators(compilation);

        // Update the compilation (simulate small change) and rerun
        compilation = compilation.AddSyntaxTrees(CSharpSyntaxTree.ParseText("// dummy"));
        driver = driver.RunGenerators(compilation);

        var runResult = driver.GetRunResult();

        // Assert: Inspect the generator result
        var result = runResult.Results.Single();

        // Print outputs for inspection (optional for debug)
        foreach (var generated in result.GeneratedSources)
        {
            Console.WriteLine($"Generated HintName: {generated.HintName}");
            Console.WriteLine(generated.SourceText.ToString());
        }

        // Basic asserts
        Assert.True(result.GeneratedSources.Length > 0);
        Assert.Contains(result.GeneratedSources, g => g.HintName.Contains("MyServiceOptions"));
    }
}