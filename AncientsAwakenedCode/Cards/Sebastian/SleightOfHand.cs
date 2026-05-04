using AncientsAwakened.AncientsAwakenedCode.Cards;
using AncientsAwakened.AncientsAwakenedCode.Powers;
using BaseLib.Extensions;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.CardPools;

namespace AncientsAwakened.AncientsAwakenedCode.Cards.Sebastian;


[Pool(typeof(SilentCardPool))]
public class SleightOfHand() : AncientsAwakenedCard(1,
    CardType.Skill, CardRarity.Ancient,
    TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new PowerVar<SleightOfHandPower>(1M)];

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        await PowerCmd.Apply<SleightOfHandPower>(Owner.Creature, DynamicVars.Power<SleightOfHandPower>().BaseValue, Owner.Creature, this);
    }

    protected override void OnUpgrade() => AddKeyword(CardKeyword.Retain);
}