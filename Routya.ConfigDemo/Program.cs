using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Routya.ConfigDemo;

internal class Program
{
    static void Main(string[] args)
    {
        var builder = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json");

        var configuration = builder.Build();

        var services = new ServiceCollection();

        services.AddMyServiceOptions(configuration);

        var provider = services.BuildServiceProvider();

        // Resolve the options
        var options = provider.GetRequiredService<IOptions<MyServiceOptions>>();

        // Output values to the console
        Console.WriteLine("------ MyServiceOptions ------");
        Console.WriteLine($"RetryCount: {options.Value.RetryCount}");
        Console.WriteLine($"TimeoutSeconds: {options.Value.TimeoutSeconds}");
        Console.WriteLine($"UseCaching: {options.Value.UseCaching}");
        Console.WriteLine("--------------------------------");
        Console.ReadLine();
    }
}
