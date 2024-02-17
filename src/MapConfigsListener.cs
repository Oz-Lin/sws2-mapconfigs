using Microsoft.Extensions.Logging;
using SwiftlyS2.Shared;
using SwiftlyS2.Shared.Events;
using SwiftlyS2.Shared.Scheduler;
using SwiftlyS2.Shared.Services;

namespace MapConfigs;

public class MapConfigsListener
{
  private readonly ILogger<MapConfigsListener> _logger;
  private readonly IEngineService _engine;
  private readonly ISchedulerService _scheduler;
  private readonly string _configFolderPath;

  public MapConfigsListener(ISwiftlyCore core, ILogger<MapConfigsListener> logger, IEngineService engine, ISchedulerService scheduler)
  {
    _logger = logger;
    _engine = engine;
    _scheduler = scheduler;

    // Determine CFGs folder path once at initialization
    string currentDirectory = Directory.GetCurrentDirectory();
    string csgoPath = currentDirectory.Replace(@"game\bin\win64", @"game\csgo");
    _configFolderPath = Path.Combine(csgoPath, "cfg", "MapConfigs");

    // Subscribe to map load event
    core.Event.OnMapLoad += OnMapLoad;
  }

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

  private void EnsureConfigFolderExists()
  {
    _logger.LogInformation("Checking if folder exists: {FolderPath}", _configFolderPath);

    if (!Directory.Exists(_configFolderPath))
    {
      _logger.LogWarning("Missing folder: {FolderPath}. Don't worry, creating it now.", _configFolderPath);
      Directory.CreateDirectory(_configFolderPath);
    }
    else
    {
      _logger.LogInformation("Folder exists: {FolderPath}", _configFolderPath);
    }
  }

  private void ApplyMapConfig(string mapName)
  {
    // Get all .cfg files in the MapConfigs folder
    if (!Directory.Exists(_configFolderPath))
    {
      _logger.LogWarning("Configuration folder does not exist: {FolderPath}", _configFolderPath);
      return;
    }

    var configFiles = Directory.GetFiles(_configFolderPath, "*.cfg");
    
    if (configFiles.Length == 0)
    {
      _logger.LogWarning("✖\n✖\n✖\n✖\n✖");
      _logger.LogWarning("No configuration files found in {FolderPath}", _configFolderPath);
      _logger.LogWarning("✖\n✖\n✖\n✖\n✖");
      return;
    }

    // Sort files by filename length in descending order (longest first)
    var sortedConfigFiles = configFiles
      .OrderByDescending(file => Path.GetFileNameWithoutExtension(file).Length)
      .ToArray();

    // Search for the first config file whose name (without extension) is contained in the map name
    foreach (var configFile in sortedConfigFiles)
    {
      var fileName = Path.GetFileNameWithoutExtension(configFile);
      
      // Check if the map name contains the config file name
      if (mapName.Contains(fileName, StringComparison.OrdinalIgnoreCase))
      {
        _logger.LogInformation("Map detected => {MapName}", mapName);
        _logger.LogInformation("Found => {FileName}", fileName);
        _logger.LogInformation("Executing => MapConfigs/{FileName}.cfg", fileName);
        
        var execCommand = $"exec MapConfigs/{fileName}.cfg";
        
        _logger.LogInformation("✔\n✔\n✔\n✔\n✔");
        _logger.LogInformation("Executing command => {ExecCommand}", execCommand);
        _logger.LogInformation("✔\n✔\n✔\n✔\n✔");
        
        _engine.ExecuteCommand(execCommand);
        return;
      }
    }

    // No matching config file found
    _logger.LogWarning("✖\n✖\n✖\n✖\n✖");
    _logger.LogWarning("No configuration file found for this map: {MapName}", mapName);
    _logger.LogWarning("✖\n✖\n✖\n✖\n✖");
  }
}