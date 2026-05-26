using AncientsAwakened.AncientsAwakenedCode.Relics;
using BaseLib.Abstracts;
using BaseLib.Extensions;
using BaseLib.Utils;
using Godot;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Events;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Relics;

namespace AncientsAwakened.AncientsAwakenedCode.Ancients;

public class MountainAncient : CustomAncientModel
{
    protected override OptionPools MakeOptionPools =>

        new(
            MakePool(
                AncientOption<SquirrelInABottle>(),
                AncientOption<PackRat>()
            ),
            MakePool(
                AncientOption<TheSmoke>(), 
                AncientOption<ProspectingPick>()
            ),
            MakePool(
                AncientOption<FilmRoll>(),
                AncientOption<Goobert>()
            ));
    
    public override Color ButtonColor => new(0.15f, 0.04f, 0.07f, 0.8f);

    public override Color DialogueColor => new("693019");
    
    public override bool IsValidForAct(ActModel act)
    {
        return false;
        return act.ActNumber() == 2;
    }
    
    public override bool ShouldForceSpawn(ActModel act, AncientEventModel? rngChosenAncient)
    {
        return false;
        //return act.ActNumber() == 2;
    }

    public override IEnumerable<EventOption> AllPossibleOptions => [
    ];
}