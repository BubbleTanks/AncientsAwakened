using AncientsAwakened.AncientsAwakenedCode.Powers;
using AncientsAwakened.AncientsAwakenedCode.Powers.Leshy;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models.RelicPools;
using MegaCrit.Sts2.Core.MonsterMoves.Intents;
using MegaCrit.Sts2.Core.Rooms;

namespace AncientsAwakened.AncientsAwakenedCode.Relics.Leshy;

[Pool(typeof(EventRelicPool))]
public sealed class FishHook : AncientsAwakenedRelic
{
    public override RelicRarity Rarity => RelicRarity.Ancient;
    
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [StunIntent.GetStaticHoverTip()];

    public override async Task BeforeSideTurnStart(
        PlayerChoiceContext choiceContext,
        CombatSide side,
        IReadOnlyList<Creature> participants,
        ICombatState combatState)
    {
        if (!participants.Contains(Owner.Creature) || Owner.PlayerCombatState?.TurnNumber > 1 || Owner.RunState.CurrentRoom?.RoomType != RoomType.Elite)
            return;
        Flash();
        var hittableEnemies = Owner.Creature.CombatState.HittableEnemies;
        VfxCmd.PlayOnCreatureCenters(hittableEnemies, "vfx/vfx_starry_impact");
        foreach (Creature creature in hittableEnemies)
        {
            if (!creature.IsStunned)
                await CreatureCmd.Stun(creature);
            else
                await PowerCmd.Apply<StunNextTurnPower>(choiceContext, creature, 1, null, null);
        }
    }
}