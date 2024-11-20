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
    [PluginService] internal static ITextureProvider TextureProvider { get; private set; } = null!;
    [PluginService] internal static ICommandManager CommandManager { get; private set; } = null!;

    private const string CommandName = "/quietden";

    public Configuration Configuration { get; init; }

    public readonly WindowSystem WindowSystem = new("SamplePlugin");
    private ConfigWindow ConfigWindow { get; init; }
    private MainWindow MainWindow { get; init; }

    public Plugin()
    {
        Configuration = PluginInterface.GetPluginConfig() as Configuration ?? new Configuration();

        // you might normally want to embed resources and load them from the manifest stream
        var goatImagePath = Path.Combine(PluginInterface.AssemblyLocation.Directory?.FullName!, "goat.png");

        ConfigWindow = new ConfigWindow(this);
        MainWindow = new MainWindow(this, goatImagePath);

        WindowSystem.AddWindow(ConfigWindow);
        WindowSystem.AddWindow(MainWindow);

        PluginInterface.Create<Service>();

        Service.ClientState.TerritoryChanged += CheckForTheDen;

        CommandManager.AddHandler(CommandName, new CommandInfo(OnCommand)
        {
            HelpMessage = "A useful message to display in /xlhelp"
        });

        PluginInterface.UiBuilder.Draw += DrawUI;

        // This adds a button to the plugin installer entry of this plugin which allows
        // to toggle the display status of the configuration ui
        PluginInterface.UiBuilder.OpenConfigUi += ToggleConfigUI;

        // Adds another button that is doing the same but for the main ui of the plugin
        PluginInterface.UiBuilder.OpenMainUi += ToggleMainUI;
    }

    private async void CheckForTheDen(ushort obj)
    {
        if (Service.ClientState.MapId.Equals(51))
        {

            Service.GameConfig.TryGet(Dalamud.Game.Config.SystemConfigOption.IsSndBgm, out uint IsSndBgmMuted);

            if (IsSndBgmMuted == 0)
            {
                var muteTask = LoadingDelay(2000, 1);
                await System.Threading.Tasks.Task.WhenAll(muteTask);
                Service.PluginLog.Debug($"Disabling Wolve's Den BGM to save yer ears");
            }
        }
        else
        {
            if (Configuration.UnmuteOnZoneOut)
            {

                var unmuteTask = LoadingDelay(4000, 0);
                await System.Threading.Tasks.Task.WhenAll(unmuteTask);
                Service.PluginLog.Debug($"Enabling BGM to bless yer ears");
            }

        }

     
    }
    private static async System.Threading.Tasks.Task LoadingDelay(int milliseconds, uint bgmStatus)
    {
        await System.Threading.Tasks.Task.Delay(milliseconds);
        Service.GameConfig.Set(Dalamud.Game.Config.SystemConfigOption.IsSndBgm, bgmStatus);
        Console.WriteLine($"Task completed after {milliseconds} milliseconds");
    }
    public void Dispose()
    {
        WindowSystem.RemoveAllWindows();

        ConfigWindow.Dispose();
        MainWindow.Dispose();

        CommandManager.RemoveHandler(CommandName);
    }

    private void OnCommand(string command, string args)
    {
        // in response to the slash command, just toggle the display status of our main ui
        ToggleMainUI();
    }

    private void DrawUI() => WindowSystem.Draw();

    public void ToggleConfigUI() => ConfigWindow.Toggle();
    public void ToggleMainUI() => MainWindow.Toggle();

}
