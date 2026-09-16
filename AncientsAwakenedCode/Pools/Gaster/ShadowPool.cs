using BaseLib.Abstracts;
using Godot;
using MegaCrit.Sts2.Core.Models;

namespace AncientsAwakened.AncientsAwakenedCode.Pools.Gaster;

public class ShadowPool : CustomCardPoolModel
{
    protected override CardModel[] GenerateAllCards()
    {
        throw new NotImplementedException();
    }

    public override string Title => "shadow";
    public override string EnergyColorName => "colorless";
    public override Color DeckEntryCardColor => new Color("333333");
    public override bool IsColorless => false;

    public override float H => 0.5F;
    public override float S => 0.0F;
    public override float V => 0.2F;
}