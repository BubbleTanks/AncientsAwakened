using BaseLib.Abstracts;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;

namespace AncientsAwakened.AncientsAwakenedCode.Enchantments;

public class Charmed : CustomEnchantmentModel
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new IntVar("Times", 1M)];
    
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.Static(StaticHoverTip.ReplayDynamic, DynamicVars["Times"])];
    
    public override bool CanEnchant(CardModel card) => base.CanEnchant(card) && card.Type == CardType.Power;
    
    public override int EnchantPlayCount(int originalPlayCount)
    {
        return originalPlayCount + DynamicVars["Times"].IntValue;
    }
}