using System.Reflection;
using BaseLib.Utils.ModInterop;
using MegaCrit.Sts2.Core.Models;

namespace AncientsAwakened.AncientsAwakenedCode.Interops;

[ModInterop("AncientConfigsPlus", "AncientConfigsPlus.AncientConfigsPlusCode.AncientConfigsPlusConfig")]
public static class AncientConfigsPlusInterop
{
    public static Dictionary<int, PropertyInfo> SlotProps { get; }

    public static Dictionary<string, decimal> ParseWeights(int slot)
    {
        return null;
    }
    
    public static List<AncientEventModel> GetAncientsForSlot(int slot)
    {
        
        return null;
    }
}