using System.Reflection;
using AncientsAwakened.AncientsAwakenedCode.Ancients;
using BaseLib.Abstracts;
using BaseLib.Extensions;
using HarmonyLib;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Unlocks;

namespace AncientsAwakened.AncientsAwakenedCode.Patches;

// Ripped directly from HadesAncients mod lmao. MegaCrit please make your Ancient code not god awful.
[HarmonyPatch]
public static class Act1AncientPatch
{
    private static readonly IReadOnlyList<ModelId> Act1AncientIds =
    [
        ModelDb.GetId<GasterAncient>()
    ];

    [HarmonyTargetMethods]
    public static IEnumerable<MethodBase> TargetMethods()
    {
        var abstractMethod = AccessTools.DeclaredMethod(
            typeof(ActModel),
            nameof(ActModel.GetUnlockedAncients),
            [typeof(UnlockState)]);

        return AccessTools.AllTypes()
            .Where(type =>
                type != typeof(ActModel) &&
                typeof(ActModel).IsAssignableFrom(type))
            .Select(type => AccessTools.Method(
                type,
                nameof(ActModel.GetUnlockedAncients),
                [typeof(UnlockState)]))
            .Where(method =>
                method is not null &&
                !method.IsAbstract &&
                method.GetBaseDefinition() == abstractMethod)
            .Distinct();
    }

    [HarmonyPostfix]
    private static IEnumerable<AncientEventModel> Postfix(
        IEnumerable<AncientEventModel> ancients,
        ActModel __instance)
    {
        if (__instance.ActNumber() != 1)
            return ancients;

        var result = ancients.ToList();
        result.AddRange(Act1AncientIds.Select(ModelDb.GetById<CustomAncientModel>).Where(ancient => ancient.IsValidForAct(__instance)));

        return result;
    }
}