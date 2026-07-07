using BaseLib.Extensions;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Multiplayer;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.CardPools;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.Random;

namespace AncientsAwakened.AncientsAwakenedCode.Cards.LunaticCultist;

[Pool(typeof(EventCardPool))]
public class PowerChoice() : AncientsAwakenedCard(-1, CardType.Skill, CardRarity.Ancient, TargetType.None), Starshine.ICardChoice
{
    public const int ExhaustValue = 4;
    public const int ExhaustUpgrade = 2;
    
    protected override IEnumerable<DynamicVar> CanonicalVars => [new CardsVar(ExhaustValue)];
    
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromKeyword(CardKeyword.Exhaust)];
    
    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        await OnChosen(choiceContext);
    }

    protected override void OnUpgrade() => DynamicVars.Cards.UpgradeValueBy(ExhaustUpgrade);

    public async Task OnChosen(PlayerChoiceContext context)
    {
        var cards = await CardSelectCmd.FromCombatPile(
            context, PileType.Draw.GetPile(Owner), Owner,
            new CardSelectorPrefs(CardSelectorPrefs.ExhaustSelectionPrompt, DynamicVars.Cards.IntValue)
            {
                Cancelable = true
            });
        if (cards == null)
            return;
        foreach(var card in cards)
            await CardCmd.Exhaust(context, card);
    }
}