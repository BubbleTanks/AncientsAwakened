using AncientsAwakened.AncientsAwakenedCode.Powers;
using AncientsAwakened.AncientsAwakenedCode.Powers.Mountain;
using BaseLib.Extensions;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.CardPools;

namespace AncientsAwakened.AncientsAwakenedCode.Cards.Mountain;


[Pool(typeof(CurseCardPool))]
public class Stress() : AncientsAwakenedCard(
    -1, CardType.Curse, CardRarity.Curse, 
    TargetType.None)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new PowerVar<StressPower>(2M)];
    
    public override bool CanBeGeneratedByModifiers => false;
    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Unplayable, CardKeyword.Eternal];
    public override int MaxUpgradeLevel => 0;
    
    public override async Task AfterCardDrawn(PlayerChoiceContext choiceContext, CardModel card, bool fromHandDraw)
    {
        if (card != this)
            return;
        await PowerCmd.Apply<StressPower>(choiceContext, Owner.Creature, DynamicVars.Power<StressPower>().BaseValue, Owner.Creature, null);
    }
}