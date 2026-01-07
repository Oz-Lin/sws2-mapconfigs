<div align="center">

# [SwiftlyS2] MapConfigs

[![GitHub Release](https://img.shields.io/github/v/release/criskkky/sws2-mapconfigs?color=FFFFFF&style=flat-square)](https://github.com/criskkky/sws2-mapconfigs/releases/latest)
[![GitHub Issues](https://img.shields.io/github/issues/criskkky/sws2-mapconfigs?color=FF0000&style=flat-square)](https://github.com/criskkky/sws2-mapconfigs/issues)
[![GitHub Downloads](https://img.shields.io/github/downloads/criskkky/sws2-mapconfigs/total?color=blue&style=flat-square)](https://github.com/criskkky/sws2-mapconfigs/releases)
[![GitHub Stars](https://img.shields.io/github/stars/criskkky/sws2-mapconfigs?style=social)](https://github.com/criskkky/sws2-mapconfigs/stargazers)<br/>
  <sub>Made with ❤️ by <a href="https://github.com/criskkky" rel="noopener noreferrer" target="_blank">criskkky</a> and modified by <a href="https://github.com/oz-lin" rel="noopener noreferrer" target="_blank">oz-lin</a></sub>
  <br/>
</div>

## Overview

Execute individualized configuration files for each map, or connect them all to a common prefix configuration.

## Download Shortcuts
<ul>
  <li>
    <code>📦</code>
    <strong>&nbspDownload Latest Plugin Version</strong> ⇢
    <a href="https://github.com/criskkky/sws2-mapconfigs/releases/latest" target="_blank" rel="noopener noreferrer">Click Here</a>
  </li>
  <li>
    <code>⚙️</code>
    <strong>&nbspDownload Latest SwiftlyS2 Version</strong> ⇢
    <a href="https://github.com/swiftly-solution/swiftlys2/releases/latest" target="_blank" rel="noopener noreferrer">Click Here</a>
  </li>
</ul>

## Features
- **Automatic Map Configuration**: Automatically applies specific configuration files when a map loads based on the map name.
- **Flexible Matching**: Supports partial name matching, allowing configurations for map prefixes or specific maps.
- **Folder Management**: Automatically creates the configuration folder if it doesn't exist.
- **Logging**: Provides detailed logging for map loading and configuration application.

## Screenshots
> No screenshots needed for this plugin.

## Plugin Setup
> [!WARNING]
> Make sure you **have installed SwiftlyS2 Framework** before proceeding.

1. Download and extract the latest plugin version into your `swiftlys2/plugins` folder.
2. Perform an initial run to allow folder generation.
3. The configuration folder will be created at: `game/csgo/cfg/MapConfigs`
4. Create your map-specific configuration files (e.g., `de_dust2.cfg` or `de_.cfg`) in this folder.
5. The plugin will automatically apply the matching configuration when the corresponding map loads.
6. Enjoy!

## Configuration Example

```cfg
// Path: /cfg/mapconfigs/prefix/de_.cfg
// File: de_.cfg
mp_freezetime 3
mp_autoteambalance 0
mp_autokick 0
exec otherconfigmaybe.cfg
```
```cfg
// Path: /cfg/mapconfigs/de_dust2.cfg
// File: de_dust2.cfg
// cfg with exact map name overrides the de_ prefix above
mp_freezetime 5
mp_autoteambalance 0
mp_autokick 0
bot_quota 10
exec otherconfigmaybe.cfg
```
```cfg
// Path: /cfg/mapconfigs/forced/de_why_this_map_keeps_changing_my_cvar.cfg
// File: de_why_this_map_keeps_changing_my_cvar.cfg
// executes every round start
mp_freezetime 5
mp_autoteambalance 0
mp_autokick 0
bot_quota 10
exec otherconfigmaybe.cfg
```

## Backend Logic (How It Works)
Rework to adapt https://github.com/SlashPlayGG/MapConfigurator 
1. Install the MapConfigurator plugin to enable per-map configs.
2. Map-specific config files live in csgo/cfg/mapconfigs/.
3. A map's config (mapname.cfg) is executed once the map is fully loaded and will overwrite values from server.cfg or gamemode configs.
4. Files for common maps are pre-created (empty) — add variables as needed.
5. To add a config for a new map: create cfg/mapconfigs/<mapname>.cfg; it loads next time the map is played.
6. For prefix-wide settings (e.g., de_): create cfg/mapconfigs/prefixes/de_.cfg.
7. For per-round forced overrides (use sparingly): create cfg/mapconfigs/forced/<mapname>.cfg — executed every round after freeze time.

## Support and Feedback
Feel free to [open an issue](https://github.com/criskkky/sws2-mapconfigs/issues/new/choose) for any bugs or feature requests. If it's all working fine, consider starring the repository to show your support!

## Contribution Guidelines
Contributions are welcome only if they align with the plugin's purpose. For major changes, please open an issue first to discuss what you would like to change.
