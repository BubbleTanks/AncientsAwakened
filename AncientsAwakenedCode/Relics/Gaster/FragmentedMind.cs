using BaseLib.Utils;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Models.RelicPools;

namespace AncientsAwakened.AncientsAwakenedCode.Relics.Gaster;

[Pool(typeof(EventRelicPool))]
public sealed class FragmentedMind : AncientsAwakenedRelic
{
    public override RelicRarity Rarity => RelicRarity.Ancient;
    
    public override bool HasUponPickupEffect => true;

    public override async Task AfterObtained()
    {
        var prefs = new CardSelectorPrefs(SelectionScreenPrompt, 1, 1)
        {
            Cancelable = false,
            RequireManualConfirmation = true
        };

        foreach (var card in await CardSelectCmd.FromDeckGeneric(Owner, prefs))
        {
            var clonedCard = Owner.RunState.CloneCard(card);
            CardCmd.Upgrade(clonedCard);
            CardCmd.PreviewCardPileAdd(await CardPileCmd.Add(clonedCard, PileType.Deck));
        }
    }
}