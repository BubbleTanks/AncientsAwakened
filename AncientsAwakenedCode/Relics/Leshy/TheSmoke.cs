using AncientsAwakened.AncientsAwakenedCode.Enchantments.Leshy;
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

namespace AncientsAwakened.AncientsAwakenedCode.Relics.Leshy;

[Pool(typeof(EventRelicPool))]
public class TheSmoke : AncientsAwakenedRelic
{
    public override RelicRarity Rarity => RelicRarity.Ancient;

    public override bool HasUponPickupEffect => true;

    protected override IEnumerable<DynamicVar> CanonicalVars => [new CardsVar(1)];
    
    protected override IEnumerable<IHoverTip> ExtraHoverTips => HoverTipFactory.FromEnchantment<Smokey>();

    protected override bool RelicAllowedToSpawn(Player owner)
    {
        return owner.Deck.Cards.Any(ModelDb.Enchantment<Smokey>().CanEnchant);
    }

    public override async Task AfterObtained()
    {
        var enchantment = ModelDb.Enchantment<Smokey>();
        var list = PileType.Deck.GetPile(Owner).Cards.Where(enchantment.CanEnchant).ToList();
        var prefs = new CardSelectorPrefs(CardSelectorPrefs.EnchantSelectionPrompt, 1);
        var card = (await CardSelectCmd.FromDeckForEnchantment(list.UnstableShuffle(Owner.RunState.Rng.Niche).ToList(), enchantment, 1, prefs)).FirstOrDefault();
        if (card == null)
            return;
        CardCmd.Enchant<Smokey>(card, 1M);
        var child = NCardEnchantVfx.Create(card);
        if (child == null)
            return;
        NRun.Instance?.GlobalUi.CardPreviewContainer.AddChildSafely(child);
    }
}