using AncientsAwakened.AncientsAwakenedCode.Enchantments;
using AncientsAwakened.AncientsAwakenedCode.Relics;
using BaseLib.Utils;
using Godot;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Extensions;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.RelicPools;
using MegaCrit.Sts2.Core.Nodes;
using MegaCrit.Sts2.Core.Nodes.Vfx;
using MegaCrit.Sts2.Core.Runs;

namespace AncientsAwakened.AncientsAwakenedCode.Relics;



[Pool(typeof(EventRelicPool))]
public class SebbyCharm : AncientsAwakenedRelic
{
    public override RelicRarity Rarity => RelicRarity.Ancient;

    public override bool HasUponPickupEffect => true;

    protected override IEnumerable<DynamicVar> CanonicalVars => [new CardsVar(1), new StringVar("Enchantment", ModelDb.Enchantment<Charmed>().Title.GetFormattedText())];
    
    protected override IEnumerable<IHoverTip> ExtraHoverTips
    {
        get => HoverTipFactory.FromEnchantment<Charmed>();
    }

    public override bool IsAllowed(IRunState runState)
    {
        foreach (CardModel c in Owner.Deck.Cards)
        {
            if (c.Type == CardType.Power)
            {
                return true;
            }
        }

        return false;
    }

    public override async Task AfterObtained()
    {
        EnchantmentModel royalStamp = ModelDb.Enchantment<Charmed>();
        List<CardModel> list = PileType.Deck.GetPile(Owner).Cards.Where(c => royalStamp.CanEnchant(c)).ToList();
        CardSelectorPrefs prefs = new CardSelectorPrefs(CardSelectorPrefs.EnchantSelectionPrompt, 1);
        CardModel card = (await CardSelectCmd.FromDeckForEnchantment(list.UnstableShuffle(Owner.RunState.Rng.Niche).ToList(), royalStamp, 1, prefs)).FirstOrDefault();
        if (card == null)
            return;
        CardCmd.Enchant<Charmed>(card, 1M);
        NCardEnchantVfx child = NCardEnchantVfx.Create(card);
        if (child == null)
            return;
        NRun instance = NRun.Instance;
        if (instance == null)
            return;
        instance.GlobalUi.CardPreviewContainer.AddChildSafely(child);
    }
    
}