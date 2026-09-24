using AncientsAwakened.AncientsAwakenedCode.Relics.Gaster;
using AncientsAwakened.AncientsAwakenedCode.UI;
using HarmonyLib;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Runs;

namespace AncientsAwakened.AncientsAwakenedCode.Patches;

public static class ActModelPatch
{
    [HarmonyPatch(typeof(ActModel), nameof(ActModel.GetNumberOfRooms))]
    public class FilterForCombatPatch
    {
        public static int Postfix(int __result)
        {
            var relicCount = RunManager.Instance.State?.Players.Sum(player => player.Relics.Count(r => r is DarkSpark));
            if (relicCount is <= 0 or null)
                return __result;
            if (!AncientConfigs.UncapDarkSpark)
                relicCount = int.Min((int) relicCount, 9);
            return __result + (int) relicCount * DarkSpark.AdditionalFloors;
        }
    }
}