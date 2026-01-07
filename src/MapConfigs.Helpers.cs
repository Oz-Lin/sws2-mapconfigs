using Microsoft.Extensions.Logging;

namespace MapConfigs;

public partial class MapConfigs
{
  private void EnsureConfigFolderExists()
  {
    try
    {
      EnsureFolderExists(_mapConfigsFolderPath);
      EnsureFolderExists(_prefixesFolderPath);
      EnsureFolderExists(_forcedFolderPath);
    }
    catch (Exception ex)
    {
      _logger.LogError(ex, "Failed to create or access configuration folders under: {FolderPath}", _mapConfigsFolderPath);
    }
  }

  private void EnsureFolderExists(string folderPath)
  {
    _logger.LogInformation("Checking if folder exists: {FolderPath}", folderPath);

    if (!Directory.Exists(folderPath))
    {
      _logger.LogWarning("Missing folder: {FolderPath}. Don't worry, creating it now.", folderPath);
      Directory.CreateDirectory(folderPath);
      _logger.LogInformation("Successfully created folder: {FolderPath}", folderPath);
    }
    else
    {
      _logger.LogInformation("Folder exists: {FolderPath}", folderPath);
    }
  }

  private void ApplyMapConfigsOnMapLoad(string mapName)
  {
    try
    {
      ExecutePrefixConfigurationFile(mapName);
      ExecuteSpecificMapConfigurationFile(mapName);
    }
    catch (Exception ex)
    {
      _logger.LogError(ex, "Error while applying map-load configuration for: {MapName}", mapName);
    }
  }

  private void ExecutePrefixConfigurationFile(string mapName)
  {
    var prefix = GetMapPrefix(mapName);
    if (string.IsNullOrWhiteSpace(prefix))
      return;

    var prefixCfgPath = $"mapconfigs/prefixes/{prefix}.cfg";
    ExecIfExists(prefixCfgPath);
  }

  private void ExecuteSpecificMapConfigurationFile(string mapName)
  {
    var mapCfgPath = $"mapconfigs/{mapName}.cfg";
    ExecIfExists(mapCfgPath);
  }

  private void ExecuteForcedMapConfigurationFile(string mapName)
  {
    var forcedCfgPath = $"mapconfigs/forced/{mapName}.cfg";
    ExecIfExists(forcedCfgPath);
  }

  private string GetMapPrefix(string mapName)
  {
    if (string.IsNullOrWhiteSpace(mapName))
      return string.Empty;

    var idx = mapName.IndexOf('_');
    if (idx <= 0)
      return string.Empty;

    return mapName[..(idx + 1)];
  }

  private void ExecIfExists(string cfgRelativeToCfgFolder)
  {
    var normalized = cfgRelativeToCfgFolder.Replace('\\', '/').TrimStart('/');

    var diskPath = Path.Combine(Core.CSGODirectory, "cfg", normalized.Replace('/', Path.DirectorySeparatorChar));

    if (!File.Exists(diskPath))
    {
      _logger.LogDebug("Config not found (skipping): {Cfg}", normalized);
      return;
    }

    var command = $"exec {normalized}";
    _logger.LogInformation("Executing => {Command}", command);
    _engine.ExecuteCommand(command);
  }
}
