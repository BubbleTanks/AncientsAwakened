using AncientsAwakened.AncientsAwakenedCode.Patches;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Factories;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.RelicPools;
using MegaCrit.Sts2.Core.ValueProps;

namespace AncientsAwakened.AncientsAwakenedCode.Relics.LunaticCultist;

[Pool(typeof(EventRelicPool))]
public sealed class ManaFlower : AncientsAwakenedRelic
{
    public override RelicRarity Rarity => RelicRarity.Ancient;
    
    public override bool HasUponPickupEffect => true;
    
    protected override IEnumerable<DynamicVar> CanonicalVars => [new ("DamageDecrease", 15M)];

    public override async Task AfterPlayerTurnStart(
        PlayerChoiceContext choiceContext, 
        Player player)
    {
        if (player != Owner)
            return;
        Flash();
        var potion = PotionFactory.CreateRandomPotionInCombat(Owner, Owner.RunState.Rng.CombatPotionGeneration).ToMutable();
        potion.Owner = Owner;
        ManaFlowerPatch.ManaFlowerPotionField.Set(potion, true);
        if (potion.TargetType == TargetType.AnyEnemy)
        {
            if(player.Creature.CombatState?.HittableEnemies.Count == 0)
                return;
            await potion.OnUseWrapper(choiceContext,
                Owner.RunState.Rng.CombatTargets.NextItem(player.Creature.CombatState.HittableEnemies));
        }
        else
            await potion.OnUseWrapper(choiceContext, Owner.Creature);
    }
    
    public override decimal ModifyDamageMultiplicative(
        Creature? target,
        decimal amount,
        ValueProp props,
        Creature? dealer,
        CardModel? cardSource,
        CardPlay? _)
    {
        if (dealer != Owner.Creature || !props.IsPoweredAttack())
            return 1M;
        var amount1 = 1.0M - (DynamicVars["DamageDecrease"].BaseValue / 100M);
        return amount1;
    }
}