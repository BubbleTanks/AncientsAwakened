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


public class MithrixAncient : CustomAncientModel
{
    protected override OptionPools MakeOptionPools =>

        new(
            OptionPool1,
            OptionPool2,
            OptionPool3);

    private WeightedList<AncientOption> OptionPool1 =>
    [
        AncientOption<SharedDesign>(3),
        AncientOption<ShapedGlass>(2),
        AncientOption<PillarOfMass>(3)
    ];
    
    private WeightedList<AncientOption> OptionPool2 =>
    [
        AncientOption<Starseed>(3), 
        AncientOption<FracturedBlade>(3),
        AncientOption<MonsoonCharm>(2)
    ];
    
    private WeightedList<AncientOption> OptionPool3
    {
        get
        {
            WeightedList<AncientOption> list = new WeightedList<AncientOption>();

            list.Add(AncientOption<FlawlessHammer>(), 3);
            list.Add(AncientOption<ArtifactOfCommand>(), 1);
            list.Add(AncientOption<EulogyZero>(), 2);

            if (((AncientScepter)ModelDb.Relic<AncientScepter>().ToMutable()).SetupForPlayer(Owner))
            {
                list.Add(AncientOption<AncientScepter>(), 3);
            }
            
            return list;
        }
    } 
    
    public override Color ButtonColor => new(0.05f, 0.07f, 0.2f, 0.8f);

    public override Color DialogueColor => new("384d7a");
    
    public override bool IsValidForAct(ActModel act)
    {
        return act.ActNumber() == 3;
    }

    public override IEnumerable<EventOption> AllPossibleOptions => [
        RelicOption<SharedDesign>(),
        RelicOption<ShapedGlass>(),
        RelicOption<PillarOfMass>(),
        RelicOption<Starseed>(),
        RelicOption<FracturedBlade>(),
        RelicOption<MonsoonCharm>(),
        RelicOption<FlawlessHammer>(),
        RelicOption<ArtifactOfCommand>(),
        RelicOption<AncientScepter>(),
        RelicOption<EulogyZero>()
    ];
}