using AncientsAwakened.AncientsAwakenedCode.Relics.Meloetta;
using AncientsAwakened.AncientsAwakenedCode.UI;
using BaseLib.Abstracts;
using BaseLib.Extensions;
using BaseLib.Utils;
using Godot;
using MegaCrit.Sts2.Core.Models;

namespace AncientsAwakened.AncientsAwakenedCode.Ancients;

public class MeloettaAncient : CustomAncientModel
{
    protected override OptionPools MakeOptionPools =>

        new(
            MakePool(
                AncientOption<CardSleeve>(),
                AncientOption<AncientLullaby>(),
                AncientOption<TrebleClef>()
            ),
            MakePool(
                AncientOption<MelodyBerryBasket>(),
                AncientOption<SepiaPhotograph>(),
                AncientOption<RelicSong>()
            ),
            MakePool(
                AncientOption<AriaMastery>(),
                AncientOption<PirouetteMastery>(),
                AncientOption<Harmonizer>(2),
                AncientOption<SingerScarf>(2)
            ));
    
    public override Color ButtonColor => new(0.17f, 0.45f, 0.21f, 0.8f);

    public override Color DialogueColor => new("75e07f");
    
    public override bool IsValidForAct(ActModel act)
    {
        return act.ActNumber() == 2;
    }
}