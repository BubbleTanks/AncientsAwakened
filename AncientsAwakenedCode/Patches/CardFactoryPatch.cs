using HarmonyLib;
using MegaCrit.Sts2.Core.Entities.Cards;
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
            
            IEnumerable<CardModel> card = __result.Where(c => c.Rarity != CardRarity.Token);
            return card;

        }
    }
    
}