using BaseLib.Utils;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.RelicPools;
using MegaCrit.Sts2.Core.Rewards;
using MegaCrit.Sts2.Core.Rooms;
using MegaCrit.Sts2.Core.Saves.Runs;

namespace AncientsAwakened.AncientsAwakenedCode.Relics.Gaster;

[Pool(typeof(EventRelicPool))]
public sealed class TwistedCoin : AncientsAwakenedRelic
{
    public override RelicRarity Rarity =>
        RelicRarity.Ancient;

    private bool _hasTriggered;

    protected override IEnumerable<DynamicVar> CanonicalVars => [new GoldVar(333)];
    
    [SavedProperty]
    private bool HasTriggered
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