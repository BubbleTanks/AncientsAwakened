using AncientsAwakened.AncientsAwakenedCode.Relics.Mountain;
using AncientsAwakened.AncientsAwakenedCode.UI;
using BaseLib.Abstracts;
using BaseLib.Extensions;
using BaseLib.Utils;
using Godot;
using MegaCrit.Sts2.Core.Models;

namespace AncientsAwakened.AncientsAwakenedCode.Ancients;

public class MountainAncient : CustomAncientModel
{
    protected override OptionPools MakeOptionPools =>

        new(
            MakePool(
                AncientOption<Loathing>(),
                AncientOption<InfernalFlame>(),
                AncientOption<EvangelistBlades>()
            ),
            MakePool(
                AncientOption<CardinalStar>(), 
                AncientOption<ExemplarTrophy>(),
                AncientOption<DeaconFlesh>()
            ),
            MakePool(
                AncientOption<IronCrown>(),
                AncientOption<EyeOfObsession>(),
                AncientOption<CursedCache>(),
                AncientOption<BloodstainedSnow>()
            ));
    
    public override Color ButtonColor => new(0.05f, 0.05f, 0.05f, 0.8f);

    public override Color DialogueColor => new("060606");
    
    public override bool IsValidForAct(ActModel act)
    {
        return act.ActNumber() == 3;
    }
}