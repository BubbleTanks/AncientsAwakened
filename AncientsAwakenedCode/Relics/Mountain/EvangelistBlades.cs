using AncientsAwakened.AncientsAwakenedCode.Cards.Mountain;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models.RelicPools;

namespace AncientsAwakened.AncientsAwakenedCode.Relics.Mountain;

[Pool(typeof(EventRelicPool))]
public class EvangelistBlades : AncientsAwakenedRelic
{
    public override RelicRarity Rarity => RelicRarity.Ancient;
    
    public override bool HasUponPickupEffect => true;
    
    protected override IEnumerable<IHoverTip> ExtraHoverTips => HoverTipFactory.FromCardWithCardHoverTips<NocturneCommute>();

    public override async Task AfterObtained()
    {
        CardCmd.PreviewCardPileAdd([await CardPileCmd.Add(Owner.RunState.CreateCard<NocturneCommute>(Owner), PileType.Deck)], 2F);
    }
}