using AncientsAwakened.AncientsAwakenedCode.Enchantments;
using BaseLib.Utils;
using Godot;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.RelicPools;
using MegaCrit.Sts2.Core.Nodes;
using MegaCrit.Sts2.Core.Nodes.Vfx;
using MegaCrit.Sts2.Core.Runs;
using MegaCrit.Sts2.Core.ValueProps;

namespace AncientsAwakened.AncientsAwakenedCode.Relics;

[Pool(typeof(EventRelicPool))]
public class ShapedGlass : AncientsAwakenedRelic
{
    public override RelicRarity Rarity => RelicRarity.Ancient;

    // OLD CODE IGNORE THIS
    
    /*public override decimal ModifyDamageMultiplicative(Creature? target, decimal amount, ValueProp props, Creature? dealer,
        CardModel? cardSource)
    {
        if (target == Owner.Creature)
            return 2M;
        
        if (!props.IsPoweredAttack() || cardSource == null || dealer != Owner.Creature && dealer != Owner.Osty)
            return 1M;
        
        return 2M;
    }*/
    
    public override bool HasUponPickupEffect => true;
    
    protected override IEnumerable<IHoverTip> ExtraHoverTips
    {
        get => HoverTipFactory.FromEnchantment<Design>();
    }

    public override async Task AfterObtained()
    {
        foreach (CardModel card in (IEnumerable<CardModel>) PileType.Deck.GetPile(Owner).Cards.ToList())
        {
            if (card.Type == CardType.Attack && ModelDb.Enchantment<Design>().CanEnchant(card))
            {
                CardCmd.Enchant<Design>(card, 1M);
                NCardEnchantVfx child = NCardEnchantVfx.Create(card);
                if (child != null)
                {
                    NRun instance = NRun.Instance;
                    if (instance != null)
                        instance.GlobalUi.CardPreviewContainer.AddChildSafely(child);
                }
            }
        }

        int amount = Owner.Creature.MaxHp / 2;
        await CreatureCmd.LoseMaxHp(new ThrowingPlayerChoiceContext(), Owner.Creature, amount, false);
    }
}