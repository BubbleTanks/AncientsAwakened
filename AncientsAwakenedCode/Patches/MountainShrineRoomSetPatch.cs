using HarmonyLib;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Rooms;

namespace AncientsAwakened.AncientsAwakenedCode.Patches;

public static class MountainShrineRoomSetPatch
{
    /*[HarmonyPatch(typeof(RoomSet), nameof(RoomSet.NextBossEncounter), MethodType.Getter)]
    public class NextBossEncounterPatch
    {
        public static EncounterModel Postfix(RoomSet __instance, EncounterModel __result)
        {
            if(__instance.bossEncountersVisited <= 2)
                return __result;
            
        }
    }*/
}