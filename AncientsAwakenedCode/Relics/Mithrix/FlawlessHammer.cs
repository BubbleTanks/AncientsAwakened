using AncientsAwakened.AncientsAwakenedCode.Cards;
using AncientsAwakened.AncientsAwakenedCode.Cards.Mithrix;
using AncientsAwakened.AncientsAwakenedCode.UI;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models.RelicPools;
using MegaCrit.Sts2.Core.Runs;

namespace AncientsAwakened.AncientsAwakenedCode.Relics;


[Pool(typeof(EventRelicPool))]
public class FlawlessHammer : AncientsAwakenedRelic
{
    public override RelicRarity Rarity => RelicRarity.Ancient;
    
    protected override IEnumerable<IHoverTip> ExtraHoverTips => HoverTipFactory.FromCardWithCardHoverTips<BreakBeneathMe>();

    public override bool HasUponPickupEffect => true;
    
    public override bool IsAllowed(IRunState runState)
    {
        return runState.Players.Count == 1 || AncientConfigs.MultiplayerFlawlessHammer;
    }

    public override async Task AfterObtained()
    {
        CardCmd.PreviewCardPileAdd([await CardPileCmd.Add(Owner.RunState.CreateCard<BreakBeneathMe>(Owner), PileType.Deck)], 2F);
    }
}