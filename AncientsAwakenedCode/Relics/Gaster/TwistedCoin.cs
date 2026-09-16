using AncientsAwakened.AncientsAwakenedCode.Relics;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Factories;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.RelicPools;
using MegaCrit.Sts2.Core.Rewards;
using MegaCrit.Sts2.Core.Rooms;
using MegaCrit.Sts2.Core.Saves.Runs;

namespace AncientsAwakened.AncientsAwakenedCode.Relics.Gaster;

[Pool(typeof(EventRelicPool))]
public class TwistedCoin() : AncientsAwakenedRelic
{
    public override RelicRarity Rarity =>
        RelicRarity.Ancient;
    
    public bool _hasTriggered;

    protected override IEnumerable<DynamicVar> CanonicalVars => [new GoldVar(100)];
    
    [SavedProperty]
    public bool HasTriggered
    {
        get => _hasTriggered;
        set
        {
            AssertMutable();
            _hasTriggered = value;
            InvokeDisplayAmountChanged();
        }
    }
    
    public override bool TryModifyRewards(Player player, List<Reward> rewards, AbstractRoom? room)
    {
        if (player != Owner || (room != null ? (room.RoomType != RoomType.Boss ? 1 : 0) : 1) != 0 || Owner.RunState.CurrentActIndex != 0 || HasTriggered)
            return false;
        Flash();
        rewards.Add(new GoldReward(DynamicVars.Gold.IntValue, player));
        HasTriggered = true;
        Status = RelicStatus.Disabled;
        return true;
    }
}