using HarmonyLib;
using MegaCrit.Sts2.Core.Nodes.Potions;

namespace AncientsAwakened.AncientsAwakenedCode.Patches;

public class NPotionPopupPatches
{
    [HarmonyPatch(typeof(NPotionPopup), "RefreshButtons")]
    [HarmonyPatch(typeof(NPotionPopup), "_Ready")]
    public class SaltyPotions
    {
        public static void Postfix(NPotionPopup __instance)
        {
            if (__instance.Potion is IDisablePotionDiscard)
            {
                __instance._discardButton.Disable();
            }
        }
    }
    
    internal interface IDisablePotionDiscard;
}