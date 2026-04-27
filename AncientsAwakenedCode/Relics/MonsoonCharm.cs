using AncientsAwakened.AncientsAwakenedCode.Relics;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Map;
using MegaCrit.Sts2.Core.Models.RelicPools;
using MegaCrit.Sts2.Core.Rewards;
using MegaCrit.Sts2.Core.Rooms;
using MegaCrit.Sts2.Core.Runs;

namespace AncientsAwakened.AncientsAwakenedCode.Relics;


[Pool(typeof(EventRelicPool))]
public class MonsoonCharm : AncientsAwakenedRelic, EulogyZero.IBlacklistFromEulogy
{
    public override RelicRarity Rarity => RelicRarity.Ancient;

    public override async Task AfterObtained()
    {

        IEnumerable<MapPoint> map = RunManager.Instance.State.Map.GetAllMapPoints();

        foreach (MapPoint point in map)
        {
            if (point.PointType == MapPointType.RestSite)
            {
                point.PointType = MapPointType.Elite; 
            }
        }
        
        

    }
    
    public override bool TryModifyRewards(Player player, List<Reward> rewards, AbstractRoom? room)
    {
        if (player != Owner || (room != null ? (room.RoomType != RoomType.Elite && room.RoomType != RoomType.Monster ? 1 : 0) : 1) != 0)
            return false;
        rewards.Add((Reward) new RelicReward(player));
        return true;
    }
}