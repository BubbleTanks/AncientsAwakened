using AncientsAwakened.AncientsAwakenedCode.Enchantments.Mithrix;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.RelicPools;
using MegaCrit.Sts2.Core.Nodes;
using MegaCrit.Sts2.Core.Nodes.Vfx;

namespace AncientsAwakened.AncientsAwakenedCode.Relics.Mithrix;

[Pool(typeof(EventRelicPool))]
public sealed class ShapedGlass : AncientsAwakenedRelic
{
    public override RelicRarity Rarity => RelicRarity.Ancient;
    
    protected override bool RelicAllowedToSpawn(Player owner)
    {
        return owner.Deck.Cards.Any(c => c.Type == CardType.Attack);
    }

    public override bool HasUponPickupEffect => true;
    
    protected override IEnumerable<IHoverTip> ExtraHoverTips => HoverTipFactory.FromEnchantment<Design>();

    public override async Task AfterObtained()
    {
        foreach (var card in (IEnumerable<CardModel>)[.. PileType.Deck.GetPile(Owner).Cards])
        {
            if (card.Type != CardType.Attack || !ModelDb.Enchantment<Design>().CanEnchant(card)) 
                continue;
            
            CardCmd.Enchant<Design>(card, 1M);
            var child = NCardEnchantVfx.Create(card);
            if (child != null)
            {
                NRun.Instance?.GlobalUi.CardPreviewContainer.AddChildSafely(child);
            }
        }

        var amount = Owner.Creature.MaxHp / 2;
        await CreatureCmd.LoseMaxHp(new ThrowingPlayerChoiceContext(), Owner.Creature, amount, false);
    }
}