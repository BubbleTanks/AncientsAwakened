using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.RelicPools;
using MegaCrit.Sts2.Core.Rooms;

namespace AncientsAwakened.AncientsAwakenedCode.Relics.Leshy;

[Pool(typeof(EventRelicPool))]
public sealed class BoneLordBoon : AncientsAwakenedRelic
{
    public override RelicRarity Rarity => RelicRarity.Ancient;

    protected override IEnumerable<DynamicVar> CanonicalVars => [new("HpPercentage", 20)];
    
    public override async Task BeforeCombatStart()
    {
        if(Owner.RunState.CurrentRoom?.RoomType == RoomType.Boss)
            return;
        Flash();
        var hittableEnemies = Owner.Creature.CombatState.HittableEnemies;
        VfxCmd.PlayOnCreatureCenters(hittableEnemies, "vfx/vfx_bite");
        foreach (var creature in hittableEnemies)
            await CreatureCmd.SetCurrentHp(creature, Math.Max(creature.CurrentHp - creature.MaxHp * (DynamicVars["HpPercentage"].BaseValue / 100M), 1));
    }
}