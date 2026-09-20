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
public sealed class EntrySeventeen() : AncientsAwakenedCard(2,
    CardType.Power, CardRarity.Ancient,
    TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new(CombatsKey, 5M)];
    private const int MaxCombats = 5;
    private const string CombatsKey = "Combats";
    private int _combatsSeen;

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        if (CombatState?.RunState.CurrentRoom is CombatRoom combatRoom) 
            combatRoom.AddExtraReward(Owner, new CardReward(CardCreationOptions.ForRoom(Owner, combatRoom.RoomType), 3, Owner));
        await PowerCmd.Apply<EntrySeventeenPower>(choiceContext, Owner.Creature, 1M, Owner.Creature, this);
    }

    private int CombatsSeen
    {
        get => _combatsSeen;
        set
        {
            AssertMutable();
            _combatsSeen = value;
            DynamicVars[CombatsKey].BaseValue = MaxCombats - CombatsSeen;
        }
    }

    public override async Task AfterCombatEnd(CombatRoom _)
    {
        if ((Pile != null ? Pile.Type != PileType.Deck ? 1 : 0 : 1) != 0)
            return;
        CombatsSeen++;
        if (CombatsSeen < 5 || Pile?.Type != PileType.Deck)
            return;
        await CardPileCmd.RemoveFromDeck(this);
    }

    protected override void OnUpgrade() => EnergyCost.UpgradeBy(-1);
}