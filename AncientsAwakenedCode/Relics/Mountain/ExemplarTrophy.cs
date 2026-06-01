using AncientsAwakened.AncientsAwakenedCode.Cards.Mountain;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.RelicPools;
using MegaCrit.Sts2.Core.Rewards;

namespace AncientsAwakened.AncientsAwakenedCode.Relics.Mountain;

[Pool(typeof(EventRelicPool))]
public class ExemplarTrophy : AncientsAwakenedRelic
{
    public override RelicRarity Rarity => RelicRarity.Ancient;
    
    protected override IEnumerable<DynamicVar> CanonicalVars => [new ("Relics",7M)];
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromCard<Stress>()];
    
    public override async Task AfterObtained()
    {
        await CardPileCmd.AddCurseToDeck<Stress>(Owner);
        await Cmd.Wait(0.75f);
        await RewardsCmd.OfferCustom(Owner, GenerateRewards());
    }

    private List<Reward> GenerateRewards()
    {
        List<Reward> list = [];
        for (var i = 0; i < DynamicVars["Relics"].IntValue; ++i)
        {
            list.Add(new RelicReward(RelicRarity.Common, Owner));
        }
        return list;
    }
}