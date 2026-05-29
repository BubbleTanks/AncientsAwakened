using AncientsAwakened.AncientsAwakenedCode.Relics;
using AncientsAwakened.AncientsAwakenedCode.Relics.Leshy;
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
    
    public override Color ButtonColor => new(0.05f, 0.05f, 0.05f, 0.8f);

    public override Color DialogueColor => new("060606");
    
    public override bool IsValidForAct(ActModel act)
    {
        return act.ActNumber() == 3 && AncientConfigs.EnableMountainAncient;
    }
    
    public override bool ShouldForceSpawn(ActModel act, AncientEventModel? rngChosenAncient)
    {
        return AncientConfigs.ForceMountainEnabler && act.ActNumber() == 3;
    }

    public override IEnumerable<EventOption> AllPossibleOptions => [
    ];
}