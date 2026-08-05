using BaseLib.Utils;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.RelicPools;
using MegaCrit.Sts2.Core.Nodes.Combat;
using MegaCrit.Sts2.Core.Nodes.Screens.CardSelection;
using MegaCrit.Sts2.Core.Nodes.Screens.Overlays;
using MegaCrit.Sts2.Core.Runs;

namespace AncientsAwakened.AncientsAwakenedCode.Relics.Mithrix;

[Pool(typeof(EventRelicPool))]
public class ArtifactOfCommand : AncientsAwakenedRelic
{
    public override RelicRarity Rarity => RelicRarity.Ancient;

    public override bool HasUponPickupEffect => true;
    
    public override async Task AfterObtained()
    {
        
        CardSelectorPrefs prefs = new CardSelectorPrefs(SelectionScreenPrompt, 1);
        List<CardModel> cards = Owner.UnlockState.Cards.Where(c => c.Type != CardType.Status && c.Type != CardType.Curse && c.Rarity != CardRarity.Token && c.Rarity != CardRarity.Ancient && c.Rarity != CardRarity.Quest && c.Rarity != CardRarity.Event).ToList();
        
        CardModel card = (await CardSelectCmd.FromSimpleGrid(new BlockingPlayerChoiceContext(), cards, Owner, prefs)).FirstOrDefault();
        if (card != null)
        {
            CardModel card2 = Owner.RunState.CloneCard(card.ToMutable());
            card2.Owner = Owner;
            CardCmd.PreviewCardPileAdd([await CardPileCmd.Add(card2, PileType.Deck)], 2F);
        }
    }
}