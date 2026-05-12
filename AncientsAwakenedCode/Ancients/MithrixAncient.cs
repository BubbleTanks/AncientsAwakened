using AncientsAwakened.AncientsAwakenedCode.Relics;
using AncientsAwakened.AncientsAwakenedCode.UI;
using BaseLib.Abstracts;
using BaseLib.Extensions;
using BaseLib.Utils;
using Godot;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Events;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Relics;

namespace AncientsAwakened.AncientsAwakenedCode.Ancients;


public class MithrixAncient : CustomAncientModel
{
    protected override OptionPools MakeOptionPools =>

        new(
            MakePool(
                AncientOption<SharedDesign>(3),
                AncientOption<ShapedGlass>(2),
                AncientOption<PillarOfMass>(3)
            ),
            MakePool(
                AncientOption<Starseed>(3),
                AncientOption<FracturedBlade>(3),
                AncientOption<MonsoonCharm>(2)
            ),
            MakePool(
                AncientOption<FlawlessHammer>(3),
                AncientOption<ArtifactOfCommand>(1),
                AncientOption<EulogyZero>(2),
                AncientOption<AncientScepter>(3)
            ));
    
    public override Color ButtonColor => new(0.05f, 0.07f, 0.2f, 0.8f);

    public override Color DialogueColor => new("384d7a");
    
    public override bool ShouldForceSpawn(ActModel act, AncientEventModel? rngChosenAncient)
    {
        return AncientConfigs.EnableMithrixForcer;
    }
    
    public override bool IsValidForAct(ActModel act)
    {
        return act.ActNumber() == 3 && AncientConfigs.EnableMithrixAncient;
    }
}