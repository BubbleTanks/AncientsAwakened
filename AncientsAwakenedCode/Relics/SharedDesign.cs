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

namespace AncientsAwakened.AncientsAwakenedCode.Relics;

  
[Pool(typeof(EventRelicPool))]
public class SharedDesign : AncientsAwakenedRelic
{
    public override RelicRarity Rarity => RelicRarity.Ancient;
 
    public bool _tookDamageThisCombat;
    
    [SavedProperty]
    public bool TookDamageThisCombat
    {
        get => this._tookDamageThisCombat;
        set
        {
            this.AssertMutable();
            this._tookDamageThisCombat = value;
        }
    }

    public override Task AfterRoomEntered(AbstractRoom room)
    {
        this.TookDamageThisCombat = false;
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
        if (!(this.Owner.RunState.CurrentRoom is CombatRoom) || target != this.Owner.Creature || result.UnblockedDamage <= 0 || props.HasFlag((Enum) ValueProp.Unblockable))
            return Task.CompletedTask;
        this.TookDamageThisCombat = true;
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