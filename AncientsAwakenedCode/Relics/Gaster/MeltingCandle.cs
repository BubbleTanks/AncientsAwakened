using AncientsAwakened.AncientsAwakenedCode.Potions.Gaster;
using AncientsAwakened.AncientsAwakenedCode.Relics;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Factories;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;

namespace AncientsAwakened.AncientsAwakenedCode.Relics.Gaster;

public sealed class MeltingCandle : AncientsAwakenedRelic
{
    public override RelicRarity Rarity =>
        RelicRarity.Ancient;

    protected override IEnumerable<DynamicVar> CanonicalVars => [new CardsVar(2)];
    
    public override async Task AfterObtained()
    {
        CardSelectorPrefs prefs = new CardSelectorPrefs(CardSelectorPrefs.TransformSelectionPrompt, DynamicVars.Cards.IntValue);
        foreach (CardModel original in (await CardSelectCmd.FromDeckForTransformation(Owner, prefs)).ToList())
        {
            CardModel cardForTransform = CardFactory.CreateRandomCardForTransform(original, false, Owner.RunState.Rng.Niche);
            CardCmd.Upgrade(cardForTransform);
            await CardCmd.Transform(original, cardForTransform);
        }
        
        await PotionCmd.TryToProcure<HotWax>(Owner);
        await PotionCmd.TryToProcure<HotWax>(Owner);
    }
}