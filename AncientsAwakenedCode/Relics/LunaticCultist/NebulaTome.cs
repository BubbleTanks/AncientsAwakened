using AncientsAwakened.AncientsAwakenedCode.Cards.LunaticCultist;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Factories;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.RelicPools;
using MegaCrit.Sts2.Core.Saves.Runs;

namespace AncientsAwakened.AncientsAwakenedCode.Relics.LunaticCultist;

[Pool(typeof(EventRelicPool))]
public class NebulaTome : AncientsAwakenedRelic
{
    public override RelicRarity Rarity => RelicRarity.Ancient;
    
    protected override IEnumerable<DynamicVar> CanonicalVars => [new CardsVar(3), new ("Status", 2)];
    
    protected override IEnumerable<IHoverTip> ExtraHoverTips => HoverTipFactory.FromCardWithCardHoverTips<CosmicDust>();
    
    protected override string IconBaseName => !HasBookmark() ? base.IconBaseName + "_2" : base.IconBaseName;

    private bool _bookmark;
    
    [SavedProperty]
    private bool Bookmark
    {
        get => _bookmark;
        set
        {
            AssertMutable();
            _bookmark = value;
            InvokeDisplayAmountChanged();
        }
    }

    public void SetupForPlayer(Player player) => Bookmark = player.RunState.Rng.Niche.NextBool();
    
    private bool HasBookmark()
    {
        return !IsMutable || Bookmark;
    }
    
    public override async Task BeforeHandDraw(
        Player player,
        PlayerChoiceContext choiceContext,
        ICombatState combatState)
    {
        if (player != Owner || Owner.PlayerCombatState?.TurnNumber != 1)
            return;
        for (var i = 0; i < DynamicVars["Status"].BaseValue; ++i)
            CardCmd.PreviewCardPileAdd(await CardPileCmd.AddGeneratedCardToCombat(Owner.Creature.CombatState.CreateCard<CosmicDust>(Owner), PileType.Draw, Owner, CardPilePosition.Random));
        Flash();
        await Cmd.Wait(1f);
        var card = await CardSelectCmd.FromChooseACardScreen(choiceContext, CardFactory.GetDistinctForCombat(Owner, Owner.Character.CardPool.GetUnlockedCards(Owner.UnlockState, Owner.RunState.CardMultiplayerConstraint).Where(c => c.Type == CardType.Power), DynamicVars.Cards.IntValue, Owner.RunState.Rng.CombatCardGeneration).ToList(), Owner, true);
        if (card == null)
            return;
        card.SetToFreeThisTurn();
        await CardPileCmd.AddGeneratedCardToCombat(card, PileType.Hand, Owner);
    }
}