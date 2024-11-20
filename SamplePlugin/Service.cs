using Dalamud.IoC;
using Dalamud.Plugin.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuietDen
{
    public class Service
    {
        [PluginService] public static IDataManager Data { get; private set; }
        [PluginService] public static IChatGui ChatGui { get; private set; }
        [PluginService] public static ICommandManager Commands { get; private set; }
        [PluginService] public static IPluginLog PluginLog { get; private set; }
        [PluginService] public static ICondition Condition { get; private set; }
        [PluginService] public static IGameConfig GameConfig { get; private set; }
        [PluginService] public static IClientState ClientState { get; private set; }
    }
}
