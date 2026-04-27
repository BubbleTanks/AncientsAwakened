using BaseLib.Abstracts;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

namespace AncientsAwakened.AncientsAwakenedCode.Enchantments;

public class Design : CustomEnchantmentModel
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new IntVar("Times", 1M)];
    
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.Static(StaticHoverTip.ReplayDynamic, DynamicVars["Times"])];
    
    public override int EnchantPlayCount(int originalPlayCount)
    {
        return originalPlayCount + DynamicVars["Times"].IntValue;
    }
}