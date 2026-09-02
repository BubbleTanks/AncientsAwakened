using AncientsAwakened.AncientsAwakenedCode.Extensions;
using BaseLib.Abstracts;
using BaseLib.Extensions;
using Godot;
using HarmonyLib;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Models;

namespace AncientsAwakened.AncientsAwakenedCode.Ancients;

public abstract class AncientsAwakenedAncient : CustomAncientModel
{
    public override string? CustomScenePath    
    {
        get
        {
            var path = $"{Id.Entry.RemovePrefix().ToLowerInvariant()}.tscn".AncientScenePath();
            return ResourceLoader.Exists(path) ? path : null;
        }
    }

    public override string CustomMapIconPath
    {
        get
        {
            var path = $"{Id.Entry.RemovePrefix().ToLowerInvariant()}.png".AncientMapIconImagePath();
            return ResourceLoader.Exists(path) ? path : "placeholder.png".AncientMapIconImagePath();
        }
    }

    public override string CustomMapIconOutlinePath
    {
        get
        {
            var path = $"{Id.Entry.RemovePrefix().ToLowerInvariant()}_outline.png".AncientMapIconImagePath();
            return ResourceLoader.Exists(path) ? path : "placeholder_outline.png".AncientMapIconImagePath();
        }
    }

    public override string CustomRunHistoryIconPath
    {
        get
        {
            var path = $"{Id.Entry.RemovePrefix().ToLowerInvariant()}.png".AncientRunHistoryIconImagePath();
            return ResourceLoader.Exists(path) ? path : "placeholder.png".AncientRunHistoryIconImagePath();
        }
    }

    public override string CustomRunHistoryIconOutlinePath
    {
        get
        {
            var path = $"{Id.Entry.RemovePrefix().ToLowerInvariant()}_outline.png".AncientRunHistoryIconImagePath();
            return ResourceLoader.Exists(path) ? path : "placeholder_outline.png".AncientRunHistoryIconImagePath();
        }
    }
    
    [HarmonyPatch(typeof(AncientEventModel), nameof(RunHistoryIconOutlinePath), MethodType.Getter)]
    public static class CustomRunHistoryIconOutlinePatch
    {
        [HarmonyPrefix]
        public static bool UseAltTexture(AncientEventModel __instance, ref string __result)
        {
            if (__instance is not AncientsAwakenedAncient ancientsAwakenedAncient)
                return true;
            __result = ancientsAwakenedAncient.CustomRunHistoryIconOutlinePath;
            return false;
        }
    }
    
    [HarmonyPatch(typeof(AncientEventModel), nameof(RunHistoryIcon), MethodType.Getter)]
    public static class CustomRunHistoryIconPatch
    {
        [HarmonyPrefix]
        public static bool UseAltTexture(AncientEventModel __instance, ref Texture2D __result)
        {
            if (__instance is not AncientsAwakenedAncient ancientsAwakenedAncient)
                return true;
            __result = PreloadManager.Cache.GetCompressedTexture2D(ancientsAwakenedAncient.CustomRunHistoryIconPath);
            return false;
        }
    }
}