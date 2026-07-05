using AncientsAwakened.AncientsAwakenedCode.Powers.LunaticCultist;
using BaseLib.Extensions;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.CardPools;

namespace AncientsAwakened.AncientsAwakenedCode.Cards.LunaticCultist;

[Pool(typeof(EventCardPool))]
public class GuidanceChoice() : AncientsAwakenedCard(-1, CardType.Power, CardRarity.Ancient, TargetType.None), Starshine.ICardChoice
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new PowerVar<GuidancePower>(1)];
    
    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        await OnChosen();
    }

    public async Task OnChosen()
    {
        await CreatureCmd.TriggerAnim(Owner.Creature, "PowerUp", Owner.Character.PowerUpAnimDelay);
        await PowerCmd.Apply<GuidancePower>(new ThrowingPlayerChoiceContext(), Owner.Creature, DynamicVars.Power<GuidancePower>().BaseValue, Owner.Creature, this); 
    }
}