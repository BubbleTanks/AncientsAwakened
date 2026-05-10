using AncientsAwakened.AncientsAwakenedCode.Cards;
using AncientsAwakened.AncientsAwakenedCode.Powers;
using BaseLib.Extensions;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.CardPools;
using MegaCrit.Sts2.Core.Models.Powers;

namespace AncientsAwakened.AncientsAwakenedCode.Cards.Sebastian;


[Pool(typeof(SilentCardPool))]
public class SleightOfHand() : AncientsAwakenedCard(1,
    CardType.Skill, CardRarity.Ancient,
    TargetType.AnyEnemy)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new PowerVar<PoisonPower>(4)];
    
    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Sly];

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        await PowerCmd.Apply<PoisonPower>(choiceContext, play.Target, DynamicVars.Poison.BaseValue, Owner.Creature, this);
        
    }

    public override async Task AfterCardDiscarded(PlayerChoiceContext choiceContext, CardModel card)
    {
        if (card.Owner != Owner || Owner.Creature.Side != Owner.Creature.CombatState.CurrentSide || Pile.Type == PileType.Hand)
            return;
        if (card != this)
        {
            await CardPileCmd.Add(this, PileType.Hand);
        }
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Poison.UpgradeValueBy(2);
    }
}