using BaseLib.Extensions;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.Models.RelicPools;
using MegaCrit.Sts2.Core.Rooms;

namespace AncientsAwakened.AncientsAwakenedCode.Relics.LunaticCultist;

[Pool(typeof(EventRelicPool))]
public class BottleOfEctoplasm : AncientsAwakenedRelic
{
    private bool _shouldTrigger;
    private bool _usedThisCombat = false;
    
    public override RelicRarity Rarity => RelicRarity.Ancient;
    
    protected override IEnumerable<DynamicVar> CanonicalVars => [new PowerVar<IntangiblePower>(1M)];

    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<IntangiblePower>()];

    private bool ShouldTrigger
    {
        get => _shouldTrigger;
        set
        {
            AssertMutable();
            _shouldTrigger = value;
        }
    }
    
    private bool UsedThisCombat
    {
        get => _usedThisCombat;
        set
        {
            AssertMutable();
            _usedThisCombat = value;
        }
    }
    
    public override Task AfterSideTurnStart(
        CombatSide side,
        IReadOnlyList<Creature> participants,
        ICombatState combatState)
    {
        if (side != Owner.Creature.Side || UsedThisCombat)
            return Task.CompletedTask;
        Status = participants.Contains(Owner.Creature) ? RelicStatus.Active : RelicStatus.Normal;
        return Task.CompletedTask;
    }
    
    public override Task BeforeSideTurnEndVeryEarly(
        PlayerChoiceContext choiceContext,
        CombatSide side,
        IEnumerable<Creature> participants)
    {
        if (!participants.Contains(Owner.Creature) || Owner.Creature.Block > 0 || UsedThisCombat)
            return Task.CompletedTask;
        ShouldTrigger = true;
        return Task.CompletedTask;
    }
    
    public override async Task BeforeSideTurnEnd(
        PlayerChoiceContext choiceContext,
        CombatSide side,
        IEnumerable<Creature> participants)
    {
        if (!ShouldTrigger)
            return;
        ShouldTrigger = false;
        UsedThisCombat = true;
        Flash();
        await PowerCmd.Apply<IntangiblePower>(choiceContext, Owner.Creature, DynamicVars.Power<IntangiblePower>().BaseValue, Owner.Creature, null);
    }

    public override Task BeforeSideTurnStart(
        PlayerChoiceContext choiceContext,
        CombatSide side,
        IReadOnlyList<Creature> participants,
        ICombatState combatState)
    {
        if (!participants.Contains(Owner.Creature))
            return Task.CompletedTask;
        ShouldTrigger = false;
        Status = RelicStatus.Normal;
        return Task.CompletedTask;
    }
    
    public override Task AfterCombatEnd(CombatRoom _)
    {
        UsedThisCombat = false;
        Status =  RelicStatus.Normal;
        return Task.CompletedTask;
    }
}