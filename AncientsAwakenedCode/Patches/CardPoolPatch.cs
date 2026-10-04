using AncientsAwakened.AncientsAwakenedCode.Pools.Gaster;
using AncientsAwakened.AncientsAwakenedCode.Pools.Mithrix;
using HarmonyLib;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes.Screens.CardLibrary;

namespace AncientsAwakened.AncientsAwakenedCode.Patches;

public static class CardPoolPatch
{
    internal static readonly List<CardPoolModel> SpecialTokenPools = 
    [
        ModelDb.CardPool<PerfectedPool>(), 
        ModelDb.CardPool<ShadowPool>()
    ];
    
    
    [HarmonyPatch(typeof(ModelDb), "get_AllSharedCardPools")]
    public class GetAllSharedCardPools
    {
        public static IEnumerable<CardPoolModel> Postfix(IEnumerable<CardPoolModel> __result)
        {
            var cardPoolModels = __result.ToList();
            foreach (var cardPoolModel in SpecialTokenPools)
            {
                cardPoolModels.AddItem(cardPoolModel);
            }
            return cardPoolModels;
        }
    }
    
    [HarmonyPatch(typeof(NCardLibrary), "_Ready")]
    public class RemovePerfectedFromMisc
    {
        public static void Postfix(NCardLibrary __instance)
        {
            var miscPoolFilter = __instance._poolFilters[__instance._miscPoolFilter];
            __instance._poolFilters[__instance._miscPoolFilter] = c => !SpecialTokenPools.Contains(c.VisualCardPool) && miscPoolFilter(c);
        }
    }
}