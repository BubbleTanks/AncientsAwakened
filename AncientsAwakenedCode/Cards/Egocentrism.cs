using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.CardPools;

namespace AncientsAwakened.AncientsAwakenedCode.Cards;

  [Pool(typeof(CurseCardPool))]
public class Egocentrism : AncientsAwakenedCard
{
    public Egocentrism()
        : base(2, CardType.Curse, CardRarity.Curse, TargetType.None)
    {}
    
    public override bool CanBeGeneratedByModifiers => false;
    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Eternal];
    public override bool HasTurnEndInHandEffect => true;
    public override int MaxUpgradeLevel => 0;

    public override async Task OnTurnEndInHand(PlayerChoiceContext choiceContext)
    {
        List<CardModel> list = PileType.Hand.GetPile(Owner).Cards.ToList();

        List<CardModel> list2 = list.Where(delegate(CardModel c)
        {
            return !(c is Egocentrism);
        }).ToList();
        
        foreach (CardModel card in list2)
        {
            CardModel ego = CombatState.CreateCard<Egocentrism>(Owner);
            await CardCmd.Transform(card, ego);
        }
    }
}