using AncientsAwakened.AncientsAwakenedCode.Powers;
using BaseLib.Extensions;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.RelicPools;
using MegaCrit.Sts2.Core.Rooms;
using MegaCrit.Sts2.Core.Saves.Runs;

namespace AncientsAwakened.AncientsAwakenedCode.Relics.Sebastian;


[Pool(typeof(EventRelicPool))]
public class FlashBeacon : AncientsAwakenedRelic
{
    public override RelicRarity Rarity => RelicRarity.Ancient;

    private const int turnsThreshold = 4;
    private const string _turnsKey = "Turns";
    private bool _isActivating;
    private int _turnsSeen;

    public override bool ShowCounter => true;

    public override int DisplayAmount
    {
        get => !IsActivating ? TurnsSeen : DynamicVars[_turnsKey].IntValue;
    }
    
    [SavedProperty]
    private int TurnsSeen
    {
        get => _turnsSeen;
        set
        {
            AssertMutable();
            _turnsSeen = value;
            InvokeDisplayAmountChanged();
        }
    }

    protected override IEnumerable<DynamicVar> CanonicalVars => [new (_turnsKey, turnsThreshold), new PowerVar<FlashBeaconPower>(10)];

    private bool IsActivating
    {
        get => _isActivating;
        set
        {
            AssertMutable();
            _isActivating = value;
            InvokeDisplayAmountChanged();
        }
    }
    
    public override async Task AfterSideTurnStart(CombatSide side, ICombatState combatState)
    {
        if (side != Owner.Creature.Side)
            return;
        TurnsSeen = (TurnsSeen + 1) % DynamicVars["Turns"].IntValue;
        Status = TurnsSeen == DynamicVars["Turns"].IntValue - 1 ? RelicStatus.Active : RelicStatus.Normal;
        if (TurnsSeen != 0)
            return;
        TaskHelper.RunSafely(DoActivateVisuals());
        await PowerCmd.Apply<FlashBeaconPower>(new ThrowingPlayerChoiceContext(), combatState.HittableEnemies, DynamicVars.Power<FlashBeaconPower>().BaseValue, Owner.Creature, null);
    }
    
    private async Task DoActivateVisuals()
    {
        IsActivating = true;
        Flash();
        await Cmd.Wait(1f);
        IsActivating = false;
    }
    
    public override Task AfterCombatEnd(CombatRoom _)
    {
        this.Status = RelicStatus.Normal;
        return Task.CompletedTask;
    }
}