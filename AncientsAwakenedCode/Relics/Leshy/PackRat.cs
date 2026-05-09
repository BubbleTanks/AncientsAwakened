using AncientsAwakened.AncientsAwakenedCode.Relics;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Models.RelicPools;
using MegaCrit.Sts2.Core.Rewards;
using MegaCrit.Sts2.Core.Rooms;
using MegaCrit.Sts2.Core.Runs;

namespace AncientsAwakened.AncientsAwakenedCode.Relics;


[Pool(typeof(EventRelicPool))]
public class PackRat : AncientsAwakenedRelic
{
    public override RelicRarity Rarity => RelicRarity.Ancient;

    public override bool TryModifyRewards(Player player, List<Reward> rewards, AbstractRoom? room)
    {
        if (player != Owner || (room != null ? (room.RoomType != RoomType.Elite && room.RoomType != RoomType.Monster ? 1 : 0) : 1) != 0)
            return false;

        int rng = Owner.RunState.Rng.Niche.NextInt(1, 10);

        switch (rng)
        {
            case 1 or 2 or 3 or 4:
                rewards.Add(new GoldReward(Owner.RunState.Rng.Niche.NextInt(10,20), player));
                break;
            case 5 or 6 or 7:
                rewards.Add(new PotionReward(player));
                break;
            case 8 or 9:
                rewards.Add(new CardReward(new CardCreationOptions([Owner.Character.CardPool], CardCreationSource.Other, CardRarityOddsType.RegularEncounter), 3, player));
                break;
            case 10:
                rewards.Add(new RelicReward(player));
                break;
            default:
                Log.Error("Pack Rat generated invalid integer!");
                break;
        }
        return true;
    }
}