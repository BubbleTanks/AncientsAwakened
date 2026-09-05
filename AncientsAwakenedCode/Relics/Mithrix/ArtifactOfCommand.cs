using BaseLib.Utils;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models.RelicPools;

namespace AncientsAwakened.AncientsAwakenedCode.Relics.Mithrix;

[Pool(typeof(EventRelicPool))]
public sealed class ArtifactOfCommand : AncientsAwakenedRelic
{
    public override RelicRarity Rarity => RelicRarity.Ancient;

    public override bool HasUponPickupEffect => true;
    
    public override async Task AfterObtained()
    {
        
        var prefs = new CardSelectorPrefs(SelectionScreenPrompt, 1);
        var cards = Owner.UnlockState.Cards.Where(c => c.Type != CardType.Status && c.Type != CardType.Curse && c.Rarity != CardRarity.Token && c.Rarity != CardRarity.Ancient && c.Rarity != CardRarity.Quest && c.Rarity != CardRarity.Event).ToList();
        
        var card = (await CardSelectCmd.FromSimpleGrid(new BlockingPlayerChoiceContext(), cards, Owner, prefs)).FirstOrDefault();
        if (card != null)
        {
            var card2 = Owner.RunState.CreateCard(card, Owner);
            CardCmd.PreviewCardPileAdd([await CardPileCmd.Add(card2, PileType.Deck)], 2F);
        }
    }
}