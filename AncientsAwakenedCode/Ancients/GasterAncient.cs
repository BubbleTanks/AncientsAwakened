using AncientsAwakened.AncientsAwakenedCode.Relics.Gaster;
using BaseLib.Extensions;
using BaseLib.Utils;
using Godot;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Relics;

namespace AncientsAwakened.AncientsAwakenedCode.Ancients;

public class GasterAncient : AncientsAwakenedAncient
{
    protected override OptionPools MakeOptionPools =>

        new(
            MakePool(
                AncientOption<BrokenRaincatcher>(),
                AncientOption<CoreTablet>()
            ),
            MakePool(
                AncientOption<ErodedOnyx>(),
                AncientOption<FragmentedMind>()
            ),
            MakePool(
                AncientOption<RingingPhone>(),
                AncientOption<ShatteredHand>(),
                AncientOption<TwistedCoin>()
            ));

    public override Color ButtonColor => new("16161D");

    public override Color DialogueColor => new("0D0D11");

    public override bool IsValidForAct(ActModel act)
    {
        return act.ActNumber() == 1;
    }
}