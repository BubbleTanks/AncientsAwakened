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


public class SebastianAncient : CustomAncientModel
{
    protected override OptionPools MakeOptionPools =>

        new(
            OptionPool1,
            OptionPool2,
            OptionPool3);

    private WeightedList<AncientOption> OptionPool1 =>
    [
        AncientOption<Anchor>(),
        AncientOption<Anchor>(),
        AncientOption<Anchor>()
    ];
    
    private WeightedList<AncientOption> OptionPool2 =>
    [
        AncientOption<Anchor>(), 
        AncientOption<Anchor>(),
        AncientOption<Anchor>()
    ];
    
    private WeightedList<AncientOption> OptionPool3
    {
        get
        {
            WeightedList<AncientOption> list = new WeightedList<AncientOption>();

            list.Add(AncientOption<Anchor>());
            list.Add(AncientOption<Anchor>());
            list.Add(AncientOption<Anchor>());
            
            return list;
        }
    } 
    
    public override Color ButtonColor => new(0.05f, 0.05f, 0.15f, 0.8f);

    public override Color DialogueColor => new("161430");
    
    public override bool IsValidForAct(ActModel act)
    {
        return false;
        
        return act.ActNumber() == 2;
    }

    public override IEnumerable<EventOption> AllPossibleOptions => [
        RelicOption<Anchor>()
    ];
}