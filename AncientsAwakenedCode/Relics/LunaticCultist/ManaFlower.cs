using AncientsAwakened.AncientsAwakenedCode.Patches;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Factories;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.Models.RelicPools;

namespace AncientsAwakened.AncientsAwakenedCode.Relics.LunaticCultist;

[Pool(typeof(EventRelicPool))]
public class ManaFlower : AncientsAwakenedRelic
{
    public override RelicRarity Rarity => RelicRarity.Ancient;
    
    public override bool HasUponPickupEffect => true;
    
    protected override IEnumerable<DynamicVar> CanonicalVars => [new PowerVar<BufferPower>(3M)];

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
}