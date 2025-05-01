using Routya.ConfigKit;
using System.ComponentModel.DataAnnotations;

[ConfigSection("MyService", ConfigBindingMode.IOptions)]
public class MyServiceOptions
{
    [Required]
    public int? RetryCount { get; init; }

    [Range(1, 60)]
    public int TimeoutSeconds { get; init; }

    public bool UseCaching { get; init; } = true;
}