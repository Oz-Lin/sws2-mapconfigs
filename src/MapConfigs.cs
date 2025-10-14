using Microsoft.Extensions.DependencyInjection;
using SwiftlyS2.Shared.Plugins;
using SwiftlyS2.Shared;

namespace MapConfigs;

public partial class MapConfigs : BasePlugin
{
  public MapConfigs(ISwiftlyCore core) : base(core)
  {
  }

  private IServiceProvider? _provider;

  public override void Load(bool hotReload)
  {
    var collection = new ServiceCollection()
      .AddSwiftly(Core)
      .AddSingleton<MapConfigsListener>();

    _provider = collection.BuildServiceProvider();
    _provider.GetRequiredService<MapConfigsListener>();
  }

  public override void Unload()
  {
    if (_provider is not null && _provider is IDisposable disposable)
    {
      disposable.Dispose();
      _provider = null;
    }
  }
}
