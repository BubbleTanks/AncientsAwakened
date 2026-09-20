using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Factories;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.RelicPools;

namespace AncientsAwakened.AncientsAwakenedCode.Relics.Gaster;

[Pool(typeof(EventRelicPool))]
public sealed class RingingPhone : AncientsAwakenedRelic
{
    public override RelicRarity Rarity =>
        RelicRarity.Ancient;

    protected override IEnumerable<DynamicVar> CanonicalVars => [new GoldVar(100), new("Potions", 1M)];

    public override bool HasUponPickupEffect => true;
    
    public override async Task AfterObtained()
    {
        await PlayerCmd.GainGold(DynamicVars.Gold.BaseValue, Owner);
        foreach (var potionModel in PotionFactory.CreateRandomPotionsOutOfCombat(Owner, DynamicVars["Potions"].IntValue, Owner.RunState.Rng.CombatPotionGeneration))
        {
            await PotionCmd.TryToProcure(potionModel.ToMutable(), Owner);
        }
    }
}