using System.Reflection;
using System.Reflection.Emit;
using BaseLib.Utils;
using HarmonyLib;
using MegaCrit.Sts2.Core.Factories;
using MegaCrit.Sts2.Core.Models;

namespace AncientsAwakened.AncientsAwakenedCode.Patches;

public class NoncurseToCurseTransformPatch
{
    
    public static readonly SpireField<CardModel, bool> CursableField = new(() => false);
    
    [HarmonyPatch(typeof(CardFactory), nameof(CardFactory.GetFilteredTransformationOptions))]
    public class CardFactoryTransformFilterPatch
    {
        static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            var codeMatcher = new CodeMatcher(instructions);
            MethodInfo check = AccessTools.Method(typeof(CardFactoryTransformFilterPatch), nameof(CheckForField));

            codeMatcher.MatchStartForward(
                    new CodeMatch(OpCodes.Ldloc_S),
                    new CodeMatch(OpCodes.Brtrue_S)
                )
                .ThrowIfInvalid("Could not find flag check")
                .Advance()
                .InsertAndAdvance(
                    new CodeInstruction(OpCodes.Ldarg_0),
                    new CodeInstruction(OpCodes.Call, check),
                    new CodeInstruction(OpCodes.Stloc_S, 4),
                    new CodeInstruction(OpCodes.Ldloc_S, 4)
                );
            
            return codeMatcher.InstructionEnumeration();
        }

        static bool CheckForField(bool init, CardModel cardModel)
        {
            return !init ? CursableField.Get(cardModel) : init;
        }
    }
}