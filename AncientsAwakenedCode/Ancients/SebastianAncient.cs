using AncientsAwakened.AncientsAwakenedCode.Relics;
using AncientsAwakened.AncientsAwakenedCode.UI;
using BaseLib.Abstracts;
using BaseLib.Extensions;
using BaseLib.Utils;
using Godot;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Events;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Characters;
using MegaCrit.Sts2.Core.Models.Relics;

namespace AncientsAwakened.AncientsAwakenedCode.Ancients;


public class SebastianAncient : CustomAncientModel
{
    protected override OptionPools MakeOptionPools =>

        new(
            MakePool(
                AncientOption<FlashBeacon>(3, flash =>
                {
                    Log.Info("flash beacon prep start");
                    if (Owner != null)
                    {
                        Log.Info("flash beacon owner nullcheck");
                    }
                    return flash;
                }),
                AncientOption<WildlifeDocuments>(2),
                AncientOption<MedicalKit>(2)
            ),
            MakePool(
                AncientOption<SebbyCharm>(4),
                AncientOption<SebastiansScanner>(3),
                AncientOption<SalineInfuser>(2),
                AncientOption<ShippingRequest>(2)
            ),
            MakePool(
                AncientOption<ShotgunShells>(3),
                AncientOption<GlowingVial>(1),
                AncientOption<ExperimentalSerum>(5, serum =>
                {
                    if (Owner != null)
                    {
                        Log.Info("relic prepped");
                        serum.SetupForPlayer(Owner);
                    }
                    return serum;
                })
            ));

    public override bool ShouldForceSpawn(ActModel act, AncientEventModel? rngChosenAncient)
    {
        return false;
        //return act.ActNumber() == 2;
    }

    public override Color ButtonColor => new(0.05f, 0.05f, 0.15f, 0.8f);

    public override Color DialogueColor => new("161430");
    
    public override bool IsValidForAct(ActModel act)
    {
        return act.ActNumber() == 2 && AncientConfigs.EnableSebastianAncient;
    }
}