using BaseLib.Utils;
using HarmonyLib;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Saves.Runs;

namespace AncientsAwakened.AncientsAwakenedCode.Patches;

public class SaltyPatch
{
    [SavedProperty]
    public static readonly SpireField<PotionModel, bool> SaltyField = new(() => false);

    /* FUCKED UP CODE THAT DOESNT WORK PLEASE DONT LOOK AT THIS IM SO SORRY
    [HarmonyPatch(typeof(PotionModel), "set_Title")]
    public class SaltyPotions
    {
        public static LocString Postfix(PotionModel __instance, LocString __result)
        {
            if (SaltyField.Get(__instance))
            {
                __result.get
            }
            return __result;
        }
    } */
}