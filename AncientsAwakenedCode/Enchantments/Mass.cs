using BaseLib.Abstracts;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Enchantments;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;

namespace AncientsAwakened.AncientsAwakenedCode.Enchantments;

public class Mass : CustomEnchantmentModel
{

    public override Task AfterCardPlayed(PlayerChoiceContext context, CardPlay cardPlay)
    {
        if (cardPlay.Card != Card)
            return Task.CompletedTask;

        Status = EnchantmentStatus.Disabled;
        return Task.CompletedTask;
    }

    protected override void OnEnchant()
    {
        Card.EnergyCost.AddUntilPlayed(-1, true);
    }
    
}