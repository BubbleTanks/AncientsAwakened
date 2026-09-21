using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Potions;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.PotionPools;

namespace AncientsAwakened.AncientsAwakenedCode.Potions.Gaster;

[Pool(typeof(EventPotionPool))]
public class BottledDream : AncientsAwakenedPotion
{
    public override PotionRarity Rarity => PotionRarity.Event;
    public override PotionUsage Usage => PotionUsage.AnyTime;
    public override TargetType TargetType => TargetType.AnyPlayer;

    protected override async Task OnUse(PlayerChoiceContext choiceContext, Creature? target)
    {
        AssertValidForTargetedPotion(target);
        
        CardModel card = await CardSelectCmd.FromHandForUpgrade(choiceContext, Owner, this);
        if (card == null)
            return;
        CardCmd.Upgrade(card);
        if (card.DeckVersion != null) 
            CardCmd.Upgrade(card.DeckVersion);
    }
}