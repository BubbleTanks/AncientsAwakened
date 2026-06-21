using System.Reflection;
using BaseLib.Utils.ModInterop;
using MegaCrit.Sts2.Core.Models;

namespace AncientsAwakened.AncientsAwakenedCode.Interops;

[ModInterop("AncientConfigsPlus", "AncientConfigsPlus.AncientConfigsPlusCode.AncientConfigsPlusConfig")]
public static class AncientConfigsPlusInterop
{
    public static string EnabledAct1 { get; set; } = null;
    
    public static Dictionary<string, int> ParseWeights(int slot)
    {
        return null;
    }
}