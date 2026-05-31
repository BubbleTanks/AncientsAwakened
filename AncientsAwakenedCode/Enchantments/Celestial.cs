using BaseLib.Abstracts;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Enchantments;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

namespace AncientsAwakened.AncientsAwakenedCode.Enchantments;

public class Celestial : CustomEnchantmentModel
{
    public override bool HasExtraCardText => true;

    public override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay? cardPlay)
    {
        if (Status != EnchantmentStatus.Normal)
            return;
        await PlayerCmd.GainEnergy(Amount, Card.Owner);
    }
}