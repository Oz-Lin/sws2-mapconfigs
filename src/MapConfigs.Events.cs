using Microsoft.Extensions.Logging;
using SwiftlyS2.Shared.Events;

namespace MapConfigs;

public partial class MapConfigs
{
  private void OnMapLoad(IOnMapLoadEvent ev)
  {
    // Log map load event
    _logger.LogInformation("Map loaded: {MapName}", ev.MapName);
    EnsureConfigFolderExists();
    _scheduler.NextTick(() =>
    {
      // Apply config at next tick to ensure configs are applying correctly
      ApplyMapConfig(ev.MapName);
    });
  }
}
