using AncientsAwakened.AncientsAwakenedCode.Cards.Mountain;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.RelicPools;
using MegaCrit.Sts2.Core.Rewards;

namespace AncientsAwakened.AncientsAwakenedCode.Relics.Mountain;

[Pool(typeof(EventRelicPool))]
public class DeaconFlesh : AncientsAwakenedRelic
{
    public override RelicRarity Rarity => RelicRarity.Ancient;
    
    protected override IEnumerable<DynamicVar> CanonicalVars => [new CardsVar(2)];
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromCard<Stress>()];
    
    public override async Task AfterObtained()
    {
        List<CardPileAddResult> stressResults = [];
        for (int i = 0; i < DynamicVars.Cards.IntValue; ++i)
            stressResults.Add(await CardPileCmd.Add(Owner.RunState.CreateCard(ModelDb.Card<Stress>(), Owner), PileType.Deck));
        CardCmd.PreviewCardPileAdd(stressResults, 2f);
        await Cmd.Wait(0.75f);
        await RewardsCmd.OfferCustom(Owner, GenerateRewards());
    }

    private List<Reward> GenerateRewards()
    {
        List<Reward> list = [];
        foreach (var relic in Owner.Relics.Where(r => r.Rarity != RelicRarity.Ancient))
        {
            list.Add(new RelicReward(relic.CanonicalInstance.ToMutable(), Owner));
        }
        return list;
    }
}