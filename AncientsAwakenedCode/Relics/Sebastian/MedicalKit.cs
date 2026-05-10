using AncientsAwakened.AncientsAwakenedCode.Potions.Sebastian;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models.RelicPools;
using MegaCrit.Sts2.Core.Rooms;

namespace AncientsAwakened.AncientsAwakenedCode.Relics.Sebastian;

[Pool(typeof(EventRelicPool))]
public class MedicalKit : AncientsAwakenedRelic
{
    public override RelicRarity Rarity => RelicRarity.Ancient;
    
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPotion<NeloprephineVial>()];

    public override async Task AfterRoomEntered(AbstractRoom room)
    {
        if (!(room is RestSiteRoom))
            return;
        Flash();
        await PotionCmd.TryToProcure<NeloprephineVial>(Owner);
    }
}