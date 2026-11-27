using BaseLib.Extensions;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.CardPools;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.Saves.Runs;
using MegaCrit.Sts2.Core.ValueProps;

namespace AncientsAwakened.AncientsAwakenedCode.Cards.Mithrix;
[Pool(typeof(EventCardPool))]
public sealed class BreakBeneathMe() : AncientsAwakenedCard(4, CardType.Attack, CardRarity.Ancient, TargetType.AnyEnemy)
{
    private bool _isMultiplayer;
    
    [SavedProperty]
    public bool IsMultiplayer
    {
        get => _isMultiplayer;
        set
        {
            AssertMutable();
            _isMultiplayer = value;
            ((BoolVar)DynamicVars["IsMultiplayer"]).BoolVal = value;
            if(value) 
                _energyCost = new CardEnergyCost(this, 5, false);
        }
    }
    protected override IEnumerable<DynamicVar> CanonicalVars => [new DamageVar(36, ValueProp.Move), new PowerVar<ExtraTurnPower>(1), new BoolVar("IsMultiplayer", false)];
    
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await DamageCmd.Attack(DynamicVars.Damage.BaseValue).FromCard(this, cardPlay).Targeting(cardPlay.Target).WithHitFx("vfx/vfx_attack_blunt", tmpSfx: "blunt_attack.mp3").Execute(choiceContext);
        if (((BoolVar)DynamicVars["IsMultiplayer"]).BoolVal)
        {
            foreach(var creature in CombatState.GetTeammatesOf(Owner.Creature).Where(c => c is { IsAlive: true, IsPlayer: true }))
            {
                await PowerCmd.Apply<ExtraTurnPower>(choiceContext, creature,
                    DynamicVars.Power<ExtraTurnPower>().BaseValue, Owner.Creature, this);
            }
        } 
        else 
            await PowerCmd.Apply<ExtraTurnPower>(choiceContext, Owner.Creature, DynamicVars.Power<ExtraTurnPower>().BaseValue, Owner.Creature, this);
    }
    
    protected override void OnUpgrade() => DynamicVars.Damage.UpgradeValueBy(10M);
    
}