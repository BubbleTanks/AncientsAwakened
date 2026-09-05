using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.CardPools;

namespace AncientsAwakened.AncientsAwakenedCode.Cards.Mithrix;

  [Pool(typeof(CurseCardPool))]
public sealed class Egocentrism() : AncientsAwakenedCard(2, CardType.Curse, CardRarity.Curse, TargetType.None)
{
    public override bool CanBeGeneratedByModifiers => false;
    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Eternal];
    public override bool HasTurnEndInHandEffect => true;
    public override int MaxUpgradeLevel => 0;

    protected override async Task OnTurnEndInHand(PlayerChoiceContext choiceContext)
    {
        var list = PileType.Hand.GetPile(Owner).Cards.Where(c => c is not Egocentrism).ToList();
        
        foreach (var card in list)
        {
            CardModel ego = CombatState.CreateCard<Egocentrism>(Owner);
            await CardCmd.Transform(card, ego);
        }
    }
}