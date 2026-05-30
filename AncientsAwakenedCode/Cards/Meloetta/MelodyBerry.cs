using AncientsAwakened.AncientsAwakenedCode.Cards;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.CardPools;

namespace AncientsAwakened.AncientsAwakenedCode.Cards.Meloetta;

[Pool(typeof(EventCardPool))]
public class MelodyBerry() : AncientsAwakenedCard(1,
    CardType.Skill, CardRarity.Ancient,
    TargetType.None)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new HealVar(3M), new CardsVar(1)];
    
    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Exhaust];

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        await CreatureCmd.TriggerAnim(Owner.Creature, "Cast", Owner.Character.CastAnimDelay);
        await CreatureCmd.Heal(Owner.Creature, DynamicVars.Heal.BaseValue);
        await CardPileCmd.Draw(choiceContext, DynamicVars.Cards.IntValue, Owner);
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Heal.UpgradeValueBy(2M);
    }
}