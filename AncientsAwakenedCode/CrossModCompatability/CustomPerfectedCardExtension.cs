using AncientsAwakened.AncientsAwakenedCode.Pools.Mithrix;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Models;

namespace AncientsAwakened.AncientsAwakenedCode.CrossModCompatability;

public static class CustomPerfectedCardExtension
{
    internal static readonly Dictionary<ModelId, ModelId> CustomPerfectedStrikeCards = [];
    internal static readonly Dictionary<ModelId, ModelId> CustomPerfectedDefendCards = [];
    
    /// <summary>
    /// Call this extension in your card constructor (with an Interop for an optional dependency), along with the original card's ModelId to make custom perfected cards.
    /// Note this will not alter the card's VisualCardPool, you will have to do that manually with the PerfectedCardPool method.
    /// Cards will still appear in the Compendium when the mod isn't loaded. You need a separate patch for this, which you can check the wiki for!
    /// </summary>
    public static void AddPerfectedCardForCustomCharacters(this CardModel cardModel, CardModel originalCardModel)
    {
        if (cardModel.Tags.Contains(CardTag.Strike))
        {
            if (CustomPerfectedStrikeCards.ContainsKey(originalCardModel.Id))
            {
                AncientsAwakenedMain.Logger.Warn($"Character {originalCardModel.Id} attempted to add card multiple times to perfected strike list.");
            }
            CustomPerfectedStrikeCards[originalCardModel.Id] = cardModel.Id;
        } else if (cardModel.Tags.Contains(CardTag.Defend))
        {
            if (CustomPerfectedDefendCards.ContainsKey(originalCardModel.Id))
            {
                AncientsAwakenedMain.Logger.Warn($"Character {originalCardModel.Id} attempted to add card multiple times to perfected defend list.");
            }
            CustomPerfectedDefendCards[originalCardModel.Id] = cardModel.Id;
        }
        else
        {
            AncientsAwakenedMain.Logger.Warn($"Character {originalCardModel.Id} attempted to add perfected card that is neither strike or defend.");
        }
    }

    /// <summary>
    /// A simple method that returns the PerfectedPool, allowing you to avoid complex code.
    /// Note that you need to define a fallback, whether you use the argument or make it return null.
    /// </summary>
    /// <param name="optionalFallBack">A dud argument that exists so that you may use it in your own code if the mod is not found.</param>
    /// <returns></returns>
    public static CardPoolModel? PerfectedCardPool(CardPoolModel? optionalFallBack)
    {
        return ModelDb.CardPool<PerfectedPool>();
    }
}