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
public sealed class NocturneCommute() : AncientsAwakenedCard(
    2, CardType.Power, CardRarity.Ancient, 
    TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new PowerVar<BufferPower>(1), new PowerVar<DoubleDamagePower>(1)];
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<BufferPower>()];
    
    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay cardPlay)
    {
        await PowerCmd.Apply<BufferPower>(choiceContext, Owner.Creature, DynamicVars.Power<BufferPower>().BaseValue, Owner.Creature, this);
        await PowerCmd.Apply<DoubleDamagePower>(choiceContext, Owner.Creature, DynamicVars.Power<DoubleDamagePower>().BaseValue, Owner.Creature, this);
    }

    protected override void OnUpgrade()
    {
        AddKeyword(CardKeyword.Retain);
    }
    
}