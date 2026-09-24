using System.Reflection;
using AncientsAwakened.AncientsAwakenedCode.Relics.Gaster;
using AncientsAwakened.AncientsAwakenedCode.UI;
using HarmonyLib;
using MegaCrit.Sts2.Core.Map;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Random;
using MegaCrit.Sts2.Core.Runs;
using MegaCrit.Sts2.Core.Unlocks;

namespace AncientsAwakened.AncientsAwakenedCode.Patches;

public static class ActModelPatch
{
    [HarmonyPatch(typeof(ActModel), nameof(ActModel.GetNumberOfRooms))]
    public class GetNumberOfRoomsPatch
    {
        public static int Postfix(int __result)
        {
            var relicCount = RunManager.Instance.State?.Players.Sum(player => player.Relics.Count(r => r is DarkSpark));
            if (relicCount is <= 0 or null)
                return __result;
            if (!AncientConfigs.UncapDarkSpark)
                relicCount = int.Min((int) relicCount, 9);
            return __result + (int) relicCount * DarkSpark.AdditionalFloors;
        }
    }
    
    [HarmonyPatch]
    public static class GetMapPointTypesPatch
    {
        [HarmonyTargetMethods]
        public static IEnumerable<MethodBase> TargetMethods()
        {
            var abstractMethod = AccessTools.DeclaredMethod(
                typeof(ActModel),
                nameof(ActModel.GetMapPointTypes),
                [typeof(Rng)]);

            return AccessTools.AllTypes()
                .Where(type =>
                    type != typeof(ActModel) &&
                    typeof(ActModel).IsAssignableFrom(type))
                .Select(type => AccessTools.Method(
                    type,
                    nameof(ActModel.GetMapPointTypes),
                    [typeof(Rng)]))
                .Where(method =>
                    method is not null &&
                    !method.IsAbstract &&
                    method.GetBaseDefinition() == abstractMethod)
                .Distinct();
        }

        [HarmonyPostfix]
        private static MapPointTypeCounts Postfix(
            MapPointTypeCounts __result,
            ActModel __instance)
        {
            var relicCount = RunManager.Instance.State?.Players.Sum(player => player.Relics.Count(r => r is DarkSpark));
            if (relicCount is <= 0 or null)
                return __result;
            var newCounts = new CustomMapPointTypeCounts(__result, (int) relicCount);
            Traverse.Create(newCounts).Field("<"+nameof(MapPointTypeCounts.NumOfShops)+">k__BackingField").SetValue(__result.NumOfShops + 1);
            return newCounts;
        }
    }

    private class CustomMapPointTypeCounts : MapPointTypeCounts
    {
        public CustomMapPointTypeCounts(MapPointTypeCounts typeCounts, int relicCount) : base(typeCounts.NumOfUnknowns + relicCount * 2, typeCounts.NumOfRests + relicCount)
        {
            NumOfElites = typeCounts.NumOfElites + relicCount;
        }
    }
    
}