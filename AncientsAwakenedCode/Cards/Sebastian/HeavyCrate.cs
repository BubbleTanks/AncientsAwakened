using AncientsAwakened.AncientsAwakenedCode.Cards;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.CardPools;

namespace AncientsAwakened.AncientsAwakenedCode.Cards.Sebastian;


[Pool(typeof(QuestCardPool))]
public class HeavyCrate() : AncientsAwakenedCard(-1, CardType.Quest, CardRarity.Quest, TargetType.None)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [];
    
    public override int MaxUpgradeLevel => 0;

    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Unplayable];

    

    
}