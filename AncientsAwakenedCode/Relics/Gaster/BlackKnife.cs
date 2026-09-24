using BaseLib.Utils;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Models.RelicPools;
using MegaCrit.Sts2.Core.Rooms;
using MegaCrit.Sts2.Core.Saves.Runs;

namespace AncientsAwakened.AncientsAwakenedCode.Relics.Gaster;

[Pool(typeof(EventRelicPool))]
public sealed class BlackKnife : AncientsAwakenedRelic
{
    private bool _hasRelicBeenUsed;
    
    public override bool IsUsedUp => HasRelicBeenUsed;

    [SavedProperty]
    private bool HasRelicBeenUsed
    {
        get => _hasRelicBeenUsed;
        set
        {
            AssertMutable();
            _hasRelicBeenUsed = value;
            if (!IsUsedUp)
                return;
            Status = RelicStatus.Disabled;
        }
    }
    
    public override RelicRarity Rarity =>
        RelicRarity.Ancient;

    public override async Task BeforeCombatStart()
    {
        if(Owner.RunState.CurrentRoom?.RoomType != RoomType.Elite || HasRelicBeenUsed)
            return;
        Flash();
        var enemies = Owner.Creature.CombatState.Enemies;
        VfxCmd.PlayOnCreatureCenters(enemies, "vfx/vfx_bite");
        foreach (var creature in enemies)
            await CreatureCmd.Kill(creature);
        await CombatManager.Instance.CheckWinCondition();
        HasRelicBeenUsed = true;
    }
}