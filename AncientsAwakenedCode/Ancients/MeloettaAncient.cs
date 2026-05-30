using AncientsAwakened.AncientsAwakenedCode.Relics;
using AncientsAwakened.AncientsAwakenedCode.Relics.Meloetta;
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

public class MeloettaAncient : CustomAncientModel
{
    protected override OptionPools MakeOptionPools =>

        new(
            MakePool(
                AncientOption<MelodyBerryBasket>(),
                AncientOption<Harmonizer>()
            ),
            MakePool(
                AncientOption<AncientLullaby>(),
                AncientOption<TrebleClef>()
            ),
            MakePool(
                AncientOption<SepiaPhotograph>(),
                AncientOption<RelicSong>()
            ));
    
    public override Color ButtonColor => new(0.17f, 0.45f, 0.21f, 0.8f);

    public override Color DialogueColor => new("75e07f");
    
    public override bool IsValidForAct(ActModel act)
    {
        return act.ActNumber() == 2 && AncientConfigs.EnableMeloettaAncient;
    }
    
    public override bool ShouldForceSpawn(ActModel act, AncientEventModel? rngChosenAncient)
    {
        return AncientConfigs.ForceMeloettaEnabler && act.ActNumber() == 2;
    }
}