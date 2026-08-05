using AncientsAwakened.AncientsAwakenedCode.Pools.Mithrix;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.CardPools;
using MegaCrit.Sts2.Core.ValueProps;

namespace AncientsAwakened.AncientsAwakenedCode.Cards.Mithrix;

[Pool(typeof(ColorlessCardPool))]
public class PerfectDefend() : AncientsAwakenedCard(1,
    CardType.Skill, CardRarity.Token,
    TargetType.Self)
{
    public override CardPoolModel VisualCardPool => ModelDb.CardPool<PerfectedPool>();
    
    public override bool GainsBlock => true;
    protected override IEnumerable<DynamicVar> CanonicalVars => [new BlockVar(8, ValueProp.Move), new EnergyVar(1)];
    protected override HashSet<CardTag> CanonicalTags => [CardTag.Defend];

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        await CreatureCmd.GainBlock(Owner.Creature, DynamicVars.Block, play);
        await PlayerCmd.GainEnergy(DynamicVars.Energy.BaseValue, Owner);
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Block.UpgradeValueBy(3M);
    }
}