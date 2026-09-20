using MegaCrit.Sts2.Core.Models;

namespace AncientsAwakened.AncientsAwakenedCode.Extensions;

public static class CustomShadowCrystalCardExtension
{
    internal static readonly Dictionary<ModelId, ModelId> CustomShadowCrystalCards = [];
    
    /// <summary>
    /// Call this extension in the original card's constructor (with an Interop for an optional dependency), along with the replacement card's ModelId to make custom Shadow Crystal cards.
    /// Note that this will not correctly change the visual pool, you will have to do that manually (Currently not easily done through interops).
    /// </summary>
    public static void AddPerfectedCardForCustomCharacters(this CardModel originalCard, CardModel replacementCard)
    {
        if (CustomShadowCrystalCards.ContainsValue(originalCard.Id))
        {
            AncientsAwakenedMain.Logger.Warn($"Card {originalCard.Id} attempted to be added multiple times to Shadow Crystal cards dictionary.");
        }
        CustomShadowCrystalCards[originalCard.Id] = replacementCard.Id;
    }
}