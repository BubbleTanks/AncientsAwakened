using AncientsAwakened.AncientsAwakenedCode.Extensions;
using BaseLib.Abstracts;
using BaseLib.Extensions;
using Godot;
using HarmonyLib;
using MegaCrit.Sts2.Core.Models;

namespace AncientsAwakened.AncientsAwakenedCode.Affliction;

public class AncientsAwakenedAffliction : AfflictionModel, ICustomModel
{
    private string? CustomOverlayPath     
    {
        get
        {
            AncientsAwakenedMain.Logger.Info(Id.Entry);
            var path = $"{Id.Entry.RemovePrefix().ToLowerInvariant()}.tscn".AfflictionScenePath();
            AncientsAwakenedMain.Logger.Info(path);
            return ResourceLoader.Exists(path) ? path : null;
        }
    }
    
    [HarmonyPatch(typeof(AfflictionModel), nameof(OverlayPath), MethodType.Getter)]
    public static class CustomRestSiteIconPatch
    {
        [HarmonyPrefix]
        public static bool UseAltTexture(AfflictionModel __instance, ref string __result)
        {
            if (__instance is not AncientsAwakenedAffliction customAffliction)
                return true;
            if (customAffliction.CustomOverlayPath != null)
                __result = customAffliction.CustomOverlayPath;
            return false;
        }
    }
}