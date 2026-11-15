using BaseLib.Utils;
using HarmonyLib;
using MegaCrit.Sts2.Core.Models;

namespace AncientsAwakened.AncientsAwakenedCode.Patches;

public static class ManaFlowerPatch
{
    public static readonly SpireField<PotionModel, bool> ManaFlowerPotionField = new(() => false);
    
    [HarmonyPatch(typeof(PotionModel), nameof(PotionModel.RemoveBeforeUse))]
    public class RemovePotionInternalPatch
    {
        public static bool Prefix(PotionModel __instance)
        {
            return !ManaFlowerPotionField.Get(__instance);
        }
    }
}