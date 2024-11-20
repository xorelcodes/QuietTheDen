using System;
using Dalamud.Interface.Utility;
using Dalamud.Interface.Windowing;
using ImGuiNET;

namespace QuietDen.Windows;

public class ConfigWindow : Window, IDisposable
{
    private readonly Configuration configuration;

    public ConfigWindow(Plugin plugin) : base("Settings###SettingsWindow")
    {

        Size = ImGuiHelpers.ScaledVector2(230, 90);
        SizeCondition = ImGuiCond.Once;

        configuration = plugin.Configuration;
    }

    public void Dispose() {
        GC.SuppressFinalize(this);
            }

    public override void Draw()
    {
        var configValue = configuration.UnmuteOnZoneOut;
        if (ImGui.Checkbox("Unmute on Zoning", ref configValue))
        {
            configuration.UnmuteOnZoneOut = configValue;
            configuration.Save();
        }

      
    }
}
