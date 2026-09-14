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
public sealed class InfernalFlame : AncientsAwakenedRelic
{
    public override RelicRarity Rarity => RelicRarity.Ancient;

    public override decimal ModifyRestSiteHealAmount(Creature creature, decimal amount)
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
        var index = 0;
        var items = new LocString[1 + currentExtraText.Count];
        foreach (var locString in currentExtraText)
        {
            items[index] = locString;
            ++index;
        }
        items[index] = AdditionalRestSiteHealText;
        return items;
    }

    public override Task AfterRoomEntered(AbstractRoom room)
    {
        Status = room is RestSiteRoom ? RelicStatus.Active : RelicStatus.Normal;
        return Task.CompletedTask;
    }
}