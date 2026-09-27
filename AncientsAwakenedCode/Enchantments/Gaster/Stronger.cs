using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;

namespace AncientsAwakened.AncientsAwakenedCode.Enchantments.Gaster;

public sealed class Stronger : AncientsAwakenedEnchantment
{
    public override Decimal EnchantBlockAdditive(Decimal originalBlock)
    {
        if (originalBlock <= 0)
            return 0M;
        return this.Amount;
    }
    
    public override Decimal EnchantDamageAdditive(Decimal originalDamage, ValueProp props)
    {
        if (originalDamage <= 0M)
            return 0M;
        return !props.IsPoweredAttack() ? 0M : (Decimal) this.Amount;
    }

    public override bool CanEnchant(CardModel card) => base.CanEnchant(card) && (card.GainsBlock || card.Type == CardType.Attack);
}