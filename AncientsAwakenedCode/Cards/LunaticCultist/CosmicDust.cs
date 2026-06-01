using BaseLib.Utils;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Models.CardPools;

namespace AncientsAwakened.AncientsAwakenedCode.Cards.LunaticCultist;


[Pool(typeof(StatusCardPool))]
public class CosmicDust() : AncientsAwakenedCard(-1, CardType.Status, CardRarity.Status, TargetType.None)
{
    public override int MaxUpgradeLevel => 0;
    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Unplayable];
}