using AncientsAwakened.AncientsAwakenedCode.Patches;
using BaseLib.Common.Rewards.LinkedRewardSet;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.RelicPools;
using MegaCrit.Sts2.Core.Rewards;

namespace AncientsAwakened.AncientsAwakenedCode.Relics.Gaster;

[Pool(typeof(EventRelicPool))]
public sealed class StrangeKey : AncientsAwakenedRelic
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new MaxHpVar(15M), new RepeatVar(3)];

    protected override IEnumerable<IHoverTip> ExtraHoverTips => [];

    public override RelicRarity Rarity => RelicRarity.Ancient;

    public override async Task AfterObtained()
    {
        await CreatureCmd.LoseMaxHp(new ThrowingPlayerChoiceContext(), Owner.Creature, DynamicVars.MaxHp.BaseValue, false);

        List<Reward> rewards = [];
        for (var i = 0; i < DynamicVars.Repeat.BaseValue; i++)
        {
            rewards.Add(new RelicReward(RelicRarity.Rare, Owner));
        }
        await RewardsCmd.OfferCustom(Owner, [new CustomLinkedRewardSet(rewards, Owner)]);
    }
}