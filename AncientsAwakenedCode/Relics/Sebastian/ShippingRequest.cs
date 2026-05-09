using AncientsAwakened.AncientsAwakenedCode.Cards;
using AncientsAwakened.AncientsAwakenedCode.Cards.Sebastian;
using AncientsAwakened.AncientsAwakenedCode.Relics;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.RelicPools;
using MegaCrit.Sts2.Core.Random;
using MegaCrit.Sts2.Core.Rewards;
using MegaCrit.Sts2.Core.Rooms;
using MegaCrit.Sts2.Core.Runs;

namespace AncientsAwakened.AncientsAwakenedCode.Relics;



[Pool(typeof(EventRelicPool))]
public class ShippingRequest : AncientsAwakenedRelic
{
    public override RelicRarity Rarity => RelicRarity.Ancient;
    
    protected override IEnumerable<IHoverTip> ExtraHoverTips => HoverTipFactory.FromCardWithCardHoverTips<WeighedDown>();

    private int RewardAmount;

    public override async Task AfterObtained()
    {
        CardCmd.PreviewCardPileAdd([await CardPileCmd.Add(Owner.RunState.CreateCard<HeavyCrate>(Owner), PileType.Deck)], 2F);
        await CardPileCmd.AddCurseToDeck<WeighedDown>(Owner);
    }
    
    public override async Task AfterCombatEnd(CombatRoom room)
    {
        if (room.RoomType != RoomType.Boss)
            return;
        Flash();

        List<CardModel> removeCards = new List<CardModel>();
        
        foreach (CardModel card in Owner.Deck.Cards.Where(c => c is HeavyCrate))
        {
            RewardAmount++;
            removeCards.Add(card);
        }
        
        /*
        foreach (CardModel card in Owner.Deck.Cards.Where(c => c is WeighedDown))
        {
            removeCards.Add(card);
        } 
        */

        foreach (CardModel card in removeCards)
        {
            await CardPileCmd.RemoveFromDeck(card);
        }
    }
    
    public override bool TryModifyRewards(Player player, List<Reward> rewards, AbstractRoom? room)
    {
        if (RewardAmount > 0 && player == Owner)
        {
            for (int i = 0; i < RewardAmount; i++)
            {
                
                rewards.Add(new GoldReward(Owner.RunState.Rng.Niche.NextInt(200,300), player));
                rewards.Add(new PotionReward(player));
                rewards.Add(new PotionReward(player));
                rewards.Add(new RelicReward(RelicRarity.Common, player));
                rewards.Add(new RelicReward(RelicRarity.Uncommon, player));
                rewards.Add(new RelicReward(RelicRarity.Uncommon, player));
                rewards.Add(new RelicReward(RelicRarity.Rare, player));
                rewards.Add(new CardReward(CardCreationOptions.ForNonCombatWithUniformOdds([Owner.Character.CardPool], c => c.Rarity == CardRarity.Rare).WithFlags(CardCreationFlags.NoRarityModification), 3, player));
                rewards.Add(new CardReward(new CardCreationOptions([Owner.Character.CardPool], CardCreationSource.Other, CardRarityOddsType.RegularEncounter), 3, player));
                rewards.Add(new CardReward(new CardCreationOptions([Owner.Character.CardPool], CardCreationSource.Other, CardRarityOddsType.RegularEncounter), 3, player));
                rewards.Add(new CardReward(new CardCreationOptions([Owner.Character.CardPool], CardCreationSource.Other, CardRarityOddsType.RegularEncounter), 3, player));
                rewards.Add(new CardRemovalReward(player));
                rewards.Add(new CardRemovalReward(player));
            }
            RewardAmount = 0;
            return true;
        }
        return false;   
    }
}