using AncientsAwakened.AncientsAwakenedCode.Pools.Gaster;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.CardPools;
using MegaCrit.Sts2.Core.ValueProps;

namespace AncientsAwakened.AncientsAwakenedCode.Cards.Gaster;

[Pool(typeof(SilentCardPool))]
public sealed class Vestige() : AncientsAwakenedCard(1,
    CardType.Attack, CardRarity.Token,
    TargetType.AllEnemies)
{
    public override bool GainsBlock => true;
    public override CardPoolModel VisualCardPool => ModelDb.CardPool<ShadowPool>();
    protected override IEnumerable<DynamicVar> CanonicalVars => [new DamageVar(3M, ValueProp.Move), new BlockVar(3M, ValueProp.Move)];
    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Sly];
    
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await CreatureCmd.GainBlock(Owner.Creature, DynamicVars.Block, cardPlay);
        await DamageCmd.Attack(DynamicVars.Damage.BaseValue).FromCard(this, cardPlay).TargetingAllOpponents(CombatState).WithHitFx("vfx/vfx_attack_blunt", tmpSfx: "heavy_attack.mp3").Execute(choiceContext);
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Damage.UpgradeValueBy(2M);
        DynamicVars.Block.UpgradeValueBy(2M);
    }
}