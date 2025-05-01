namespace Routya.ConfigKit.Generator;
/// <summary>
/// AddSingleton(config), services.Configure<T>(), AddSingleton + Configure
/// </summary>
internal enum ConfigBindingMode
{
    Singleton,
    IOptions,
    Both
}