using AncientsAwakened.AncientsAwakenedCode.Cards.Sebastian;
using AncientsAwakened.AncientsAwakenedCode.Extensions;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Map;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.RelicPools;
using MegaCrit.Sts2.Core.Rewards;
using MegaCrit.Sts2.Core.Rooms;
using MegaCrit.Sts2.Core.Runs;
using MegaCrit.Sts2.Core.Saves.Runs;

namespace AncientsAwakened.AncientsAwakenedCode.Relics.Sebastian;

[Pool(typeof(EventRelicPool))]
public class ShippingRequest : AncientsAwakenedRelic
{
    public override RelicRarity Rarity => RelicRarity.Ancient;
    
    public override bool HasUponPickupEffect => true;
    
    protected override IEnumerable<IHoverTip> ExtraHoverTips => HoverTipFactory.FromCardWithCardHoverTips<WeighedDown>().Concat(HoverTipFactory.FromCardWithCardHoverTips<HeavyCrate>());
    
    private int _rewardAmount;

    private bool _gaveReward;

    [SavedProperty]
    private int RewardAmount
    {
        get => _rewardAmount;
        set
        {
            AssertMutable();
            _rewardAmount = value;
        }
    }
    
    public ShippingRequest()
    {
        this.BlacklistFromEulogy();
    }

    public override async Task AfterObtained()
    {
        CardCmd.PreviewCardPileAdd([await CardPileCmd.Add(Owner.RunState.CreateCard<HeavyCrate>(Owner), PileType.Deck)], 2F);
        WeighedDown c = (WeighedDown) await CardPileCmd.AddCurseToDeck<WeighedDown>(Owner);
        c.FindTreasureCoords();
    }
    
    public override async Task AfterCombatEnd(CombatRoom room)
    {
        if(RewardAmount >= 1) RewardAmount = 0;
        if (room.RoomType != RoomType.Boss)
            return;

        List<CardModel> removeCards = new List<CardModel>();
        
        foreach (CardModel card in Owner.Deck.Cards.Where(c => c is HeavyCrate))
        {
            RewardAmount++;
            removeCards.Add(card);
        }

        foreach (CardModel card in removeCards)
        {
            await CardPileCmd.RemoveFromDeck(card);
            Flash();
        }
    }
    
    public override bool TryModifyRewards(Player player, List<Reward> rewards, AbstractRoom? room)
    {
        if (RewardAmount > 0 && player == Owner)
        {
            for (int i = 0; i < RewardAmount; i++)
            {
                rewards.Add(new GoldReward(Owner.RunState.Rng.Niche.NextInt(250,300), player));
                rewards.Add(new PotionReward(player));
                rewards.Add(new PotionReward(player));
                rewards.Add(new RelicReward(RelicRarity.Common, player));
                rewards.Add(new RelicReward(RelicRarity.Uncommon, player));
                rewards.Add(new RelicReward(RelicRarity.Rare, player));
                rewards.Add(new CardReward(CardCreationOptions.ForNonCombatWithUniformOdds([Owner.Character.CardPool], c => c.Rarity == CardRarity.Rare).WithFlags(CardCreationFlags.NoRarityModification), 3, player));
                rewards.Add(new CardReward(new CardCreationOptions([Owner.Character.CardPool], CardCreationSource.Other, CardRarityOddsType.RegularEncounter), 3, player));
                rewards.Add(new CardReward(new CardCreationOptions([Owner.Character.CardPool], CardCreationSource.Other, CardRarityOddsType.RegularEncounter), 3, player));
                rewards.Add(new CardReward(new CardCreationOptions([Owner.Character.CardPool], CardCreationSource.Other, CardRarityOddsType.RegularEncounter), 3, player));
                rewards.Add(new CardRemovalReward(player));
                rewards.Add(new CardRemovalReward(player));
                rewards.Add(new CardRemovalReward(player));
            }
            _gaveReward = true;
            return true;
        }
        return false;   
    }
    
    public override Task AfterMapGenerated(ActMap map, int actIndex)
    {
        if(_gaveReward) RewardAmount = 0;
        return Task.CompletedTask;
    }
}