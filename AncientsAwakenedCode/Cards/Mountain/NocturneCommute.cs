using AncientsAwakened.AncientsAwakenedCode.Powers.Mountain;
using BaseLib.Extensions;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.CardPools;
using MegaCrit.Sts2.Core.Models.Powers;

namespace AncientsAwakened.AncientsAwakenedCode.Cards.Mountain;

[Pool(typeof(EventCardPool))]
public class NocturneCommute() : AncientsAwakenedCard(
    2, CardType.Power, CardRarity.Ancient, 
    TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new PowerVar<BufferPower>(1), new PowerVar<TripleDamagePower>(1)];
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<BufferPower>()];
    
    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay cardPlay)
    {
        await PowerCmd.Apply<BufferPower>(choiceContext, Owner.Creature, DynamicVars.Power<BufferPower>().BaseValue, Owner.Creature, this);
        await PowerCmd.Apply<TripleDamagePower>(choiceContext, Owner.Creature, DynamicVars.Power<TripleDamagePower>().BaseValue, Owner.Creature, this);
    }

    protected override void OnUpgrade()
    {
        AddKeyword(CardKeyword.Retain);
    }
    
}