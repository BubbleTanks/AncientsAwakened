using AncientsAwakened.AncientsAwakenedCode.Cards;
using AncientsAwakened.AncientsAwakenedCode.Powers.Gaster;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.CardPools;
using MegaCrit.Sts2.Core.Rewards;
using MegaCrit.Sts2.Core.Rooms;
using MegaCrit.Sts2.Core.Runs;

namespace AncientsAwakened.AncientsAwakenedCode.Cards.Gaster;

[Pool(typeof(EventCardPool))]
public class EntrySeventeen() : AncientsAwakenedCard(2,
    CardType.Power, CardRarity.Ancient,
    TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new DynamicVar("Combats", 5M)];
    public const int maxCombats = 5;
    public const string _combatsKey = "Combats";
    public int _combatsSeen;

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        if (CombatState.RunState.CurrentRoom is CombatRoom combatRoom) combatRoom.AddExtraReward(Owner, new CardReward(CardCreationOptions.ForRoom(Owner, combatRoom.RoomType), 3, Owner));
        await PowerCmd.Apply<EntrySeventeenPower>(choiceContext, Owner.Creature, 1M, Owner.Creature, this);
    }
    
    public int CombatsSeen
    {
        get => _combatsSeen;
        set
        {
            AssertMutable();
            _combatsSeen = value;
            DynamicVars["Combats"].BaseValue = 5 - CombatsSeen;
        }
    }

    public override async Task AfterCombatEnd(CombatRoom _)
    {
        CardPile pile = Pile;
        if ((pile != null ? pile.Type != PileType.Deck ? 1 : 0 : 1) != 0)
            return;
        CombatsSeen++;
        if (CombatsSeen < 5 || Pile.Type != PileType.Deck)
            return;
        await CardPileCmd.RemoveFromDeck(this);
    }

    protected override void OnUpgrade() => EnergyCost.UpgradeBy(-1);
}