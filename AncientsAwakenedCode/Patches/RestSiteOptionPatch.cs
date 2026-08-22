using AncientsAwakened.AncientsAwakenedCode.RestSiteOptions;
using HarmonyLib;
using MegaCrit.Sts2.Core.Entities.RestSite;
using MegaCrit.Sts2.Core.Localization;

namespace AncientsAwakened.AncientsAwakenedCode.Patches;

public class RestSiteOptionPatch
{
    [HarmonyPatch(typeof(RestSiteOption), nameof(RestSiteOption.IconPath), MethodType.Getter)]
    public class CustomRestSiteIcon
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
    
    [HarmonyPatch(typeof(RestSiteOption), nameof(RestSiteOption.Title), MethodType.Getter)]
    public class CustomRestSiteTitle
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
