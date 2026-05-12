using AncientsAwakened.AncientsAwakenedCode.Pools;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.CardPools;
using MegaCrit.Sts2.Core.Models.Monsters;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;

namespace AncientsAwakened.AncientsAwakenedCode.Cards.Mithrix;

[Pool(typeof(NecrobinderCardPool))]
public class EternalStrike() : AncientsAwakenedCard(1,
    CardType.Attack, CardRarity.Token,
    TargetType.AnyEnemy)
{
    public override CardPoolModel VisualCardPool => ModelDb.CardPool<PerfectedPool>();
    
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<DoomPower>()];
    
    protected override IEnumerable<DynamicVar> CanonicalVars => [new OstyDamageVar(6, ValueProp.Move), new PowerVar<DoomPower>(10)];
    protected override HashSet<CardTag> CanonicalTags => [CardTag.Strike, CardTag.OstyAttack];

    protected override bool ShouldGlowRedInternal => Owner.IsOstyMissing;
    
    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        if (!Osty.CheckMissingWithAnim(Owner))
            await DamageCmd.Attack(DynamicVars.OstyDamage.BaseValue).FromOsty(Owner.Osty, this).Targeting(play.Target).WithHitFx("vfx/vfx_attack_blunt", tmpSfx: "blunt_attack.mp3").Execute(choiceContext);
        await PowerCmd.Apply<DoomPower>(choiceContext, play.Target, DynamicVars.Doom.BaseValue, Owner.Creature, this);
    }

    protected override void OnUpgrade()
    {
        DynamicVars.OstyDamage.UpgradeValueBy(3M);
        DynamicVars.Doom.UpgradeValueBy(5M);
    }
}