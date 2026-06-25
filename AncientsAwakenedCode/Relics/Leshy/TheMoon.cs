using BaseLib.Utils;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.RelicPools;
using MegaCrit.Sts2.Core.ValueProps;

namespace AncientsAwakened.AncientsAwakenedCode.Relics.Leshy;

[Pool(typeof(EventRelicPool))]
public class TheMoon : AncientsAwakenedRelic
{
    public override RelicRarity Rarity => RelicRarity.Ancient;
    
    protected override IEnumerable<DynamicVar> CanonicalVars => [new ("DamageThreshold", 7M), new ("DamageMaximum", 1M)];
    
    public override Decimal ModifyHpLostAfterOstyLate(
        Creature target,
        Decimal amount,
        ValueProp props,
        Creature? dealer,
        CardModel? cardSource)
    {
        return target != Owner.Creature || !props.IsPoweredAttack() || decimal.Truncate(amount) <= DynamicVars["DamageMaximum"].BaseValue || decimal.Truncate(amount) > DynamicVars["DamageThreshold"].BaseValue ? amount : DynamicVars["DamageMaximum"].BaseValue;
    }
    
    public override Task AfterModifyingHpLostAfterOsty()
    {
        Flash();
        return Task.CompletedTask;
    }
}