using Microsoft.Extensions.Logging;

namespace MapConfigs;

public partial class MapConfigs
{
  private void EnsureConfigFolderExists()
  {
    try
    {
      _logger.LogInformation("Checking if folder exists: {FolderPath}", _configFolderPath);

      if (!Directory.Exists(_configFolderPath))
      {
        _logger.LogWarning("Missing folder: {FolderPath}. Don't worry, creating it now.", _configFolderPath);
        Directory.CreateDirectory(_configFolderPath);
        _logger.LogInformation("Successfully created folder: {FolderPath}", _configFolderPath);
      }
      else
      {
        _logger.LogInformation("Folder exists: {FolderPath}", _configFolderPath);
      }
    }
    catch (Exception ex)
    {
      _logger.LogError(ex, "Failed to create or access configuration folder: {FolderPath}", _configFolderPath);
    }
  }

  private void ApplyMapConfig(string mapName)
  {
    try
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
        _logger.LogWarning("No configuration files found in {FolderPath}", _configFolderPath);
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
        
          _logger.LogInformation("Executing command => {ExecCommand}", execCommand);
        
          _engine.ExecuteCommand(execCommand);
          return;
        }
      }

      // No matching config file found
      _logger.LogWarning("No configuration file found for this map: {MapName}", mapName);
    }
    catch (Exception ex)
    {
      _logger.LogError(ex, "Error while applying map configuration for: {MapName}", mapName);
    }
  }
}
