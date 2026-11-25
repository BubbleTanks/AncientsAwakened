using AncientsAwakened.AncientsAwakenedCode.Affliction.LunaticCultist;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

namespace AncientsAwakened.AncientsAwakenedCode.Enchantments.LunaticCultist;

public sealed class Celestial : AncientsAwakenedEnchantment
{
    public override bool HasExtraCardText => true;
    
    protected override IEnumerable<DynamicVar> CanonicalVars => [new CardsVar(2)];

    public override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay? cardPlay)
    {
        await CardPileCmd.Draw(choiceContext, DynamicVars.Cards.BaseValue, Card.Owner);
    }
    
    public override async Task AfterSideTurnStart(
        CombatSide side,
        IReadOnlyList<Creature> participants,
        ICombatState combatState)
    {
        if (!participants.Contains(Card.Owner.Creature) || Card.Owner.PlayerCombatState?.TurnNumber > 1)
            return;
        await CardCmd.Afflict<Shackled>(Card, 1);
    }
}