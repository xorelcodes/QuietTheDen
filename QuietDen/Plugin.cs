using Dalamud.Game.Command;
using Dalamud.IoC;
using Dalamud.Plugin;
using System.IO;
using Dalamud.Interface.Windowing;
using Dalamud.Plugin.Services;
using SamplePlugin.Windows;
using QuietDen;
using FFXIVClientStructs.FFXIV.Common.Lua;
using System;
using FFXIVClientStructs.FFXIV.Client.System.Threading;
using System.Threading;
using FFXIVClientStructs.FFXIV.Client.System.Framework;
using System.Threading.Tasks;

namespace SamplePlugin;

public sealed class Plugin : IDalamudPlugin
{
    [PluginService] internal static IDalamudPluginInterface PluginInterface { get; private set; } = null!;
    [PluginService] internal static ICommandManager CommandManager { get; private set; } = null!;
    [PluginService] internal static IGameConfig GameConfig { get; private set; } = null!;
    [PluginService] internal static IClientState ClientState { get; private set; } = null!;
    [PluginService] public static IPluginLog PluginLog { get; private set; } = null!;

    private const string CommandName = "/quietden";

    public Configuration Configuration { get; init; }

    public readonly WindowSystem WindowSystem = new("QuietTheDen");
    private ConfigWindow ConfigWindow { get; init; }

    public Plugin()
    {
        Configuration = PluginInterface.GetPluginConfig() as Configuration ?? new Configuration();


        ConfigWindow = new ConfigWindow(this);

        WindowSystem.AddWindow(ConfigWindow);

        PluginInterface.Create<Service>();

        ClientState.TerritoryChanged += CheckForTheDen;

        CommandManager.AddHandler(CommandName, new CommandInfo(OnCommand)
        {
            HelpMessage = "Opens plugin menu"
        });

        PluginInterface.UiBuilder.Draw += DrawUI;


        PluginInterface.UiBuilder.OpenConfigUi += ToggleConfigUI;
        PluginInterface.UiBuilder.OpenMainUi += ToggleConfigUI;

    }

    private async void CheckForTheDen(ushort obj)
    {
        if (ClientState.MapId.Equals(51))
        {

            GameConfig.TryGet(Dalamud.Game.Config.SystemConfigOption.IsSndBgm, out uint IsSndBgmMuted);

            if (IsSndBgmMuted == 0)
            {
                var muteTask = LoadingDelay(2000, 1);
                await System.Threading.Tasks.Task.WhenAll(muteTask);
                PluginLog.Debug($"Disabling Wolves' Den BGM to save yer ears");
            }
        }
        else
        {
            if (Configuration.UnmuteOnZoneOut)
            {

                var unmuteTask = LoadingDelay(4000, 0);
                await System.Threading.Tasks.Task.WhenAll(unmuteTask);
                PluginLog.Debug($"Enabling BGM to bless yer ears");
            }

        }

     
    }
    private static async System.Threading.Tasks.Task LoadingDelay(int milliseconds, uint bgmStatus)
    {
        await System.Threading.Tasks.Task.Delay(milliseconds);
        Service.GameConfig.Set(Dalamud.Game.Config.SystemConfigOption.IsSndBgm, bgmStatus);
    }
    public void Dispose()
    {
        WindowSystem.RemoveAllWindows();

        ConfigWindow.Dispose();

        CommandManager.RemoveHandler(CommandName);
    }

    private void OnCommand(string command, string args)
    {
        ToggleConfigUI();
    }

    private void DrawUI() => WindowSystem.Draw();

    public void ToggleConfigUI() => ConfigWindow.Toggle();

}
