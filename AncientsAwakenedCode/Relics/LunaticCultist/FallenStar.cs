using BaseLib.Utils;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Extensions;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.RelicPools;
using MegaCrit.Sts2.Core.Rooms;
using MegaCrit.Sts2.Core.ValueProps;

namespace AncientsAwakened.AncientsAwakenedCode.Relics.LunaticCultist;

[Pool(typeof(EventRelicPool))]
public class FallenStar : AncientsAwakenedRelic
{
    private const string _damageTurnKey = "DamageTurn"; 
    private bool _isActivating;
  
    public override RelicRarity Rarity => RelicRarity.Ancient;
    
    public override bool ShowCounter => DisplayAmount > -1;

    public override int DisplayAmount
    { 
        get 
        { 
            if (!CombatManager.Instance.IsInProgress || IsCanonical) 
                return -1;
            int intValue = DynamicVars[_damageTurnKey].IntValue;
            if (IsActivating) 
                return intValue;
            int turnNumber = Owner.PlayerCombatState.TurnNumber;
            return turnNumber >= intValue ? -1 : turnNumber;
        }
    }

    public bool IsActivating
    { 
        get => _isActivating;
        set 
        {
            AssertMutable();
            _isActivating = value;
            InvokeDisplayAmountChanged(); 
        } 
    }

    protected override IEnumerable<DynamicVar> CanonicalVars => [new DamageVar(99M, ValueProp.Unpowered), new(_damageTurnKey, 4M)];

    public override Task AfterSideTurnStart(
        CombatSide side,
        IReadOnlyList<Creature> participants,
        ICombatState combatState)
    {
        if (!participants.Contains(Owner.Creature))
            return Task.CompletedTask;
        if (Owner.PlayerCombatState.TurnNumber == DynamicVars[_damageTurnKey].IntValue)
            Status = RelicStatus.Active;
        InvokeDisplayAmountChanged();
        return Task.CompletedTask;
    }

    public override async Task BeforeSideTurnEnd(
        PlayerChoiceContext choiceContext,
        CombatSide side,
        IEnumerable<Creature> participants)
    {
        if (!participants.Contains(Owner.Creature))
            return;
        int intValue = DynamicVars[_damageTurnKey].IntValue;
        int turnNumber = Owner.PlayerCombatState.TurnNumber;
        Status = RelicStatus.Normal;
        if (turnNumber != intValue)
            return;
        TaskHelper.RunSafely(DoActivateVisuals());
        var hittableEnemies = Owner.Creature.CombatState.HittableEnemies;
        await CreatureCmd.Damage(choiceContext, hittableEnemies.Where(c => c.CurrentHp == hittableEnemies.Max(c => c.CurrentHp)).TakeRandom(1, Owner.RunState.Rng.CombatTargets), DynamicVars.Damage, Owner.Creature);
        InvokeDisplayAmountChanged();
    }

    public override Task AfterCombatEnd(CombatRoom _)
    {
        Status = RelicStatus.Normal;
        InvokeDisplayAmountChanged();
        return Task.CompletedTask;
    }

    public override Task AfterRoomEntered(AbstractRoom room)
    {
        if (!(room is CombatRoom))
            return Task.CompletedTask;
        Status = RelicStatus.Normal;
        InvokeDisplayAmountChanged();
        return Task.CompletedTask;
    }

    private async Task DoActivateVisuals()
    {
        IsActivating = true;
        Flash();
        await Cmd.Wait(1f);
        IsActivating = false;
    }
}