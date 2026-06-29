using AncientsAwakened.AncientsAwakenedCode.Cards.LunaticCultist;
using AncientsAwakened.AncientsAwakenedCode.Relics.Mithrix;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.RelicPools;
using MegaCrit.Sts2.Core.Nodes.CommonUi;

namespace AncientsAwakened.AncientsAwakenedCode.Relics.LunaticCultist;

[Pool(typeof(EventRelicPool))]
public class WhisperingTendrils : AncientsAwakenedRelic
{
    public override RelicRarity Rarity => RelicRarity.Ancient;
    
    protected override IEnumerable<DynamicVar> CanonicalVars => [new CardsVar(10)];
    
    protected override IEnumerable<IHoverTip> ExtraHoverTips => HoverTipFactory.FromCardWithCardHoverTips<Lunacy>(true);
    
    public override async Task AfterObtained()
    {
        CardSelectorPrefs prefs = new CardSelectorPrefs(SelectionScreenPrompt, 1, DynamicVars.Cards.IntValue)
        {
            RequireManualConfirmation = true,
        };
        List<CardModel> cardSelection = [];
        for (int i = 0; i < DynamicVars.Cards.IntValue; i++)
        {
            cardSelection.Add(ModelDb.Card<Lunacy>().ToMutable());
        }

        List<CardPileAddResult> results = [];
        foreach (CardModel _ in await CardSelectCmd.FromSimpleGrid(new BlockingPlayerChoiceContext(),
                     cardSelection, Owner, prefs))
        {
            var c = await CardPileCmd.Add(Owner.RunState.CreateCard(ModelDb.Card<Lunacy>(), Owner), PileType.Deck);
            CardCmd.Upgrade(c.cardAdded, CardPreviewStyle.None);
            results.Add(c);
        }
        CardCmd.PreviewCardPileAdd(results, 2f);
        await Cmd.Wait(0.75f);
    }
}