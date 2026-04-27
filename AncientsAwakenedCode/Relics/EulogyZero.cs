using AncientsAwakened.AncientsAwakenedCode.Relics;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Events;
using MegaCrit.Sts2.Core.Extensions;
using MegaCrit.Sts2.Core.Factories;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Events;
using MegaCrit.Sts2.Core.Models.RelicPools;
using MegaCrit.Sts2.Core.Models.Relics;
using MegaCrit.Sts2.Core.Rewards;
using MegaCrit.Sts2.Core.Rooms;
// ReSharper disable All

namespace AncientsAwakened.AncientsAwakenedCode.Relics;

/// <summary>
/// If you are looking for ways to blacklist relics from appearing for Eulogy Zero, please look at the IBlacklistFromEulogy interface.
/// </summary>
[Pool(typeof(EventRelicPool))]
public class EulogyZero : AncientsAwakenedRelic
{
    public override RelicRarity Rarity => RelicRarity.Ancient;

    public override bool TryModifyRewards(Player player, List<Reward> rewards, AbstractRoom? room)
    {

        if (player != Owner || room == null)
            return false;

        foreach (Reward r in rewards)
        {
            if (r is RelicReward)
            {

                ((RelicReward)r)._relic = RelicFactory.PullNextRelicFromFront(Owner, RelicRarity.Ancient).ToMutable();

            }
        }

        return true;
    }

    private bool IsPopulated => Owner.RelicGrabBag._deques.ContainsKey(RelicRarity.Ancient);

    public override async Task AfterObtained()
    {

        if (IsPopulated)
        {
            return;
        }
        
        List<RelicModel> relics = ModelDb.RelicPool<EventRelicPool>().GetUnlockedRelics(Owner.UnlockState).Where(r => r.Rarity == RelicRarity.Ancient && !(r is IBlacklistFromEulogy) && !(BlacklistedRelics().Any(relic => relic.Id == r.Id)) && !(Owner.Relics.Any(relic => relic.Id == r.Id))).ToList();
        
        foreach (RelicModel relicModel in relics)
        {
            if (!Owner.RelicGrabBag._deques.TryGetValue(RelicRarity.Ancient, out List<RelicModel> relicModelList))
            {
                relicModelList = new List<RelicModel>();
                Owner.RelicGrabBag._deques[RelicRarity.Ancient] = relicModelList;
            }
            relicModelList.Add(relicModel);
        }

        Owner.RelicGrabBag._deques[RelicRarity.Ancient].UnstableShuffle(Owner.RunState.Rng.UpFront);

    }
    /// <summary>
    /// This is just for vanilla relics (I did not want to patch them 3:)
    /// </summary>
    private List<RelicModel> BlacklistedRelics()
    {
        var listVar = new List<RelicModel>();
        
        listVar.Add(ModelDb.Relic<GoldenCompass>());
        listVar.Add(ModelDb.Relic<BlackStar>());
        foreach (EventOption relic in ModelDb.Event<Neow>().AllPossibleOptions)
        {
            listVar.Add(relic.Relic);
        }
        
        return listVar;
    }

    /// <summary>
    /// Implement this into your relics if you want to blacklist them
    /// </summary>
    public interface IBlacklistFromEulogy
    {
    }
}