using BaseLib.Abstracts;
using Godot;
using MegaCrit.Sts2.Core.Models;

namespace AncientsAwakened.AncientsAwakenedCode.Pools;

public class PerfectedPool : CustomCardPoolModel
{
    protected override CardModel[] GenerateAllCards()
    {
        throw new NotImplementedException();
    }

    public override string Title => "perfected";
    public override string EnergyColorName => "colorless";
    public override Color DeckEntryCardColor => new Color("00EEFF");
    public override bool IsColorless => false;

    public override float H => 0.5F;
    public override float S => 0.15F;
    public override float V => 2.5F;
}