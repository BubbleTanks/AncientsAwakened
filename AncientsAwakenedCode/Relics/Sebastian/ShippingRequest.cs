using AncientsAwakened.AncientsAwakenedCode.Cards.Sebastian;
using AncientsAwakened.AncientsAwakenedCode.Extensions;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models.RelicPools;

namespace AncientsAwakened.AncientsAwakenedCode.Relics.Sebastian;

[Pool(typeof(EventRelicPool))]
public sealed class ShippingRequest : AncientsAwakenedRelic
{
    public override RelicRarity Rarity => RelicRarity.Ancient;
    
    public override bool HasUponPickupEffect => true;
    
    protected override IEnumerable<IHoverTip> ExtraHoverTips => HoverTipFactory.FromCardWithCardHoverTips<WeighedDown>().Concat(HoverTipFactory.FromCardWithCardHoverTips<HeavyCrate>());
    
    public ShippingRequest()
    {
        this.BlacklistFromEulogy();
    }

    public override async Task AfterObtained()
    {
        var list = new List<CardPileAddResult>
        {
            await CardPileCmd.Add(Owner.RunState.CreateCard<WeighedDown>(Owner), PileType.Deck)
        };
        var result = await CardPileCmd.Add(Owner.RunState.CreateCard<HeavyCrate>(Owner), PileType.Deck);
        list.Add(result);
        ((HeavyCrate)result.cardAdded).InitializeRewards();
        CardCmd.PreviewCardPileAdd(list, 2F);
    }
}