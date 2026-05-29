using AncientsAwakened.AncientsAwakenedCode.Cards.Meloetta;
using AncientsAwakened.AncientsAwakenedCode.Relics;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models.RelicPools;

namespace AncientsAwakened.AncientsAwakenedCode.Relics.Meloetta;


[Pool(typeof(EventRelicPool))]
public class MelodyBerryBasket() : AncientsAwakenedRelic
{
    public override RelicRarity Rarity => RelicRarity.Ancient;

    protected override IEnumerable<IHoverTip> ExtraHoverTips => HoverTipFactory.FromCardWithCardHoverTips<MelodyBerry>();
    
    public override bool HasUponPickupEffect => true;
    
    public override async Task AfterObtained()
    {
        CardCmd.PreviewCardPileAdd([await CardPileCmd.Add(Owner.RunState.CreateCard<MelodyBerry>(Owner), PileType.Deck)], 2F);
        CardCmd.PreviewCardPileAdd([await CardPileCmd.Add(Owner.RunState.CreateCard<MelodyBerry>(Owner), PileType.Deck)], 2F);
        CardCmd.PreviewCardPileAdd([await CardPileCmd.Add(Owner.RunState.CreateCard<MelodyBerry>(Owner), PileType.Deck)], 2F);
        CardCmd.PreviewCardPileAdd([await CardPileCmd.Add(Owner.RunState.CreateCard<MelodyBerry>(Owner), PileType.Deck)], 2F);
        CardCmd.PreviewCardPileAdd([await CardPileCmd.Add(Owner.RunState.CreateCard<MelodyBerry>(Owner), PileType.Deck)], 2F);
    }
    
}