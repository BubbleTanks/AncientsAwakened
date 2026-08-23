using System.Reflection;
using AncientsAwakened.AncientsAwakenedCode.UI;
using BaseLib.Config;
using Godot;
using HarmonyLib;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Modding;
using Logger = MegaCrit.Sts2.Core.Logging.Logger;

namespace AncientsAwakened.AncientsAwakenedCode;

[ModInitializer(nameof(Initialize))]
public partial class AncientsAwakenedMain : Node
{
    public const string ModId = "AncientsAwakened"; //Used for resource filepath
    public const string ResPath = $"res://{ModId}";

    public static Logger Logger { get; } =
        new(ModId, LogType.Generic);
    
    public static void Initialize()
    {
        Godot.Bridge.ScriptManagerBridge.LookupScriptsInAssembly(Assembly.GetExecutingAssembly());
        Harmony harmony = new(ModId);
        ModConfigRegistry.Register(ModId, new AncientConfigs());
        harmony.PatchAll(Assembly.GetExecutingAssembly());
    }
}