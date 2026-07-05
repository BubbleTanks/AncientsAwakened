using BaseLib.Abstracts;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Enchantments;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models;

namespace AncientsAwakened.AncientsAwakenedCode.Enchantments.Leshy;

public class Smokey : CustomEnchantmentModel
{
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromKeyword(CardKeyword.Exhaust)];
    
    public override Task OnPlay(PlayerChoiceContext context, CardPlay? cardPlay)
    {
        if (Status != EnchantmentStatus.Normal)
            return Task.CompletedTask;
        Status = EnchantmentStatus.Disabled;
        CardCmd.ApplyKeyword(Card, CardKeyword.Exhaust);
        return Task.CompletedTask;
    }

    public override bool CanEnchant(CardModel card) => base.CanEnchant(card) && card.GetKeywordsWithSources(KeywordSources.Local).Contains(CardKeyword.Exhaust);
    protected override void OnEnchant() => CardCmd.RemoveKeyword(Card, CardKeyword.Exhaust);
}