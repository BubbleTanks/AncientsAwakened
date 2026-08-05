using AncientsAwakened.AncientsAwakenedCode.Powers.LunaticCultist;
using BaseLib.Extensions;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.CardPools;
using MegaCrit.Sts2.Core.Models.Powers;

namespace AncientsAwakened.AncientsAwakenedCode.Cards.LunaticCultist;

[Pool(typeof(EventCardPool))]
public class Starshine() : AncientsAwakenedCard(0,
    CardType.Skill, CardRarity.Ancient,
    TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new ("Exhaust", PowerChoice.ExhaustValue), new EnergyVar(EnergyChoice.EnergyValue), new CardsVar(EnergyChoice.DrawValue)];

    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Exhaust];
    protected override IEnumerable<IHoverTip> ExtraHoverTips => CardOptions.SelectMany(card => card.HoverTips);

    private static readonly IEnumerable<CardModel> CardOptions = [ModelDb.Card<PowerChoice>(), ModelDb.Card<EnergyChoice>(), ModelDb.Card<GuidanceChoice>()];

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        var cardModel = await CardSelectCmd.FromChooseACardScreen(choiceContext,  CardOptions.Select(c =>
        {
            CardModel card = CombatState.CreateCard(c, Owner);
            if(IsUpgraded) CardCmd.Upgrade(card);
            return card;
        }).ToList(), Owner);
        if (cardModel == null)
            return;
        await ((ICardChoice) cardModel).OnChosen(choiceContext);
    }

    protected override void OnUpgrade()
    {
        DynamicVars["Exhaust"].UpgradeValueBy(PowerChoice.ExhaustUpgrade);
        DynamicVars.Cards.UpgradeValueBy(EnergyChoice.DrawUpgrade);
    }
    
    public interface ICardChoice
    {
        Task OnChosen(PlayerChoiceContext context);
    }
}