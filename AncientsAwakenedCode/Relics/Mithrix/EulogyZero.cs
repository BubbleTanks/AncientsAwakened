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


namespace AncientsAwakened.AncientsAwakenedCode.Relics.Mithrix;

/// <summary>
/// If you are looking for ways to blacklist relics from appearing for Eulogy Zero, look into BlacklistFromEulogyRelicExtension.
/// </summary>
[Pool(typeof(EventRelicPool))]
public sealed class EulogyZero : AncientsAwakenedRelic
{
    public override RelicRarity Rarity => RelicRarity.Ancient;

    private static bool IsPopulated(Player player) => player.RelicGrabBag._deques.ContainsKey(RelicRarity.Ancient);


    private void SetupForPlayer(Player player)
    {
        if (IsPopulated(player))
            return;
        
        var ancient = ModelDb.AncientEvent<Tanx>(); // Just using any Ancient they shouldn't be able to encounter.
        ancient.Owner = player;
        
        var relics = ModelDb.RelicPool<EventRelicPool>().GetUnlockedRelics(player.UnlockState).Where(
            r => r.Rarity == RelicRarity.Ancient && player.Relics.All(relic => relic.Id != r.Id) &&
                 r.RelicCanSpawnAtCustomAncient(ancient) && BlacklistedRelics().All(relic => relic.Id != r.Id)).ToList();
        
        foreach (var relicModel in relics)
        {
            if (!player.RelicGrabBag._deques.TryGetValue(RelicRarity.Ancient, out var relicModelList))
            {
                relicModelList = [];
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
        
        foreach (var r in rewards)
        {
            if (r is RelicReward reward)
            {
                reward._relic = RelicFactory.PullNextRelicFromFront(Owner, RelicRarity.Ancient).ToMutable();
            }
        }

        return true;
    }

    public override Task AfterObtained()
    {
        if (IsPopulated(Owner))
            return Task.CompletedTask;
        SetupForPlayer(Owner);
        return Task.CompletedTask;
    }
    
    /// <summary>
    /// If you wish to add your mod to this list, add the BlacklistFromEulogyRelicExtension in your relic constructor with a Mod Interop.
    /// </summary>
    private static List<RelicModel> BlacklistedRelics()
    {
        var listVar = BlacklistFromEulogyRelicExtension.EulogyBlacklist.Select(ModelDb.GetById<RelicModel>).ToList();

        listVar.Add(ModelDb.Relic<GoldenCompass>());
        listVar.Add(ModelDb.Relic<FurCoat>());
        listVar.Add(ModelDb.Relic<DustyTome>());

        listVar.AddRange(ModelDb.Event<Neow>().AllPossibleOptions.Select(relic => relic.Relic));

        if (AncientConfigsPlusInterop.EnabledAct1 != null)
        {
            for (var slot = 1; slot <= 3; slot++)
            {
                foreach (var kv in AncientConfigsPlusInterop.ParseWeights(slot))
                {
                    if (kv.Value != 0) 
                        continue;
                    var ancientModel = ModelDb.AllAncients.FirstOrDefault(a => a.GetType().Name == kv.Key);
                    listVar.AddRange(ancientModel.AllPossibleOptions.Select(relic => relic.Relic));
                }
            }
        }
        
        return listVar.Distinct().ToList();
    }
}