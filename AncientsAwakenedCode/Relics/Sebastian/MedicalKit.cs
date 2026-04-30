using AncientsAwakened.AncientsAwakenedCode.Potions.Sebastian;
using AncientsAwakened.AncientsAwakenedCode.Relics;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Models.RelicPools;
using MegaCrit.Sts2.Core.Rooms;

namespace AncientsAwakened.AncientsAwakenedCode.Relics;

[Pool(typeof(EventRelicPool))]
public class MedicalKit : AncientsAwakenedRelic
{
    public override RelicRarity Rarity => RelicRarity.Ancient;

    public override async Task AfterRoomEntered(AbstractRoom room)
    {
        if (!(room is RestSiteRoom))
            return;
        Flash();
        await PotionCmd.TryToProcure<NeloprephineVial>(Owner);
    }
}