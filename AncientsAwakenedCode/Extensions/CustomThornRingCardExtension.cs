using MegaCrit.Sts2.Core.Models;

namespace AncientsAwakened.AncientsAwakenedCode.Extensions;

public static class CustomThornRingCardExtension
{
    internal static readonly Dictionary<ModelId, ModelId> CustomThornRingRelics = [];
    
    /// <summary>
    /// Call this extension in the starter relic's constructor (with an Interop for an optional dependency), along with the replacement relic's ModelId to make custom Thorn Ring cards.
    /// </summary>
    public static void AddPerfectedCardForCustomCharacters(this RelicModel originalRelic, RelicModel replacementRelic)
    {
        if (CustomThornRingRelics.ContainsValue(originalRelic.Id))
        {
            AncientsAwakenedMain.Logger.Warn($"Relic {originalRelic.Id} attempted to be added multiple times to Shadow Crystal cards dictionary.");
        }
        CustomThornRingRelics[originalRelic.Id] = replacementRelic.Id;
    }
}