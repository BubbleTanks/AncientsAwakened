using AncientsAwakened.AncientsAwakenedCode.Extensions;
using AncientsAwakened.AncientsAwakenedCode.Interops;
using BaseLib.Extensions;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Events;
using MegaCrit.Sts2.Core.Extensions;
using MegaCrit.Sts2.Core.Factories;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Events;
using MegaCrit.Sts2.Core.Models.RelicPools;
using MegaCrit.Sts2.Core.Models.Relics;
using MegaCrit.Sts2.Core.Rewards;
using MegaCrit.Sts2.Core.Rooms;

// ReSharper disable All

namespace AncientsAwakened.AncientsAwakenedCode.Relics.Mithrix;

/// <summary>
/// If you are looking for ways to blacklist relics from appearing for Eulogy Zero, look into BlacklistFromEulogyRelicExtension.
/// </summary>
[Pool(typeof(EventRelicPool))]
public class EulogyZero : AncientsAwakenedRelic
{
    public override RelicRarity Rarity => RelicRarity.Ancient;

    private bool IsPopulated(Player player) => player.RelicGrabBag._deques.ContainsKey(RelicRarity.Ancient);
    

    public void SetupForPlayer(Player player)
    {
        if (IsPopulated(player))
            return;
        
        var ancient = ModelDb.AncientEvent<Tanx>(); // Just using any Ancient they shouldn't be able to encounter.
        ancient.Owner = player;
        
        List<RelicModel> relics = ModelDb.RelicPool<EventRelicPool>().GetUnlockedRelics(player.UnlockState).Where(
            r => r.Rarity == RelicRarity.Ancient &&
                 !(BlacklistedRelics().Any(relic => relic.Id == r.Id)) && 
                 !(player.Relics.Any(relic => relic.Id == r.Id)) && 
                 RelicModelExtensions.RelicCanSpawnAtCustomAncient(r, ancient)
        ).ToList();
        
        foreach (RelicModel relicModel in relics)
        {
            if (!player.RelicGrabBag._deques.TryGetValue(RelicRarity.Ancient, out List<RelicModel> relicModelList))
            {
                relicModelList = new List<RelicModel>();
                player.RelicGrabBag._deques[RelicRarity.Ancient] = relicModelList;
            }
            relicModelList.Add(relicModel);
        }

        ancient.Owner = null;

        player.RelicGrabBag._deques[RelicRarity.Ancient].UnstableShuffle(player.RunState.Rng.UpFront);
    }
    public override bool TryModifyRewardsLate(Player player, List<Reward> rewards, AbstractRoom? room)
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

    public override async Task AfterObtained()
    {
        if (IsPopulated(Owner))
            return;
        SetupForPlayer(Owner);
    }
    
    /// <summary>
    /// If you wish to add your mod to this list, add the BlacklistFromEulogyRelicExtension in your relic constructor with a Mod Interop.
    /// </summary>
    private List<RelicModel> BlacklistedRelics()
    {
        var listVar = new List<RelicModel>();

        foreach (var relicModel in BlacklistFromEulogyRelicExtension.EulogyBlacklist)
        {
            listVar.Add(ModelDb.GetById<RelicModel>(relicModel));
        }
        listVar.Add(ModelDb.Relic<GoldenCompass>());
        
        foreach (EventOption relic in ModelDb.Event<Neow>().AllPossibleOptions)
        {
            listVar.Add(relic.Relic);
        }
        
        if (AncientConfigsPlusInterop.EnabledAct1 != null)
        {
            for (int slot = 1; slot <= 3; slot++)
            {
                foreach (var kv in AncientConfigsPlusInterop.ParseWeights(slot))
                {
                    if (kv.Value == 0)
                    {
                        var ancientModel = ModelDb.AllAncients.Where(a => a.GetType().Name == kv.Key).FirstOrDefault();
                        foreach (EventOption relic in ancientModel.AllPossibleOptions)
                        {
                            listVar.Add(relic.Relic);
                        }
                    }
                }
            }
        }
        
        listVar = listVar.Distinct().ToList();
        
        return listVar;
    }
}