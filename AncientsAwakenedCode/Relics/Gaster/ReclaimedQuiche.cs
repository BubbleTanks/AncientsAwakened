using AncientsAwakened.AncientsAwakenedCode.Potions.Gaster;
using AncientsAwakened.AncientsAwakenedCode.Relics;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.Models.RelicPools;

namespace AncientsAwakened.AncientsAwakenedCode.Relics.Gaster;

[Pool(typeof(EventRelicPool))]
public sealed class ReclaimedQuiche : AncientsAwakenedRelic
{
    public override RelicRarity Rarity =>
        RelicRarity.Ancient;

    public override async Task AfterObtained()
    {
        await PotionCmd.TryToProcure<BottledDream>(Owner);
        await PotionCmd.TryToProcure<BottledDream>(Owner);
        await CardPileCmd.AddCurseToDeck<Guilty>(Owner);
    }
}