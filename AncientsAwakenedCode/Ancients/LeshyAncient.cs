using AncientsAwakened.AncientsAwakenedCode.Relics.Leshy;
using AncientsAwakened.AncientsAwakenedCode.UI;
using BaseLib.Abstracts;
using BaseLib.Extensions;
using BaseLib.Utils;
using Godot;
using MegaCrit.Sts2.Core.Models;

namespace AncientsAwakened.AncientsAwakenedCode.Ancients;


public class LeshyAncient : CustomAncientModel
{
    protected override OptionPools MakeOptionPools =>

        new(
            MakePool(
                AncientOption<SquirrelInABottle>(),
                AncientOption<PackRat>(),
                AncientOption<BoneLordBoon>()
            ),
            MakePool(
                AncientOption<TheSmoke>(), 
                AncientOption<CarvingKnife>(),
                AncientOption<FishHook>(),
                AncientOption<ProspectingPick>()
            ),
            MakePool(
                AncientOption<FilmRoll>(),
                AncientOption<TheMoon>(),
                AncientOption<Goobert>()
            ));
    
    public override Color ButtonColor => new(0.15f, 0.04f, 0.07f, 0.8f);

    public override Color DialogueColor => new("693019");
    
    public override bool IsValidForAct(ActModel act)
    {
        return act.ActNumber() == 2;
    }
}