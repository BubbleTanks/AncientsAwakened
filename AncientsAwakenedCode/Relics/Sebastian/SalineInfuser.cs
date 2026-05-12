using AncientsAwakened.AncientsAwakenedCode.Patches;
using AncientsAwakened.AncientsAwakenedCode.UI;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.RelicPools;
using MegaCrit.Sts2.Core.Runs;

namespace AncientsAwakened.AncientsAwakenedCode.Relics.Sebastian;


[Pool(typeof(EventRelicPool))]
public class SalineInfuser : AncientsAwakenedRelic
{
    public override RelicRarity Rarity => RelicRarity.Ancient;
    
    public override bool IsAllowed(IRunState runState)
    {
        return runState.Players.Count == 1;
    }
    
    public override async Task AfterPotionUsed(PotionModel potion, Creature? target)
    {
        if (potion.Owner != Owner || SaltyPatch.SaltyField.Get(potion))
            return;
        PotionModel potionSalty = (PotionModel)potion.MutableClone();
        potionSalty.IsQueued = false;
        SaltyPatch.SaltyField.Set(potionSalty, true);
        await PotionCmd.TryToProcure(potionSalty, Owner);
        Flash();
    }
    
}