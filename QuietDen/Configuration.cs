using Dalamud.Configuration;
using System;

namespace QuietDen;

[Serializable]
public class Configuration : IPluginConfiguration
{
    public int Version { get; set; } = 1;

    public bool UnmuteOnZoneOut { get; set; } = true;

    public void Save()
    {
        Plugin.PluginInterface.SavePluginConfig(this);
    }
}
