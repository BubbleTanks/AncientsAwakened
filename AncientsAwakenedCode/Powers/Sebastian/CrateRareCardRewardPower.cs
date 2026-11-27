using MegaCrit.Sts2.Core.Entities.Powers;

namespace AncientsAwakened.AncientsAwakenedCode.Powers.Sebastian;

/// <summary>
/// This power acts as a visual indicator to the player that the Hunt card was successful. The actual behavior
/// lives in <see cref="AncientsAwakened.AncientsAwakenedCode.Cards.Sebastian.HeavyCrate">Heavy Crate</see> .
/// </summary>
public sealed class CrateRareCardRewardPower : AncientsAwakenedPower
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;
}