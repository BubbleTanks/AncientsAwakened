using AncientsAwakened.AncientsAwakenedCode.Relics;
using AncientsAwakened.AncientsAwakenedCode.Relics.Mithrix;
using AncientsAwakened.AncientsAwakenedCode.UI;
using BaseLib.Abstracts;
using BaseLib.Extensions;
using BaseLib.Utils;
using Godot;
using MegaCrit.Sts2.Core.Models;

namespace AncientsAwakened.AncientsAwakenedCode.Ancients;


public class MithrixAncient : CustomAncientModel
{
    protected override OptionPools MakeOptionPools =>

        new(
            MakePool(
                AncientOption<SharedDesign>(),
                AncientOption<ShapedGlass>(),
                AncientOption<PillarOfMass>()
            ),
            MakePool(
                AncientOption<Starseed>(3),
                AncientOption<FracturedBlade>(3),
                AncientOption<MonsoonCharm>(2)
            ),
            MakePool(
                AncientOption<FlawlessHammer>(3, hammer =>
                {
                    if(Owner != null)
                        hammer.SetupForPlayer(Owner);
                    return hammer;
                }),
                AncientOption<ArtifactOfCommand>(2),
                AncientOption<EulogyZero>(2),
                AncientOption<AncientScepter>(3, relic =>
                {
                    if (Owner != null)
                        relic.SetupForPlayer(Owner);
                    return relic;
                }))
            );
    
    public override Color ButtonColor => new(0.05f, 0.07f, 0.2f, 0.8f);

    public override Color DialogueColor => new("384d7a");
    
    public override bool IsValidForAct(ActModel act)
    {
        return act.ActNumber() == 3;
    }
}