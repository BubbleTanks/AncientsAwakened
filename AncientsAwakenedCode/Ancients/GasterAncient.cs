using AncientsAwakened.AncientsAwakenedCode.Relics.Gaster;
using BaseLib.Extensions;
using BaseLib.Utils;
using Godot;
using MegaCrit.Sts2.Core.Models;

namespace AncientsAwakened.AncientsAwakenedCode.Ancients;

public class GasterAncient : AncientsAwakenedAncient
{
    protected override OptionPools MakeOptionPools =>

        new(
            MakePool(
                AncientOption<BrokenRaincatcher>(),
                AncientOption<CoreTablet>(),
                AncientOption<RingingPhone>(),
                AncientOption<ShatteredHand>(),
                AncientOption<TwistedCoin>(),
                AncientOption<ErodedOnyx>(),
                AncientOption<FragmentedMind>()
            ),
            MakePool(
                AncientOption<ReclaimedQuiche>(),
                AncientOption<StrangeKey>(),
                AncientOption<MeltingCandle>(),
                AncientOption<SnowFeather>(),
                AncientOption<SpikedCirclet>(3, relic =>
                {
                    if (Owner != null)
                        relic.SetupForPlayer(Owner);
                    return relic;
                }),
                AncientOption<ShadowCrystal>(3, relic =>
                {
                    if (Owner != null)
                        relic.SetupForPlayer(Owner);
                    return relic;
                })
            ));

    public override Color ButtonColor => new("16161D");

    public override Color DialogueColor => new("0D0D11");

    public override bool IsValidForAct(ActModel act)
    {
        return act.ActNumber() == 1;
    }
}