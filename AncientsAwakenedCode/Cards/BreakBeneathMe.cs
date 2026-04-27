using AncientsAwakened.AncientsAwakenedCode.Powers;
using BaseLib.Extensions;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Commands.Builders;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.CardPools;
using MegaCrit.Sts2.Core.ValueProps;

namespace AncientsAwakened.AncientsAwakenedCode.Cards;
[Pool(typeof(EventCardPool))]
public class BreakBeneathMe() : AncientsAwakenedCard(4, CardType.Attack, CardRarity.Ancient, TargetType.AnyEnemy)
{
    
    protected override IEnumerable<DynamicVar> CanonicalVars => [new DamageVar(36, ValueProp.Move), new PowerVar<BreakBeneathMePower>(1)];
    
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await DamageCmd.Attack(DynamicVars.Damage.BaseValue).FromCard(this).Targeting(cardPlay.Target).WithHitFx("vfx/vfx_attack_blunt", tmpSfx: "blunt_attack.mp3").Execute(choiceContext);
        await PowerCmd.Apply<BreakBeneathMePower>(choiceContext, Owner.Creature, DynamicVars.Power<BreakBeneathMePower>().BaseValue, Owner.Creature, this);
    }
    
    protected override void OnUpgrade() => DynamicVars.Damage.UpgradeValueBy(10M);
    
}