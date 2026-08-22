using AncientsAwakened.AncientsAwakenedCode.Extensions;
using AncientsAwakened.AncientsAwakenedCode.UI;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Map;
using MegaCrit.Sts2.Core.Models.RelicPools;
using MegaCrit.Sts2.Core.Rewards;
using MegaCrit.Sts2.Core.Rooms;
using MegaCrit.Sts2.Core.Runs;

namespace AncientsAwakened.AncientsAwakenedCode.Relics.Mithrix;

[Pool(typeof(EventRelicPool))]
public sealed class MonsoonCharm : AncientsAwakenedRelic
{
    public override RelicRarity Rarity => RelicRarity.Ancient;

    public override bool HasUponPickupEffect => true;

    public MonsoonCharm()
    {
        this.BlacklistFromEulogy();
    }
    
    public override Task AfterObtained()
    {
        var map = RunManager.Instance.State.Map.GetAllMapPoints();

        foreach (var point in map)
        {
            if (point.PointType == MapPointType.RestSite)
            {
                point.PointType = MapPointType.Elite; 
            }
        }

        return Task.CompletedTask;
    }
    
    public override bool IsAllowed(IRunState runState)
    {
        return runState.Players.Count == 1 || AncientConfigs.MultiplayerMonsoonCharm;
    }

    protected override bool RelicAllowedToSpawn(Player owner)
    {
        return IsAllowed(owner.RunState);
    }

    public override bool TryModifyRewards(Player player, List<Reward> rewards, AbstractRoom? room)
    {
        if (player != Owner || (room != null ? (room.RoomType != RoomType.Elite && room.RoomType != RoomType.Monster ? 1 : 0) : 1) != 0)
            return false;
        rewards.Add((Reward) new RelicReward(player));
        return true;
    }
}