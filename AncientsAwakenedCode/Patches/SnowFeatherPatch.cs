using HarmonyLib;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Rewards;

namespace AncientsAwakened.AncientsAwakenedCode.Relics.Gaster;

//patch dedicated to this cuz I couldn't find a hook for card rewards being skipped. this should work for every instance of a card being skipped, including events, which is nice. 
[HarmonyPatch(typeof(CardReward), nameof(CardReward.OnSkipped))]
public static class SnowFeatherPatch
{
    public static void Postfix(CardReward __instance)
    {
        SnowFeather? feather = __instance.Player.GetRelic<SnowFeather>();
        if (feather == null || feather.WasDisabled)
            return;
        feather.WasDisabled = true;
    }
}