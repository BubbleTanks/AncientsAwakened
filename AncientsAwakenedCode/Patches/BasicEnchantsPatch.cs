using System.Reflection.Emit;
using System.Text.RegularExpressions;
using AncientsAwakened.AncientsAwakenedCode.Pools;
using Godot.Bridge;
using HarmonyLib;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Enchantments;
using MethodInfo = System.Reflection.MethodInfo;

namespace AncientsAwakened.AncientsAwakenedCode.Patches;

public class BasicEnchantsPatch
{

    [HarmonyPatch(typeof(Spiral), "CanEnchant")]
    public class PatchSpiral
    {

        public static bool Prefix(ref bool __result, CardModel c)
        {
            if (c.VisualCardPool is PerfectedPool && c.Enchantment == null)
            {
                __result = true;
                return false;
            }
            return true;
        }
    }
    
    [HarmonyPatch(typeof(Goopy), "CanEnchant")]
    public class PatchGoopy
    {

        public static bool Prefix(ref bool __result, CardModel card)
        {
            if (card.VisualCardPool is PerfectedPool && card.Enchantment == null)
            {
                __result = true;
                return false;
            }
            return true;
        }
    }
}