using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.CardPools;

namespace AncientsAwakened.AncientsAwakenedCode.Cards;


[Pool(typeof(CurseCardPool))]
public class WeighedDown : AncientsAwakenedCard
{
    public WeighedDown()
        : base(-1, CardType.Curse, CardRarity.Curse, TargetType.None)
    {}
    
    public override bool CanBeGeneratedByModifiers => false;
    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Unplayable, CardKeyword.Innate, CardKeyword.Eternal, CardKeyword.Retain];
    public override int MaxUpgradeLevel => 0;
}