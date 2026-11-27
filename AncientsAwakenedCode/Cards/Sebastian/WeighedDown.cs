using BaseLib.Utils;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Models.CardPools;

namespace AncientsAwakened.AncientsAwakenedCode.Cards.Sebastian;

[Pool(typeof(CurseCardPool))]
public sealed class WeighedDown() : AncientsAwakenedCard(-1, CardType.Curse, CardRarity.Curse, TargetType.None)
{
    public override bool CanBeGeneratedByModifiers => false;
    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Unplayable, CardKeyword.Innate, CardKeyword.Retain];
    public override int MaxUpgradeLevel => 0;

}