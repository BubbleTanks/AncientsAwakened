using AncientsAwakened.AncientsAwakenedCode.Cards.Mountain;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.CardPools;
using MegaCrit.Sts2.Core.Models.RelicPools;
using MegaCrit.Sts2.Core.Rewards;
using MegaCrit.Sts2.Core.Runs;

namespace AncientsAwakened.AncientsAwakenedCode.Relics.Mountain;

[Pool(typeof(EventRelicPool))]
public class CursedCache : AncientsAwakenedRelic
{
    public override RelicRarity Rarity => RelicRarity.Ancient;
    
    protected override IEnumerable<DynamicVar> CanonicalVars => [new CardsVar(3), new GoldVar(300), new ("Relics", 3), new ("Curses", 2)];
    
    public override async Task AfterObtained()
    {
        List<Reward> list = [];
        var options = CardCreationOptions.ForNonCombatWithUniformOdds([Owner.Character.CardPool], c => c.Rarity == CardRarity.Rare).WithFlags(CardCreationFlags.NoRarityModification);
        for (var i = 0; i < DynamicVars.Cards.IntValue; i++)
        {
            list.Add(new CardReward(options, 3, Owner));
        }
        for (var i = 0; i < DynamicVars["Relics"].IntValue; ++i)
        {
            list.Add(new RelicReward(Owner));
        }
        list.Add(new GoldReward(DynamicVars.Gold.IntValue, Owner));
        await RewardsCmd.OfferCustom(Owner, list);
    }
    
    public override async Task BeforeHandDraw(
        Player player,
        PlayerChoiceContext choiceContext,
        ICombatState combatState)
    {
        if (player != Owner || Owner.PlayerCombatState.TurnNumber > 1)
            return;
        Flash();
        var availableCurses = ModelDb.CardPool<CurseCardPool>().GetUnlockedCards(Owner.UnlockState, Owner.RunState.CardMultiplayerConstraint).Where(c => c.CanBeGeneratedByModifiers).OrderBy((Func<CardModel, ModelId>) (c => c.Id)).ToList();
        List<CardModel> curses = [];
        for (var i = 0; i < DynamicVars["Curses"].IntValue; ++i)
        {
            var cardModel = Owner.RunState.Rng.CombatCardGeneration.NextItem(availableCurses);
            availableCurses.Remove(cardModel);
            curses.Add(combatState.CreateCard(cardModel, Owner));
        }
        CardCmd.PreviewCardPileAdd(await CardPileCmd.AddGeneratedCardsToCombat(curses, PileType.Draw, Owner, CardPilePosition.Random));
        await Cmd.Wait(3f);
    }
}