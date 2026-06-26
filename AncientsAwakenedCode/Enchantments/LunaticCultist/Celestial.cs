using BaseLib.Abstracts;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;

namespace AncientsAwakened.AncientsAwakenedCode.Enchantments.LunaticCultist;

public class Celestial : CustomEnchantmentModel
{
    public override bool HasExtraCardText => true;

    public override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay? cardPlay)
    {
        await PlayerCmd.GainEnergy(Amount, Card.Owner);
    }
}