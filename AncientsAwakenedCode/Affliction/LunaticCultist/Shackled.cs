using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;

namespace AncientsAwakened.AncientsAwakenedCode.Affliction.LunaticCultist;

public sealed class Shackled : AncientsAwakenedAffliction
{
    public override void AfterApplied()
    {
        Card.EnergyCost.AddUntilPlayed(1);
    }

    public override Task OnPlay(PlayerChoiceContext choiceContext, Creature? target)
    {
        CardCmd.ClearAffliction(Card);
        return Task.CompletedTask;
    }
}