using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;

namespace AncientsAwakened.AncientsAwakenedCode.Powers.Mithrix;

public sealed class BreakBeneathMePower : AncientsAwakenedPower
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;

    public override bool ShouldTakeExtraTurn(Player player)
    {
        return player == Owner.Player;
    }

    public override async Task AfterTakingExtraTurn(Player player)
    {
        if (player != Owner.Player) return;
        if (Owner.IsAlive) await PowerCmd.Decrement(this);
    }
}