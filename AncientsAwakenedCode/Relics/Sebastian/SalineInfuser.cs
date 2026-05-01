using AncientsAwakened.AncientsAwakenedCode.Patches;
using AncientsAwakened.AncientsAwakenedCode.Relics;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Potions;
using MegaCrit.Sts2.Core.Models.RelicPools;

namespace AncientsAwakened.AncientsAwakenedCode.Relics;


[Pool(typeof(EventRelicPool))]
public class SalineInfuser : AncientsAwakenedRelic
{
    public override RelicRarity Rarity => RelicRarity.Ancient;
    
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