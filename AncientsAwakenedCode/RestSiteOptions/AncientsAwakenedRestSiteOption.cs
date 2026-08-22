using AncientsAwakened.AncientsAwakenedCode.Extensions;
using BaseLib.Abstracts;
using BaseLib.Extensions;
using Godot;
using HarmonyLib;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.RestSite;
using MegaCrit.Sts2.Core.Localization;

namespace AncientsAwakened.AncientsAwakenedCode.RestSiteOptions;

public abstract class AncientsAwakenedRestSiteOption(Player owner) : RestSiteOption(owner), ICustomModel
{
    public override string OptionId => $"{AncientsAwakenedMain.ModId.ToUpperInvariant()}-{GetType().Name.ToSnakeCase().ToUpperInvariant()}";
    
    public override LocString Description => new("rest_site_ui", $"{OptionId}.description");
    
    protected virtual LocString CustomTitle => new("rest_site_ui", $"{OptionId}.name");

    protected virtual string CustomIconPath
    {
        get
        {
            var path = $"{OptionId.RemovePrefix().ToLowerInvariant()}.png".RestSiteImagePath();
            return ResourceLoader.Exists(path) ? path : "placeholder.png".RestSiteImagePath();
        }
    }
    
    [HarmonyPatch(typeof(RestSiteOption), nameof(IconPath), MethodType.Getter)]
    public static class CustomRestSiteIconPatch
    {
        [HarmonyPrefix]
        public static bool UseAltTexture(RestSiteOption __instance, ref string __result)
        {
            if (__instance is not AncientsAwakenedRestSiteOption customRestSiteOption)
                return true;
            if (customRestSiteOption.CustomIconPath != null)
                __result = customRestSiteOption.CustomIconPath;
            return false;
        }
    }
    
    [HarmonyPatch(typeof(RestSiteOption), nameof(Title), MethodType.Getter)]
    public static class CustomRestSiteTitlePatch
    {
        [HarmonyPrefix]
        public static bool UseAltTexture(RestSiteOption __instance, ref LocString __result)
        {
            if (__instance is not AncientsAwakenedRestSiteOption customRestSiteOption)
                return true;
            if (customRestSiteOption.CustomTitle != null)
                __result = customRestSiteOption.CustomTitle;
            return false;
        }
    }
}