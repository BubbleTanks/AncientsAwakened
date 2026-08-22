using BaseLib.Utils;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.RelicPools;

namespace AncientsAwakened.AncientsAwakenedCode.Relics.Leshy;

[Pool(typeof(EventRelicPool))]
public class Goobert : AncientsAwakenedRelic
{
    public override RelicRarity Rarity => RelicRarity.Ancient;
    
    public override bool HasUponPickupEffect => true;

    protected override bool RelicAllowedToSpawn(Player owner)
    {
        return owner.Deck.Cards.Count >= 3;
    }
    
    protected override IEnumerable<DynamicVar> CanonicalVars => [new CardsVar(2)];

    public override async Task AfterObtained()
    {
        var prefs = new CardSelectorPrefs(SelectionScreenPrompt, 0, DynamicVars.Cards.IntValue)
        {
            Cancelable = false,
            RequireManualConfirmation = true
        };

        foreach (var card in await CardSelectCmd.FromDeckGeneric(Owner, prefs))
        {
            CardCmd.PreviewCardPileAdd(await CardPileCmd.Add(Owner.RunState.CloneCard(card), PileType.Deck));
        }
    }
}