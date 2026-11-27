using BaseLib.Utils;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Hooks;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.CardPools;
using MegaCrit.Sts2.Core.ValueProps;

namespace AncientsAwakened.AncientsAwakenedCode.Cards.Sebastian;

[Pool(typeof(IroncladCardPool))]
public sealed class Cinderborn() : AncientsAwakenedCard(2,
    CardType.Skill, CardRarity.Ancient,
    TargetType.Self)
{
    public override bool GainsBlock => true;
    protected override IEnumerable<DynamicVar> CanonicalVars => [new BlockVar(14M, ValueProp.Move), new CardsVar(4)];
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromKeyword(CardKeyword.Exhaust)];

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        var pile = PileType.Draw.GetPile(Owner);
        var cardsToScry = pile.Cards.Take(DynamicVars.Cards.IntValue).ToList();
        var cardsToExhaust = (await CardSelectCmd.FromSimpleGrid(choiceContext, cardsToScry, Owner, new CardSelectorPrefs(CardSelectorPrefs.ExhaustSelectionPrompt, 0, cardsToScry.Count))).ToList();
        foreach (var card in cardsToExhaust)
        {
            await CardCmd.Exhaust(choiceContext, card, false, true);
            CombatManager.Instance.History.CardDiscarded(Owner.Creature.CombatState, card);
            await Hook.AfterCardDiscarded(Owner.Creature.CombatState, choiceContext, card);
        }
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Block.UpgradeValueBy(2M);
        DynamicVars.Cards.UpgradeValueBy(2M);
    }
}