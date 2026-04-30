using AncientsAwakened.AncientsAwakenedCode.Enchantments;
using AncientsAwakened.AncientsAwakenedCode.Relics;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.RelicPools;
using MegaCrit.Sts2.Core.Runs;

namespace AncientsAwakened.AncientsAwakenedCode.Relics;


[Pool(typeof(EventRelicPool))]
public class PillarOfMass : AncientsAwakenedRelic
{
    public override RelicRarity Rarity => RelicRarity.Ancient;

    protected override IEnumerable<IHoverTip> ExtraHoverTips
    {
        get => HoverTipFactory.FromEnchantment<Mass>();
    }

    public override bool TryModifyCardRewardOptionsLate(
        Player player,
        List<CardCreationResult> cardRewards,
        CardCreationOptions options)
    {
        if (player != Owner)
            return false;
        Mass mass = ModelDb.Enchantment<Mass>();
        foreach (CardCreationResult cardReward in cardRewards)
        {
            CardModel card1 = cardReward.Card;
            if (mass.CanEnchant(card1))
            {
                CardModel card2 = Owner.RunState.CloneCard(card1);
                CardCmd.Enchant<Mass>(card2, 1M);
                cardReward.ModifyCard(card2, this);
            }
        }
        return true;
    }
}