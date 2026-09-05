using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.CardPools;

namespace AncientsAwakened.AncientsAwakenedCode.Cards.LunaticCultist;

[Pool(typeof(CurseCardPool))]
public sealed class Insanity() : AncientsAwakenedCard(0, CardType.Curse, CardRarity.Curse, TargetType.None)
{
    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Exhaust];
    protected override IEnumerable<DynamicVar> CanonicalVars => [new EnergyVar(2), new CardsVar(1)];
    public override int MaxUpgradeLevel => 0;
    public override bool CanBeGeneratedByModifiers => false;
    
    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        await CreatureCmd.TriggerAnim(Owner.Creature, "Cast", Owner.Character.CastAnimDelay);
        var combatCardSelection = Owner.RunState.Rng.CombatCardSelection;
        var cards = PileType.Hand.GetPile(Owner).Cards;
        var list = cards.Where(c => c.EnergyCost.GetWithModifiers(CostModifiers.None) >= 0).ToList();
        (combatCardSelection.NextItem(list.Where(c => c.CostsEnergyOrStars(true))) ??
         combatCardSelection.NextItem(cards.Where(c => c.CostsEnergyOrStars(true))) ?? 
         combatCardSelection.NextItem(list) ?? combatCardSelection.NextItem(cards))?.EnergyCost.AddThisCombat(DynamicVars.Energy.IntValue);
        await CardPileCmd.Draw(choiceContext, DynamicVars.Cards.BaseValue, Owner);
    }

    protected override void OnUpgrade() => EnergyCost.UpgradeBy(-1);
    
}