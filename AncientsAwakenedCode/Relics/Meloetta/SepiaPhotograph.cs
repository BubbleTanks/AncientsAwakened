using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.RelicPools;
using MegaCrit.Sts2.Core.Rewards;
using MegaCrit.Sts2.Core.Rooms;
using MegaCrit.Sts2.Core.Saves.Runs;

namespace AncientsAwakened.AncientsAwakenedCode.Relics.Meloetta;

[Pool(typeof(EventRelicPool))]
public class SepiaPhotograph() : AncientsAwakenedRelic
{
    public override RelicRarity Rarity => RelicRarity.Ancient;

    private bool _hasTriggered;
    
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
    
    protected override IEnumerable<DynamicVar> CanonicalVars => [new ("RelicsNow", 2),new ("RelicsLater", 2)];
    
    public override bool HasUponPickupEffect => true;
    
    public override async Task AfterObtained()
    {
        List<Reward> rewards = new List<Reward>();
        for (int index = 0; index < DynamicVars["RelicsNow"].IntValue; ++index)
            rewards.Add(new RelicReward(Owner));
        await RewardsCmd.OfferCustom(Owner, rewards);
    }
    
    public override bool TryModifyRewards(Player player, List<Reward> rewards, AbstractRoom? room)
    {
        if (player != Owner || (room != null ? (room.RoomType != RoomType.Boss ? 1 : 0) : 1) != 0 || HasTriggered)
            return false;
        Flash();
        for (int index = 0; index < DynamicVars["RelicsLater"].IntValue; ++index)
            rewards.Add(new RelicReward(player));
        HasTriggered = true;
        Status = RelicStatus.Disabled;
        return true;
    }
}