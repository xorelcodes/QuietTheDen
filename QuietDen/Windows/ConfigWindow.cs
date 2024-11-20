using System;
using System.Numerics;
using Dalamud.Interface.Windowing;
using ImGuiNET;

namespace SamplePlugin.Windows;

public class ConfigWindow : Window, IDisposable
{
    private Configuration Configuration;

    public ConfigWindow(Plugin plugin) : base("Settings###SettingsWindow")
    {
        Flags = ImGuiWindowFlags.NoResize | ImGuiWindowFlags.NoCollapse | ImGuiWindowFlags.NoScrollbar |
                ImGuiWindowFlags.NoScrollWithMouse;

        Size = new Vector2(232, 90);
        SizeCondition = ImGuiCond.Always;

        Configuration = plugin.Configuration;
    }

    public void Dispose() { }

    public override void PreDraw()
    {
       
    }

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
