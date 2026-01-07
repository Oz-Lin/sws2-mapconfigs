using Microsoft.Extensions.Logging;
using SwiftlyS2.Shared.Events;
using SwiftlyS2.Shared;
using SwiftlyS2.Shared.GameEventDefinitions;
using SwiftlyS2.Shared.Misc;

namespace MapConfigs;

public partial class MapConfigs
{
  private const int MapLoadDelaySeconds = 15;

  private void OnMapLoad(IOnMapLoadEvent ev)
  {
    // Log map load event
    _logger.LogInformation("Map loaded: {MapName}", ev.MapName);

    EnsureConfigFolderExists();

    // Run after a short delay to better match 'map fully loaded' behavior.
    RunAfterSeconds(MapLoadDelaySeconds, () => ApplyMapConfigsOnMapLoad(ev.MapName));
  }

  private void RunAfterSeconds(int seconds, Action action)
  {
    if (seconds <= 0)
    {
      _scheduler.NextTick(action);
      return;
    }

    var remainingTicks = seconds * 64; // approx. 64 ticks/s

    void Tick()
    {
      remainingTicks--;
      if (remainingTicks <= 0)
      {
        action();
        return;
      }

      _scheduler.NextTick(Tick);
    }

    _scheduler.NextTick(Tick);
  }

  private HookResult OnRoundFreezeEnd(EventRoundFreezeEnd ev)
  {
    try
    {
      ExecuteForcedMapConfigurationFile();
    }
    catch (Exception ex)
    {
            _logger.LogError(ex, "Error while applying forced configuration");// for: {MapName}", mapName);
    }

    return HookResult.Continue;
  }
}
