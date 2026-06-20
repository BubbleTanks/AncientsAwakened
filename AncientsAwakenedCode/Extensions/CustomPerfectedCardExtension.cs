using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Models;

namespace AncientsAwakened.AncientsAwakenedCode.Extensions;

public static class CustomPerfectedCardExtension
{
    internal static readonly Dictionary<ModelId, ModelId> CustomPerfectedStrikeCards = [];
    internal static readonly Dictionary<ModelId, ModelId> CustomPerfectedDefendCards = [];
    
    /// <summary>
    /// Call this extension in your card constructor (with an Interop for an optional dependency), along with the character ModelId to make custom perfected cards.
    /// </summary>
    public static void AddPerfectedCardForCustomCharacters(this CardModel cardModel, CharacterModel characterModel)
    {
        if (cardModel.Tags.Contains(CardTag.Strike))
        {
            if (CustomPerfectedStrikeCards.ContainsValue(characterModel.Id))
            {
                AncientsAwakenedMain.Logger.Warn($"Character {characterModel.Id} attempted to add card multiple times to perfected strike list.");
            }
            CustomPerfectedStrikeCards[characterModel.Id] = cardModel.Id;
        } else if (cardModel.Tags.Contains(CardTag.Defend))
        {
            if (CustomPerfectedDefendCards.ContainsValue(characterModel.Id))
            {
                AncientsAwakenedMain.Logger.Warn($"Character {characterModel.Id} attempted to add card multiple times to perfected defend list.");
            }
            CustomPerfectedDefendCards[characterModel.Id] = cardModel.Id;
        }
        else
        {
            AncientsAwakenedMain.Logger.Warn($"Character {characterModel.Id} attempted to add perfected card that is neither strike or defend.");
        }
    }
}