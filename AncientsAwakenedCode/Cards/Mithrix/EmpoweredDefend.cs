using AncientsAwakened.AncientsAwakenedCode.Cards;
using AncientsAwakened.AncientsAwakenedCode.Pools;
using AncientsAwakened.AncientsAwakenedCode.Powers;
using BaseLib.Extensions;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.CardPools;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;

namespace AncientsAwakened.AncientsAwakenedCode.Cards;

[Pool(typeof(DefectCardPool))]
public class EmpoweredDefend() : AncientsAwakenedCard(1,
    CardType.Skill, CardRarity.Token,
    TargetType.Self)
{
    public override CardPoolModel VisualCardPool => ModelDb.CardPool<PerfectedPool>();

    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<FocusPower>()];
    
    protected override IEnumerable<DynamicVar> CanonicalVars => [new BlockVar(8, ValueProp.Move),new PowerVar<FocusPower>(1)];
    protected override HashSet<CardTag> CanonicalTags => [CardTag.Defend];

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        await CreatureCmd.GainBlock(Owner.Creature, DynamicVars.Block, play);
        await PowerCmd.Apply<EmpoweredDefendPower>(Owner.Creature, DynamicVars.Power<FocusPower>().BaseValue, Owner.Creature, this);
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Block.UpgradeValueBy(3M);
        DynamicVars.Power<FocusPower>().UpgradeValueBy(1);
    }
}