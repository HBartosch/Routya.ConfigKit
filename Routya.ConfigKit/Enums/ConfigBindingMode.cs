namespace Routya.ConfigKit;
/// <summary>
/// AddSingleton(config), services.Configure<T>(), AddSingleton + Configure
/// </summary>
public enum ConfigBindingMode
{
    Singleton,
    IOptions,
    Both
}