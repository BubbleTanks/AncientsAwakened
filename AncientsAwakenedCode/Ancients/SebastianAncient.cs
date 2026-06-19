using AncientsAwakened.AncientsAwakenedCode.Relics.Sebastian;
using AncientsAwakened.AncientsAwakenedCode.UI;
using BaseLib.Abstracts;
using BaseLib.Extensions;
using BaseLib.Utils;
using Godot;
using MegaCrit.Sts2.Core.Models;

namespace AncientsAwakened.AncientsAwakenedCode.Ancients;


public class SebastianAncient : CustomAncientModel
{
    protected override OptionPools MakeOptionPools =>

        new(
            MakePool(
                AncientOption<FlashBeacon>(),
                AncientOption<WildlifeDocuments>(),
                AncientOption<MedicalKit>()
            ),
            MakePool(
                AncientOption<SebbyCharm>(3),
                AncientOption<SebastiansScanner>(3),
                AncientOption<SalineInfuser>(3),
                AncientOption<ShippingRequest>(2)
            ),
            MakePool(
                AncientOption<ShotgunShells>(3),
                AncientOption<GlowingVial>(2),
                AncientOption<ExperimentalSerum>(5, serum =>
                {
                    if (Owner != null)
                    {
                        serum.SetupForPlayer(Owner);
                    }

                    return serum;
                })
            ));

    public override Color ButtonColor => new(0.05f, 0.05f, 0.15f, 0.8f);

    public override Color DialogueColor => new("161430");

    public override bool IsValidForAct(ActModel act)
    {
        return act.ActNumber() == 2;
    }
}