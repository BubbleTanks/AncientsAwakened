using AncientsAwakened.AncientsAwakenedCode.Potions.Sebastian;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Factories;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.RelicPools;
using MegaCrit.Sts2.Core.Rooms;

namespace AncientsAwakened.AncientsAwakenedCode.Relics.Sebastian;

[Pool(typeof(EventRelicPool))]
public class MedicalKit : AncientsAwakenedRelic
{
    public override RelicRarity Rarity => RelicRarity.Ancient;
    
    protected override IEnumerable<DynamicVar> CanonicalVars => [new ("PotionSlots", 1M)];
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPotion<NeloprephineVial>()];

    public override async Task AfterRoomEntered(AbstractRoom room)
    {
        if (!(room is RestSiteRoom))
            return;
        int originalSlotCount = Owner.MaxPotionCount;
        Flash();
        await PlayerCmd.GainMaxPotionCount(DynamicVars["PotionSlots"].IntValue, Owner);
        await PotionCmd.TryToProcure(ModelDb.Potion<NeloprephineVial>().ToMutable(), Owner, originalSlotCount);
    }
}