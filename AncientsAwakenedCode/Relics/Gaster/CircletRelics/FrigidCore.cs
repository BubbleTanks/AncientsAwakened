using AncientsAwakened.AncientsAwakenedCode.Relics;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models.Orbs;
using MegaCrit.Sts2.Core.Models.RelicPools;

namespace AncientsAwakened.AncientsAwakenedCode.Relics.Gaster.CircletRelics;

[Pool(typeof(DefectRelicPool))]
public class FrigidCore() : AncientsAwakenedRelic
{
    public override RelicRarity Rarity =>
        RelicRarity.Starter;

    public override async Task BeforeSideTurnStart(
        PlayerChoiceContext choiceContext,
        CombatSide side,
        IReadOnlyList<Creature> participants,
        ICombatState combatState)
    {
        if (!participants.Contains(Owner.Creature) || Owner.PlayerCombatState.TurnNumber > 1)
            return;
        await OrbCmd.Channel<FrostOrb>(new BlockingPlayerChoiceContext(), Owner);
    }
}