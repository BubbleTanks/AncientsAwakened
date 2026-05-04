using AncientsAwakened.AncientsAwakenedCode.Cards;
using AncientsAwakened.AncientsAwakenedCode.Powers;
using BaseLib.Extensions;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.CardPools;
using MegaCrit.Sts2.Core.Models.Orbs;
using MegaCrit.Sts2.Core.Models.Powers;

namespace AncientsAwakened.AncientsAwakenedCode.Cards.Sebastian;


[Pool(typeof(DefectCardPool))]
public class Electrolyze() : AncientsAwakenedCard(2,
    CardType.Skill, CardRarity.Ancient,
    TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new RepeatVar(4), new PowerVar<FocusPower>(4)];

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        await CreatureCmd.TriggerAnim(Owner.Creature, "Cast", Owner.Character.CastAnimDelay);
        for (int i = 0; i < DynamicVars.Repeat.IntValue; ++i)
            await OrbCmd.Channel<LightningOrb>(choiceContext, Owner);
        await PowerCmd.Apply<ElectrolyzePower>(Owner.Creature, DynamicVars.Power<FocusPower>().BaseValue, Owner.Creature, this);
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Repeat.UpgradeValueBy(1M);
        DynamicVars.Power<FocusPower>().UpgradeValueBy(1M);
    }
}