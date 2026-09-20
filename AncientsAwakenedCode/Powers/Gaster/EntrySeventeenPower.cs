using MegaCrit.Sts2.Core.Entities.Powers;

namespace AncientsAwakened.AncientsAwakenedCode.Powers.Gaster;

public sealed class EntrySeventeenPower : AncientsAwakenedPower
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;

    // functionality is in the on play of Entry Seventeen, to retain consistency with The Hunt
}