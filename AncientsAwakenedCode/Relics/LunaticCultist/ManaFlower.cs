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
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.Models.RelicPools;
using MegaCrit.Sts2.Core.ValueProps;

namespace AncientsAwakened.AncientsAwakenedCode.Relics.LunaticCultist;

[Pool(typeof(EventRelicPool))]
public class ManaFlower : AncientsAwakenedRelic
{
    public override RelicRarity Rarity => RelicRarity.Ancient;
    
    public override bool HasUponPickupEffect => true;
    
    protected override IEnumerable<DynamicVar> CanonicalVars => [new ("DamageDecrease", 25M)];

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
            await potion.OnUseWrapper(choiceContext,Owner.RunState.Rng.CombatTargets.NextItem(player.Creature.CombatState.HittableEnemies));
        else
            await potion.OnUseWrapper(choiceContext, Owner.Creature);
    }
    
    public override Decimal ModifyDamageMultiplicative(
        Creature? target,
        Decimal amount,
        ValueProp props,
        Creature? dealer,
        CardModel? cardSource,
        CardPlay? _)
    {
        if (dealer != Owner.Creature || !props.IsPoweredAttack())
            return 1M;
        Decimal amount1 = 1.0M - (DynamicVars["DamageDecrease"].BaseValue / 100M);
        return amount1;
    }
}