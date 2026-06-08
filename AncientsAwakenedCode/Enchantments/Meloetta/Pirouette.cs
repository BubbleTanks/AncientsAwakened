using BaseLib.Abstracts;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;

namespace AncientsAwakened.AncientsAwakenedCode.Enchantments.Meloetta;

public class Pirouette : CustomEnchantmentModel
{
    public override bool HasExtraCardText => true;
    
    protected override IEnumerable<DynamicVar> CanonicalVars => [new BlockVar(5M, ValueProp.Move), new PowerVar<DexterityPower>(1)];
    
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<DexterityPower>()];

    public override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay? cardPlay)
    {
        await PowerCmd.Apply<DexterityPower>(choiceContext, Card.Owner.Creature, DynamicVars.Dexterity.BaseValue, Card.Owner.Creature, Card);
    }
    
    public override Decimal EnchantBlockAdditive(Decimal originalBlock) => DynamicVars.Block.BaseValue;
}