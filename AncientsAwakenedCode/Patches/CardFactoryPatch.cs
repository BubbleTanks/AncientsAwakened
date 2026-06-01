using AncientsAwakened.AncientsAwakenedCode.Pools;
using AncientsAwakened.AncientsAwakenedCode.Pools.Mithrix;
using HarmonyLib;
using MegaCrit.Sts2.Core.Factories;
using MegaCrit.Sts2.Core.Models;

namespace AncientsAwakened.AncientsAwakenedCode.Patches;

public class CardFactoryPatch
{

    [HarmonyPatch(typeof(CardFactory), "FilterForCombat")]
    public class FilterForCombatPatch
    {
        public static IEnumerable<CardModel> Postfix(IEnumerable<CardModel> __result)
        {
            
            IEnumerable<CardModel> card = __result.Where(c => c.VisualCardPool is not PerfectedPool);
            return card;

        }
    }
    
}