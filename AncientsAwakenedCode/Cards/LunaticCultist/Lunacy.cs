using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.CardPools;
using MegaCrit.Sts2.Core.Random;

namespace AncientsAwakened.AncientsAwakenedCode.Cards.LunaticCultist;

[Pool(typeof(StatusCardPool))]
public class Lunacy() : AncientsAwakenedCard(1, CardType.Skill, CardRarity.Ancient, TargetType.None)
{
    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Exhaust];
    
    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        await CreatureCmd.TriggerAnim(Owner.Creature, "Cast", Owner.Character.CastAnimDelay);
        Rng combatCardSelection = Owner.RunState.Rng.CombatCardSelection;
        IReadOnlyList<CardModel> cards = PileType.Hand.GetPile(Owner).Cards;
        List<CardModel> list = cards.Where(c => c.EnergyCost.GetWithModifiers(CostModifiers.None) > 0 || c.BaseStarCost > 0).ToList();
        (combatCardSelection.NextItem(list.Where(c => c.CostsEnergyOrStars(true))) ??
         combatCardSelection.NextItem(cards.Where(c => c.CostsEnergyOrStars(true))) ?? 
         combatCardSelection.NextItem(list) ?? combatCardSelection.NextItem(cards))?.SetToFreeThisCombat();
    }

    protected override void OnUpgrade() => EnergyCost.UpgradeBy(-1);
    
}