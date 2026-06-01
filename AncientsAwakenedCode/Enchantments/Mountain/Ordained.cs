using BaseLib.Abstracts;
using BaseLib.Extensions;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;

namespace AncientsAwakened.AncientsAwakenedCode.Enchantments.Mountain;

public class Ordained : CustomEnchantmentModel
{
    public override bool HasExtraCardText => true;
    
    public override bool CanEnchantCardType(CardType cardType) => cardType == CardType.Attack;
    
    protected override IEnumerable<DynamicVar> CanonicalVars => [new PowerVar<StrengthPower>(1)];
    
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<StrengthPower>()];
    
    public override async Task AfterCardDrawn(PlayerChoiceContext choiceContext, CardModel card, bool fromHandDraw)
    {
        if (card != Card)
            return;
        await PowerCmd.Apply<StrengthPower>(choiceContext, Card.Owner.Creature, DynamicVars.Power<StrengthPower>().BaseValue, Card.Owner.Creature, null);
    }
}