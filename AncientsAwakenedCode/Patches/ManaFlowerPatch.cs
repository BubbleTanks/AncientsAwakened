using BaseLib.Utils;
using HarmonyLib;
using MegaCrit.Sts2.Core.Models;

namespace AncientsAwakened.AncientsAwakenedCode.Patches;

public class ManaFlowerPatch
{
    
    public static readonly SpireField<PotionModel, bool> ManaFlowerPotionField = new(() => false);
    
    /*[HarmonyPatch(typeof(UsePotionAction), MethodType.Constructor, typeof(PotionModel), typeof(Creature), typeof(bool))]
    public class UsePotionActionPatch
    {
        static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            var codeMatcher = new CodeMatcher(instructions);
            MethodInfo check = AccessTools.Method(typeof(UsePotionActionPatch), nameof(CheckForManaFlower));

            codeMatcher.MatchStartForward(
                    new CodeMatch(OpCodes.Ldloc_0),
                    new CodeMatch(OpCodes.Ldc_I4_0)
                )
                .ThrowIfInvalid("Could not find UsePotionAction num check")
                .Advance(2)
                .InsertAndAdvance(
                    new CodeInstruction(OpCodes.Ldarg_1),
                    new CodeInstruction(OpCodes.Call, check),
                    new CodeInstruction(OpCodes.Ldc_I4_0)
                );
            
            return codeMatcher.InstructionEnumeration();
        }

        static int CheckForManaFlower(int i, int _, PotionModel potion)
        {
            return ManaFlowerPotionField.Get(potion) ? 1 : i;
        }
    }*/
    
    [HarmonyPatch(typeof(PotionModel), nameof(PotionModel.RemoveBeforeUse))]
    public class RemovePotionInternalPatch
    {
        public static bool Prefix(PotionModel __instance)
        {
            return !ManaFlowerPotionField.Get(__instance);
        }
    }
}