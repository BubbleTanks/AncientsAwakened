using BaseLib.Utils;
using MegaCrit.Sts2.Core.Context;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Models.RelicPools;
using MegaCrit.Sts2.Core.Rooms;

namespace AncientsAwakened.AncientsAwakenedCode.Relics.Mountain;

[Pool(typeof(EventRelicPool))]
public class InfernalFlame : AncientsAwakenedRelic
{
    public override RelicRarity Rarity => RelicRarity.Ancient;

    public override Decimal ModifyRestSiteHealAmount(Creature creature, Decimal amount)
    {
        return creature.Player != Owner && creature.PetOwner != Owner ? amount : creature.MaxHp;
    }
    
    public override Task AfterRestSiteHeal(Player player, bool isMimicked)
    {
        if (player != Owner)
            return Task.CompletedTask;
        Flash();
        Status = RelicStatus.Normal;
        return Task.CompletedTask;
    }

    public override IReadOnlyList<LocString> ModifyExtraRestSiteHealText(
        Player player,
        IReadOnlyList<LocString> currentExtraText)
    {
        if (!LocalContext.IsMe(Owner))
            return currentExtraText;
        IReadOnlyList<LocString> locStringList = currentExtraText;
        int index = 0;
        LocString[] items = new LocString[1 + locStringList.Count];
        foreach (LocString locString in locStringList)
        {
            items[index] = locString;
            ++index;
        }
        items[index] = AdditionalRestSiteHealText;
        return items;
    }

    public override Task AfterRoomEntered(AbstractRoom room)
    {
        this.Status = room is RestSiteRoom ? RelicStatus.Active : RelicStatus.Normal;
        return Task.CompletedTask;
    }
}