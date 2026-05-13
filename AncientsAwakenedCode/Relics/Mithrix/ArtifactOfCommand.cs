using AncientsAwakened.AncientsAwakenedCode.Pools;
using AncientsAwakened.AncientsAwakenedCode.Relics;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Context;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Multiplayer;
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

namespace AncientsAwakened.AncientsAwakenedCode.Relics;


[Pool(typeof(EventRelicPool))]
public class ArtifactOfCommand : AncientsAwakenedRelic
{
    public override RelicRarity Rarity => RelicRarity.Ancient;

    public override bool HasUponPickupEffect => true;
    
    public override async Task AfterObtained()
    {
        
        CardSelectorPrefs prefs = new CardSelectorPrefs(SelectionScreenPrompt, 1);
        List<CardModel> cards = Owner.UnlockState.Cards.Where(c => c.Type != CardType.Status && c.Type != CardType.Curse && c.Rarity != CardRarity.Token && c.Rarity != CardRarity.Ancient && c.Rarity != CardRarity.Quest && c.Rarity != CardRarity.Event).ToList();
        
        CardModel card = (await FromSimpleGrid(cards, Owner, prefs))
            .FirstOrDefault();
        if (card != null)
        {
            CardModel card2 = Owner.RunState.CloneCard(card.ToMutable());
            card2.Owner = Owner;
            CardCmd.PreviewCardPileAdd([await CardPileCmd.Add(card2, PileType.Deck)], 2F);
        }
    }
    
    public static async Task<IEnumerable<CardModel>> FromSimpleGrid(
        IReadOnlyList<CardModel> cardsIn,
        Player player,
        CardSelectorPrefs prefs)
    {
        if (CombatManager.Instance.IsEnding)
            return Array.Empty<CardModel>();
        List<CardModel> cards = cardsIn.ToList();
        if (cards.Count == 0)
            return Array.Empty<CardModel>();
        List<CardModel> result;
        if (!prefs.RequireManualConfirmation && cards.Count <= prefs.MinSelect)
        {
            result = cards.ToList();
        }
        else
        {
            uint choiceId = RunManager.Instance.PlayerChoiceSynchronizer.ReserveChoiceId(player);
            if (CardSelectCmd.ShouldSelectLocalCard(player))
            {
                if (CardSelectCmd.Selector != null)
                {
                    result = (await CardSelectCmd.Selector.GetSelectedCards(cards, prefs.MinSelect, prefs.MaxSelect)).ToList();
                }
                else
                {
                    NPlayerHand.Instance?.CancelAllCardPlay();
                    NSimpleCardSelectScreen screen = NSimpleCardSelectScreen.Create(cards, prefs);
                    NOverlayStack.Instance.Push(screen);
                    result = (await screen.CardsSelected()).ToList();
                }
                RunManager.Instance.PlayerChoiceSynchronizer.SyncLocalChoice(player, choiceId, PlayerChoiceResult.FromIndexes(result.Select(c => cards.IndexOf(c)).ToList()));
            }
            else
                result = (await RunManager.Instance.PlayerChoiceSynchronizer.WaitForRemoteChoice(player, choiceId)).AsIndexes().Select<int, CardModel>(i => cards[i]).ToList();
        }
        CardSelectCmd.LogChoice(player, result);
        return result;
    }
}