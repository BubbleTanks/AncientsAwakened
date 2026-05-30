using BaseLib.Extensions;
using BaseLib.Hooks;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.Models.RelicPools;

namespace AncientsAwakened.AncientsAwakenedCode.Relics.Mountain;

[Pool(typeof(EventRelicPool))]
public class IronCrown : AncientsAwakenedRelic, IHealAmountModifier
{
    public override RelicRarity Rarity => RelicRarity.Ancient;
    
    protected override IEnumerable<DynamicVar> CanonicalVars => [new PowerVar<PlatingPower>(15M)];
    
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<PlatingPower>()];

    public override async Task BeforeSideTurnStart(
        PlayerChoiceContext choiceContext,
        CombatSide side,
        IReadOnlyList<Creature> participants,
        ICombatState combatState)
    {
        if (!participants.Contains(Owner.Creature) || Owner.PlayerCombatState.TurnNumber > 1)
            return;
        Flash();
        await PowerCmd.Apply<PlatingPower>(choiceContext, Owner.Creature, DynamicVars.Power<PlatingPower>().BaseValue, Owner.Creature, null);
    }
    
    public decimal ModifyHealMultiplicative(Creature creature, decimal amount)
    {
        if (creature.Player != Owner)
            return 1M;
        if (amount > 0)
            Flash();
        return 0M;
    }
}