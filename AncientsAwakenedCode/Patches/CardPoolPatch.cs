using AncientsAwakened.AncientsAwakenedCode.Pools;
using HarmonyLib;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Badges;

namespace AncientsAwakened.AncientsAwakenedCode.Patches;

public class CardPoolPatch
{
    [HarmonyPatch(typeof(ModelDb), "get_AllSharedCardPools")]
    public class GetAllSharedCardPools
    {
        public static void Postfix(IEnumerable<CardPoolModel> __result)
        {

            __result.AddItem(ModelDb.CardPool<PerfectedPool>());

        }
    }
}