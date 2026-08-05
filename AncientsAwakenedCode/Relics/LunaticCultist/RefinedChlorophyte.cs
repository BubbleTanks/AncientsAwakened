using BaseLib.Extensions;
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
using MegaCrit.Sts2.Core.Saves.Runs;

namespace AncientsAwakened.AncientsAwakenedCode.Relics.LunaticCultist;

[Pool(typeof(EventRelicPool))]
public class RefinedChlorophyte : AncientsAwakenedRelic
{
    private int _platingPower = 5;
    
    public override RelicRarity Rarity => RelicRarity.Ancient;
    
    [SavedProperty]
    private int PlatingPower
    {
        get => _platingPower;
        set
        {
            AssertMutable();
            _platingPower = value;
            DynamicVars.Power<PlatingPower>().BaseValue = value;
            InvokeDisplayAmountChanged();
        }
    }
    
    protected override IEnumerable<DynamicVar> CanonicalVars => [new PowerVar<PlatingPower>(PlatingPower), new ("Scaling", 1M)];

    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<PlatingPower>()];

    public override bool ShowCounter => DisplayAmount > -1;
    
    public override int DisplayAmount
    { 
        get 
        {
            if (IsCanonical)
                return -1;
            return PlatingPower;
        }
    }
    
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
        PlatingPower += DynamicVars["Scaling"].IntValue;
    }
}