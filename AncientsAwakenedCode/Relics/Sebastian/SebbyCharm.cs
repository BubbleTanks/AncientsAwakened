using AncientsAwakened.AncientsAwakenedCode.Enchantments.Sebastian;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Extensions;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.RelicPools;
using MegaCrit.Sts2.Core.Nodes;
using MegaCrit.Sts2.Core.Nodes.Vfx;

namespace AncientsAwakened.AncientsAwakenedCode.Relics.Sebastian;

[Pool(typeof(EventRelicPool))]
public class SebbyCharm : AncientsAwakenedRelic
{
    public override RelicRarity Rarity => RelicRarity.Ancient;

    public override bool HasUponPickupEffect => true;

    protected override IEnumerable<DynamicVar> CanonicalVars => [new CardsVar(1)];
    
    protected override IEnumerable<IHoverTip> ExtraHoverTips => HoverTipFactory.FromEnchantment<Charmed>();

    protected override bool RelicAllowedToSpawn(Player owner)
    {
        return owner.Deck.Cards.Any(card => card.Type == CardType.Power);
    }

    public override async Task AfterObtained()
    {
        EnchantmentModel enchantment = ModelDb.Enchantment<Charmed>();
        List<CardModel> list = PileType.Deck.GetPile(Owner).Cards.Where(enchantment.CanEnchant).ToList();
        CardSelectorPrefs prefs = new CardSelectorPrefs(CardSelectorPrefs.EnchantSelectionPrompt, 1);
        CardModel card = (await CardSelectCmd.FromDeckForEnchantment(list.UnstableShuffle(Owner.RunState.Rng.Niche).ToList(), enchantment, 1, prefs)).FirstOrDefault();
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