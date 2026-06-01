using AncientsAwakened.AncientsAwakenedCode.Enchantments.Meloetta;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.RelicPools;
using MegaCrit.Sts2.Core.Nodes;
using MegaCrit.Sts2.Core.Nodes.Vfx;

namespace AncientsAwakened.AncientsAwakenedCode.Relics.Meloetta;

[Pool(typeof(EventRelicPool))]
public class AriaMastery : AncientsAwakenedRelic
{
    public override RelicRarity Rarity => RelicRarity.Ancient;

    public override bool HasUponPickupEffect => true;
    
    protected override IEnumerable<IHoverTip> ExtraHoverTips => HoverTipFactory.FromEnchantment<Aria>();

    protected override bool RelicAllowedToSpawn(Player owner)
    {
        return owner.Deck.Cards.Any(card => card.Tags.Contains(CardTag.Strike) && card.Rarity == CardRarity.Basic);
    }

    public override Task AfterObtained()
    {
        foreach (CardModel card in (IEnumerable<CardModel>) PileType.Deck.GetPile(Owner).Cards.ToList())
        {
            if (card.Rarity == CardRarity.Basic && card.Tags.Contains(CardTag.Strike) && ModelDb.Enchantment<Aria>().CanEnchant(card))
            {
                CardCmd.Enchant<Aria>(card, 1M);
                NCardEnchantVfx child = NCardEnchantVfx.Create(card);
                if (child != null)
                {
                    NRun instance = NRun.Instance;
                    if (instance != null)
                        instance.GlobalUi.CardPreviewContainer.AddChildSafely(child);
                }
            }
        }
        return Task.CompletedTask;
    }
    
}