using AncientsAwakened.AncientsAwakenedCode.Cards.Mountain;
using AncientsAwakened.AncientsAwakenedCode.Enchantments;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.RelicPools;

namespace AncientsAwakened.AncientsAwakenedCode.Relics.Mountain;

[Pool(typeof(EventRelicPool))]
public class CardinalStar : AncientsAwakenedRelic
{
    public override RelicRarity Rarity => RelicRarity.Ancient;
    
    protected override IEnumerable<DynamicVar> CanonicalVars => [new CardsVar(3)];
    
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [..HoverTipFactory.FromEnchantment<Ordained>(), HoverTipFactory.FromCard<Stress>()];
    
    public override async Task AfterObtained()
    {
        await CardPileCmd.AddCurseToDeck<Stress>(Owner);
        await Cmd.Wait(0.75f);
        CardSelectorPrefs prefs = new CardSelectorPrefs(CardSelectorPrefs.EnchantSelectionPrompt, 0, DynamicVars.Cards.IntValue)
        {
            Cancelable = false,
            RequireManualConfirmation = true
        };
        Ordained canonicalEnchantment = ModelDb.Enchantment<Ordained>();
        foreach (CardModel card in await CardSelectCmd.FromDeckForEnchantment(Owner,  canonicalEnchantment, 1, prefs))
        {
            CardCmd.Enchant(canonicalEnchantment.ToMutable(), card, 1);
            CardCmd.Preview(card);
        }
    }
}