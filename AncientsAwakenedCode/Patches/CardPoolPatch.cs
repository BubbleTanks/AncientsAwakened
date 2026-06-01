using AncientsAwakened.AncientsAwakenedCode.Pools;
using AncientsAwakened.AncientsAwakenedCode.Pools.Mithrix;
using HarmonyLib;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes.Screens.CardLibrary;

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
    
    [HarmonyPatch(typeof(NCardLibrary), "_Ready")]
    public class RemovePerfectedFromMisc
    {
        public static void Postfix(NCardLibrary __instance)
        {
            var miscPoolFilter = __instance._poolFilters[__instance._miscPoolFilter];
            __instance._poolFilters[__instance._miscPoolFilter] = c =>
            {
                if (c.VisualCardPool is PerfectedPool) return false;
                return miscPoolFilter(c);
            };

        }
    }
}