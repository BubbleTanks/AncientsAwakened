using BaseLib.Utils;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.RelicPools;
using MegaCrit.Sts2.Core.Rewards;
using MegaCrit.Sts2.Core.Rooms;
using MegaCrit.Sts2.Core.Saves.Runs;
using MegaCrit.Sts2.Core.ValueProps;

namespace AncientsAwakened.AncientsAwakenedCode.Relics.Mithrix;

[Pool(typeof(EventRelicPool))]
public sealed class SharedDesign : AncientsAwakenedRelic
{
    public override RelicRarity Rarity => RelicRarity.Ancient;

    private bool _tookDamageThisCombat;
    
    [SavedProperty]
    private bool TookDamageThisCombat
    {
        get => _tookDamageThisCombat;
        set
        {
            AssertMutable();
            _tookDamageThisCombat = value;
        }
    }

    public override Task AfterRoomEntered(AbstractRoom room)
    {
        TookDamageThisCombat = false;
        return Task.CompletedTask;
    }

    public override Task AfterDamageReceived(
        PlayerChoiceContext choiceContext,
        Creature target,
        DamageResult result,
        ValueProp props,
        Creature? dealer,
        CardModel? cardSource)
    {
        if (Owner.RunState.CurrentRoom is not CombatRoom || target != Owner.Creature || result.UnblockedDamage <= 0 || props.HasFlag(ValueProp.Unblockable))
            return Task.CompletedTask;
        TookDamageThisCombat = true;
        return Task.CompletedTask;
    }
    
    public override Task AfterCombatEnd(CombatRoom room)
    {
        if (!TookDamageThisCombat)
        {
            room.AddExtraReward(Owner, new CardRemovalReward(Owner));
            room.AddExtraReward(Owner, new CardRemovalReward(Owner));
        }
        return Task.CompletedTask;
    }
    
}