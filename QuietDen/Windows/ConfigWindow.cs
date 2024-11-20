using System;
using System.Numerics;
using Dalamud.Interface.Utility;
using Dalamud.Interface.Windowing;
using ImGuiNET;

namespace SamplePlugin.Windows;

public class ConfigWindow : Window, IDisposable
{
    private Configuration Configuration;

    public ConfigWindow(Plugin plugin) : base("Settings###SettingsWindow")
    {

        Size = ImGuiHelpers.ScaledVector2(230, 90);
        SizeCondition = ImGuiCond.Once;

        Configuration = plugin.Configuration;
    }

    public void Dispose() { }

    public override void Draw()
    {
        var configValue = Configuration.UnmuteOnZoneOut;
        if (ImGui.Checkbox("Unmute on Zoning", ref configValue))
        {
            Configuration.UnmuteOnZoneOut = configValue;
            Configuration.Save();
        }

      
    }
}
