using AncientsAwakened.AncientsAwakenedCode.Relics.LunaticCultist;
using AncientsAwakened.AncientsAwakenedCode.UI;
using BaseLib.Abstracts;
using BaseLib.Extensions;
using BaseLib.Utils;
using Godot;
using MegaCrit.Sts2.Core.Models;

namespace AncientsAwakened.AncientsAwakenedCode.Ancients;

public class LunaticCultistAncient : CustomAncientModel
{
    protected override OptionPools MakeOptionPools =>

        new(
            MakePool(
                AncientOption<RefinedChlorophyte>(),
                AncientOption<ManaFlower>(),
                AncientOption<FallenStar>()
            ),
            MakePool(
                AncientOption<SolarBrazier>(),
                AncientOption<VortexEye>(),
                AncientOption<NebulaTome>(),
                AncientOption<StardustShackles>()
            ),
            MakePool(
                AncientOption<MysteriousTablet>(),
                AncientOption<WhisperingTendrils>(),
                AncientOption<ShimmeringBottle>()
            ));
    
    public override Color ButtonColor => new(0.1f, 0.04f, 0.2f, 0.8f);

    public override Color DialogueColor => new("0d0f3b");
    
    public override bool IsValidForAct(ActModel act)
    {
        return act.ActNumber() == 3;
    }
}