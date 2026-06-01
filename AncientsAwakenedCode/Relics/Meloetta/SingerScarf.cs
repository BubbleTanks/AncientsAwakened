using BaseLib.Utils;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.RelicPools;
using MegaCrit.Sts2.Core.Rooms;

namespace AncientsAwakened.AncientsAwakenedCode.Relics.Meloetta;

[Pool(typeof(EventRelicPool))]
public class SingerScarf : AncientsAwakenedRelic
{
    private CardType? _cardTypeThisTurn;
    
    public override RelicRarity Rarity => RelicRarity.Ancient;

    protected override IEnumerable<DynamicVar> CanonicalVars => [new EnergyVar(1)];
    
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.ForEnergy(this)];

    private CardType? CardTypeThisTurn
    {
        get => _cardTypeThisTurn;
        set
        {
            AssertMutable();
            _cardTypeThisTurn = value;
        }
    }
    
    public override Decimal ModifyMaxEnergy(Player player, Decimal amount)
    {
        return player != Owner ? amount : amount + DynamicVars.Energy.BaseValue;
    }
    
    public override bool TryModifyEnergyCostInCombatLate(
        CardModel card,
        Decimal originalCost,
        out Decimal modifiedCost)
    {
        if (card.Owner.Creature != Owner.Creature || CardTypeThisTurn == null || card.Type == CardTypeThisTurn)
        {
            modifiedCost = originalCost;
            return false;
        }
        modifiedCost = originalCost + 1;
        return true;
    }
    
    public override Task AfterCardPlayed(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        if (cardPlay.Card.Owner != Owner || CardTypeThisTurn != null)
            return Task.CompletedTask;
        Flash();
        CardTypeThisTurn = cardPlay.Card.Type;
        return Task.CompletedTask;
    }
    
    public override Task AfterSideTurnEnd(
        PlayerChoiceContext choiceContext,
        CombatSide side,
        IEnumerable<Creature> participants)
    {
        if (!participants.Contains(Owner.Creature))
            return Task.CompletedTask;
        CardTypeThisTurn = null;
        return Task.CompletedTask;
    }
    
    public override Task AfterCombatEnd(CombatRoom _)
    {
        CardTypeThisTurn = null;
        return Task.CompletedTask;
    }
}