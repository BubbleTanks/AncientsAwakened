using MegaCrit.Sts2.Core.Models;

namespace AncientsAwakened.AncientsAwakenedCode.Extensions;

public static class CustomExperimentalCardExtension
{
    internal static readonly Dictionary<ModelId, ModelId> CustomExperimentalCards = [];
    
    /// <summary>
    /// Call this extension in your card constructor (with an Interop for an optional dependency), along with the character ModelId to add a custom Experimental Card.
    /// </summary>
    public static void AddExperimentalCardForCustomCharacters(this CardModel cardModel, CharacterModel characterModel)
    {
        if (CustomExperimentalCards.ContainsKey(characterModel.Id))
        {
            AncientsAwakenedMain.Logger.Warn($"Character {characterModel.Id} attempted to add card multiple times to Experimental Serum list.");
        }
        CustomExperimentalCards[characterModel.Id] = cardModel.Id;
    }
}