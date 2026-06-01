using AncientsAwakened.AncientsAwakenedCode.Cards.Meloetta;
using AncientsAwakened.AncientsAwakenedCode.Relics;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.RelicPools;

namespace AncientsAwakened.AncientsAwakenedCode.Relics.Meloetta;

[Pool(typeof(EventRelicPool))]
public class MelodyBerryBasket() : AncientsAwakenedRelic
{
    public override RelicRarity Rarity => RelicRarity.Ancient;
    protected override IEnumerable<DynamicVar> CanonicalVars => [new CardsVar(5)];
    protected override IEnumerable<IHoverTip> ExtraHoverTips => HoverTipFactory.FromCardWithCardHoverTips<MelodyBerry>();
    public override bool HasUponPickupEffect => true;
    
    public override async Task AfterObtained()
    {
        List<CardPileAddResult> results = new List<CardPileAddResult>();
        for (int i = 0; i < DynamicVars.Cards.IntValue; ++i)
        {
            CardModel card = Owner.RunState.CreateCard<MelodyBerry>(Owner);
            List<CardPileAddResult> cardPileAddResultList = results;
            cardPileAddResultList.Add(await CardPileCmd.Add(card, PileType.Deck));
        }
        CardCmd.PreviewCardPileAdd(results, 2f);
    }
    
}