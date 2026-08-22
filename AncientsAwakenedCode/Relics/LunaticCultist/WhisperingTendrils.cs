using AncientsAwakened.AncientsAwakenedCode.Cards.LunaticCultist;
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
    
    protected override IEnumerable<DynamicVar> CanonicalVars => [new CardsVar(10), new ("InsanityCount", 2)];
    
    protected override IEnumerable<IHoverTip> ExtraHoverTips => HoverTipFactory.FromCardWithCardHoverTips<Lunacy>(true).Concat(HoverTipFactory.FromCardWithCardHoverTips<Insanity>());
    
    public override async Task AfterObtained()
    {
        var prefs = new CardSelectorPrefs(SelectionScreenPrompt, 1, DynamicVars.Cards.IntValue)
        {
            RequireManualConfirmation = true,
        };
        List<CardModel> cardSelection = [];
        for (var i = 0; i < DynamicVars.Cards.IntValue; i++)
        {
            cardSelection.Add(ModelDb.Card<Lunacy>().ToMutable());
        }
        
        List<CardPileAddResult> results = [];
        foreach (var _ in await CardSelectCmd.FromSimpleGrid(new BlockingPlayerChoiceContext(),
                     cardSelection, Owner, prefs))
        {
            var c = Owner.RunState.CreateCard(ModelDb.Card<Lunacy>(), Owner);
            CardCmd.Upgrade(c, CardPreviewStyle.None);
            results.Add(await CardPileCmd.Add(c, PileType.Deck));
        }
        List<CardPileAddResult> curseResults = [];
        for (var i = 0; i < results.Count / DynamicVars["InsanityCount"].IntValue; i++)
        {
            curseResults.Add(await CardPileCmd.Add(Owner.RunState.CreateCard(ModelDb.Card<Insanity>(), Owner), PileType.Deck));
        }
        CardCmd.PreviewCardPileAdd(results, 2f);
        await Cmd.Wait(0.75f);
        CardCmd.PreviewCardPileAdd(curseResults, 2f);
        await Cmd.Wait(0.75f);
    }
}