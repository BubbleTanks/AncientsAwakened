using BaseLib.Utils;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Potions;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models.PotionPools;

namespace AncientsAwakened.AncientsAwakenedCode.Potions.Gaster;

[Pool(typeof(EventPotionPool))]
public sealed class BottledDream : AncientsAwakenedPotion
{
    public override PotionRarity Rarity => PotionRarity.Event;
    public override PotionUsage Usage => PotionUsage.AnyTime;
    public override TargetType TargetType => TargetType.AnyPlayer;

    protected override async Task OnUse(PlayerChoiceContext choiceContext, Creature? target)
    {
        AssertValidForTargetedPotion(target);
        var prefs = new CardSelectorPrefs(SelectionScreenPrompt, 1);
        var card = (await CardSelectCmd.FromHand(choiceContext,Owner, prefs, c => c.IsUpgradable, this)).FirstOrDefault();
        if (card == null)
            return;
        CardCmd.Upgrade(card);
        if (card.DeckVersion != null) 
            CardCmd.Upgrade(card.DeckVersion);
    }
}