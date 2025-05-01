# ⚙️ Routya.ConfigKit.Generator

> 🔧 A lightweight, source-generated configuration binder for .NET — just tag your class with `[ConfigSection]` and go.

![CI](https://img.shields.io/github/actions/workflow/status/hbartosch/routya.configkit/dotnet-routya-configkit-generator.yml?label=CI&style=flat-square)
![CI](https://img.shields.io/github/actions/workflow/status/hbartosch/routya.configkit/build-and-test.yml?label=Build&style=flat-square)
[![NuGet](https://img.shields.io/nuget/v/Routya.ConfigKit.Generator)](https://www.nuget.org/packages/Routya.ConfigKit.Generator)
[![NuGet](https://img.shields.io/nuget/dt/Routya.ConfigKit.Generator)](https://www.nuget.org/packages/Routya.ConfigKit.Generator)

---

## ✨ Features

- ✅ Compile-time generation of config binding and registration code  
- ✅ Supports both `IOptions<T>` and `AddSingleton<T>` modes  
- ✅ Full support for System.ComponentModel.Annotations
- ✅ No reflection at runtime
- ✅ Drop-in integration with `appsettings.json`
- ✅ Currently only supports {get; set;} and {get; init;}. 
---

## 🚀 Getting Started

🛠 Binding Modes
| Mode        | Behavior                              |
|-------------|----------------------------------------|
| `Singleton` | Registers the instance via `AddSingleton<T>` |
| `IOptions`  | Uses `services.Configure<T>()`         |
| `Both`      | Adds both for flexibility              |

### 1. Install package

```bash
dotnet add package Routya.ConfigKit.Generator
```

### 2. Create your config class

ConfigSection("MyService", ConfigBindingMode.IOptions)

The 'MyService' in ConfigSection is the section name within your configuration (eg. appsettings.json, Azure App Configuration)

```json
{
  "MyService": {
    "RetryCount": 3,
    "UseCaching": false
  }
}
```

ConfigBindingMode.IOptions

```C#
using System.ComponentModel.DataAnnotations;
using Routya.ConfigKit;

[ConfigSection("MyService", mode: ConfigBindingMode.IOptions)]
public partial class MyServiceOptions
{
    [Required]
    public int RetryCount { get; init; }

    public bool UseCaching { get; init; } = true;
}
```

Generates
```C#
public static IServiceCollection AddMyServiceOptions(this IServiceCollection services, IConfiguration configuration)
{
	services.AddOptions<MyServiceOptions>()
			.Bind(configuration.GetSection("MyService"))
			.ValidateDataAnnotations()
			.ValidateOnStart();

	return services;
}
```

ConfigBindingMode.Singleton
```C#
using System.ComponentModel.DataAnnotations;
using Routya.ConfigKit;

[ConfigSection("MyService", ConfigBindingMode.Singleton)]
public partial class MyServiceOptions
{
    [Required]
    public int RetryCount { get; init; }

    public bool UseCaching { get; init; } = true;
}
```

Generates
```C#
 public static IServiceCollection AddMyServiceOptions(this IServiceCollection services, IConfiguration configuration)
 {
     var options = new MyServiceOptions()
     {
         RetryCount = configuration.GetValue<int>("MyService:RetryCount"),
         UseCaching = configuration.GetValue<bool>("MyService:UseCaching"),
     };

     var validationContext = new ValidationContext(options);
     Validator.ValidateObject(options, validationContext, validateAllProperties: true);

     services.AddSingleton(options);

     return services;
 }
```

### 3. Register the generated method in your startup
```C#
builder.Services.AddMyServiceOptions(builder.Configuration);
```
## 📅 Roadmap
- Add support for complex/nested config objects
- {get; private set;}

## 🔍 More from this author

### 🧰 [Routya](https://github.com/HBartosch/Routya)  
A high-performance, minimal-overhead CQRS + MediatR alternative for .NET applications. Supports request/notification dispatching, behavior pipelines, scoped resolution, and performance-optimized dispatchers.

### 📦 [Routya.ResultKit](https://github.com/HBartosch/Routya.ResultKit)  
A companion library for consistent API response modeling. Wraps results with success/failure metadata, integrates with ProblemDetails, and streamlines controller return types.
