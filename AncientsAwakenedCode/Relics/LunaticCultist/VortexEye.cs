using AncientsAwakened.AncientsAwakenedCode.Powers.LunaticCultist;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.RelicPools;
using MegaCrit.Sts2.Core.Nodes.Rooms;

namespace AncientsAwakened.AncientsAwakenedCode.Relics.LunaticCultist;

[Pool(typeof(EventRelicPool))]
public class VortexEye : AncientsAwakenedRelic
{
    public override RelicRarity Rarity => RelicRarity.Ancient;
    
    protected override IEnumerable<DynamicVar> CanonicalVars => [new ("DamagePercentage", 10)];

    public override async Task BeforeSideTurnStart(
        PlayerChoiceContext choiceContext,
        CombatSide side,
        IReadOnlyList<Creature> participants,
        ICombatState combatState)
    {
        if (!participants.Contains(Owner.Creature))
            return;
        Flash();
        await PowerCmd.Apply<VortexPower>(choiceContext, Owner.Creature, DynamicVars["DamagePercentage"].BaseValue, Owner.Creature, null);
        Grow(Owner.Creature.GetPowerAmount<VortexPower>());
    }

    private void Grow(int amount)
    {
        NCombatRoom.Instance?.GetCreatureNode(Owner.Creature)?.ScaleTo(1.0f + amount * 0.01f, 0.3f);
    }
}