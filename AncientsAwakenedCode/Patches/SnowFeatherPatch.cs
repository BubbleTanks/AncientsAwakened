using AncientsAwakened.AncientsAwakenedCode.Relics.Gaster;
using BaseLib.Audio;
using HarmonyLib;
using MegaCrit.Sts2.Core.Rewards;

namespace AncientsAwakened.AncientsAwakenedCode.Patches;

//patch dedicated to this cuz I couldn't find a hook for card rewards being skipped. this should work for every instance of a card being skipped, including events, which is nice. 
[HarmonyPatch(typeof(CardReward), nameof(CardReward.OnSkipped))]
public static class SnowFeatherPatch
{
    private static readonly ModSound CancelSound = new("res://AncientsAwakened/audio/weird-route-cancel.mp3");
    
    public static void Postfix(CardReward __instance)
    {
        var feather = __instance.Player.GetRelic<SnowFeather>();
        if (feather == null || feather.WasDisabled)
            return;
        feather.WasDisabled = true;
        // audio has to be played here so that it doesn't replay when loading the run.
        ModAudio.PlaySoundInRun(CancelSound);
    }
}