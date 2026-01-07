using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SwiftlyS2.Shared.Plugins;
using SwiftlyS2.Shared;
using SwiftlyS2.Shared.Scheduler;
using SwiftlyS2.Shared.Services;

namespace MapConfigs;

public partial class MapConfigs(ISwiftlyCore core) : BasePlugin(core)
{
  private ILogger<MapConfigs> _logger = null!;
  private IEngineService _engine = null!;
  private ISchedulerService _scheduler = null!;

  private string _mapConfigsFolderPath = string.Empty;
  private string _prefixesFolderPath = string.Empty;
  private string _forcedFolderPath = string.Empty;

  public override void Load(bool hotReload)
  {
    // Create logger using LoggerFactory
    _logger = Core.LoggerFactory.CreateLogger<MapConfigs>();

    // Get services from DI container
    var services = new ServiceCollection()
      .AddSwiftly(Core)
      .BuildServiceProvider();

    _engine = services.GetRequiredService<IEngineService>();
    _scheduler = services.GetRequiredService<ISchedulerService>();

    // Folder layout (similar to classic MapConfigurator):
    // csgo/cfg/mapconfigs/
    // csgo/cfg/mapconfigs/prefixes/
    // csgo/cfg/mapconfigs/forced/
    _mapConfigsFolderPath = Path.Combine(Core.CSGODirectory, "cfg", "mapconfigs");
    _prefixesFolderPath = Path.Combine(_mapConfigsFolderPath, "prefixes");
    _forcedFolderPath = Path.Combine(_mapConfigsFolderPath, "forced");

    _logger.LogInformation("MapConfigs folder path: {FolderPath}", _mapConfigsFolderPath);

    // Subscribe to map load event
    Core.Event.OnMapLoad += OnMapLoad;
  }

  public override void Unload()
  {
    // Unsubscribe from events to prevent memory leaks
    Core.Event.OnMapLoad -= OnMapLoad;
  }
}
